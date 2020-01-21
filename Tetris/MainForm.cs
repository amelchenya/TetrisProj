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
    public partial class MainForm : Form
    {
        private Graphics g, gNext;
        private PictureBox boardPBox = new PictureBox();
        private PictureBox nextFigurePBox = new PictureBox();
        private Label scoreLabel = new Label();
        private Label levelLabel = new Label();
        private TetrisBoard board;

        public MainForm()
        {
            InitializeComponent();

            boardPBox.Size = TetrisBoard.Size;
            boardPBox.BackColor = TetrisBoard.BackColor;
            boardPBox.BorderStyle = BorderStyle.FixedSingle;
            boardPBox.Location = new Point(10, 30);
            boardPBox.Margin = new Padding(10);
            boardPBox.Padding = new Padding(10);
            this.Controls.Add(boardPBox);

            g = boardPBox.CreateGraphics();

            nextFigurePBox.Size = TetrisBoard.NextPanelSize;
            nextFigurePBox.BackColor = TetrisBoard.BackColor;
            nextFigurePBox.BorderStyle = BorderStyle.FixedSingle;
            nextFigurePBox.Location = new Point(boardPBox.Location.X + boardPBox.Width + 10, 30);
            nextFigurePBox.Margin = new Padding(10);
            nextFigurePBox.Padding = new Padding(10);
            this.Controls.Add(nextFigurePBox);

            gNext = nextFigurePBox.CreateGraphics();

            scoreLabel.Text = "Score: ";
            scoreLabel.Font = new Font("Calibri", 20, FontStyle.Bold);
            scoreLabel.AutoSize = true;
            scoreLabel.Location = new Point(nextFigurePBox.Location.X, nextFigurePBox.Location.Y + nextFigurePBox.Height + 10);
            this.Controls.Add(scoreLabel);

            levelLabel.Text = "Level: ";
            levelLabel.Font = new Font("Calibri", 20, FontStyle.Bold);
            levelLabel.AutoSize = true;
            levelLabel.Location = new Point(scoreLabel.Location.X, scoreLabel.Location.Y + scoreLabel.Height + 10);
            this.Controls.Add(levelLabel);

            fallingTimer.Interval = 600 - 100 * 1; //Level = 1
            fallingTimer.Tick += FallingTimer_Tick;

            newToolStripMenuItem.Click += InitNewGame;
            pauseToolStripMenuItem.Click += PauseGame;
            exitToolStripMenuItem.Click += (object sender, EventArgs e) => Application.Exit();

            this.KeyDown += MainForm_KeyDown;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (board != null)
                {
                    PauseGame(null, null);
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                if (board.CurrentFigure.CanMove(Direction.Left, board.Grid)) { board.CurrentFigure.Move(Direction.Left); }
            }
            if (e.KeyCode == Keys.Right)
            {
                if (board.CurrentFigure.CanMove(Direction.Right, board.Grid)) { board.CurrentFigure.Move(Direction.Right); }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (board.CurrentFigure.CanMove(Direction.Down, board.Grid)) { board.CurrentFigure.Move(Direction.Down); }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (board.CurrentFigure.CanRotate(board.Grid)) { board.CurrentFigure.Rotate(); }
            }
        }

        private void InitNewGame(object sender, EventArgs e)
        {
            board = new TetrisBoard(g, gNext);
            board.ScoreChanged += (object obj, EventArgs args) => scoreLabel.Text = "Score: " + board.Score;
            board.LevelChanged += (object obj, EventArgs args) => levelLabel.Text = "Level: " + ((board.Level < TetrisBoard.LevelMax) ? board.Level.ToString() : "Max");
            board.Score = 0;
            board.Level = 1;
            fallingTimer.Enabled = true;
        }

        private void PauseGame(object sender, EventArgs e)
        {
            if (fallingTimer.Enabled == true)
            {
                fallingTimer.Enabled = false;
                pauseToolStripMenuItem.Text = "Continue";
            }
            else
            {
                fallingTimer.Enabled = true;
                pauseToolStripMenuItem.Text = "Pause";
            }
        }

        private void InitGameOver()
        {
            fallingTimer.Enabled = false;

            if (TetrisBoard.ScoresTable.Count > 0)
            {
                foreach (var player in TetrisBoard.ScoresTable)
                {
                    if (board.Score > player.Score || TetrisBoard.ScoresTable.Count < TetrisBoard.MaxScores)
                    {
                        SaveScoreForm saveScoreForm = new SaveScoreForm();
                        saveScoreForm.ShowDialog();
                        if (!String.IsNullOrWhiteSpace(saveScoreForm.PlayerName))
                        {
                            board.SaveScore(saveScoreForm.PlayerName);
                        }
                        break;
                    }
                }
            }
            else
            {
                SaveScoreForm saveScoreForm = new SaveScoreForm();
                saveScoreForm.ShowDialog();
                if (!String.IsNullOrWhiteSpace(saveScoreForm.PlayerName))
                {
                    board.SaveScore(saveScoreForm.PlayerName);
                }
            }

            board = null;
            g.Clear(TetrisBoard.BackColor);
            gNext.Clear(TetrisBoard.BackColor);
            MessageBox.Show("Game Over!");
        }

        private void FallingTimer_Tick(object sender, EventArgs e)
        {
            if (board.CurrentFigure.CanMove(Direction.Down, board.Grid))
            {
                board.CurrentFigure.Move(Direction.Down);
            }
            else
            {
                if (board.GameIsOver())
                {
                    InitGameOver();
                    return;
                }
                fallingTimer.Interval = 600 - 100 * board.Level;
                board.GetNextFigure();
            }
        }

        private void scoresTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScoresTableForm scoresTableForm = new ScoresTableForm();
            scoresTableForm.ShowDialog();
        }

        private void SaveScore()
        {
            if (TetrisBoard.ScoresTable.Count == 0) { return; }

            SaveScoreForm saveScoreForm = new SaveScoreForm();
            saveScoreForm.ShowDialog();
            if (!String.IsNullOrWhiteSpace(saveScoreForm.PlayerName))
            {
                TetrisBoard.ScoresTable.Add(new Player(saveScoreForm.PlayerName, board.Score));
            }

            var sr = new System.IO.StreamWriter(TetrisBoard.ScoresTablePath);
            TetrisBoard.ScoresTable.OrderBy(player => player.Score).ToList().ForEach(player => sr.WriteLine(player));
        }
    }
}
