// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuBoard.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The sudoku board class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Languages.Interfaces;

    /// <summary>
    /// The sudoku board class.
    /// </summary>
    public class SudokuBoard
    {
        /// <summary>
        /// The maximum value.
        /// </summary>
        private readonly int maximumValue;

        /// <summary>
        /// The rules.
        /// </summary>
        private readonly ISet<SudokuRule> rules = new HashSet<SudokuRule>();

        /// <summary>
        /// The tiles.
        /// </summary>
        private readonly SudokuTile[,] tiles;

        /// <summary>
        /// The language.
        /// </summary>
        private readonly ILanguage language;

        /// <summary>
        /// The row add index.
        /// </summary>
        private int rowAddIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuBoard"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <param name="language">The language.</param>
        public SudokuBoard(int width, int height, int maximumValue, ILanguage language)
        {
            this.language = language;
            this.maximumValue = maximumValue;
            this.tiles = new SudokuTile[width, height];
            this.CreateTiles();

            // If maxValue is not width or height, then adding line rules would be stupid
            if (this.maximumValue == width || this.maximumValue == height)
            {
                this.SetupLineRules();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuBoard"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="language">The language.</param>
        public SudokuBoard(int width, int height, ILanguage language) : this(width, height, Math.Max(width, height), language)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuBoard"/> class.
        /// </summary>
        /// <param name="copy">The copy.</param>
        /// <param name="language">The language.</param>
        private SudokuBoard(SudokuBoard copy, ILanguage language)
        {
            this.language = language;
            this.maximumValue = copy.maximumValue;
            this.tiles = new SudokuTile[copy.Width, copy.Height];
            this.CreateTiles();

            // Copy the tile values
            foreach (var position in SudokuFactory.Box(this.Width, this.Height))
            {
                this.tiles[position.Item1, position.Item2] =
                    new SudokuTile(position.Item1, position.Item2, this.maximumValue, this.language)
                    {
                        Value = copy.tiles[position.Item1, position.Item2].Value
                    };
            }

            // Copy the rules
            foreach (var rule in copy.rules)
            {
                var ruleTiles = new HashSet<SudokuTile>();

                foreach (var tile in rule)
                {
                    ruleTiles.Add(this.tiles[tile.X, tile.Y]);
                }

                this.rules.Add(new SudokuRule(ruleTiles, rule.Description));
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width => this.tiles.GetLength(0);

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height => this.tiles.GetLength(1);

        /// <summary>
        /// Gets the output tiles.
        /// </summary>
        /// <returns>The output tiles.</returns>
        public SudokuTile[,] OutputTiles()
        {
            return this.tiles;
        }

        /// <summary>
        /// Gets a tile.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <param name="y">THe Y value.</param>
        /// <returns>The <see cref="SudokuTile"/>.</returns>
        public SudokuTile Tile(int x, int y)
        {
            return this.tiles[x, y];
        }

        /// <summary>
        /// Creates a rule.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="tilesParam">The tiles.</param>
        public void CreateRule(string description, params SudokuTile[] tilesParam)
        {
            this.rules.Add(new SudokuRule(tilesParam, description));
        }

        /// <summary>
        /// Creates a rule.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="tilesParam">The tiles.</param>
        public void CreateRule(string description, IEnumerable<SudokuTile> tilesParam)
        {
            this.rules.Add(new SudokuRule(tilesParam, description));
        }

        /// <summary>
        /// Outputs the solution.
        /// </summary>
        /// <returns>The solution <see cref="string"/>.</returns>
        public string OutputSolution()
        {
            var solution = string.Empty;

            for (var y = 0; y < this.tiles.GetLength(1); y++)
            {
                for (var x = 0; x < this.tiles.GetLength(0); x++)
                {
                    solution += this.tiles[x, y].ToStringSimple();
                }

                solution += Environment.NewLine;
            }

            return solution;
        }

        /// <summary>
        /// Solves the sudoku.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="SudokuBoard"/>s.</returns>
        public IEnumerable<SudokuBoard> Solve()
        {
            this.ResetSolutions();
            var simplify = SudokuProgress.Progress;

            while (simplify == SudokuProgress.Progress)
            {
                simplify = this.Simplify();
            }

            if (simplify == SudokuProgress.Failed)
            {
                yield break;
            }

            // Find one of the values with the least number of alternatives, but that still has at least 2 alternatives
            var query = from rule in this.rules
                        from tile in rule
                        where tile.PossibleCount > 1
                        orderby tile.PossibleCount
                        select tile;

            var chosen = query.FirstOrDefault();

            if (chosen == null)
            {
                // The board has been completed, we're done!
                yield return this;
                yield break;
            }

            Console.WriteLine(this.language.GetWord("SudokuTile") + chosen);

            foreach (var value in Enumerable.Range(1, this.maximumValue))
            {
                // Iterate through all the valid possibles on the chosen square and pick a number for it
                if (!chosen.IsValuePossible(value))
                {
                    continue;
                }

                var copy = new SudokuBoard(this, this.language);
                copy.Tile(chosen.X, chosen.Y).Fix(value, this.language.GetWord("TryAndError"));

                foreach (var innerSolution in copy.Solve())
                {
                    yield return innerSolution;
                }
            }
        }

        /// <summary>
        /// Outputs the rules to the dialog.
        /// </summary>
        /// <returns>The output rules as <see cref="string"/>.</returns>
        public string OutputRulesToDialog()
        {
            foreach (var rule in this.rules)
            {
                return string.Join(",", rule) + " - " + rule;
            }

            return string.Empty;
        }

        /// <summary>
        /// Adds a row.
        /// </summary>
        /// <param name="s">The text.</param>
        public void AddRow(string s)
        {
            // Method for initializing a board from string
            for (var i = 0; i < s.Length; i++)
            {
                var tile = this.tiles[i, this.rowAddIndex];

                if (s[i] == '/')
                {
                    tile.Block();
                    continue;
                }

                var value = s[i] == '.' ? 0 : (int)char.GetNumericValue(s[i]);
                tile.Value = value;
            }

            this.rowAddIndex++;
        }

        /// <summary>
        /// Adds the boxes count.
        /// </summary>
        /// <param name="boxesX">The boxes X value.</param>
        /// <param name="boxesY">The boxes Y value.</param>
        internal void AddBoxesCount(int boxesX, int boxesY)
        {
            var sizeX = this.Width / boxesX;
            var sizeY = this.Height / boxesY;

            var boxes = SudokuFactory.Box(sizeX, sizeY);

            foreach (var position in boxes)
            {
                var boxTiles = this.TileBox(position.Item1 * sizeX, position.Item2 * sizeY, sizeX, sizeY);
                this.CreateRule(this.language.GetWord("BoxAt") + position.Item1 + ", " + position.Item2 + ")", boxTiles);
            }
        }

        /// <summary>
        /// Creates the tiles.
        /// </summary>
        private void CreateTiles()
        {
            foreach (var position in SudokuFactory.Box(this.tiles.GetLength(0), this.tiles.GetLength(1)))
            {
                this.tiles[position.Item1, position.Item2] = new SudokuTile(position.Item1, position.Item2, this.maximumValue, this.language);
            }
        }

        /// <summary>
        /// Sets up the line rules.
        /// </summary>
        private void SetupLineRules()
        {
            // Create rules for rows and columns
            for (var x = 0; x < this.Width; x++)
            {
                var row = this.GetCol(x);
                this.rules.Add(new SudokuRule(row, this.language.GetWord("Row") + x)); 
            }

            for (var y = 0; y < this.Height; y++)
            {
                var col = this.GetRow(y);
                this.rules.Add(new SudokuRule(col, this.language.GetWord("Col") + y));
            }
        }

        /// <summary>
        /// Gets the tile box.
        /// </summary>
        /// <param name="startX">The start x value.</param>
        /// <param name="startY">The start y value.</param>
        /// <param name="sizeX">The size x value.</param>
        /// <param name="sizeY">The size y value.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="SudokuTile"/>s.</returns>
        private IEnumerable<SudokuTile> TileBox(int startX, int startY, int sizeX, int sizeY)
        {
            return from pos in SudokuFactory.Box(sizeX, sizeY) select this.tiles[startX + pos.Item1, startY + pos.Item2];
        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="SudokuTile"/>s.</returns>
        private IEnumerable<SudokuTile> GetRow(int row)
        {
            for (var i = 0; i < this.tiles.GetLength(0); i++)
            {
                yield return this.tiles[i, row];
            }
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <param name="column">The column index.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="SudokuTile"/>s.</returns>
        private IEnumerable<SudokuTile> GetCol(int column)
        {
            for (var i = 0; i < this.tiles.GetLength(1); i++)
            {
                yield return this.tiles[column, i];
            }
        }

        /// <summary>
        /// Checks whether the rules are valid or not.
        /// </summary>
        /// <returns><c>true</c> if the rules are valid, <c>false</c> else.</returns>
        private bool CheckValid()
        {
            return this.rules.All(rule => rule.CheckValid());
        }

        /// <summary>
        /// Resets the solutions.
        /// </summary>
        private void ResetSolutions()
        {
            foreach (var tile in this.tiles)
            {
                tile.ResetPossibles();
            }
        }

        /// <summary>
        /// Simplifies the progress.
        /// </summary>
        /// <returns>The <see cref="SudokuProgress"/>.</returns>
        private SudokuProgress Simplify()
        {
            var valid = this.CheckValid();
            return !valid
                       ? SudokuProgress.Failed
                       : this.rules.Aggregate(
                           SudokuProgress.NoProgress,
                           (current, rule) => SudokuTile.CombineSolvedState(current, rule.Solve(this.language)));
        }
    }
}