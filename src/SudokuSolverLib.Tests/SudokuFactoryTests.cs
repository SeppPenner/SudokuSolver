// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuFactoryTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="SudokuFactory" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib.Tests;

/// <summary>
/// A class to test the <see cref="SudokuFactory"/> class.
/// </summary>
[TestClass]
public class SudokuFactoryTests
{
    /// <summary>
    /// Checks whether the box helper walks the positions column by column, which is the order every rule of the
    /// library is built with.
    /// </summary>
    [TestMethod]
    public void BoxWalksThePositionsColumnByColumn()
    {
        var positions = SudokuFactory.Box(3, 2).ToList();

        Assert.HasCount(6, positions);
        Assert.AreEqual(new Tuple<int, int>(0, 0), positions[0]);
        Assert.AreEqual(new Tuple<int, int>(0, 1), positions[1]);
        Assert.AreEqual(new Tuple<int, int>(1, 0), positions[2]);
        Assert.AreEqual(new Tuple<int, int>(2, 1), positions[5]);
    }

    /// <summary>
    /// Checks whether a classic board carries the 27 rules of a sudoku, nine rows, nine columns and nine boxes.
    /// </summary>
    [TestMethod]
    public void AClassicBoardHasTwentySevenRules()
    {
        var board = SudokuFactory.ClassicWith3X3Boxes(TestDataProvider.GetLanguage());

        Assert.HasCount(27, GetRuleLines(board));
    }

    /// <summary>
    /// Checks whether the hyper regions add four rules on top of the classic ones and are named after the
    /// language file.
    /// </summary>
    [TestMethod]
    public void TheHyperRegionsAddFourRules()
    {
        var language = TestDataProvider.GetLanguage();

        var board = SudokuFactory.ClassicWith3X3BoxesAndHyperRegions(language);
        var lines = GetRuleLines(board);

        Assert.HasCount(31, lines);
        Assert.AreEqual(1, lines.Count(line => line.EndsWith(language.GetWord("HyperA") ?? string.Empty, StringComparison.Ordinal)));
        Assert.AreEqual(1, lines.Count(line => line.EndsWith(language.GetWord("HyperD") ?? string.Empty, StringComparison.Ordinal)));
    }

    /// <summary>
    /// Checks whether the areas of a board with special boxes are read row by row on a board that is wider than
    /// it is high. The row index was computed from the height until version 1.0.8.0, which threw an
    /// <see cref="IndexOutOfRangeException"/> here and went unnoticed on square boards, where both are equal.
    /// </summary>
    [TestMethod]
    public void SpecialBoxesUseTheWidthForTheRowIndex()
    {
        var language = TestDataProvider.GetLanguage();

        var board = SudokuFactory.ClassicWithSpecialBoxes(["aabb", "aabb"], language);

        Assert.AreEqual(4, board.Width);
        Assert.AreEqual(2, board.Height);

        // Four columns, two rows and the two areas a and b.
        var lines = GetRuleLines(board);

        Assert.HasCount(8, lines);

        var areaA = lines.Single(line => line.EndsWith((language.GetWord("Area") ?? string.Empty) + "a", StringComparison.Ordinal));

        // The area a covers the left half of both rows, so the four tiles 0, 0 and 1, 0 and 0, 1 and 1, 1.
        Assert.Contains(FormatPosition(language, 0, 0), areaA);
        Assert.Contains(FormatPosition(language, 1, 0), areaA);
        Assert.Contains(FormatPosition(language, 0, 1), areaA);
        Assert.Contains(FormatPosition(language, 1, 1), areaA);
        Assert.DoesNotContain(FormatPosition(language, 2, 0), areaA);
    }

    /// <summary>
    /// Checks whether the samurai board spans five overlapping sudokus and blocks the four empty corners between
    /// them.
    /// </summary>
    [TestMethod]
    public void TheSamuraiBoardBlocksTheEmptyAreas()
    {
        var board = SudokuFactory.Samurai(TestDataProvider.GetLanguage());

        Assert.AreEqual(21, board.Width);
        Assert.AreEqual(21, board.Height);
        Assert.IsFalse(board.Tile(0, 0).IsBlocked);
        Assert.IsTrue(board.Tile(9, 0).IsBlocked);
        Assert.IsTrue(board.Tile(0, 9).IsBlocked);
    }

    /// <summary>
    /// Gets the lines that the board writes for its rules, one line per rule.
    /// </summary>
    /// <param name="board">The board.</param>
    /// <returns>The rule lines of the board.</returns>
    private static string[] GetRuleLines(SudokuBoard board)
    {
        return board.OutputRulesToDialog().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Formats a position the way <see cref="SudokuTile.ToString"/> writes it, so that the tiles of a rule can be
    /// recognized in the rule line.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <param name="x">The X value.</param>
    /// <param name="y">The Y value.</param>
    /// <returns>The position as it appears in a rule line.</returns>
    private static string FormatPosition(ILanguage language, int x, int y)
    {
        return string.Format(language.GetWord("ValueAtPosXY") ?? string.Empty, 0, x, y);
    }
}
