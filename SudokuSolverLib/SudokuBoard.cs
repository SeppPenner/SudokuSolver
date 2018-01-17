using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverLib
{
    public class SudokuBoard
    {
        private readonly int _maxValue;

        private readonly ISet<SudokuRule> _rules = new HashSet<SudokuRule>();
        private readonly SudokuTile[,] _tiles;

        private int _rowAddIndex;

        private SudokuBoard(SudokuBoard copy)
        {
            _maxValue = copy._maxValue;
            _tiles = new SudokuTile[copy.Width, copy.Height];
            CreateTiles();
            // Copy the tile values
            foreach (var pos in SudokuFactory.Box(Width, Height))
                _tiles[pos.Item1, pos.Item2] =
                    new SudokuTile(pos.Item1, pos.Item2, _maxValue) {Value = copy._tiles[pos.Item1, pos.Item2].Value};

            // Copy the rules
            foreach (var rule in copy._rules)
            {
                var ruleTiles = new HashSet<SudokuTile>();
                foreach (var tile in rule)
                    ruleTiles.Add(_tiles[tile.X, tile.Y]);
                _rules.Add(new SudokuRule(ruleTiles, rule.Description));
            }
        }

        public SudokuBoard(int width, int height, int maxValue)
        {
            _maxValue = maxValue;
            _tiles = new SudokuTile[width, height];
            CreateTiles();
            if (_maxValue == width || _maxValue == height
            ) // If maxValue is not width or height, then adding line rules would be stupid
                SetupLineRules();
        }

        public SudokuBoard(int width, int height) : this(width, height, Math.Max(width, height))
        {
        }

        public int Width => _tiles.GetLength(0);

        public int Height => _tiles.GetLength(1);

        private void CreateTiles()
        {
            foreach (var pos in SudokuFactory.Box(_tiles.GetLength(0), _tiles.GetLength(1)))
                _tiles[pos.Item1, pos.Item2] = new SudokuTile(pos.Item1, pos.Item2, _maxValue);
        }

        private void SetupLineRules()
        {
            // Create rules for rows and columns
            for (var x = 0; x < Width; x++)
            {
                var row = GetCol(x);
                _rules.Add(new SudokuRule(row, "Row " + x));
            }
            for (var y = 0; y < Height; y++)
            {
                var col = GetRow(y);
                _rules.Add(new SudokuRule(col, "Col " + y));
            }
        }

        private IEnumerable<SudokuTile> TileBox(int startX, int startY, int sizeX, int sizeY)
        {
            return from pos in SudokuFactory.Box(sizeX, sizeY) select _tiles[startX + pos.Item1, startY + pos.Item2];
        }

        private IEnumerable<SudokuTile> GetRow(int row)
        {
            for (var i = 0; i < _tiles.GetLength(0); i++)
                yield return _tiles[i, row];
        }

        private IEnumerable<SudokuTile> GetCol(int col)
        {
            for (var i = 0; i < _tiles.GetLength(1); i++)
                yield return _tiles[col, i];
        }

        public void CreateRule(string description, params SudokuTile[] tiles)
        {
            _rules.Add(new SudokuRule(tiles, description));
        }

        public void CreateRule(string description, IEnumerable<SudokuTile> tiles)
        {
            _rules.Add(new SudokuRule(tiles, description));
        }

        private bool CheckValid()
        {
            return _rules.All(rule => rule.CheckValid());
        }

        public IEnumerable<SudokuBoard> Solve()
        {
            ResetSolutions();
            var simplify = SudokuProgress.Progress;
            while (simplify == SudokuProgress.Progress) simplify = Simplify();

            if (simplify == SudokuProgress.Failed)
                yield break;

            // Find one of the values with the least number of alternatives, but that still has at least 2 alternatives
            var query = from rule in _rules
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

            Console.WriteLine("SudokuTile: " + chosen);

            foreach (var value in Enumerable.Range(1, _maxValue))
            {
                // Iterate through all the valid possibles on the chosen square and pick a number for it
                if (!chosen.IsValuePossible(value))
                    continue;
                var copy = new SudokuBoard(this);
                copy.Tile(chosen.X, chosen.Y).Fix(value, "Trial and error");
                foreach (var innerSolution in copy.Solve())
                    yield return innerSolution;
            }
        }

        public string OutputSolution()
        {
            var solution = string.Empty;
            for (var y = 0; y < _tiles.GetLength(1); y++)
            {
                for (var x = 0; x < _tiles.GetLength(0); x++)
                    solution += _tiles[x, y].ToStringSimple();
                solution += Environment.NewLine;
            }
            return solution;
        }

        public SudokuTile[,] OutputTiles()
        {
            return _tiles;
        }

        public SudokuTile Tile(int x, int y)
        {
            return _tiles[x, y];
        }

        public void AddRow(string s)
        {
            // Method for initializing a board from string
            for (var i = 0; i < s.Length; i++)
            {
                var tile = _tiles[i, _rowAddIndex];
                if (s[i] == '/')
                {
                    tile.Block();
                    continue;
                }
                var value = s[i] == '.' ? 0 : (int) char.GetNumericValue(s[i]);
                tile.Value = value;
            }
            _rowAddIndex++;
        }

        private void ResetSolutions()
        {
            foreach (var tile in _tiles)
                tile.ResetPossibles();
        }

        private SudokuProgress Simplify()
        {
            var valid = CheckValid();
            return !valid
                ? SudokuProgress.Failed
                : _rules.Aggregate(SudokuProgress.NoProgress,
                    (current, rule) => SudokuTile.CombineSolvedState(current, rule.Solve()));
        }

        internal void AddBoxesCount(int boxesX, int boxesY)
        {
            var sizeX = Width / boxesX;
            var sizeY = Height / boxesY;

            var boxes = SudokuFactory.Box(sizeX, sizeY);
            foreach (var pos in boxes)
            {
                var boxTiles = TileBox(pos.Item1 * sizeX, pos.Item2 * sizeY, sizeX, sizeY);
                CreateRule("Box at (" + pos.Item1 + ", " + pos.Item2 + ")", boxTiles);
            }
        }

        public string OutputRulesToDialog()
        {
            foreach (var rule in _rules)
                return string.Join(",", rule) + " - " + rule;
            return string.Empty;
        }
    }
}