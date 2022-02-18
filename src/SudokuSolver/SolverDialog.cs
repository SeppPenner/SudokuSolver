// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SolverDialog.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The solver dialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolver;

/// <summary>
/// The solver dialog.
/// </summary>
public partial class SolverDialog : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SolverDialog"/> class.
    /// </summary>
    public SolverDialog()
    {
        this.InitializeComponent();
        this.LoadTitleAndDescription();
    }

    /// <summary>
    /// Writes a line.
    /// </summary>
    /// <param name="text">The text.</param>
    public void WriteLine(string text)
    {
        this.richTextBoxLogging.AppendText(text + Environment.NewLine);
    }

    /// <summary>
    /// Loads the title and description.
    /// </summary>
    private void LoadTitleAndDescription()
    {
        this.Text = Application.ProductName + @" " + Application.ProductVersion;
    }
}
