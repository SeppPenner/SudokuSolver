// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to provide the test data used in the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib.Tests;

/// <summary>
/// A class to provide the test data used in the tests.
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// The language manager. It loads the language files from the languages folder below the test output
    /// directory, which is where the linked files of the application project end up.
    /// </summary>
    private static readonly ILanguageManager languageManager = new LanguageManager();

    /// <summary>
    /// Gets the rows of the classic sudoku from the Wikipedia article, in the format
    /// <see cref="SudokuBoard.AddRow"/> expects. It has exactly one solution.
    /// </summary>
    public static string[] ClassicPuzzle => new[]
    {
        "53..7....",
        "6..195...",
        ".98....6.",
        "8...6...3",
        "4..8.3..1",
        "7...2...6",
        ".6....28.",
        "...419..5",
        "....8..79"
    };

    /// <summary>
    /// Gets the one solution of <see cref="ClassicPuzzle"/>, in the format
    /// <see cref="SudokuBoard.OutputSolution"/> returns it.
    /// </summary>
    public static string[] ClassicSolution => new[]
    {
        "534678912",
        "672195348",
        "198342567",
        "859761423",
        "426853791",
        "713924856",
        "961537284",
        "287419635",
        "345286179"
    };

    /// <summary>
    /// Gets the rows of a board that holds the same value twice in its first row, which makes it unsolvable
    /// before the solver even starts to guess.
    /// </summary>
    public static string[] ContradictingPuzzle => new[]
    {
        "55.......",
        ".........",
        ".........",
        ".........",
        ".........",
        ".........",
        ".........",
        ".........",
        "........."
    };

    /// <summary>
    /// Gets the language used in all tests. The library takes an <see cref="ILanguage"/> everywhere, and the
    /// German file is the one the application selects on startup.
    /// </summary>
    /// <returns>The <see cref="ILanguage"/> for the identifier de-DE.</returns>
    public static ILanguage GetLanguage()
    {
        return GetLanguage("de-DE");
    }

    /// <summary>
    /// Gets the language for an identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the language, for example de-DE.</param>
    /// <returns>The <see cref="ILanguage"/> for the given identifier.</returns>
    public static ILanguage GetLanguage(string identifier)
    {
        languageManager.SetCurrentLanguage(identifier);
        return languageManager.GetCurrentLanguage();
    }

    /// <summary>
    /// Gets a board with 3 x 3 boxes that is filled with the rows of a puzzle.
    /// </summary>
    /// <param name="rows">The rows of the puzzle.</param>
    /// <returns>A <see cref="SudokuBoard"/> that holds the given rows.</returns>
    public static SudokuBoard GetClassicBoard(string[] rows)
    {
        var board = SudokuFactory.ClassicWith3X3Boxes(GetLanguage());

        foreach (var row in rows)
        {
            board.AddRow(row);
        }

        return board;
    }
}
