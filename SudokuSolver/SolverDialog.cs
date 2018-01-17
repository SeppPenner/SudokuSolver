using System;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class SolverDialog : Form
    {
        public SolverDialog()
        {
            InitializeComponent();
            LoadTitleAndDescription();
        }

        private void LoadTitleAndDescription()
        {
            Text = Application.ProductName + @" " + Application.ProductVersion;
        }

        public void WriteLine(string text)
        {
            richTextBoxLogging.AppendText(text + Environment.NewLine);
        }
    }
}