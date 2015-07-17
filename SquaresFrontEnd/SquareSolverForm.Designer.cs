namespace Squares
{
    partial class SquareSolverForm
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
            this.btnSolveOnline = new System.Windows.Forms.Button();
            this.btnSolveLocal = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mtbPuzzleCount = new System.Windows.Forms.MaskedTextBox();
            this.labelSolve = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.CheckBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.cbInverseY = new System.Windows.Forms.CheckBox();
            this.cbInverseX = new System.Windows.Forms.CheckBox();
            this.cbTranspose = new System.Windows.Forms.CheckBox();
            this.labelResultsProcessed = new System.Windows.Forms.Label();
            this.labelSquareCount = new System.Windows.Forms.Label();
            this.openLocalFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSolveOnline
            // 
            this.btnSolveOnline.Location = new System.Drawing.Point(168, 12);
            this.btnSolveOnline.Name = "btnSolveOnline";
            this.btnSolveOnline.Size = new System.Drawing.Size(146, 23);
            this.btnSolveOnline.TabIndex = 0;
            this.btnSolveOnline.Text = "Solve Online Puzzles";
            this.btnSolveOnline.UseVisualStyleBackColor = true;
            this.btnSolveOnline.Click += new System.EventHandler(this.btnSolveOnline_Click);
            // 
            // btnSolveLocal
            // 
            this.btnSolveLocal.Location = new System.Drawing.Point(425, 12);
            this.btnSolveLocal.Name = "btnSolveLocal";
            this.btnSolveLocal.Size = new System.Drawing.Size(146, 23);
            this.btnSolveLocal.TabIndex = 1;
            this.btnSolveLocal.Text = "Re-Solve Local Puzzles";
            this.btnSolveLocal.UseVisualStyleBackColor = true;
            this.btnSolveLocal.Click += new System.EventHandler(this.btnSolveLocal_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 127);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(944, 795);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mtbPuzzleCount);
            this.panel1.Controls.Add(this.labelSolve);
            this.panel1.Controls.Add(this.cbMode);
            this.panel1.Controls.Add(this.lbLog);
            this.panel1.Controls.Add(this.cbInverseY);
            this.panel1.Controls.Add(this.cbInverseX);
            this.panel1.Controls.Add(this.cbTranspose);
            this.panel1.Controls.Add(this.labelResultsProcessed);
            this.panel1.Controls.Add(this.labelSquareCount);
            this.panel1.Controls.Add(this.btnSolveOnline);
            this.panel1.Controls.Add(this.btnSolveLocal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(944, 127);
            this.panel1.TabIndex = 3;
            // 
            // mtbPuzzleCount
            // 
            this.mtbPuzzleCount.Location = new System.Drawing.Point(52, 14);
            this.mtbPuzzleCount.Mask = "00000";
            this.mtbPuzzleCount.Name = "mtbPuzzleCount";
            this.mtbPuzzleCount.Size = new System.Drawing.Size(42, 20);
            this.mtbPuzzleCount.TabIndex = 13;
            this.mtbPuzzleCount.Text = "1";
            this.mtbPuzzleCount.ValidatingType = typeof(int);
            // 
            // labelSolve
            // 
            this.labelSolve.AutoSize = true;
            this.labelSolve.Location = new System.Drawing.Point(12, 17);
            this.labelSolve.Name = "labelSolve";
            this.labelSolve.Size = new System.Drawing.Size(34, 13);
            this.labelSolve.TabIndex = 12;
            this.labelSolve.Text = "Solve";
            // 
            // cbMode
            // 
            this.cbMode.AutoSize = true;
            this.cbMode.Location = new System.Drawing.Point(100, 16);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(62, 17);
            this.cbMode.TabIndex = 10;
            this.cbMode.Text = "Contest";
            this.cbMode.UseVisualStyleBackColor = true;
            // 
            // lbLog
            // 
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(0, 45);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(944, 82);
            this.lbLog.TabIndex = 9;
            // 
            // cbInverseY
            // 
            this.cbInverseY.AutoSize = true;
            this.cbInverseY.Location = new System.Drawing.Point(857, 16);
            this.cbInverseY.Name = "cbInverseY";
            this.cbInverseY.Size = new System.Drawing.Size(71, 17);
            this.cbInverseY.TabIndex = 7;
            this.cbInverseY.Text = "Inverse Y";
            this.cbInverseY.UseVisualStyleBackColor = true;
            // 
            // cbInverseX
            // 
            this.cbInverseX.AutoSize = true;
            this.cbInverseX.Location = new System.Drawing.Point(780, 16);
            this.cbInverseX.Name = "cbInverseX";
            this.cbInverseX.Size = new System.Drawing.Size(71, 17);
            this.cbInverseX.TabIndex = 6;
            this.cbInverseX.Text = "Inverse X";
            this.cbInverseX.UseVisualStyleBackColor = true;
            // 
            // cbTranspose
            // 
            this.cbTranspose.AutoSize = true;
            this.cbTranspose.Location = new System.Drawing.Point(698, 16);
            this.cbTranspose.Name = "cbTranspose";
            this.cbTranspose.Size = new System.Drawing.Size(76, 17);
            this.cbTranspose.TabIndex = 5;
            this.cbTranspose.Text = "Transpose";
            this.cbTranspose.UseVisualStyleBackColor = true;
            // 
            // labelResultsProcessed
            // 
            this.labelResultsProcessed.AutoSize = true;
            this.labelResultsProcessed.Location = new System.Drawing.Point(649, 17);
            this.labelResultsProcessed.Name = "labelResultsProcessed";
            this.labelResultsProcessed.Size = new System.Drawing.Size(13, 13);
            this.labelResultsProcessed.TabIndex = 4;
            this.labelResultsProcessed.Text = "1";
            // 
            // labelSquareCount
            // 
            this.labelSquareCount.AutoSize = true;
            this.labelSquareCount.Location = new System.Drawing.Point(599, 17);
            this.labelSquareCount.Name = "labelSquareCount";
            this.labelSquareCount.Size = new System.Drawing.Size(13, 13);
            this.labelSquareCount.TabIndex = 2;
            this.labelSquareCount.Text = "1";
            // 
            // openLocalFileDialog
            // 
            this.openLocalFileDialog.Filter = "Json Puzzles|*.json";
            this.openLocalFileDialog.InitialDirectory = ".";
            this.openLocalFileDialog.Multiselect = true;
            // 
            // SquareSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 922);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Name = "SquareSolverForm";
            this.Text = "Square Solver";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSolveOnline;
        private System.Windows.Forms.Button btnSolveLocal;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelSquareCount;
        private System.Windows.Forms.Label labelResultsProcessed;
        private System.Windows.Forms.CheckBox cbInverseY;
        private System.Windows.Forms.CheckBox cbInverseX;
        private System.Windows.Forms.CheckBox cbTranspose;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.MaskedTextBox mtbPuzzleCount;
        private System.Windows.Forms.Label labelSolve;
        private System.Windows.Forms.CheckBox cbMode;
        private System.Windows.Forms.OpenFileDialog openLocalFileDialog;
    }
}

