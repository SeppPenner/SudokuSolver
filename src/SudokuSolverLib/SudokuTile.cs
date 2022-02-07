// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuTile.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The sudoku tile class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Languages.Interfaces;

    /// <summary>
    /// The sudoku tile class.
    /// </summary>
    public class SudokuTile
    {
        /// <summary>
        /// The cleared count.
        /// </summary>
        private const int Cleared = 0;

        /// <summary>
        /// The language.
        /// </summary>
        private static ILanguage? language;

        /// <summary>
        /// The maximum value.
        /// </summary>
        private readonly int maximumValue;

        /// <summary>
        /// The possible values.
        /// </summary>
        private ISet<int> possibleValues;

        /// <summary>
        /// The value.
        /// </summary>
        private int value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuTile"/> class.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <param name="y">The Y value.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <param name="language">The language.</param>
        public SudokuTile(int x, int y, int maximumValue, ILanguage language)
        {
            SudokuTile.language = language;
            this.X = x;
            this.Y = y;
            this.IsBlocked = false;
            this.maximumValue = maximumValue;
            this.possibleValues = new HashSet<int>();
            this.value = 0;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get => this.value;
            set
            {
                if (language is null)
                {
                    return;
                }

                if (value > this.maximumValue)
                {
                    throw new ArgumentOutOfRangeException(string.Format(language.GetWord("TileValueCantBeGreaterThan"), value));
                }

                if (value < Cleared)
                {
                    throw new ArgumentOutOfRangeException(language.GetWord("TileValueCantBeZeroOrSmaller") + value);
                }

                this.value = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tile has a value or not.
        /// </summary>
        public bool HasValue => this.Value != Cleared;

        /// <summary>
        /// Gets the X value.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y value.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets a value indicating whether the tile is blocked or not.
        /// </summary>
        public bool IsBlocked
        {
            // A blocked field can not contain a value -- used for creating 'holes' in the map
            get;
            private set;
        }

        /// <summary>
        /// Gets the possible count.
        /// </summary>
        public int PossibleCount => this.IsBlocked ? 1 : this.possibleValues.Count;

        /// <summary>
        /// Blocks the tile.
        /// </summary>
        public void Block()
        {
            this.IsBlocked = true;
        }

        /// <summary>
        /// Converts the object to a simple <see cref="string"/>.
        /// </summary>
        /// <returns>The object as simple <see cref="string"/>.</returns>
        public string ToStringSimple()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Converts the object to a <see cref="string"/>.
        /// </summary>
        /// <returns>The object as <see cref="string"/>.</returns>
        public override string ToString()
        {
            if (language is null)
            {
                return string.Empty;
            }

            return string.Format(language.GetWord("ValueAtPosXY"), this.Value, this.X, this.Y);
        }

        /// <summary>
        /// Checks whether the value is possible or not.
        /// </summary>
        /// <param name="i">The value.</param>
        /// <returns>A value indicating whether the value is possible or not.</returns>
        public bool IsValuePossible(int i)
        {
            return this.possibleValues.Contains(i);
        }

        /// <summary>
        /// Combines the solved states.
        /// </summary>
        /// <param name="a">The a value.</param>
        /// <param name="b">The b value.</param>
        /// <returns>The <see cref="SudokuProgress"/>.</returns>
        internal static SudokuProgress CombineSolvedState(SudokuProgress a, SudokuProgress b)
        {
            if (a == SudokuProgress.Failed)
            {
                return a;
            }

            if (a == SudokuProgress.NoProgress)
            {
                return b;
            }

            if (a == SudokuProgress.Progress)
            {
                return b == SudokuProgress.Failed ? b : a;
            }

            if (language is null)
            {
                return SudokuProgress.Failed;
            }

            throw new InvalidOperationException(language.GetWord("InvalidValueForA"));
        }

        /// <summary>
        /// Resets the possibles.
        /// </summary>
        internal void ResetPossibles()
        {
            this.possibleValues.Clear();
            foreach (var i in Enumerable.Range(1, this.maximumValue))
            {
                if (!this.HasValue || this.Value == i)
                {
                    this.possibleValues.Add(i);
                }
            }
        }

        /// <summary>
        /// Fixes the value.
        /// </summary>
        /// <param name="valueParam">The value.</param>
        /// <param name="reason">The reason.</param>
        internal void Fix(int valueParam, string reason)
        {
            if (language is null)
            {
                return;
            }

            Console.WriteLine(language.GetWord("FixingOnPositionReason"), valueParam, this.X, this.Y, reason);
            this.Value = valueParam;
            this.ResetPossibles();
        }

        /// <summary>
        /// Removes the possibles.
        /// </summary>
        /// <param name="existingNumbers">The existing numbers.</param>
        /// <returns>The <see cref="SudokuProgress"/>.</returns>
        internal SudokuProgress RemovePossibles(IEnumerable<int> existingNumbers)
        {
            if (this.IsBlocked)
            {
                return SudokuProgress.NoProgress;
            }

            // Takes the current possible values and removes the ones existing in `existingNumbers`
            this.possibleValues = new HashSet<int>(this.possibleValues.Where(x => !existingNumbers.Contains(x)));
            var result = SudokuProgress.NoProgress;

            if (this.possibleValues.Count == 1)
            {
                if (language is null)
                {
                    return SudokuProgress.Failed;
                }

                this.Fix(this.possibleValues.First(), language.GetWord("OnlyOnePossibility"));
                result = SudokuProgress.Progress;
            }

            if (this.possibleValues.Count == 0)
            {
                return SudokuProgress.Failed;
            }

            return result;
        }
    }
}