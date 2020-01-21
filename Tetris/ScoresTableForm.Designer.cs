namespace Tetris
{
    partial class ScoresTableForm
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
            this.ScoresTableDGView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ScoresTableDGView)).BeginInit();
            this.SuspendLayout();
            // 
            // ScoresTableDGView
            // 
            this.ScoresTableDGView.AllowUserToAddRows = false;
            this.ScoresTableDGView.AllowUserToDeleteRows = false;
            this.ScoresTableDGView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ScoresTableDGView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ScoresTableDGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScoresTableDGView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScoresTableDGView.Location = new System.Drawing.Point(0, 0);
            this.ScoresTableDGView.MultiSelect = false;
            this.ScoresTableDGView.Name = "ScoresTableDGView";
            this.ScoresTableDGView.ReadOnly = true;
            this.ScoresTableDGView.RowHeadersVisible = false;
            this.ScoresTableDGView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ScoresTableDGView.Size = new System.Drawing.Size(296, 272);
            this.ScoresTableDGView.TabIndex = 0;
            // 
            // ScoresTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 272);
            this.Controls.Add(this.ScoresTableDGView);
            this.Name = "ScoresTableForm";
            this.Text = "Scores Table";
            ((System.ComponentModel.ISupportInitialize)(this.ScoresTableDGView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ScoresTableDGView;
    }
}