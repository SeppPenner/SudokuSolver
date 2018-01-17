using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverLib
{
    public class SudokuTile
    {
        private const int Cleared = 0;
        private readonly int _maxValue;
        private ISet<int> _possibleValues;
        private int _value;

        public SudokuTile(int x, int y, int maxValue)
        {
            X = x;
            Y = y;
            IsBlocked = false;
            _maxValue = maxValue;
            _possibleValues = new HashSet<int>();
            _value = 0;
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value > _maxValue)
                    throw new ArgumentOutOfRangeException("SudokuTile Value cannot be greater than " +
                                                          _maxValue + ". Was " + value);
                if (value < Cleared)
                    throw new ArgumentOutOfRangeException("SudokuTile Value cannot be zero or smaller. Was " + value);
                _value = value;
            }
        }

        public bool HasValue => Value != Cleared;

        public int X { get; }

        public int Y { get; }

        public bool IsBlocked
            // A blocked field can not contain a value -- used for creating 'holes' in the map
        {
            get;
            private set;
        }

        public int PossibleCount => IsBlocked ? 1 : _possibleValues.Count;

        internal static SudokuProgress CombineSolvedState(SudokuProgress a, SudokuProgress b)
        {
            if (a == SudokuProgress.Failed)
                return a;
            if (a == SudokuProgress.NoProgress)
                return b;
            if (a == SudokuProgress.Progress)
                return b == SudokuProgress.Failed ? b : a;
            throw new InvalidOperationException("Invalid value for a");
        }

        public string ToStringSimple()
        {
            return Value.ToString();
        }

        public override string ToString()
        {
            return $"Value {Value} at pos {X}, {Y}. ";
        }

        internal void ResetPossibles()
        {
            _possibleValues.Clear();
            foreach (var i in Enumerable.Range(1, _maxValue))
                if (!HasValue || Value == i)
                    _possibleValues.Add(i);
        }

        public void Block()
        {
            IsBlocked = true;
        }

        internal void Fix(int value, string reason)
        {
            Console.WriteLine("Fixing {0} on pos {1}, {2}: {3}", value, X, Y, reason);
            Value = value;
            ResetPossibles();
        }

        internal SudokuProgress RemovePossibles(IEnumerable<int> existingNumbers)
        {
            if (IsBlocked)
                return SudokuProgress.NoProgress;
            // Takes the current possible values and removes the ones existing in `existingNumbers`
            _possibleValues = new HashSet<int>(_possibleValues.Where(x => !existingNumbers.Contains(x)));
            var result = SudokuProgress.NoProgress;
            if (_possibleValues.Count == 1)
            {
                Fix(_possibleValues.First(), "Only one possibility");
                result = SudokuProgress.Progress;
            }
            if (_possibleValues.Count == 0)
                return SudokuProgress.Failed;
            return result;
        }

        public bool IsValuePossible(int i)
        {
            return _possibleValues.Contains(i);
        }
    }
}