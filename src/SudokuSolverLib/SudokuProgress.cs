// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SudokuProgress.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The sudoku progress enumeration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolverLib
{
    /// <summary>
    /// The sudoku progress enumeration.
    /// </summary>
    public enum SudokuProgress
    {
        /// <summary>
        /// The failed sudoku progress.
        /// </summary>
        Failed,

        /// <summary>
        /// The no progress sudoku progress.
        /// </summary>
        NoProgress,

        /// <summary>
        /// The progress sudoku progress.
        /// </summary>
        Progress
    }
}