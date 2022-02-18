// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuFactory.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The sudoku factory class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib;

/// <summary>
/// The sudoku factory class.
/// </summary>
public static class SudokuFactory
{
    /// <summary>
    /// The default size.
    /// </summary>
    private const int DefaultSize = 9;

    /// <summary>
    /// The samurai areas.
    /// </summary>
    private const int SamuraiAreas = 7;

    /// <summary>
    /// The box size.
    /// </summary>
    private const int BoxSize = 3;

    /// <summary>
    /// The hyper margin.
    /// </summary>
    private const int HyperMargin = 1;

    /// <summary>
    /// The language.
    /// </summary>
    private static ILanguage? language;

    /// <summary>
    /// Boxes the values.
    /// </summary>
    /// <param name="sizeX">The X size.</param>
    /// <param name="sizeY">The Y size.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Tuple"/>s of <see cref="int"/> and <see cref="int"/>.</returns>
    public static IEnumerable<Tuple<int, int>> Box(int sizeX, int sizeY)
    {
        foreach (var x in Enumerable.Range(0, sizeX))
        {
            foreach (var y in Enumerable.Range(0, sizeY))
            {
                yield return new Tuple<int, int>(x, y);
            }
        }
    }

    /// <summary>
    /// Builds the samurai of the board.
    /// </summary>
    /// <param name="languageParam">The language.</param>
    /// <returns>The <see cref="SudokuBoard"/>.</returns>
    public static SudokuBoard Samurai(ILanguage languageParam)
    {
        language = languageParam;
        var board = new SudokuBoard(SamuraiAreas * BoxSize, SamuraiAreas * BoxSize, DefaultSize, languageParam);

        // Removed the empty areas where there are no tiles
        var queriesForBlocked = new List<IEnumerable<SudokuTile>>
        {
            from pos in Box(BoxSize, BoxSize * 2) select board.Tile(pos.Item1 + DefaultSize, pos.Item2),
            from pos in Box(BoxSize, BoxSize * 2) select board.Tile(pos.Item1 + DefaultSize, pos.Item2 + (DefaultSize * 2) - BoxSize),
            from pos in Box(BoxSize * 2, BoxSize) select board.Tile(pos.Item1, pos.Item2 + DefaultSize),
            from pos in Box(BoxSize * 2, BoxSize) select board.Tile(pos.Item1 + (DefaultSize * 2) - BoxSize, pos.Item2 + DefaultSize)
        };

        foreach (var tile in queriesForBlocked.SelectMany(query => query))
        {
            tile.Block();
        }

        // Select the tiles in the 3 x 3 area (area.X, area.Y) and create rules for them
        foreach (var area in Box(SamuraiAreas, SamuraiAreas))
        {
            var tilesInArea = from pos in Box(BoxSize, BoxSize) select board.Tile((area.Item1 * BoxSize) + pos.Item1, (area.Item2 * BoxSize) + pos.Item2);
            var sudokuTiles = tilesInArea as SudokuTile[] ?? tilesInArea.ToArray();

            if (sudokuTiles.First().IsBlocked)
            {
                continue;
            }

            board.CreateRule(language.GetWord("Area") + area.Item1 + ", " + area.Item2, sudokuTiles);
        }

        // Select all rows and create columns for them
        var cols = from pos in Box(board.Width, 1) select new { X = pos.Item1, Y = pos.Item2 };
        var rows = from pos in Box(1, board.Height) select new { X = pos.Item1, Y = pos.Item2 };
        foreach (var posSet in Enumerable.Range(0, board.Width))
        {
            board.CreateRule(language.GetWord("ColumnUpper") + posSet, from pos in Box(1, DefaultSize) select board.Tile(posSet, pos.Item2));
            board.CreateRule(language.GetWord("ColumnLower") + posSet, from pos in Box(1, DefaultSize) select board.Tile(posSet, pos.Item2 + DefaultSize + BoxSize));

            board.CreateRule(language.GetWord("RowLeft") + posSet, from pos in Box(DefaultSize, 1) select board.Tile(pos.Item1, posSet));
            board.CreateRule(language.GetWord("RowRight") + posSet, from pos in Box(DefaultSize, 1) select board.Tile(pos.Item1 + DefaultSize + BoxSize, posSet));

            if (posSet < BoxSize * 2 || posSet >= (BoxSize * 2) + DefaultSize)
            {
                continue;
            }

            // Create rules for the middle sudoku
            board.CreateRule(language.GetWord("ColumnMiddle") + posSet, from pos in Box(1, 9) select board.Tile(posSet, pos.Item2 + (BoxSize * 2)));
            board.CreateRule(language.GetWord("RowMiddle") + posSet, from pos in Box(9, 1) select board.Tile(pos.Item1 + (BoxSize * 2), posSet));
        }

        return board;
    }

    /// <summary>
    /// Generates a classic board with 3 x 3 boxes.
    /// </summary>
    /// <param name="languageParam">The language.</param>
    /// <returns>The <see cref="SudokuBoard"/>.</returns>
    public static SudokuBoard ClassicWith3X3Boxes(ILanguage languageParam)
    {
        language = languageParam;
        return SizeAndBoxes(DefaultSize, DefaultSize, DefaultSize / BoxSize, DefaultSize / BoxSize, languageParam);
    }

    /// <summary>
    /// Generates a classic board with 3 x 3 boxes and hyper regions.
    /// </summary>
    /// <param name="languageParam">The language.</param>
    /// <returns>The <see cref="SudokuBoard"/>.</returns>
    public static SudokuBoard ClassicWith3X3BoxesAndHyperRegions(ILanguage languageParam)
    {
        language = languageParam;
        var board = ClassicWith3X3Boxes(languageParam);
        const int HyperSecond = HyperMargin + BoxSize + HyperMargin;

        // Create the four extra hyper regions
        board.CreateRule(language.GetWord("HyperA") ?? string.Empty, from pos in Box(3, 3) select board.Tile(pos.Item1 + HyperMargin, pos.Item2 + HyperMargin));
        board.CreateRule(language.GetWord("HyperB") ?? string.Empty, from pos in Box(3, 3) select board.Tile(pos.Item1 + HyperSecond, pos.Item2 + HyperMargin));
        board.CreateRule(language.GetWord("HyperC") ?? string.Empty, from pos in Box(3, 3) select board.Tile(pos.Item1 + HyperMargin, pos.Item2 + HyperSecond));
        board.CreateRule(language.GetWord("HyperD") ?? string.Empty, from pos in Box(3, 3) select board.Tile(pos.Item1 + HyperSecond, pos.Item2 + HyperSecond));
        return board;
    }

    /// <summary>
    /// Generates a classic board with special boxes.
    /// </summary>
    /// <param name="areas">The areas.</param>
    /// <param name="languageParam">The language.</param>
    /// <returns>The <see cref="SudokuBoard"/>.</returns>
    public static SudokuBoard ClassicWithSpecialBoxes(string[] areas, ILanguage languageParam)
    {
        language = languageParam;
        var sizeX = areas[0].Length;
        var sizeY = areas.Length;
        var board = new SudokuBoard(sizeX, sizeY, language);
        var joinedString = string.Join(string.Empty, areas);
        var grouped = joinedString.Distinct();

        // Loop through all the unique characters
        foreach (var ch in grouped)
        {
            // Select the rule tiles based on the index of the character
            var ruleTiles = from i in Enumerable.Range(0, joinedString.Length)
                            where joinedString[i] == ch // filter out any non-matching characters
                            select board.Tile(i % sizeX, i / sizeY);
            board.CreateRule(language.GetWord("Area") + ch, ruleTiles);
        }

        return board;
    }

    /// <summary>
    /// Sets the sizes and boxes.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="boxCountX">The X box count.</param>
    /// <param name="boxCountY">The Y box count.</param>
    /// <param name="languageParam">The language.</param>
    /// <returns>The <see cref="SudokuBoard"/>.</returns>
    private static SudokuBoard SizeAndBoxes(int width, int height, int boxCountX, int boxCountY, ILanguage languageParam)
    {
        language = languageParam;
        var board = new SudokuBoard(width, height, languageParam);
        board.AddBoxesCount(boxCountX, boxCountY);
        return board;
    }
}
