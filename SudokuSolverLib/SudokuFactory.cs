using System;
using System.Collections.Generic;
using System.Linq;
using Languages.Interfaces;

namespace SudokuSolverLib
{
    public static class SudokuFactory
    {
        private const int DefaultSize = 9;
        private const int SamuraiAreas = 7;
        private const int BoxSize = 3;
        private const int HyperMargin = 1;
        private static ILanguage _lang;

        public static IEnumerable<Tuple<int, int>> Box(int sizeX, int sizeY)
        {
            foreach (var x in Enumerable.Range(0, sizeX))
            foreach (var y in Enumerable.Range(0, sizeY))
                yield return new Tuple<int, int>(x, y);
        }

        // ReSharper disable once UnusedMember.Global
        public static SudokuBoard Samurai(ILanguage language)
        {
            _lang = language;
            var board = new SudokuBoard(SamuraiAreas * BoxSize, SamuraiAreas * BoxSize, DefaultSize, language);
            // Removed the empty areas where there are no tiles
            var queriesForBlocked = new List<IEnumerable<SudokuTile>>
            {
                from pos in Box(BoxSize, BoxSize * 2)
                select board.Tile(pos.Item1 + DefaultSize, pos.Item2),
                from pos in Box(BoxSize, BoxSize * 2)
                select board.Tile(pos.Item1 + DefaultSize, pos.Item2 + DefaultSize * 2 - BoxSize),
                from pos in Box(BoxSize * 2, BoxSize)
                select board.Tile(pos.Item1, pos.Item2 + DefaultSize),
                from pos in Box(BoxSize * 2, BoxSize)
                select board.Tile(pos.Item1 + DefaultSize * 2 - BoxSize, pos.Item2 + DefaultSize)
            };
            foreach (var query in queriesForBlocked)
            foreach (var tile in query) tile.Block();

            // Select the tiles in the 3 x 3 area (area.X, area.Y) and create rules for them
            foreach (var area in Box(SamuraiAreas, SamuraiAreas))
            {
                var tilesInArea = from pos in Box(BoxSize, BoxSize)
                    select board.Tile(area.Item1 * BoxSize + pos.Item1, area.Item2 * BoxSize + pos.Item2);
                var sudokuTiles = tilesInArea as SudokuTile[] ?? tilesInArea.ToArray();
                if (sudokuTiles.First().IsBlocked)
                    continue;
                board.CreateRule(_lang.GetWord("Area") + area.Item1 + ", " + area.Item2, sudokuTiles);
            }

            // Select all rows and create columns for them
            // ReSharper disable UnusedVariable
            var cols = from pos in Box(board.Width, 1) select new {X = pos.Item1, Y = pos.Item2};
            var rows = from pos in Box(1, board.Height) select new {X = pos.Item1, Y = pos.Item2};
            foreach (var posSet in Enumerable.Range(0, board.Width))
            {
                board.CreateRule(_lang.GetWord("ColumnUpper") + posSet,
                    from pos in Box(1, DefaultSize) select board.Tile(posSet, pos.Item2));
                board.CreateRule(_lang.GetWord("ColumnLower") + posSet,
                    from pos in Box(1, DefaultSize) select board.Tile(posSet, pos.Item2 + DefaultSize + BoxSize));

                board.CreateRule(_lang.GetWord("RowLeft") + posSet,
                    from pos in Box(DefaultSize, 1) select board.Tile(pos.Item1, posSet));
                board.CreateRule(_lang.GetWord("RowRight") + posSet,
                    from pos in Box(DefaultSize, 1) select board.Tile(pos.Item1 + DefaultSize + BoxSize, posSet));

                if (posSet < BoxSize * 2 || posSet >= BoxSize * 2 + DefaultSize) continue;
                // Create rules for the middle sudoku
                board.CreateRule(_lang.GetWord("ColumnMiddle") + posSet,
                    from pos in Box(1, 9) select board.Tile(posSet, pos.Item2 + BoxSize * 2));
                board.CreateRule(_lang.GetWord("RowMiddle") + posSet,
                    from pos in Box(9, 1) select board.Tile(pos.Item1 + BoxSize * 2, posSet));
            }
            return board;
        }

        private static SudokuBoard SizeAndBoxes(int width, int height, int boxCountX, int boxCountY, ILanguage language)
        {
            _lang = language;
            var board = new SudokuBoard(width, height, language);
            board.AddBoxesCount(boxCountX, boxCountY);
            return board;
        }

        public static SudokuBoard ClassicWith3X3Boxes(ILanguage language)
        {
            _lang = language;
            return SizeAndBoxes(DefaultSize, DefaultSize, DefaultSize / BoxSize, DefaultSize / BoxSize, language);
        }

        // ReSharper disable once UnusedMember.Global
        public static SudokuBoard ClassicWith3X3BoxesAndHyperRegions(ILanguage language)
        {
            _lang = language;
            var board = ClassicWith3X3Boxes(language);
            const int hyperSecond = HyperMargin + BoxSize + HyperMargin;
            // Create the four extra hyper regions
            board.CreateRule(_lang.GetWord("HyperA"),
                from pos in Box(3, 3) select board.Tile(pos.Item1 + HyperMargin, pos.Item2 + HyperMargin));
            board.CreateRule(_lang.GetWord("HyperB"),
                from pos in Box(3, 3) select board.Tile(pos.Item1 + hyperSecond, pos.Item2 + HyperMargin));
            board.CreateRule(_lang.GetWord("HyperC"),
                from pos in Box(3, 3) select board.Tile(pos.Item1 + HyperMargin, pos.Item2 + hyperSecond));
            board.CreateRule(_lang.GetWord("HyperD"),
                from pos in Box(3, 3) select board.Tile(pos.Item1 + hyperSecond, pos.Item2 + hyperSecond));
            return board;
        }

        // ReSharper disable once UnusedMember.Global
        public static SudokuBoard ClassicWithSpecialBoxes(string[] areas, ILanguage language)
        {
            _lang = language;
            var sizeX = areas[0].Length;
            var sizeY = areas.Length;
            var board = new SudokuBoard(sizeX, sizeY, language);
            var joinedString = string.Join("", areas);
            var grouped = joinedString.Distinct();

            // Loop through all the unique characters
            foreach (var ch in grouped)
            {
                // Select the rule tiles based on the index of the character
                var ruleTiles = from i in Enumerable.Range(0, joinedString.Length)
                    where joinedString[i] == ch // filter out any non-matching characters
                    select board.Tile(i % sizeX, i / sizeY);
                board.CreateRule(_lang.GetWord("Area") + ch, ruleTiles);
            }

            return board;
        }
    }
}