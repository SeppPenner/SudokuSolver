// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageFileTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the language files that the library and the application share.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib.Tests;

/// <summary>
/// A class to test the language files that the library and the application share. They are linked into this
/// project from the application project, so a key that is missing there fails here.
/// </summary>
[TestClass]
public class LanguageFileTests
{
    /// <summary>
    /// The identifiers of the two shipped languages.
    /// </summary>
    private static readonly string[] Identifiers = ["de-DE", "en-US"];

    /// <summary>
    /// The keys that the library itself asks for. They have to resolve in every language, because
    /// <see cref="ILanguage.GetWord"/> returns <c>null</c> for an unknown key and never falls back to another
    /// language. Keep this list in sync with the GetWord calls below src/SudokuSolverLib.
    /// </summary>
    private static readonly string[] LibraryKeys =
    [
        "Area",
        "BoxAt",
        "Col",
        "ColumnLower",
        "ColumnMiddle",
        "ColumnUpper",
        "FixingOnPositionReason",
        "HyperA",
        "HyperB",
        "HyperC",
        "HyperD",
        "InvalidValueForA",
        "OnlyOnePossibility",
        "OnlyPossibleInRule",
        "Row",
        "RowLeft",
        "RowMiddle",
        "RowRight",
        "SudokuTile",
        "TileValueCantBeGreaterThan",
        "TileValueCantBeZeroOrSmaller",
        "TryAndError",
        "ValueAtPosXY"
    ];

    /// <summary>
    /// Checks whether every key the library uses is answered in every language.
    /// </summary>
    [TestMethod]
    public void EveryKeyTheLibraryUsesIsAnsweredInEveryLanguage()
    {
        foreach (var identifier in Identifiers)
        {
            var language = TestDataProvider.GetLanguage(identifier);

            foreach (var key in LibraryKeys)
            {
                var word = language.GetWord(key);

                Assert.IsNotNull(word, $"The key {key} is missing in {identifier}.");
                Assert.IsFalse(string.IsNullOrWhiteSpace(word), $"The key {key} is empty in {identifier}.");
            }
        }
    }

    /// <summary>
    /// Checks whether both language files hold the same keys. A key that only exists in one of them shows up as
    /// an empty text as soon as the user switches the language.
    /// </summary>
    [TestMethod]
    public void BothLanguagesHoldTheSameKeys()
    {
        var german = TestDataProvider.GetLanguage("de-DE").Words.Select(word => word.Key).ToList();
        var english = TestDataProvider.GetLanguage("en-US").Words.Select(word => word.Key).ToList();

        Assert.HasCount(german.Count, english);
        CollectionAssert.AreEquivalent(german, english);
        Assert.HasCount(german.Distinct().Count(), german, "A key is defined twice in de-DE.");
    }

    /// <summary>
    /// Checks whether a text that is used as a format string has the same number of placeholders in every
    /// language. A missing placeholder does not throw, it silently drops the value from the output.
    /// </summary>
    [TestMethod]
    public void ThePlaceholdersMatchInEveryLanguage()
    {
        var german = TestDataProvider.GetLanguage("de-DE").Words;
        var english = TestDataProvider.GetLanguage("en-US").Words;

        foreach (var word in german)
        {
            var counterpart = english.Single(other => other.Key == word.Key);

            Assert.AreEqual(
                CountPlaceholders(word.Value),
                CountPlaceholders(counterpart.Value),
                $"The number of placeholders of the key {word.Key} differs between the languages.");
        }
    }

    /// <summary>
    /// Checks whether the four texts that are used as format strings carry exactly the placeholders their call
    /// sites fill in.
    /// </summary>
    [TestMethod]
    public void TheFormatStringsCarryTheExpectedPlaceholders()
    {
        foreach (var identifier in Identifiers)
        {
            var language = TestDataProvider.GetLanguage(identifier);

            Assert.AreEqual(1, CountPlaceholders(language.GetWord("TileValueCantBeGreaterThan")), identifier);
            Assert.AreEqual(3, CountPlaceholders(language.GetWord("ValueAtPosXY")), identifier);
            Assert.AreEqual(4, CountPlaceholders(language.GetWord("FixingOnPositionReason")), identifier);
        }
    }

    /// <summary>
    /// Counts the distinct placeholders of a format string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The number of distinct placeholders as <see cref="int"/>.</returns>
    private static int CountPlaceholders(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        return Regex.Matches(text, "{[0-9]+}").Select(match => match.Value).Distinct().Count();
    }
}
