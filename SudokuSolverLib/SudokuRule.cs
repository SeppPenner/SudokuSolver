using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Languages.Interfaces;

namespace SudokuSolverLib
{
    public class SudokuRule : IEnumerable<SudokuTile>
    {
        private readonly ISet<SudokuTile> _tiles;

        internal SudokuRule(IEnumerable<SudokuTile> tiles, string description)
        {
            _tiles = new HashSet<SudokuTile>(tiles);
            Description = description;
        }

        public string Description { get; }

        public IEnumerator<SudokuTile> GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool CheckValid()
        {
            var filtered = _tiles.Where(tile => tile.HasValue);
            var groupedByValue = filtered.GroupBy(tile => tile.Value);
            return groupedByValue.All(group => group.Count() == 1);
        }

        // ReSharper disable once UnusedMember.Global
        public bool CheckComplete()
        {
            return _tiles.All(tile => tile.HasValue) && CheckValid();
        }

        private SudokuProgress RemovePossibles()
        {
            // Tiles that has a number already
            var withNumber = _tiles.Where(tile => tile.HasValue);

            // Tiles without a number
            var withoutNumber = _tiles.Where(tile => !tile.HasValue);

            // The existing numbers in this rule
            IEnumerable<int> existingNumbers =
                new HashSet<int>(withNumber.Select(tile => tile.Value).Distinct().ToList());

            return withoutNumber.Aggregate(SudokuProgress.NoProgress,
                (current, tile) => SudokuTile.CombineSolvedState(current, tile.RemovePossibles(existingNumbers)));
        }

        private SudokuProgress CheckForOnlyOnePossibility(ILanguage language)
        {
            // Check if there is only one number within this rule that can have a specific value
            IList<int> existingNumbers = _tiles.Select(tile => tile.Value).Distinct().ToList();
            var result = SudokuProgress.NoProgress;

            foreach (var value in Enumerable.Range(1, _tiles.Count))
            {
                if (existingNumbers.Contains(value)) // this rule already has the value, skip checking for it
                    continue;
                var possibles = _tiles.Where(tile => !tile.HasValue && tile.IsValuePossible(value)).ToList();
                if (possibles.Count == 0)
                    return SudokuProgress.Failed;
                if (possibles.Count == 1)
                {
                    possibles.First().Fix(value, language.GetWord("OnlyPossibleInRule") + ToString()); //, ILanguage language
                    result = SudokuProgress.Progress;
                }
            }
            return result;
        }

        internal SudokuProgress Solve(ILanguage language)
        {
            // If both are null, return null (indicating no change). If one is null, return the other. Else return result1 && result2
            var result1 = RemovePossibles();
            var result2 = CheckForOnlyOnePossibility(language);
            return SudokuTile.CombineSolvedState(result1, result2);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}