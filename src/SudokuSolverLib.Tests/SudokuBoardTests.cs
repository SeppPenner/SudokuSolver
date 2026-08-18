// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuBoardTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="SudokuBoard" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib.Tests;

/// <summary>
/// A class to test the <see cref="SudokuBoard"/> class.
/// </summary>
[TestClass]
public class SudokuBoardTests
{
    /// <summary>
    /// Checks whether a classic board has the expected size and hands out its tiles by position.
    /// </summary>
    [TestMethod]
    public void AClassicBoardIsNineByNine()
    {
        var board = SudokuFactory.ClassicWith3X3Boxes(TestDataProvider.GetLanguage());

        Assert.AreEqual(9, board.Width);
        Assert.AreEqual(9, board.Height);
        Assert.AreEqual(9, board.OutputTiles().GetLength(0));
        Assert.AreEqual(9, board.OutputTiles().GetLength(1));
        Assert.AreEqual(0, board.Tile(8, 8).Value);
    }

    /// <summary>
    /// Checks whether the rows are read into the tiles the way the application passes them in, with a dot for an
    /// empty tile, and whether the output prints them again row by row.
    /// </summary>
    [TestMethod]
    public void AddRowFillsTheTilesRowByRow()
    {
        var board = TestDataProvider.GetClassicBoard(TestDataProvider.ClassicPuzzle);

        // The first row of the puzzle is "53..7....", so the first two tiles hold 5 and 3 and the third is empty.
        Assert.AreEqual(5, board.Tile(0, 0).Value);
        Assert.AreEqual(3, board.Tile(1, 0).Value);
        Assert.AreEqual(0, board.Tile(2, 0).Value);

        // The last row is "....8..79", so the tile at the bottom right holds 9.
        Assert.AreEqual(9, board.Tile(8, 8).Value);

        var expected = string.Join(Environment.NewLine, TestDataProvider.ClassicPuzzle.Select(row => row.Replace('.', '0'))) + Environment.NewLine;

        Assert.AreEqual(expected, board.OutputSolution());
    }

    /// <summary>
    /// Checks whether a slash blocks a tile instead of being read as a value.
    /// </summary>
    [TestMethod]
    public void ASlashBlocksATile()
    {
        var board = SudokuFactory.ClassicWith3X3Boxes(TestDataProvider.GetLanguage());
        board.AddRow("/........");

        Assert.IsTrue(board.Tile(0, 0).IsBlocked);
        Assert.IsFalse(board.Tile(1, 0).IsBlocked);
    }

    /// <summary>
    /// Checks whether a sudoku with exactly one solution is solved to exactly that solution.
    /// </summary>
    [TestMethod]
    public void AProperSudokuIsSolvedToItsOnlySolution()
    {
        var board = TestDataProvider.GetClassicBoard(TestDataProvider.ClassicPuzzle);

        var solutions = board.Solve().ToList();

        Assert.HasCount(1, solutions);

        var expected = string.Join(Environment.NewLine, TestDataProvider.ClassicSolution) + Environment.NewLine;

        Assert.AreEqual(expected, solutions[0].OutputSolution());
    }

    /// <summary>
    /// Checks whether a board that already holds a full solution is returned unchanged and is not solved a
    /// second time.
    /// </summary>
    [TestMethod]
    public void AnAlreadySolvedSudokuIsReturnedAsItIs()
    {
        var board = TestDataProvider.GetClassicBoard(TestDataProvider.ClassicSolution);

        var solutions = board.Solve().ToList();

        Assert.HasCount(1, solutions);

        var expected = string.Join(Environment.NewLine, TestDataProvider.ClassicSolution) + Environment.NewLine;

        Assert.AreEqual(expected, solutions[0].OutputSolution());
    }

    /// <summary>
    /// Checks whether a board that breaks a rule right away yields no solution at all. The application shows a
    /// message box for this case, so it has to be an empty result and not an exception.
    /// </summary>
    [TestMethod]
    public void AContradictingSudokuYieldsNoSolution()
    {
        var board = TestDataProvider.GetClassicBoard(TestDataProvider.ContradictingPuzzle);

        var solutions = board.Solve().ToList();

        Assert.IsEmpty(solutions);
    }

    /// <summary>
    /// Checks whether the tiles of a solution are handed out as a two dimensional array in the order the form
    /// fills its own tiles with.
    /// </summary>
    [TestMethod]
    public void TheSolutionTilesAreHandedOutByPosition()
    {
        var board = TestDataProvider.GetClassicBoard(TestDataProvider.ClassicPuzzle);

        var tiles = board.Solve().First().OutputTiles();

        // The solution starts with the row "534678912", so the tile at the position 2, 0 holds the 4.
        Assert.AreEqual("5", tiles[0, 0].ToStringSimple());
        Assert.AreEqual("4", tiles[2, 0].ToStringSimple());
        Assert.AreEqual("9", tiles[8, 8].ToStringSimple());
    }

    /// <summary>
    /// Checks whether every rule of a classic board is written out, nine rows, nine columns and nine boxes. The
    /// method returned inside its loop before version 1.0.8.0 and listed one single rule.
    /// </summary>
    [TestMethod]
    public void EveryRuleIsWrittenOut()
    {
        var language = TestDataProvider.GetLanguage();
        var board = SudokuFactory.ClassicWith3X3Boxes(language);

        var lines = board.OutputRulesToDialog().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.HasCount(27, lines);
        Assert.AreEqual(9, lines.Count(line => line.Contains(language.GetWord("Row") ?? string.Empty)));
        Assert.AreEqual(9, lines.Count(line => line.Contains(language.GetWord("Col") ?? string.Empty)));
        Assert.AreEqual(9, lines.Count(line => line.Contains(language.GetWord("BoxAt") ?? string.Empty)));
    }
}
