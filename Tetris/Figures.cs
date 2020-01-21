using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public enum Angle { Deg0, Deg90, Deg180, Deg270 }
    public enum Direction { Left, Right, Down }

    public static class Extensions
    {
        public static Angle Next(this Angle angle)
        {
            switch (angle)
            {
                case Angle.Deg0:
                    return Angle.Deg90;
                case Angle.Deg90:
                    return Angle.Deg180;
                case Angle.Deg180:
                    return Angle.Deg270;
                case Angle.Deg270:
                    return Angle.Deg0;
                default:
                    return Angle.Deg0;
            }
        }

        public static Angle Previous(this Angle angle)
        {
            switch (angle)
            {
                case Angle.Deg0:
                    return Angle.Deg270;
                case Angle.Deg90:
                    return Angle.Deg0;
                case Angle.Deg180:
                    return Angle.Deg90;
                case Angle.Deg270:
                    return Angle.Deg180;
                default:
                    return Angle.Deg0;
            }
        }
    }

    public class Block
    {
        public static int Length { get; set; } = 30;
        public static Size Size { get { return new Size(Length, Length); } }

        public static Rectangle GetRect(int hPos, int vPos)
        {
            return new Rectangle(new Point(hPos * Length, vPos * Length), Size);
        }

        public Block(int hPos, int vPos)
        {
            if (hPos < 0 || vPos < 0)
            {
                throw new Exception("Отсчёт позиций (hPos и vPos) идёт с нуля и не может быть отрицательным!");
            }

            this.hPos = hPos;
            this.vPos = vPos;
        }

        public Block(int hPos, int vPos, Color color)
        {
            if (hPos < 0 || vPos < 0)
            {
                throw new Exception("Отсчёт позиций (hPos и vPos) идёт с нуля и не может быть отрицательным!");
            }

            this.hPos = hPos;
            this.vPos = vPos;
            this.Color = color;
        }

        public int hPos { get; set; }
        public int vPos { get; set; }
        public Point LocationPoint { get { return new Point(hPos * Length, vPos * Length); } }
        public Color Color { get; set; } = Color.Gray;
        public Rectangle Rect { get { return new Rectangle(LocationPoint, Size); } }

        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color), Rect);
            Pen pen = new Pen(Color.Gray, 2);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            g.DrawRectangle(pen, Rect);
        }
    }

    public abstract class Figure
    {
        public Figure(int hPos, int vPos, Color color)
        {
            this.hPos = hPos;
            this.vPos = vPos;
            Color = color;
            InitBlocks();
        }

        public Figure(int hPos, int vPos)
        {
            this.hPos = hPos;
            this.vPos = vPos;
            InitBlocks();
        }

        public event EventHandler PositionChanged;
        public int hPos { get; set; } //гориз позиция
        public int vPos { get; set; } //верт позиция
        public Angle angle { get; set; } = Angle.Deg0;
        public int width { get; set; } //колво блоков по гориз
        public int height { get; set; } //кол-во блоков по верт
        public Color Color { get; set; } = Color.Black;

        public Block[] Blocks { get; set; } = new Block[4];
        protected int[] hBlockPos { get; set; } = new int[4];
        protected int[] vBlockPos { get; set; } = new int[4];

        protected abstract void InitBlockPosition();

        protected void InitBlocks()
        {
            InitBlockPosition();
            for (int i = 0; i < 4; i++)
            {
                Blocks[i] = new Block(hBlockPos[i], vBlockPos[i], Color);
            }

            PositionChanged?.Invoke(this, null);
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < 4; i++)
            {
                Blocks[i].Draw(g);
            }
        }

        public void Draw(Graphics g, int hPos, int vPos)
        {
            int hPosSave = this.hPos;
            int vPosSave = this.vPos;

            this.hPos = hPos;
            this.vPos = vPos;
            InitBlocks();
            Draw(g);

            this.hPos = hPosSave;
            this.vPos = vPosSave;
            InitBlocks();
        }

        public void Rotate()
        {
            angle = angle.Next();
            InitBlocks();
        }

        public void Move(Direction Direction)
        {
            switch (Direction)
            {
                case Direction.Left:
                    hPos--;
                    break;
                case Direction.Right:
                    hPos++;
                    break;
                case Direction.Down:
                    vPos++;
                    break;
            }
            InitBlocks();
        }

        public bool CanMove(Direction Direction, bool[,] Grid)
        {
            switch (Direction)
            {
                case Direction.Left:
                    foreach (var block in Blocks)
                    {
                        if (block.hPos - 1 < 0 || Grid[block.vPos, block.hPos - 1] == true)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Right:
                    foreach (var block in Blocks)
                    {
                        if (block.hPos + 1 >= TetrisBoard.Width || Grid[block.vPos, block.hPos + 1] == true)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Down:
                    foreach (var block in Blocks)
                    {
                        if (block.vPos + 1 >= TetrisBoard.Height || Grid[block.vPos + 1, block.hPos] == true)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        public bool CanRotate(bool[,] Grid)
        {
            angle = angle.Next();
            InitBlockPosition();
            for (int i = 0; i < 4; i++)
            {
                if (hBlockPos[i] < 0 || hBlockPos[i] >= TetrisBoard.Width || 
                    vBlockPos[i] < 0 || vBlockPos[i] >= TetrisBoard.Height || 
                    Grid[vBlockPos[i], hBlockPos[i]] == true)
                {
                    angle = angle.Previous();
                    InitBlockPosition();
                    return false;
                }
            }

            angle = angle.Previous();
            InitBlockPosition();
            return true;
        }
    }

    public class Line : Figure                                                     
    {                                                                              
        public Line(int hPos, int vPos, Color color) : base(hPos, vPos, color)     
        {                                                                          
            width = 1;
            height = 4;
        }

        public Line(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 1;
            height = 4;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                case Angle.Deg180:
                    for (int i = 0; i < 4; i++)
                    {
                        hBlockPos[i] = hPos;
                        vBlockPos[i] = vPos + i;
                    }
                    width = 1; height = 4;
                    break;
                case Angle.Deg90:
                case Angle.Deg270:
                    for (int i = 0; i < 4; i++)
                    {
                        hBlockPos[i] = hPos + i;
                        vBlockPos[i] = vPos;
                    }
                    width = 4; height = 1;
                    break;
            }
        }
    }

    public class Square : Figure                                                            
    {                                                                                       
        public Square(int hPos, int vPos, Color color) : base(hPos, vPos, color)
        {
            width = 2;
            height = 2;
        }

        public Square(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 2;
            height = 2;
        }

        protected override void InitBlockPosition()
        {
            hBlockPos[0] = hPos;
            vBlockPos[0] = vPos;

            hBlockPos[1] = hPos + 1;
            vBlockPos[1] = vPos;

            hBlockPos[2] = hPos;
            vBlockPos[2] = vPos + 1;

            hBlockPos[3] = hPos + 1;
            vBlockPos[3] = vPos + 1;
        }
    }

    public class LThunder : Figure                                                   
    {                                                                                  
        public LThunder(int hPos, int vPos, Color color) : base(hPos, vPos, color)     
        {
            width = 2;
            height = 3;
        }

        public LThunder(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                case Angle.Deg180:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos + 1;

                    hBlockPos[2] = hPos + 1;
                    vBlockPos[2] = vPos + 1;

                    hBlockPos[3] = hPos + 1;
                    vBlockPos[3] = vPos + 2;

                    width = 2; height = 3;
                    break;
                case Angle.Deg90:
                case Angle.Deg270:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos + 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos + 1;
                    vBlockPos[2] = vPos - 1;

                    hBlockPos[3] = hPos + 2;
                    vBlockPos[3] = vPos - 1;

                    width = 3; height = 2;
                    break;
            }
        }
    }

    public class RThunder : Figure                                                           
    {                                                                                        
        public RThunder(int hPos, int vPos, Color color) : base(hPos, vPos, color)           
        {
            width = 2;
            height = 3;
        }

        public RThunder(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                case Angle.Deg180:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos + 1;

                    hBlockPos[2] = hPos - 1;
                    vBlockPos[2] = vPos + 1;

                    hBlockPos[3] = hPos - 1;
                    vBlockPos[3] = vPos + 2;

                    width = 2; height = 3;
                    break;
                case Angle.Deg90:
                case Angle.Deg270:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos - 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos - 1;
                    vBlockPos[2] = vPos - 1;

                    hBlockPos[3] = hPos - 2;
                    vBlockPos[3] = vPos - 1;

                    width = 3; height = 2;
                    break;
            }
        }
    }

    public class LeftT : Figure                                                           
    {                                                                                     
        public LeftT(int hPos, int vPos, Color color) : base(hPos, vPos, color)           
        {
            width = 2;
            height = 3;
        }

        public LeftT(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos + 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos + 1;
                    vBlockPos[2] = vPos + 1;

                    hBlockPos[3] = hPos + 1;
                    vBlockPos[3] = vPos + 2;

                    width = 2; height = 3;
                    break;
                case Angle.Deg90:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos + 1;

                    hBlockPos[2] = hPos - 1;
                    vBlockPos[2] = vPos + 1;

                    hBlockPos[3] = hPos - 2;
                    vBlockPos[3] = vPos + 1;

                    width = 3; height = 2;
                    break;
                case Angle.Deg180:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos - 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos - 1;
                    vBlockPos[2] = vPos - 1;

                    hBlockPos[3] = hPos - 1;
                    vBlockPos[3] = vPos - 2;

                    width = 2; height = 3;
                    break;
                case Angle.Deg270:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos - 1;

                    hBlockPos[2] = hPos + 1;
                    vBlockPos[2] = vPos - 1;

                    hBlockPos[3] = hPos + 2;
                    vBlockPos[3] = vPos - 1;

                    width = 3; height = 2;
                    break;
            }
        }
    }

    public class RightT : Figure                                                             
    {                                                                                        
        public RightT(int hPos, int vPos, Color color) : base(hPos, vPos, color)             
        {
            width = 2;
            height = 3;
        }

        public RightT(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos - 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos - 1;
                    vBlockPos[2] = vPos + 1;

                    hBlockPos[3] = hPos - 1;
                    vBlockPos[3] = vPos + 2;

                    width = 2; height = 3;
                    break;
                case Angle.Deg90:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos - 1;

                    hBlockPos[2] = hPos - 1;
                    vBlockPos[2] = vPos - 1;

                    hBlockPos[3] = hPos - 2;
                    vBlockPos[3] = vPos - 1;

                    width = 3; height = 2;
                    break;
                case Angle.Deg180:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos + 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos + 1;
                    vBlockPos[2] = vPos - 1;

                    hBlockPos[3] = hPos + 1;
                    vBlockPos[3] = vPos - 2;

                    width = 2; height = 3;
                    break;
                case Angle.Deg270:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos + 1;

                    hBlockPos[2] = hPos + 1;
                    vBlockPos[2] = vPos + 1;

                    hBlockPos[3] = hPos + 2;
                    vBlockPos[3] = vPos + 1;

                    width = 3; height = 2;
                    break;
            }
        }
    }

    public class Triangle : Figure                                                       
    {                                                                                    
        public Triangle(int hPos, int vPos, Color color) : base(hPos, vPos, color)
        {
            width = 3;
            height = 2;
        }

        public Triangle(int hPos, int vPos) : base(hPos, vPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos + 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos + 2;
                    vBlockPos[2] = vPos;

                    hBlockPos[3] = hPos + 1;
                    vBlockPos[3] = vPos + 1;

                    width = 3; height = 2;
                    break;
                case Angle.Deg90:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos + 1;

                    hBlockPos[2] = hPos;
                    vBlockPos[2] = vPos + 2;

                    hBlockPos[3] = hPos - 1;
                    vBlockPos[3] = vPos + 1;

                    width = 2; height = 3;
                    break;
                case Angle.Deg180:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos - 1;
                    vBlockPos[1] = vPos;

                    hBlockPos[2] = hPos - 2;
                    vBlockPos[2] = vPos;

                    hBlockPos[3] = hPos - 1;
                    vBlockPos[3] = vPos - 1;

                    width = 3; height = 2;
                    break;
                case Angle.Deg270:
                    hBlockPos[0] = hPos;
                    vBlockPos[0] = vPos;

                    hBlockPos[1] = hPos;
                    vBlockPos[1] = vPos - 1;

                    hBlockPos[2] = hPos;
                    vBlockPos[2] = vPos - 2;

                    hBlockPos[3] = hPos + 1;
                    vBlockPos[3] = vPos - 1;

                    width = 2; height = 3;
                    break;
            }
        }
    }
}
