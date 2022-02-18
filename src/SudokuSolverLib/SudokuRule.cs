// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuRule.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The sudoku rule class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib;

/// <summary>
/// The sudoku rule class.
/// </summary>
public class SudokuRule : IEnumerable<SudokuTile>
{
    /// <summary>
    /// The tiles.
    /// </summary>
    private readonly ISet<SudokuTile> tiles;

    /// <summary>
    /// Initializes a new instance of the <see cref="SudokuRule"/> class.
    /// </summary>
    /// <param name="tiles">The tiles.</param>
    /// <param name="description">The description.</param>
    internal SudokuRule(IEnumerable<SudokuTile> tiles, string description)
    {
        this.tiles = new HashSet<SudokuTile>(tiles);
        this.Description = description;
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>A new <see cref="IEnumerator"/>.</returns>
    public IEnumerator<SudokuTile> GetEnumerator()
    {
        return this.tiles.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>A new <see cref="IEnumerator"/>.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Checks whether the values are valid or not.
    /// </summary>
    /// <returns>A value indicating whether the values are valid or not.</returns>
    public bool CheckValid()
    {
        var filtered = this.tiles.Where(tile => tile.HasValue);
        var groupedByValue = filtered.GroupBy(tile => tile.Value);
        return groupedByValue.All(group => group.Count() == 1);
    }

    /// <summary>
    /// Checks whether the check is completed or not.
    /// </summary>
    /// <returns>A value indicating whether the check is completed or not.</returns>
    public bool CheckComplete()
    {
        return this.tiles.All(tile => tile.HasValue) && this.CheckValid();
    }

    /// <summary>
    /// Converts the object to a <see cref="string"/>.
    /// </summary>
    /// <returns>The object as <see cref="string"/>.</returns>
    public override string ToString()
    {
        return this.Description;
    }

    /// <summary>
    /// Solves the sudoku.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <returns>The <see cref="SudokuProgress"/>.</returns>
    internal SudokuProgress Solve(ILanguage language)
    {
        // If both are null, return null (indicating no change). If one is null, return the other. Else return result1 && result2
        var result1 = this.RemovePossibles();
        var result2 = this.CheckForOnlyOnePossibility(language);
        return SudokuTile.CombineSolvedState(result1, result2);
    }

    /// <summary>
    /// Removes the possibles.
    /// </summary>
    /// <returns>The <see cref="SudokuProgress"/>.</returns>
    private SudokuProgress RemovePossibles()
    {
        // Tiles that has a number already
        var withNumber = this.tiles.Where(tile => tile.HasValue);

        // Tiles without a number
        var withoutNumber = this.tiles.Where(tile => !tile.HasValue);

        // The existing numbers in this rule
        var existingNumbers =
            new HashSet<int>(withNumber.Select(tile => tile.Value).Distinct().ToList());

        return withoutNumber.Aggregate(
            SudokuProgress.NoProgress,
            (current, tile) => SudokuTile.CombineSolvedState(current, tile.RemovePossibles(existingNumbers)));
    }

    /// <summary>
    /// Checks for only one possibility.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <returns>The <see cref="SudokuProgress"/>.</returns>
    private SudokuProgress CheckForOnlyOnePossibility(ILanguage language)
    {
        // Check if there is only one number within this rule that can have a specific value
        IList<int> existingNumbers = this.tiles.Select(tile => tile.Value).Distinct().ToList();
        var result = SudokuProgress.NoProgress;

        foreach (var value in Enumerable.Range(1, this.tiles.Count))
        {
            // This rule already has the value, skip checking for it
            if (existingNumbers.Contains(value))
            {
                continue;
            }

            var possibles = this.tiles.Where(tile => !tile.HasValue && tile.IsValuePossible(value)).ToList();

            if (possibles.Count == 0)
            {
                return SudokuProgress.Failed;
            }

            if (possibles.Count == 1)
            {
                possibles.First().Fix(value, language.GetWord("OnlyPossibleInRule") + this);
                result = SudokuProgress.Progress;
            }
        }

        return result;
    }
}
