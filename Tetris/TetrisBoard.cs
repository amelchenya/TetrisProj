using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Player(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public override string ToString()
        {
            return Name + ": " + Score;
        }
    }

    public class TetrisBoard
    {
        public TetrisBoard(Graphics g, Graphics gNext)
        {
            Graphics = g;
            GraphicsNext = gNext;
            GetNextFigure();
            InitGrid();

            ScoresTable = GetScoresTable();
        }

        public static Color BackColor { get; set; } = Color.Gray;
        public static Size Size { get { return new Size(Width * Block.Length, Height * Block.Length); } }
        public static Size NextPanelSize { get { return new Size(6 * Block.Length, 5 * Block.Length); } }
        public static int Height { get; set; } = 20; 
        public static int Width { get; set; } = 10;
        public static int hStartPos { get; set; } = 5;
        public static int ScoreForLine { get; set; } = 100;
        public static int ScoreForLevelUp { get; set; } = 1000;
        public static int LevelMax { get; set; } = 5;
        public static string ScoresTablePath { get; set; } = "ScoresTable.txt";
        private static List<Player> _scoresTable;
        public static int MaxScores { get; set; } = 10;
        public static List<Player> ScoresTable
        {
            get
            {
                return (_scoresTable != null) ? _scoresTable : GetScoresTable();
            }
            set
            {
                _scoresTable = value;
            }
        }

        public bool[,] Grid { get; set; } = new bool[Height, Width];
        public List<Block> Lying { get; set; } = new List<Block>();
        public Figure CurrentFigure { get; set; }
        public Figure NextFigure { get; set; }
        public Graphics Graphics { get; set; }
        public Graphics GraphicsNext { get; set; }

        int _level;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                LevelChanged?.Invoke(this, null);
            }
        }
        public event EventHandler LevelChanged;

        int _score;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                ScoreChanged?.Invoke(this, null);
            }
        }
        public event EventHandler ScoreChanged;

        private Figure GetRandomFigure()
        {
            Random r = new Random();
            switch (r.Next(1, 8))
            {
                case 1:
                    return new Line(hStartPos, 0, Color.Red);
                case 2:
                    return new Square(hStartPos, 0, Color.Black);
                case 3:
                    return new LThunder(hStartPos, 0, Color.Blue);
                case 4:
                    return new RThunder(hStartPos, 0, Color.Yellow);
                case 5:
                    return new LeftT(hStartPos, 0, Color.Brown);
                case 6:
                    return new RightT(hStartPos, 0, Color.Chocolate);
                case 7:
                    return new Triangle(hStartPos, 0, Color.Green);
                default:
                    return new Triangle(hStartPos, 0, Color.Green);
            }
        }

        private bool FillRows(int i)
        {
            for (int j = 0; j < Width; j++)
            {
                if (Grid[i, j] != true)
                {
                    return false;
                }
            }
            return true;
        }

        private void DeleteFillRows()
        {
            for (int i = Height - 1; i >=0; i--)
            {
                if (FillRows(i))
                {
                    Lying.RemoveAll(block => block.vPos == i);

                    Score += ScoreForLine * Level;
                    if (Score / (ScoreForLevelUp * Level) > 0 && Level < LevelMax) { Level++; }

                    Lying.Where(block => block.vPos < i).ToList().ForEach(block => block.vPos++);
                    InitGrid();
                    i++;
                }
            }
        }

        public void InitGrid()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Grid[i, j] = false; 
                }
            }

            foreach (var block in Lying)
            {
                Grid[block.vPos, block.hPos] = true; 
            }
        }

        public void GetNextFigure()
        {
            if (NextFigure != null)
            {
                Lying.AddRange(CurrentFigure.Blocks);
                InitGrid();
                CurrentFigure = NextFigure;
            }
            else 
            {
                CurrentFigure = GetRandomFigure();
            }

            CurrentFigure.PositionChanged += new EventHandler(Draw); 
            NextFigure = GetRandomFigure();

            DeleteFillRows();

            Draw(null, null);
            DrawNext();
        }

        public bool GameIsOver()
        {
            if (Lying != null && Lying.Count > 0)
            {
                return Lying.Min(block => block.vPos) < NextFigure.height;
            }

            return false;
        }

        public static List<Player> GetScoresTable()
        {
            var table = new List<Player>();

            if (System.IO.File.Exists(ScoresTablePath))
            {
                var sr = new System.IO.StreamReader(ScoresTablePath);
                string line = sr.ReadLine();
                while (!String.IsNullOrWhiteSpace(line))
                {
                    table.Add(new Player(line.Split(":".ToCharArray())[0], int.Parse(line.Split(":".ToCharArray())[1])));
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            return table;
        }

        public void SaveScore(string playerName)
        {
            ScoresTable.Add(new Player(playerName, Score));
            var sr = new System.IO.StreamWriter(ScoresTablePath);
            ScoresTable.ForEach(player => sr.WriteLine(player));
            sr.Close();
        }

        public void Draw(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            Graphics gi = Graphics.FromImage(bitmap);

            InitGrid();
            gi.Clear(BackColor);
            CurrentFigure.Draw(gi);
            foreach (var block in Lying)
            {
                block.Draw(gi);
            }
            Graphics.DrawImage(bitmap, new Point(0, 0));
        }

        public void DrawNext() 
        {
            Bitmap bitmap = new Bitmap(NextPanelSize.Width, NextPanelSize.Height);
            Graphics gi = Graphics.FromImage(bitmap);

            gi.Clear(BackColor);
            NextFigure.Draw(gi, 2, 1);
            GraphicsNext.DrawImage(bitmap, new Point(0, 0));
        }
    }
}