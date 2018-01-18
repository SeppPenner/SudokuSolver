using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SudokuSolverLib;

namespace SudokuSolver
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            LoadTitleAndDescription();
        }

        private void LoadTitleAndDescription()
        {
            Text = Application.ProductName + @" " + Application.ProductVersion;
        }

        private void CompleteSolve(SudokuBoard board)
        {
            var dialog = new SolverDialog();
            dialog.Show();
            dialog.WriteLine("Rules:");
            var rules = board.OutputRulesToDialog();
            dialog.WriteLine(rules);
            dialog.WriteLine("Board:");
            var outputSolution = board.OutputSolution();
            dialog.WriteLine(outputSolution);
            var solutions = board.Solve().ToList();
            dialog.WriteLine("Base Board Progress:");
            dialog.WriteLine(outputSolution);
            dialog.WriteLine("--");
            dialog.WriteLine("--");
            dialog.WriteLine("All " + solutions.Count + " solutions:");
            var i = 1;
            if (solutions.Count == 0)
                MessageBox.Show(@"No solution was found!", @"No solution found", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            var tiles = solutions[0].OutputTiles();
            FillTiles(tiles);
            foreach (var solution in solutions)
            {
                dialog.WriteLine("----------------");
                dialog.WriteLine("Solution " + i++ + " / " + solutions.Count + ":");
                dialog.WriteLine(solution.OutputSolution());
            }
        }

        private void ButtonSolve_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetFieldCount() > 10)
                {
                    ColorSetTiles();
                    SolveSudoku();
                }
                else
                {
                    MessageBox.Show(@"Please fill more than 10 tiles!", @"Fill more than 10 tiles",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ColorSetTiles()
        {
            SetTileColor(tile1_1);
            SetTileColor(tile1_2);
            SetTileColor(tile1_3);
            SetTileColor(tile1_4);
            SetTileColor(tile1_5);
            SetTileColor(tile1_6);
            SetTileColor(tile1_7);
            SetTileColor(tile1_8);
            SetTileColor(tile1_9);
            SetTileColor(tile2_1);
            SetTileColor(tile2_2);
            SetTileColor(tile2_2);
            SetTileColor(tile2_4);
            SetTileColor(tile2_5);
            SetTileColor(tile2_6);
            SetTileColor(tile2_7);
            SetTileColor(tile2_8);
            SetTileColor(tile2_9);
            SetTileColor(tile3_1);
            SetTileColor(tile3_2);
            SetTileColor(tile3_3);
            SetTileColor(tile3_4);
            SetTileColor(tile3_5);
            SetTileColor(tile3_6);
            SetTileColor(tile3_7);
            SetTileColor(tile3_8);
            SetTileColor(tile3_9);
            SetTileColor(tile4_1);
            SetTileColor(tile4_2);
            SetTileColor(tile4_3);
            SetTileColor(tile4_4);
            SetTileColor(tile4_5);
            SetTileColor(tile4_6);
            SetTileColor(tile4_7);
            SetTileColor(tile4_8);
            SetTileColor(tile4_9);
            SetTileColor(tile5_1);
            SetTileColor(tile5_2);
            SetTileColor(tile5_3);
            SetTileColor(tile5_4);
            SetTileColor(tile5_5);
            SetTileColor(tile5_6);
            SetTileColor(tile5_7);
            SetTileColor(tile5_8);
            SetTileColor(tile5_9);
            SetTileColor(tile6_1);
            SetTileColor(tile6_2);
            SetTileColor(tile6_3);
            SetTileColor(tile6_4);
            SetTileColor(tile6_5);
            SetTileColor(tile6_6);
            SetTileColor(tile6_7);
            SetTileColor(tile6_8);
            SetTileColor(tile6_9);
            SetTileColor(tile7_1);
            SetTileColor(tile7_2);
            SetTileColor(tile7_3);
            SetTileColor(tile7_4);
            SetTileColor(tile7_5);
            SetTileColor(tile7_6);
            SetTileColor(tile7_7);
            SetTileColor(tile7_8);
            SetTileColor(tile7_9);
            SetTileColor(tile8_1);
            SetTileColor(tile8_2);
            SetTileColor(tile8_3);
            SetTileColor(tile8_4);
            SetTileColor(tile8_5);
            SetTileColor(tile8_6);
            SetTileColor(tile8_7);
            SetTileColor(tile8_8);
            SetTileColor(tile8_9);
            SetTileColor(tile9_1);
            SetTileColor(tile9_2);
            SetTileColor(tile9_3);
            SetTileColor(tile9_4);
            SetTileColor(tile9_5);
            SetTileColor(tile9_6);
            SetTileColor(tile9_7);
            SetTileColor(tile9_8);
            SetTileColor(tile9_9);
        }

        private void SetTileColor(RichTextBox tile)
        {
            tile.ForeColor = string.IsNullOrWhiteSpace(tile.Text) ? Color.Black : Color.Red;
        }

        private void ResetTileColors()
        {
            tile1_1.ForeColor = Color.Black;
            tile1_2.ForeColor = Color.Black;
            tile1_3.ForeColor = Color.Black;
            tile1_4.ForeColor = Color.Black;
            tile1_5.ForeColor = Color.Black;
            tile1_6.ForeColor = Color.Black;
            tile1_7.ForeColor = Color.Black;
            tile1_8.ForeColor = Color.Black;
            tile1_9.ForeColor = Color.Black;
            tile2_1.ForeColor = Color.Black;
            tile2_2.ForeColor = Color.Black;
            tile2_3.ForeColor = Color.Black;
            tile2_4.ForeColor = Color.Black;
            tile2_5.ForeColor = Color.Black;
            tile2_6.ForeColor = Color.Black;
            tile2_7.ForeColor = Color.Black;
            tile2_8.ForeColor = Color.Black;
            tile2_9.ForeColor = Color.Black;
            tile3_1.ForeColor = Color.Black;
            tile3_2.ForeColor = Color.Black;
            tile3_3.ForeColor = Color.Black;
            tile3_4.ForeColor = Color.Black;
            tile3_5.ForeColor = Color.Black;
            tile3_6.ForeColor = Color.Black;
            tile3_7.ForeColor = Color.Black;
            tile3_8.ForeColor = Color.Black;
            tile3_9.ForeColor = Color.Black;
            tile4_1.ForeColor = Color.Black;
            tile4_2.ForeColor = Color.Black;
            tile4_3.ForeColor = Color.Black;
            tile4_4.ForeColor = Color.Black;
            tile4_5.ForeColor = Color.Black;
            tile4_6.ForeColor = Color.Black;
            tile4_7.ForeColor = Color.Black;
            tile4_8.ForeColor = Color.Black;
            tile4_9.ForeColor = Color.Black;
            tile5_1.ForeColor = Color.Black;
            tile5_2.ForeColor = Color.Black;
            tile5_3.ForeColor = Color.Black;
            tile5_4.ForeColor = Color.Black;
            tile5_5.ForeColor = Color.Black;
            tile5_6.ForeColor = Color.Black;
            tile5_7.ForeColor = Color.Black;
            tile5_8.ForeColor = Color.Black;
            tile5_9.ForeColor = Color.Black;
            tile6_1.ForeColor = Color.Black;
            tile6_2.ForeColor = Color.Black;
            tile6_3.ForeColor = Color.Black;
            tile6_4.ForeColor = Color.Black;
            tile6_5.ForeColor = Color.Black;
            tile6_6.ForeColor = Color.Black;
            tile6_7.ForeColor = Color.Black;
            tile6_8.ForeColor = Color.Black;
            tile6_9.ForeColor = Color.Black;
            tile7_1.ForeColor = Color.Black;
            tile7_2.ForeColor = Color.Black;
            tile7_3.ForeColor = Color.Black;
            tile7_4.ForeColor = Color.Black;
            tile7_5.ForeColor = Color.Black;
            tile7_6.ForeColor = Color.Black;
            tile7_7.ForeColor = Color.Black;
            tile7_8.ForeColor = Color.Black;
            tile7_9.ForeColor = Color.Black;
            tile8_1.ForeColor = Color.Black;
            tile8_2.ForeColor = Color.Black;
            tile8_3.ForeColor = Color.Black;
            tile8_4.ForeColor = Color.Black;
            tile8_5.ForeColor = Color.Black;
            tile8_6.ForeColor = Color.Black;
            tile8_7.ForeColor = Color.Black;
            tile8_8.ForeColor = Color.Black;
            tile8_9.ForeColor = Color.Black;
            tile9_1.ForeColor = Color.Black;
            tile9_2.ForeColor = Color.Black;
            tile9_3.ForeColor = Color.Black;
            tile9_4.ForeColor = Color.Black;
            tile9_5.ForeColor = Color.Black;
            tile9_6.ForeColor = Color.Black;
            tile9_7.ForeColor = Color.Black;
            tile9_8.ForeColor = Color.Black;
            tile9_9.ForeColor = Color.Black;
        }

        private void SolveSudoku()
        {
            var board = SudokuFactory.ClassicWith3X3Boxes();
            board.AddRow(GetRowForSolver(1));
            board.AddRow(GetRowForSolver(2));
            board.AddRow(GetRowForSolver(3));
            board.AddRow(GetRowForSolver(4));
            board.AddRow(GetRowForSolver(5));
            board.AddRow(GetRowForSolver(6));
            board.AddRow(GetRowForSolver(7));
            board.AddRow(GetRowForSolver(8));
            board.AddRow(GetRowForSolver(9));
            CompleteSolve(board);
        }

        private int GetRowCount(int lineNumber)
        {
            switch (lineNumber)
            {
                case 1:
                    return CountIfTileFilled(tile1_1) + CountIfTileFilled(tile1_2) + CountIfTileFilled(tile1_3) +
                           CountIfTileFilled(tile1_4) + CountIfTileFilled(tile1_5) + CountIfTileFilled(tile1_6) +
                           CountIfTileFilled(tile1_7) + CountIfTileFilled(tile1_8) + CountIfTileFilled(tile1_9);
                case 2:
                    return CountIfTileFilled(tile2_1) + CountIfTileFilled(tile2_2) + CountIfTileFilled(tile2_3) +
                           CountIfTileFilled(tile2_4) + CountIfTileFilled(tile2_5) + CountIfTileFilled(tile2_6) +
                           CountIfTileFilled(tile2_7) + CountIfTileFilled(tile2_8) + CountIfTileFilled(tile2_9);
                case 3:
                    return CountIfTileFilled(tile3_1) + CountIfTileFilled(tile3_2) + CountIfTileFilled(tile3_3) +
                           CountIfTileFilled(tile3_4) + CountIfTileFilled(tile3_5) + CountIfTileFilled(tile3_6) +
                           CountIfTileFilled(tile3_7) + CountIfTileFilled(tile3_8) + CountIfTileFilled(tile3_9);
                case 4:
                    return CountIfTileFilled(tile4_1) + CountIfTileFilled(tile4_2) + CountIfTileFilled(tile4_3) +
                           CountIfTileFilled(tile4_4) + CountIfTileFilled(tile4_5) + CountIfTileFilled(tile4_6) +
                           CountIfTileFilled(tile4_7) + CountIfTileFilled(tile4_8) + CountIfTileFilled(tile4_9);
                case 5:
                    return CountIfTileFilled(tile5_1) + CountIfTileFilled(tile5_2) + CountIfTileFilled(tile5_3) +
                           CountIfTileFilled(tile5_4) + CountIfTileFilled(tile5_5) + CountIfTileFilled(tile5_6) +
                           CountIfTileFilled(tile5_7) + CountIfTileFilled(tile5_8) + CountIfTileFilled(tile5_9);
                case 6:
                    return CountIfTileFilled(tile6_1) + CountIfTileFilled(tile6_2) + CountIfTileFilled(tile6_3) +
                           CountIfTileFilled(tile6_4) + CountIfTileFilled(tile6_5) + CountIfTileFilled(tile6_6) +
                           CountIfTileFilled(tile6_7) + CountIfTileFilled(tile6_8) + CountIfTileFilled(tile6_9);
                case 7:
                    return CountIfTileFilled(tile7_1) + CountIfTileFilled(tile7_2) + CountIfTileFilled(tile7_3) +
                           CountIfTileFilled(tile7_4) + CountIfTileFilled(tile7_5) + CountIfTileFilled(tile7_6) +
                           CountIfTileFilled(tile7_7) + CountIfTileFilled(tile7_8) + CountIfTileFilled(tile7_9);
                case 8:
                    return CountIfTileFilled(tile8_1) + CountIfTileFilled(tile8_2) + CountIfTileFilled(tile8_3) +
                           CountIfTileFilled(tile8_4) + CountIfTileFilled(tile8_5) + CountIfTileFilled(tile8_6) +
                           CountIfTileFilled(tile8_7) + CountIfTileFilled(tile8_8) + CountIfTileFilled(tile8_9);
                case 9:
                    return CountIfTileFilled(tile9_1) + CountIfTileFilled(tile9_2) + CountIfTileFilled(tile9_3) +
                           CountIfTileFilled(tile9_4) + CountIfTileFilled(tile9_5) + CountIfTileFilled(tile9_6) +
                           CountIfTileFilled(tile9_7) + CountIfTileFilled(tile9_8) + CountIfTileFilled(tile9_9);
                default:
                    return 0;
            }
        }

        private void FillTiles(SudokuTile[,] tiles)
        {
            for (var y = 0; y < tiles.GetLength(1); y++)
            for (var x = 0; x < tiles.GetLength(0); x++)
                FillTile(x, y, tiles);
        }

        private void FillTile(int x, int y, SudokuTile[,] tiles)
        {
            if (y == 0 && x == 0)
                tile1_1.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 1)
                tile1_2.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 2)
                tile1_3.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 3)
                tile1_4.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 4)
                tile1_5.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 5)
                tile1_6.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 6)
                tile1_7.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 7)
                tile1_8.Text = tiles[x, y].ToStringSimple();
            if (y == 0 && x == 8)
                tile1_9.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 0)
                tile2_1.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 1)
                tile2_2.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 2)
                tile2_3.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 3)
                tile2_4.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 4)
                tile2_5.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 5)
                tile2_6.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 6)
                tile2_7.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 7)
                tile2_8.Text = tiles[x, y].ToStringSimple();
            if (y == 1 && x == 8)
                tile2_9.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 0)
                tile3_1.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 1)
                tile3_2.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 2)
                tile3_3.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 3)
                tile3_4.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 4)
                tile3_5.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 5)
                tile3_6.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 6)
                tile3_7.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 7)
                tile3_8.Text = tiles[x, y].ToStringSimple();
            if (y == 2 && x == 8)
                tile3_9.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 0)
                tile4_1.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 1)
                tile4_2.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 2)
                tile4_3.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 3)
                tile4_4.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 4)
                tile4_5.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 5)
                tile4_6.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 6)
                tile4_7.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 7)
                tile4_8.Text = tiles[x, y].ToStringSimple();
            if (y == 3 && x == 8)
                tile4_9.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 0)
                tile5_1.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 1)
                tile5_2.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 2)
                tile5_3.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 3)
                tile5_4.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 4)
                tile5_5.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 5)
                tile5_6.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 6)
                tile5_7.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 7)
                tile5_8.Text = tiles[x, y].ToStringSimple();
            if (y == 4 && x == 8)
                tile5_9.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 0)
                tile6_1.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 1)
                tile6_2.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 2)
                tile6_3.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 3)
                tile6_4.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 4)
                tile6_5.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 5)
                tile6_6.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 6)
                tile6_7.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 7)
                tile6_8.Text = tiles[x, y].ToStringSimple();
            if (y == 5 && x == 8)
                tile6_9.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 0)
                tile7_1.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 1)
                tile7_2.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 2)
                tile7_3.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 3)
                tile7_4.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 4)
                tile7_5.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 5)
                tile7_6.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 6)
                tile7_7.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 7)
                tile7_8.Text = tiles[x, y].ToStringSimple();
            if (y == 6 && x == 8)
                tile7_9.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 0)
                tile8_1.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 1)
                tile8_2.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 2)
                tile8_3.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 3)
                tile8_4.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 4)
                tile8_5.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 5)
                tile8_6.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 6)
                tile8_7.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 7)
                tile8_8.Text = tiles[x, y].ToStringSimple();
            if (y == 7 && x == 8)
                tile8_9.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 0)
                tile9_1.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 1)
                tile9_2.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 2)
                tile9_3.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 3)
                tile9_4.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 4)
                tile9_5.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 5)
                tile9_6.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 6)
                tile9_7.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 7)
                tile9_8.Text = tiles[x, y].ToStringSimple();
            if (y == 8 && x == 8)
                tile9_9.Text = tiles[x, y].ToStringSimple();
        }

        private void ResetTiles()
        {
            tile1_1.Text = string.Empty;
            tile1_2.Text = string.Empty;
            tile1_3.Text = string.Empty;
            tile1_4.Text = string.Empty;
            tile1_5.Text = string.Empty;
            tile1_6.Text = string.Empty;
            tile1_7.Text = string.Empty;
            tile1_8.Text = string.Empty;
            tile1_9.Text = string.Empty;
            tile2_1.Text = string.Empty;
            tile2_2.Text = string.Empty;
            tile2_3.Text = string.Empty;
            tile2_4.Text = string.Empty;
            tile2_5.Text = string.Empty;
            tile2_6.Text = string.Empty;
            tile2_7.Text = string.Empty;
            tile2_8.Text = string.Empty;
            tile2_9.Text = string.Empty;
            tile3_1.Text = string.Empty;
            tile3_2.Text = string.Empty;
            tile3_3.Text = string.Empty;
            tile3_4.Text = string.Empty;
            tile3_5.Text = string.Empty;
            tile3_6.Text = string.Empty;
            tile3_7.Text = string.Empty;
            tile3_8.Text = string.Empty;
            tile3_9.Text = string.Empty;
            tile4_1.Text = string.Empty;
            tile4_2.Text = string.Empty;
            tile4_3.Text = string.Empty;
            tile4_4.Text = string.Empty;
            tile4_5.Text = string.Empty;
            tile4_6.Text = string.Empty;
            tile4_7.Text = string.Empty;
            tile4_8.Text = string.Empty;
            tile4_9.Text = string.Empty;
            tile5_1.Text = string.Empty;
            tile5_2.Text = string.Empty;
            tile5_3.Text = string.Empty;
            tile5_4.Text = string.Empty;
            tile5_5.Text = string.Empty;
            tile5_6.Text = string.Empty;
            tile5_7.Text = string.Empty;
            tile5_8.Text = string.Empty;
            tile5_9.Text = string.Empty;
            tile6_1.Text = string.Empty;
            tile6_2.Text = string.Empty;
            tile6_3.Text = string.Empty;
            tile6_4.Text = string.Empty;
            tile6_5.Text = string.Empty;
            tile6_6.Text = string.Empty;
            tile6_7.Text = string.Empty;
            tile6_8.Text = string.Empty;
            tile6_9.Text = string.Empty;
            tile7_1.Text = string.Empty;
            tile7_2.Text = string.Empty;
            tile7_3.Text = string.Empty;
            tile7_4.Text = string.Empty;
            tile7_5.Text = string.Empty;
            tile7_6.Text = string.Empty;
            tile7_7.Text = string.Empty;
            tile7_8.Text = string.Empty;
            tile7_9.Text = string.Empty;
            tile8_1.Text = string.Empty;
            tile8_2.Text = string.Empty;
            tile8_3.Text = string.Empty;
            tile8_4.Text = string.Empty;
            tile8_5.Text = string.Empty;
            tile8_6.Text = string.Empty;
            tile8_7.Text = string.Empty;
            tile8_8.Text = string.Empty;
            tile8_9.Text = string.Empty;
            tile9_1.Text = string.Empty;
            tile9_2.Text = string.Empty;
            tile9_3.Text = string.Empty;
            tile9_4.Text = string.Empty;
            tile9_5.Text = string.Empty;
            tile9_6.Text = string.Empty;
            tile9_7.Text = string.Empty;
            tile9_8.Text = string.Empty;
            tile9_9.Text = string.Empty;
        }

        private int GetFieldCount()
        {
            return GetRowCount(1) + GetRowCount(2) + GetRowCount(3) + GetRowCount(4) + GetRowCount(5) +
                   GetRowCount(6) + GetRowCount(7) + GetRowCount(8) + GetRowCount(9);
        }

        private int CountIfTileFilled(RichTextBox tile)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile));
            return string.IsNullOrWhiteSpace(tile.Text) ? 0 : 1;
        }

        private string GetRowForSolver(int lineNumber)
        {
            switch (lineNumber)
            {
                case 1:
                    return GetTextIfTileFilled(tile1_1) + GetTextIfTileFilled(tile1_2) + GetTextIfTileFilled(tile1_3) +
                           GetTextIfTileFilled(tile1_4) + GetTextIfTileFilled(tile1_5) + GetTextIfTileFilled(tile1_6) +
                           GetTextIfTileFilled(tile1_7) + GetTextIfTileFilled(tile1_8) + GetTextIfTileFilled(tile1_9);
                case 2:
                    return GetTextIfTileFilled(tile2_1) + GetTextIfTileFilled(tile2_2) + GetTextIfTileFilled(tile2_3) +
                           GetTextIfTileFilled(tile2_4) + GetTextIfTileFilled(tile2_5) + GetTextIfTileFilled(tile2_6) +
                           GetTextIfTileFilled(tile2_7) + GetTextIfTileFilled(tile2_8) + GetTextIfTileFilled(tile2_9);
                case 3:
                    return GetTextIfTileFilled(tile3_1) + GetTextIfTileFilled(tile3_2) + GetTextIfTileFilled(tile3_3) +
                           GetTextIfTileFilled(tile3_4) + GetTextIfTileFilled(tile3_5) + GetTextIfTileFilled(tile3_6) +
                           GetTextIfTileFilled(tile3_7) + GetTextIfTileFilled(tile3_8) + GetTextIfTileFilled(tile3_9);
                case 4:
                    return GetTextIfTileFilled(tile4_1) + GetTextIfTileFilled(tile4_2) + GetTextIfTileFilled(tile4_3) +
                           GetTextIfTileFilled(tile4_4) + GetTextIfTileFilled(tile4_5) + GetTextIfTileFilled(tile4_6) +
                           GetTextIfTileFilled(tile4_7) + GetTextIfTileFilled(tile4_8) + GetTextIfTileFilled(tile4_9);
                case 5:
                    return GetTextIfTileFilled(tile5_1) + GetTextIfTileFilled(tile5_2) + GetTextIfTileFilled(tile5_3) +
                           GetTextIfTileFilled(tile5_4) + GetTextIfTileFilled(tile5_5) + GetTextIfTileFilled(tile5_6) +
                           GetTextIfTileFilled(tile5_7) + GetTextIfTileFilled(tile5_8) + GetTextIfTileFilled(tile5_9);
                case 6:
                    return GetTextIfTileFilled(tile6_1) + GetTextIfTileFilled(tile6_2) + GetTextIfTileFilled(tile6_3) +
                           GetTextIfTileFilled(tile6_4) + GetTextIfTileFilled(tile6_5) + GetTextIfTileFilled(tile6_6) +
                           GetTextIfTileFilled(tile6_7) + GetTextIfTileFilled(tile6_8) + GetTextIfTileFilled(tile6_9);
                case 7:
                    return GetTextIfTileFilled(tile7_1) + GetTextIfTileFilled(tile7_2) + GetTextIfTileFilled(tile7_3) +
                           GetTextIfTileFilled(tile7_4) + GetTextIfTileFilled(tile7_5) + GetTextIfTileFilled(tile7_6) +
                           GetTextIfTileFilled(tile7_7) + GetTextIfTileFilled(tile7_8) + GetTextIfTileFilled(tile7_9);
                case 8:
                    return GetTextIfTileFilled(tile8_1) + GetTextIfTileFilled(tile8_2) + GetTextIfTileFilled(tile8_3) +
                           GetTextIfTileFilled(tile8_4) + GetTextIfTileFilled(tile8_5) + GetTextIfTileFilled(tile8_6) +
                           GetTextIfTileFilled(tile8_7) + GetTextIfTileFilled(tile8_8) + GetTextIfTileFilled(tile8_9);
                case 9:
                    return GetTextIfTileFilled(tile9_1) + GetTextIfTileFilled(tile9_2) + GetTextIfTileFilled(tile9_3) +
                           GetTextIfTileFilled(tile9_4) + GetTextIfTileFilled(tile9_5) + GetTextIfTileFilled(tile9_6) +
                           GetTextIfTileFilled(tile9_7) + GetTextIfTileFilled(tile9_8) + GetTextIfTileFilled(tile9_9);
                default:
                    return string.Empty;
            }
        }


        private string GetTextIfTileFilled(RichTextBox tile)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile));
            return string.IsNullOrWhiteSpace(tile.Text) ? "." : tile.Text;
        }

        private void ResetTextIfNecessary(object sender)
        {
            if (sender is RichTextBox senderTile &&
                !Regex.IsMatch(senderTile.Text, "^[0-9]$"))
                senderTile.Text = string.Empty;
        }

        private void Tile1_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile1_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile2_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile3_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile4_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile5_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile6_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile7_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile8_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_1_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_2_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_3_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_4_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_5_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_6_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_7_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_8_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void Tile9_9_TextChanged(object sender, EventArgs e)
        {
            ResetTextIfNecessary(sender);
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetTiles();
                ResetTileColors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}