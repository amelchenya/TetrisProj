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
    public partial class ScoresTableForm : Form
    {
        public ScoresTableForm()
        {
            InitializeComponent();

            if (TetrisBoard.ScoresTable != null)
            {
                ScoresTableDGView.DataSource = TetrisBoard.ScoresTable.OrderByDescending(player => player.Score).ToList();
            }

            this.KeyDown += ScoresTableForm_KeyDown;
            ScoresTableDGView.KeyDown += ScoresTableForm_KeyDown;
        }

        private void ScoresTableForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }
    }
}
