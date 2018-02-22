using System;
using System.Collections.Generic;
using System.Linq;
using Languages.Interfaces;

namespace SudokuSolverLib
{
    public class SudokuTile
    {
        private const int Cleared = 0;
        private readonly int _maxValue;
        private ISet<int> _possibleValues;
        private int _value;
        private static ILanguage _lang;

        public SudokuTile(int x, int y, int maxValue, ILanguage language)
        {
            _lang = language;
            X = x;
            Y = y;
            IsBlocked = false;
            _maxValue = maxValue;
            _possibleValues = new HashSet<int>();
            _value = 0;
        }

        public int Value
        {
            //_lang.GetWord("InvalidValueForA")
            get => _value;
            set
            {
                if (value > _maxValue)
                    throw new ArgumentOutOfRangeException(string.Format(_lang.GetWord("TileValueCantBeGreaterThan"), value));
                if (value < Cleared)
                    throw new ArgumentOutOfRangeException(_lang.GetWord("TileValueCantBeZeroOrSmaller") + value);
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
            throw new InvalidOperationException(_lang.GetWord("InvalidValueForA"));
        }

        public string ToStringSimple()
        {
            return Value.ToString();
        }

        public override string ToString()
        {
            return string.Format(_lang.GetWord("ValueAtPosXY"), Value, X, Y);
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
            Console.WriteLine(_lang.GetWord("FixingOnPositionReason"), value, X, Y, reason);
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
                Fix(_possibleValues.First(), _lang.GetWord("OnlyOnePossibility"));
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