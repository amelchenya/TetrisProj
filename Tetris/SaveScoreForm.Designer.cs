namespace Tetris
{
    partial class SaveScoreForm
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
            this.congratulationLabel = new System.Windows.Forms.Label();
            this.playerNameTBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // congratulationLabel
            // 
            this.congratulationLabel.AutoSize = true;
            this.congratulationLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.congratulationLabel.Location = new System.Drawing.Point(13, 13);
            this.congratulationLabel.Name = "congratulationLabel";
            this.congratulationLabel.Size = new System.Drawing.Size(265, 38);
            this.congratulationLabel.TabIndex = 0;
            this.congratulationLabel.Text = "Congratulations! You set a new record!\r\nPlease, enter your name for save score:";
            // 
            // playerNameTBox
            // 
            this.playerNameTBox.Location = new System.Drawing.Point(17, 54);
            this.playerNameTBox.Name = "playerNameTBox";
            this.playerNameTBox.Size = new System.Drawing.Size(256, 20);
            this.playerNameTBox.TabIndex = 1;
            this.playerNameTBox.Text = "Player";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(17, 80);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(110, 35);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(145, 80);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(128, 35);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Cancel";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // SaveScoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 125);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.playerNameTBox);
            this.Controls.Add(this.congratulationLabel);
            this.Name = "SaveScoreForm";
            this.Text = "New Record!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label congratulationLabel;
        private System.Windows.Forms.TextBox playerNameTBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button exitButton;
    }
}