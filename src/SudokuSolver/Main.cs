// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SudokuSolver;

/// <summary>
/// The main form.
/// </summary>
public partial class Main : Form
{
    /// <summary>
    /// The language manager.
    /// </summary>
    private readonly ILanguageManager languageManager = new LanguageManager();

    /// <summary>
    /// The language.
    /// </summary>
    private ILanguage? language;

    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
        this.InitializeComponent();
        this.InitializeLanguageManager();
        this.LoadLanguagesToCombo();
        this.LoadTitleAndDescription();
    }

    /// <summary>
    /// Sets a tile color.
    /// </summary>
    /// <param name="tile">The tile.</param>
    private static void SetTileColor(RichTextBox tile)
    {
        tile.ForeColor = string.IsNullOrWhiteSpace(tile.Text) ? Color.Black : Color.Red;
    }

    /// <summary>
    /// Counts the tile if its filled.
    /// </summary>
    /// <param name="tile">The tile.</param>
    /// <returns>The tile count as <see cref="int"/>.</returns>
    private static int CountIfTileFilled(RichTextBox tile)
    {
        if (tile == null)
        {
            throw new ArgumentNullException(nameof(tile));
        }

        return string.IsNullOrWhiteSpace(tile.Text) ? 0 : 1;
    }

    /// <summary>
    /// Gets the text if the tile is filled.
    /// </summary>
    /// <param name="tile">The tile.</param>
    /// <returns>The text as <see cref="string"/>.</returns>
    private static string GetTextIfTileFilled(RichTextBox tile)
    {
        if (tile == null)
        {
            throw new ArgumentNullException(nameof(tile));
        }

        return string.IsNullOrWhiteSpace(tile.Text) ? "." : tile.Text;
    }

    /// <summary>
    /// Resets the text if necessary.
    /// </summary>
    /// <param name="sender">The sender.</param>
    private static void ResetTextIfNecessary(object sender)
    {
        if (sender is RichTextBox senderTile && !Regex.IsMatch(senderTile.Text, "^[0-9]$"))
        {
            senderTile.Text = string.Empty;
        }
    }

    /// <summary>
    /// Initializes the language manager.
    /// </summary>
    private void InitializeLanguageManager()
    {
        this.languageManager.SetCurrentLanguage("de-DE");
        this.languageManager.OnLanguageChanged += this.OnLanguageChanged!;
    }

    /// <summary>
    /// Loads the languages to the combo box.
    /// </summary>
    private void LoadLanguagesToCombo()
    {
        foreach (var lang in this.languageManager.GetLanguages())
        {
            this.comboBoxLanguages.Items.Add(lang.Name);
        }

        this.comboBoxLanguages.SelectedIndex = 0;
    }

    /// <summary>
    /// Handles the language changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void OnLanguageChanged(object sender, EventArgs e)
    {
        this.language = this.languageManager.GetCurrentLanguage();
        this.labelSelectLanguage.Text = this.language.GetWord("SelectLanguage");
    }

    /// <summary>
    /// Loads the title and description.
    /// </summary>
    private void LoadTitleAndDescription()
    {
        if (this.language is null)
        {
            return;
        }

        this.Text = Application.ProductName + this.language.GetWord("Empty") + Application.ProductVersion;
    }

    /// <summary>
    /// Completes the solving process.
    /// </summary>
    /// <param name="board">The board.</param>
    private void CompleteSolve(SudokuBoard board)
    {
        var dialog = new SolverDialog();
        dialog.Show();

        if (this.language is null)
        {
            return;
        }

        dialog.WriteLine(this.language.GetWord("Rules") ?? string.Empty);
        var rules = board.OutputRulesToDialog();
        dialog.WriteLine(rules);
        dialog.WriteLine(this.language.GetWord("Board") ?? string.Empty);
        var outputSolution = board.OutputSolution();
        dialog.WriteLine(outputSolution);
        var solutions = board.Solve().ToList();
        dialog.WriteLine(this.language.GetWord("BaseBoardProgress") ?? string.Empty);
        dialog.WriteLine(outputSolution);
        dialog.WriteLine(this.language.GetWord("2Minus") ?? string.Empty);
        dialog.WriteLine(this.language.GetWord("2Minus") ?? string.Empty);
        dialog.WriteLine(string.Format(this.language.GetWord("AllSolutions") ?? string.Empty, solutions.Count));
        var i = 1;

        if (solutions.Count == 0)
        {
            MessageBox.Show(
                this.language.GetWord("NoSolutionText"),
                this.language.GetWord("NoSolutionCaption"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        var tiles = solutions[0].OutputTiles();
        this.FillTiles(tiles);

        foreach (var solution in solutions)
        {
            dialog.WriteLine(this.language.GetWord("16Minus") ?? string.Empty);
            dialog.WriteLine(string.Format(this.language.GetWord("SolutionOf") ?? string.Empty, i++, solutions.Count));
            dialog.WriteLine(solution.OutputSolution());
        }
    }

    /// <summary>
    /// Handles the solve button click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ButtonSolveClick(object sender, EventArgs e)
    {
        try
        {
            if (this.GetFieldCount() > 10)
            {
                this.ColorSetTiles();
                this.SolveSudoku();
            }
            else
            {
                if (this.language is null)
                {
                    return;
                }

                MessageBox.Show(
                    this.language.GetWord("FillMoreThan10Text"),
                    this.language.GetWord("FillMoreThan10Caption"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Sets the colors for the tiles.
    /// </summary>
    private void ColorSetTiles()
    {
        SetTileColor(this.tile1_1);
        SetTileColor(this.tile1_2);
        SetTileColor(this.tile1_3);
        SetTileColor(this.tile1_4);
        SetTileColor(this.tile1_5);
        SetTileColor(this.tile1_6);
        SetTileColor(this.tile1_7);
        SetTileColor(this.tile1_8);
        SetTileColor(this.tile1_9);
        SetTileColor(this.tile2_1);
        SetTileColor(this.tile2_2);
        SetTileColor(this.tile2_2);
        SetTileColor(this.tile2_4);
        SetTileColor(this.tile2_5);
        SetTileColor(this.tile2_6);
        SetTileColor(this.tile2_7);
        SetTileColor(this.tile2_8);
        SetTileColor(this.tile2_9);
        SetTileColor(this.tile3_1);
        SetTileColor(this.tile3_2);
        SetTileColor(this.tile3_3);
        SetTileColor(this.tile3_4);
        SetTileColor(this.tile3_5);
        SetTileColor(this.tile3_6);
        SetTileColor(this.tile3_7);
        SetTileColor(this.tile3_8);
        SetTileColor(this.tile3_9);
        SetTileColor(this.tile4_1);
        SetTileColor(this.tile4_2);
        SetTileColor(this.tile4_3);
        SetTileColor(this.tile4_4);
        SetTileColor(this.tile4_5);
        SetTileColor(this.tile4_6);
        SetTileColor(this.tile4_7);
        SetTileColor(this.tile4_8);
        SetTileColor(this.tile4_9);
        SetTileColor(this.tile5_1);
        SetTileColor(this.tile5_2);
        SetTileColor(this.tile5_3);
        SetTileColor(this.tile5_4);
        SetTileColor(this.tile5_5);
        SetTileColor(this.tile5_6);
        SetTileColor(this.tile5_7);
        SetTileColor(this.tile5_8);
        SetTileColor(this.tile5_9);
        SetTileColor(this.tile6_1);
        SetTileColor(this.tile6_2);
        SetTileColor(this.tile6_3);
        SetTileColor(this.tile6_4);
        SetTileColor(this.tile6_5);
        SetTileColor(this.tile6_6);
        SetTileColor(this.tile6_7);
        SetTileColor(this.tile6_8);
        SetTileColor(this.tile6_9);
        SetTileColor(this.tile7_1);
        SetTileColor(this.tile7_2);
        SetTileColor(this.tile7_3);
        SetTileColor(this.tile7_4);
        SetTileColor(this.tile7_5);
        SetTileColor(this.tile7_6);
        SetTileColor(this.tile7_7);
        SetTileColor(this.tile7_8);
        SetTileColor(this.tile7_9);
        SetTileColor(this.tile8_1);
        SetTileColor(this.tile8_2);
        SetTileColor(this.tile8_3);
        SetTileColor(this.tile8_4);
        SetTileColor(this.tile8_5);
        SetTileColor(this.tile8_6);
        SetTileColor(this.tile8_7);
        SetTileColor(this.tile8_8);
        SetTileColor(this.tile8_9);
        SetTileColor(this.tile9_1);
        SetTileColor(this.tile9_2);
        SetTileColor(this.tile9_3);
        SetTileColor(this.tile9_4);
        SetTileColor(this.tile9_5);
        SetTileColor(this.tile9_6);
        SetTileColor(this.tile9_7);
        SetTileColor(this.tile9_8);
        SetTileColor(this.tile9_9);
    }

    /// <summary>
    /// Resets the tile colors.
    /// </summary>
    private void ResetTileColors()
    {
        this.tile1_1.ForeColor = Color.Black;
        this.tile1_2.ForeColor = Color.Black;
        this.tile1_3.ForeColor = Color.Black;
        this.tile1_4.ForeColor = Color.Black;
        this.tile1_5.ForeColor = Color.Black;
        this.tile1_6.ForeColor = Color.Black;
        this.tile1_7.ForeColor = Color.Black;
        this.tile1_8.ForeColor = Color.Black;
        this.tile1_9.ForeColor = Color.Black;
        this.tile2_1.ForeColor = Color.Black;
        this.tile2_2.ForeColor = Color.Black;
        this.tile2_3.ForeColor = Color.Black;
        this.tile2_4.ForeColor = Color.Black;
        this.tile2_5.ForeColor = Color.Black;
        this.tile2_6.ForeColor = Color.Black;
        this.tile2_7.ForeColor = Color.Black;
        this.tile2_8.ForeColor = Color.Black;
        this.tile2_9.ForeColor = Color.Black;
        this.tile3_1.ForeColor = Color.Black;
        this.tile3_2.ForeColor = Color.Black;
        this.tile3_3.ForeColor = Color.Black;
        this.tile3_4.ForeColor = Color.Black;
        this.tile3_5.ForeColor = Color.Black;
        this.tile3_6.ForeColor = Color.Black;
        this.tile3_7.ForeColor = Color.Black;
        this.tile3_8.ForeColor = Color.Black;
        this.tile3_9.ForeColor = Color.Black;
        this.tile4_1.ForeColor = Color.Black;
        this.tile4_2.ForeColor = Color.Black;
        this.tile4_3.ForeColor = Color.Black;
        this.tile4_4.ForeColor = Color.Black;
        this.tile4_5.ForeColor = Color.Black;
        this.tile4_6.ForeColor = Color.Black;
        this.tile4_7.ForeColor = Color.Black;
        this.tile4_8.ForeColor = Color.Black;
        this.tile4_9.ForeColor = Color.Black;
        this.tile5_1.ForeColor = Color.Black;
        this.tile5_2.ForeColor = Color.Black;
        this.tile5_3.ForeColor = Color.Black;
        this.tile5_4.ForeColor = Color.Black;
        this.tile5_5.ForeColor = Color.Black;
        this.tile5_6.ForeColor = Color.Black;
        this.tile5_7.ForeColor = Color.Black;
        this.tile5_8.ForeColor = Color.Black;
        this.tile5_9.ForeColor = Color.Black;
        this.tile6_1.ForeColor = Color.Black;
        this.tile6_2.ForeColor = Color.Black;
        this.tile6_3.ForeColor = Color.Black;
        this.tile6_4.ForeColor = Color.Black;
        this.tile6_5.ForeColor = Color.Black;
        this.tile6_6.ForeColor = Color.Black;
        this.tile6_7.ForeColor = Color.Black;
        this.tile6_8.ForeColor = Color.Black;
        this.tile6_9.ForeColor = Color.Black;
        this.tile7_1.ForeColor = Color.Black;
        this.tile7_2.ForeColor = Color.Black;
        this.tile7_3.ForeColor = Color.Black;
        this.tile7_4.ForeColor = Color.Black;
        this.tile7_5.ForeColor = Color.Black;
        this.tile7_6.ForeColor = Color.Black;
        this.tile7_7.ForeColor = Color.Black;
        this.tile7_8.ForeColor = Color.Black;
        this.tile7_9.ForeColor = Color.Black;
        this.tile8_1.ForeColor = Color.Black;
        this.tile8_2.ForeColor = Color.Black;
        this.tile8_3.ForeColor = Color.Black;
        this.tile8_4.ForeColor = Color.Black;
        this.tile8_5.ForeColor = Color.Black;
        this.tile8_6.ForeColor = Color.Black;
        this.tile8_7.ForeColor = Color.Black;
        this.tile8_8.ForeColor = Color.Black;
        this.tile8_9.ForeColor = Color.Black;
        this.tile9_1.ForeColor = Color.Black;
        this.tile9_2.ForeColor = Color.Black;
        this.tile9_3.ForeColor = Color.Black;
        this.tile9_4.ForeColor = Color.Black;
        this.tile9_5.ForeColor = Color.Black;
        this.tile9_6.ForeColor = Color.Black;
        this.tile9_7.ForeColor = Color.Black;
        this.tile9_8.ForeColor = Color.Black;
        this.tile9_9.ForeColor = Color.Black;
    }

    /// <summary>
    /// Solves the sudoku.
    /// </summary>
    private void SolveSudoku()
    {
        if (this.language is null)
        {
            return;
        }

        var board = SudokuFactory.ClassicWith3X3Boxes(this.language);
        board.AddRow(this.GetRowForSolver(1));
        board.AddRow(this.GetRowForSolver(2));
        board.AddRow(this.GetRowForSolver(3));
        board.AddRow(this.GetRowForSolver(4));
        board.AddRow(this.GetRowForSolver(5));
        board.AddRow(this.GetRowForSolver(6));
        board.AddRow(this.GetRowForSolver(7));
        board.AddRow(this.GetRowForSolver(8));
        board.AddRow(this.GetRowForSolver(9));
        this.CompleteSolve(board);
    }

    /// <summary>
    /// Gets the row count.
    /// </summary>
    /// <param name="lineNumber">The line number.</param>
    /// <returns>The row count as <see cref="int"/>.</returns>
    private int GetRowCount(int lineNumber)
    {
        switch (lineNumber)
        {
            case 1:
                return CountIfTileFilled(this.tile1_1) + CountIfTileFilled(this.tile1_2) + CountIfTileFilled(this.tile1_3) + CountIfTileFilled(this.tile1_4) + CountIfTileFilled(this.tile1_5) + CountIfTileFilled(this.tile1_6) + CountIfTileFilled(this.tile1_7) + CountIfTileFilled(this.tile1_8) + CountIfTileFilled(this.tile1_9);
            case 2:
                return CountIfTileFilled(this.tile2_1) + CountIfTileFilled(this.tile2_2) + CountIfTileFilled(this.tile2_3) + CountIfTileFilled(this.tile2_4) + CountIfTileFilled(this.tile2_5) + CountIfTileFilled(this.tile2_6) + CountIfTileFilled(this.tile2_7) + CountIfTileFilled(this.tile2_8) + CountIfTileFilled(this.tile2_9);
            case 3:
                return CountIfTileFilled(this.tile3_1) + CountIfTileFilled(this.tile3_2) + CountIfTileFilled(this.tile3_3) + CountIfTileFilled(this.tile3_4) + CountIfTileFilled(this.tile3_5) + CountIfTileFilled(this.tile3_6) + CountIfTileFilled(this.tile3_7) + CountIfTileFilled(this.tile3_8) + CountIfTileFilled(this.tile3_9);
            case 4:
                return CountIfTileFilled(this.tile4_1) + CountIfTileFilled(this.tile4_2) + CountIfTileFilled(this.tile4_3) + CountIfTileFilled(this.tile4_4) + CountIfTileFilled(this.tile4_5) + CountIfTileFilled(this.tile4_6) + CountIfTileFilled(this.tile4_7) + CountIfTileFilled(this.tile4_8) + CountIfTileFilled(this.tile4_9);
            case 5:
                return CountIfTileFilled(this.tile5_1) + CountIfTileFilled(this.tile5_2) + CountIfTileFilled(this.tile5_3) + CountIfTileFilled(this.tile5_4) + CountIfTileFilled(this.tile5_5) + CountIfTileFilled(this.tile5_6) + CountIfTileFilled(this.tile5_7) + CountIfTileFilled(this.tile5_8) + CountIfTileFilled(this.tile5_9);
            case 6:
                return CountIfTileFilled(this.tile6_1) + CountIfTileFilled(this.tile6_2) + CountIfTileFilled(this.tile6_3) + CountIfTileFilled(this.tile6_4) + CountIfTileFilled(this.tile6_5) + CountIfTileFilled(this.tile6_6) + CountIfTileFilled(this.tile6_7) + CountIfTileFilled(this.tile6_8) + CountIfTileFilled(this.tile6_9);
            case 7:
                return CountIfTileFilled(this.tile7_1) + CountIfTileFilled(this.tile7_2) + CountIfTileFilled(this.tile7_3) + CountIfTileFilled(this.tile7_4) + CountIfTileFilled(this.tile7_5) + CountIfTileFilled(this.tile7_6) + CountIfTileFilled(this.tile7_7) + CountIfTileFilled(this.tile7_8) + CountIfTileFilled(this.tile7_9);
            case 8:
                return CountIfTileFilled(this.tile8_1) + CountIfTileFilled(this.tile8_2) + CountIfTileFilled(this.tile8_3) + CountIfTileFilled(this.tile8_4) + CountIfTileFilled(this.tile8_5) + CountIfTileFilled(this.tile8_6) + CountIfTileFilled(this.tile8_7) + CountIfTileFilled(this.tile8_8) + CountIfTileFilled(this.tile8_9);
            case 9:
                return CountIfTileFilled(this.tile9_1) + CountIfTileFilled(this.tile9_2) + CountIfTileFilled(this.tile9_3) + CountIfTileFilled(this.tile9_4) + CountIfTileFilled(this.tile9_5) + CountIfTileFilled(this.tile9_6) + CountIfTileFilled(this.tile9_7) + CountIfTileFilled(this.tile9_8) + CountIfTileFilled(this.tile9_9);
            default:
                return 0;
        }
    }

    /// <summary>
    /// Fills the tiles.
    /// </summary>
    /// <param name="tiles">The tiles.</param>
    private void FillTiles(SudokuTile[,] tiles)
    {
        for (var y = 0; y < tiles.GetLength(1); y++)
        {
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                this.FillTile(x, y, tiles);
            }
        }
    }

    /// <summary>
    /// Fills a tile.
    /// </summary>
    /// <param name="x">The x value.</param>
    /// <param name="y">The y value.</param>
    /// <param name="tiles">The titles.</param>
    private void FillTile(int x, int y, SudokuTile[,] tiles)
    {
        if (y == 0 && x == 0)
        {
            this.tile1_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 1)
        {
            this.tile1_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 2)
        {
            this.tile1_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 3)
        {
            this.tile1_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 4)
        {
            this.tile1_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 5)
        {
            this.tile1_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 6)
        {
            this.tile1_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 7)
        {
            this.tile1_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 0 && x == 8)
        {
            this.tile1_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 0)
        {
            this.tile2_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 1)
        {
            this.tile2_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 2)
        {
            this.tile2_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 3)
        {
            this.tile2_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 4)
        {
            this.tile2_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 5)
        {
            this.tile2_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 6)
        {
            this.tile2_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 7)
        {
            this.tile2_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 1 && x == 8)
        {
            this.tile2_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 0)
        {
            this.tile3_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 1)
        {
            this.tile3_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 2)
        {
            this.tile3_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 3)
        {
            this.tile3_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 4)
        {
            this.tile3_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 5)
        {
            this.tile3_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 6)
        {
            this.tile3_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 7)
        {
            this.tile3_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 2 && x == 8)
        {
            this.tile3_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 0)
        {
            this.tile4_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 1)
        {
            this.tile4_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 2)
        {
            this.tile4_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 3)
        {
            this.tile4_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 4)
        {
            this.tile4_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 5)
        {
            this.tile4_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 6)
        {
            this.tile4_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 7)
        {
            this.tile4_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 3 && x == 8)
        {
            this.tile4_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 0)
        {
            this.tile5_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 1)
        {
            this.tile5_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 2)
        {
            this.tile5_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 3)
        {
            this.tile5_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 4)
        {
            this.tile5_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 5)
        {
            this.tile5_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 6)
        {
            this.tile5_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 7)
        {
            this.tile5_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 4 && x == 8)
        {
            this.tile5_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 0)
        {
            this.tile6_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 1)
        {
            this.tile6_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 2)
        {
            this.tile6_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 3)
        {
            this.tile6_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 4)
        {
            this.tile6_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 5)
        {
            this.tile6_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 6)
        {
            this.tile6_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 7)
        {
            this.tile6_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 5 && x == 8)
        {
            this.tile6_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 0)
        {
            this.tile7_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 1)
        {
            this.tile7_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 2)
        {
            this.tile7_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 3)
        {
            this.tile7_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 4)
        {
            this.tile7_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 5)
        {
            this.tile7_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 6)
        {
            this.tile7_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 7)
        {
            this.tile7_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 6 && x == 8)
        {
            this.tile7_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 0)
        {
            this.tile8_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 1)
        {
            this.tile8_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 2)
        {
            this.tile8_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 3)
        {
            this.tile8_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 4)
        {
            this.tile8_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 5)
        {
            this.tile8_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 6)
        {
            this.tile8_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 7)
        {
            this.tile8_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 7 && x == 8)
        {
            this.tile8_9.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 0)
        {
            this.tile9_1.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 1)
        {
            this.tile9_2.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 2)
        {
            this.tile9_3.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 3)
        {
            this.tile9_4.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 4)
        {
            this.tile9_5.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 5)
        {
            this.tile9_6.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 6)
        {
            this.tile9_7.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 7)
        {
            this.tile9_8.Text = tiles[x, y].ToStringSimple();
        }

        if (y == 8 && x == 8)
        {
            this.tile9_9.Text = tiles[x, y].ToStringSimple();
        }
    }

    /// <summary>
    /// Resets the tiles.
    /// </summary>
    private void ResetTiles()
    {
        this.tile1_1.Text = string.Empty;
        this.tile1_2.Text = string.Empty;
        this.tile1_3.Text = string.Empty;
        this.tile1_4.Text = string.Empty;
        this.tile1_5.Text = string.Empty;
        this.tile1_6.Text = string.Empty;
        this.tile1_7.Text = string.Empty;
        this.tile1_8.Text = string.Empty;
        this.tile1_9.Text = string.Empty;
        this.tile2_1.Text = string.Empty;
        this.tile2_2.Text = string.Empty;
        this.tile2_3.Text = string.Empty;
        this.tile2_4.Text = string.Empty;
        this.tile2_5.Text = string.Empty;
        this.tile2_6.Text = string.Empty;
        this.tile2_7.Text = string.Empty;
        this.tile2_8.Text = string.Empty;
        this.tile2_9.Text = string.Empty;
        this.tile3_1.Text = string.Empty;
        this.tile3_2.Text = string.Empty;
        this.tile3_3.Text = string.Empty;
        this.tile3_4.Text = string.Empty;
        this.tile3_5.Text = string.Empty;
        this.tile3_6.Text = string.Empty;
        this.tile3_7.Text = string.Empty;
        this.tile3_8.Text = string.Empty;
        this.tile3_9.Text = string.Empty;
        this.tile4_1.Text = string.Empty;
        this.tile4_2.Text = string.Empty;
        this.tile4_3.Text = string.Empty;
        this.tile4_4.Text = string.Empty;
        this.tile4_5.Text = string.Empty;
        this.tile4_6.Text = string.Empty;
        this.tile4_7.Text = string.Empty;
        this.tile4_8.Text = string.Empty;
        this.tile4_9.Text = string.Empty;
        this.tile5_1.Text = string.Empty;
        this.tile5_2.Text = string.Empty;
        this.tile5_3.Text = string.Empty;
        this.tile5_4.Text = string.Empty;
        this.tile5_5.Text = string.Empty;
        this.tile5_6.Text = string.Empty;
        this.tile5_7.Text = string.Empty;
        this.tile5_8.Text = string.Empty;
        this.tile5_9.Text = string.Empty;
        this.tile6_1.Text = string.Empty;
        this.tile6_2.Text = string.Empty;
        this.tile6_3.Text = string.Empty;
        this.tile6_4.Text = string.Empty;
        this.tile6_5.Text = string.Empty;
        this.tile6_6.Text = string.Empty;
        this.tile6_7.Text = string.Empty;
        this.tile6_8.Text = string.Empty;
        this.tile6_9.Text = string.Empty;
        this.tile7_1.Text = string.Empty;
        this.tile7_2.Text = string.Empty;
        this.tile7_3.Text = string.Empty;
        this.tile7_4.Text = string.Empty;
        this.tile7_5.Text = string.Empty;
        this.tile7_6.Text = string.Empty;
        this.tile7_7.Text = string.Empty;
        this.tile7_8.Text = string.Empty;
        this.tile7_9.Text = string.Empty;
        this.tile8_1.Text = string.Empty;
        this.tile8_2.Text = string.Empty;
        this.tile8_3.Text = string.Empty;
        this.tile8_4.Text = string.Empty;
        this.tile8_5.Text = string.Empty;
        this.tile8_6.Text = string.Empty;
        this.tile8_7.Text = string.Empty;
        this.tile8_8.Text = string.Empty;
        this.tile8_9.Text = string.Empty;
        this.tile9_1.Text = string.Empty;
        this.tile9_2.Text = string.Empty;
        this.tile9_3.Text = string.Empty;
        this.tile9_4.Text = string.Empty;
        this.tile9_5.Text = string.Empty;
        this.tile9_6.Text = string.Empty;
        this.tile9_7.Text = string.Empty;
        this.tile9_8.Text = string.Empty;
        this.tile9_9.Text = string.Empty;
    }

    /// <summary>
    /// Gets the field count.
    /// </summary>
    /// <returns>The field count as <see cref="int"/>.</returns>
    private int GetFieldCount()
    {
        return this.GetRowCount(1) + this.GetRowCount(2) + this.GetRowCount(3) + this.GetRowCount(4) + this.GetRowCount(5) + this.GetRowCount(6) + this.GetRowCount(7) + this.GetRowCount(8) + this.GetRowCount(9);
    }

    /// <summary>
    /// Gets the row for the solver.
    /// </summary>
    /// <param name="lineNumber">The line number.</param>
    /// <returns>The row as <see cref="string"/>.</returns>
    private string GetRowForSolver(int lineNumber)
    {
        switch (lineNumber)
        {
            case 1:
                return GetTextIfTileFilled(this.tile1_1) + GetTextIfTileFilled(this.tile1_2) + GetTextIfTileFilled(this.tile1_3) + GetTextIfTileFilled(this.tile1_4) + GetTextIfTileFilled(this.tile1_5) + GetTextIfTileFilled(this.tile1_6) + GetTextIfTileFilled(this.tile1_7) + GetTextIfTileFilled(this.tile1_8) + GetTextIfTileFilled(this.tile1_9);
            case 2:
                return GetTextIfTileFilled(this.tile2_1) + GetTextIfTileFilled(this.tile2_2) + GetTextIfTileFilled(this.tile2_3) + GetTextIfTileFilled(this.tile2_4) + GetTextIfTileFilled(this.tile2_5) + GetTextIfTileFilled(this.tile2_6) + GetTextIfTileFilled(this.tile2_7) + GetTextIfTileFilled(this.tile2_8) + GetTextIfTileFilled(this.tile2_9);
            case 3:
                return GetTextIfTileFilled(this.tile3_1) + GetTextIfTileFilled(this.tile3_2) + GetTextIfTileFilled(this.tile3_3) + GetTextIfTileFilled(this.tile3_4) + GetTextIfTileFilled(this.tile3_5) + GetTextIfTileFilled(this.tile3_6) + GetTextIfTileFilled(this.tile3_7) + GetTextIfTileFilled(this.tile3_8) + GetTextIfTileFilled(this.tile3_9);
            case 4:
                return GetTextIfTileFilled(this.tile4_1) + GetTextIfTileFilled(this.tile4_2) + GetTextIfTileFilled(this.tile4_3) + GetTextIfTileFilled(this.tile4_4) + GetTextIfTileFilled(this.tile4_5) + GetTextIfTileFilled(this.tile4_6) + GetTextIfTileFilled(this.tile4_7) + GetTextIfTileFilled(this.tile4_8) + GetTextIfTileFilled(this.tile4_9);
            case 5:
                return GetTextIfTileFilled(this.tile5_1) + GetTextIfTileFilled(this.tile5_2) + GetTextIfTileFilled(this.tile5_3) + GetTextIfTileFilled(this.tile5_4) + GetTextIfTileFilled(this.tile5_5) + GetTextIfTileFilled(this.tile5_6) + GetTextIfTileFilled(this.tile5_7) + GetTextIfTileFilled(this.tile5_8) + GetTextIfTileFilled(this.tile5_9);
            case 6:
                return GetTextIfTileFilled(this.tile6_1) + GetTextIfTileFilled(this.tile6_2) + GetTextIfTileFilled(this.tile6_3) + GetTextIfTileFilled(this.tile6_4) + GetTextIfTileFilled(this.tile6_5) + GetTextIfTileFilled(this.tile6_6) + GetTextIfTileFilled(this.tile6_7) + GetTextIfTileFilled(this.tile6_8) + GetTextIfTileFilled(this.tile6_9);
            case 7:
                return GetTextIfTileFilled(this.tile7_1) + GetTextIfTileFilled(this.tile7_2) + GetTextIfTileFilled(this.tile7_3) + GetTextIfTileFilled(this.tile7_4) + GetTextIfTileFilled(this.tile7_5) + GetTextIfTileFilled(this.tile7_6) + GetTextIfTileFilled(this.tile7_7) + GetTextIfTileFilled(this.tile7_8) + GetTextIfTileFilled(this.tile7_9);
            case 8:
                return GetTextIfTileFilled(this.tile8_1) + GetTextIfTileFilled(this.tile8_2) + GetTextIfTileFilled(this.tile8_3) + GetTextIfTileFilled(this.tile8_4) + GetTextIfTileFilled(this.tile8_5) + GetTextIfTileFilled(this.tile8_6) + GetTextIfTileFilled(this.tile8_7) + GetTextIfTileFilled(this.tile8_8) + GetTextIfTileFilled(this.tile8_9);
            case 9:
                return GetTextIfTileFilled(this.tile9_1) + GetTextIfTileFilled(this.tile9_2) + GetTextIfTileFilled(this.tile9_3) + GetTextIfTileFilled(this.tile9_4) + GetTextIfTileFilled(this.tile9_5) + GetTextIfTileFilled(this.tile9_6) + GetTextIfTileFilled(this.tile9_7) + GetTextIfTileFilled(this.tile9_8) + GetTextIfTileFilled(this.tile9_9);
            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// Handles the tile 1, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 1, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile1_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 2, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile2_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 3, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile3_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 4, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile4_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 5, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile5_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 6, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile6_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 7, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile7_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 8, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile8_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 1 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_1_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 2 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_2_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 3 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_3_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 4 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_4_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 5 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_5_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 6 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_6_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 7 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_7_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 8 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_8_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the tile 9, 9 text changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Tile9_9_TextChanged(object sender, EventArgs e)
    {
        ResetTextIfNecessary(sender);
    }

    /// <summary>
    /// Handles the reset button click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ButtonResetClick(object sender, EventArgs e)
    {
        try
        {
            this.ResetTiles();
            this.ResetTileColors();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Handles the selected language changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ComboBoxLanguagesSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedItem = this.comboBoxLanguages.SelectedItem.ToString();

        if (string.IsNullOrWhiteSpace(selectedItem))
        {
            return;
        }

        this.languageManager.SetCurrentLanguageFromName(selectedItem);

        if (this.language is null)
        {
            return;
        }

        this.labelSelectLanguage.Text = this.language.GetWord("SelectLanguage");
    }
}
