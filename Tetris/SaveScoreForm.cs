using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class SaveScoreForm : Form
    {
        public SaveScoreForm()
        {
            InitializeComponent();

            playerNameTBox.GotFocus += PlayerNameTBox_GotFocus;
        }

        private void PlayerNameTBox_GotFocus(object sender, EventArgs e)
        {
            TextBox tBox = sender as TextBox;
            if (tBox != null) { tBox.SelectAll(); }
        }

        public string PlayerName { get; set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            PlayerName = playerNameTBox.Text;
            this.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
