namespace SudokuSolver
{
    partial class SolverDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolverDialog));
            this.richTextBoxLogging = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxLogging
            // 
            this.richTextBoxLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLogging.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLogging.Name = "richTextBoxLogging";
            this.richTextBoxLogging.ReadOnly = true;
            this.richTextBoxLogging.Size = new System.Drawing.Size(284, 261);
            this.richTextBoxLogging.TabIndex = 0;
            this.richTextBoxLogging.Text = "";
            // 
            // SolverDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.richTextBoxLogging);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SolverDialog";
            this.Text = "SolverDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLogging;
    }
}