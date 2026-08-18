// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuTileTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="SudokuTile" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib.Tests;

/// <summary>
/// A class to test the <see cref="SudokuTile"/> class.
/// </summary>
[TestClass]
public class SudokuTileTests
{
    /// <summary>
    /// The maximum value used in the tests, the same one a classic board uses.
    /// </summary>
    private const int MaximumValue = 9;

    /// <summary>
    /// Checks whether a new tile starts empty, unblocked and without any possible value, because the possible
    /// values are only filled once the board resets them at the start of a solving run.
    /// </summary>
    [TestMethod]
    public void ANewTileIsEmptyAndHasNoPossibleValues()
    {
        var tile = new SudokuTile(3, 4, MaximumValue, TestDataProvider.GetLanguage());

        Assert.AreEqual(3, tile.X);
        Assert.AreEqual(4, tile.Y);
        Assert.AreEqual(0, tile.Value);
        Assert.IsFalse(tile.HasValue);
        Assert.IsFalse(tile.IsBlocked);
        Assert.AreEqual(0, tile.PossibleCount);
    }

    /// <summary>
    /// Checks whether a value within the allowed range is kept and reported as a value.
    /// </summary>
    [TestMethod]
    public void AValueWithinTheRangeIsKept()
    {
        var tile = new SudokuTile(0, 0, MaximumValue, TestDataProvider.GetLanguage())
        {
            Value = 7
        };

        Assert.AreEqual(7, tile.Value);
        Assert.IsTrue(tile.HasValue);
        Assert.AreEqual("7", tile.ToStringSimple());
    }

    /// <summary>
    /// Checks whether zero is treated as an empty tile, which is what the application relies on when it hands
    /// over a grid that the user left blank.
    /// </summary>
    [TestMethod]
    public void ZeroMeansThatTheTileIsEmpty()
    {
        var tile = new SudokuTile(0, 0, MaximumValue, TestDataProvider.GetLanguage())
        {
            Value = 0
        };

        Assert.IsFalse(tile.HasValue);
        Assert.AreEqual("0", tile.ToStringSimple());
    }

    /// <summary>
    /// Checks whether a value above the maximum value is rejected.
    /// </summary>
    [TestMethod]
    public void AValueAboveTheMaximumValueThrows()
    {
        var tile = new SudokuTile(0, 0, MaximumValue, TestDataProvider.GetLanguage());

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => tile.Value = MaximumValue + 1);
    }

    /// <summary>
    /// Checks whether a negative value is rejected. Zero is allowed, it is the empty tile.
    /// </summary>
    [TestMethod]
    public void ANegativeValueThrows()
    {
        var tile = new SudokuTile(0, 0, MaximumValue, TestDataProvider.GetLanguage());

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => tile.Value = -1);
    }

    /// <summary>
    /// Checks whether a blocked tile reports exactly one possibility, so that the solver never picks it as the
    /// tile with the fewest alternatives.
    /// </summary>
    [TestMethod]
    public void ABlockedTileHasExactlyOnePossibility()
    {
        var tile = new SudokuTile(0, 0, MaximumValue, TestDataProvider.GetLanguage());
        tile.Block();

        Assert.IsTrue(tile.IsBlocked);
        Assert.AreEqual(1, tile.PossibleCount);
    }

    /// <summary>
    /// Checks whether the long text of a tile is built from the language file instead of being hardcoded. The
    /// language is a static field of <see cref="SudokuTile"/> and is set by its constructor.
    /// </summary>
    [TestMethod]
    public void TheTileTextComesFromTheLanguage()
    {
        var language = TestDataProvider.GetLanguage();
        var tile = new SudokuTile(1, 2, MaximumValue, language)
        {
            Value = 5
        };

        var expected = string.Format(language.GetWord("ValueAtPosXY") ?? string.Empty, 5, 1, 2);

        Assert.AreEqual(expected, tile.ToString());
        Assert.Contains("5", tile.ToString());
    }
}
