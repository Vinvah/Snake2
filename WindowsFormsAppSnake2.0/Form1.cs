using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsAppSnake2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Level level = new Level();
            level.panel = panel2;
            level.Setup();
        }

        class Level
        {
            public Panel panel;
            public List<Snake> snakes = new List<Snake>();
            public int xLen = 20; // in squares
            public int yLen = 20; // in squares
            public List<Square> squares = new List<Square>();
            public Color backColor = Color.Gray; // backColor of panel
            public Color squareBackColor = Color.Black;
            public bool wrap = true;

            public void Setup()
            {
                int margin = 0;
                Size size = new Size( panel.Width/xLen-margin, panel.Height/yLen-margin );
                
                for (int iY = 0; iY < yLen; iY++)
                {
                    for (int iX = 0; iX < xLen; iX++)
                    {
                        Square square = new Square() { level = this, x = iX, y = iY, pictureBox = new PictureBox(), };
                        square.pictureBox.Size = size;
                        square.pictureBox.Location = new Point((size.Width + margin) * iX, (size.Height + margin) * iY);
                        square.pictureBox.BackColor = backColor;
                        squares.Add(square);
                        panel.Controls.Add(square.pictureBox);
                    }
                }
            }

            public Square GetSquare(int x, int y)
            {
                if (x < 0 || x >= xLen)
                {
                    if (wrap) 
                    {
                        x = xLen + x - (xLen * (x / xLen));
                    }
                    else { return null; }
                }
                if (y < 0 || y >= yLen)
                {
                    if (wrap) 
                    {
                        y = yLen + y - (yLen * (y / yLen));
                    }
                    else { return null; }
                }
                return squares[x + y * xLen];

            }
        }


        class Square
        {
            public PictureBox pictureBox;
            public List<Snake> snakes = new List<Snake>();
            public List<int> snakeSegmentNrs = new List<int>();
            public Level level;
            public Pickup pickup = null;
            public int x;
            public int y;
        }

        class Snake
        {
            public int playerNr;
            public Level level;
            public Snakeskin skin;
            public int startingLength = 5;
            public bool dead;
            public int length;
            public int headx = 2;
            public int heady = 2;
            public string facing = "right";
            public List<Square> occupiedSquares = new List<Square>();
            public List<Keys> controlKeys = new List<Keys>() { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Shift, Keys.Space }; // up, down, left, right, dash/slowmotion, use item
            public double speed = 10; // in squares/s
            public double dashSpeed = 2;
            public double slowmoSpeed = 0.5;

            public void Update_Render()
            {
                foreach (Square square in occupiedSquares)
                {
                    if (square.snakeSegmentNrs[0] > 0) { square.pictureBox.BackColor = Color.AliceBlue; }
                    else { square.pictureBox.BackColor = square.level.squareBackColor; }
                }
            }

            public void Move_Step()
            {
                Square nextSquare;
                switch (facing)
                {
                    case "up":
                        nextSquare = level.GetSquare(headx, heady + 1);
                        break;

                    case "down":
                        nextSquare = level.GetSquare(headx, heady - 1);
                        break;

                    case "left":
                        nextSquare = level.GetSquare(headx - 1, heady);
                        break;

                    case "right":
                        nextSquare = level.GetSquare(headx + 1, heady);
                        break;
                }
                foreach (Square square in occupiedSquares)
                {

                }
            }
            


            
            //public Color Read_Tile_Skin(Square square) { return Color.AliceBlue; }
        }

        class Snakeskin
        {
            public string name;
            public List<Color> HeadColors;
            public List<Color> HeadEatFx;
            public List<Color> ReapeatingPattern;
            public bool LockRepeatingInPlace= false;
            public List<Color> Tail;
            public List<Color> TailEatFx;
            public List<Color> Trail;
        }

        class Pickup
        {

        }

    }
}
