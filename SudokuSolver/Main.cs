using System;
using System.Linq;
using System.Windows.Forms;
using SudokuSolverLib;

namespace SudokuSolver
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private static void SolveClassic()
        {
            var board = SudokuFactory.ClassicWith3X3Boxes();
            board.AddRow("...84...9");
            board.AddRow("..1.....5");
            board.AddRow("8...2146.");
            board.AddRow("7.8....9.");
            board.AddRow(".........");
            board.AddRow(".5....3.1");
            board.AddRow(".2491...7");
            board.AddRow("9.....5..");
            board.AddRow("3...84...");
            CompleteSolve(board);
        }

        private static void CompleteSolve(SudokuBoard board)
        {
            Console.WriteLine("Rules:");
            board.OutputRules();
            Console.WriteLine("Board:");
            board.Output();
            var solutions = board.Solve().ToList();
            Console.WriteLine("Base Board Progress:");
            board.Output();
            Console.WriteLine("--");
            Console.WriteLine("--");
            Console.WriteLine("All " + solutions.Count + " solutions:");
            var i = 1;
            foreach (var solution in solutions)
            {
                Console.WriteLine("----------------");
                Console.WriteLine("Solution " + i++ + " / " + solutions.Count + ":");
                solution.Output();
            }
        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {

        }
    }
}