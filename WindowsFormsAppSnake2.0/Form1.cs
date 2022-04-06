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
            Level level = new Level
            {
                panel = panel2
            };
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
                        if (x < 0) { x = x + xLen * (x * -1 / xLen + 1); }
                        else { x = x - xLen * (x / xLen); }
                    }
                    else { return null; }
                }
                if (y < 0 || y >= yLen)
                {
                    if (wrap) 
                    {
                        if (y < 0) { y = y + yLen * (y * -1 / yLen + 1); }
                        else { y = y - yLen * (y / yLen); }
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
            public bool dashing = false;
            public double slowmoSpeed = 0.5;
            public bool slowmo = false;
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
                Square nextSquare = null;
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
                Try_Move_To_Square(nextSquare);
                foreach (Square square in occupiedSquares)
                {

                }
            }
            public void Try_Move_To_Square(Square square)
            {
                
            }
            
            public void Keyboard_Input_On_Press(Keys key)
            {
                if (key == controlKeys[0]) { facing = "up"; }
                if (key == controlKeys[1]) { facing = "down"; }
                if (key == controlKeys[2]) { facing = "left"; }
                if (key == controlKeys[3]) { facing = "right"; }
                if (key == controlKeys[4]) { dashing = true; slowmo = false; }
                if (key == controlKeys[5]) { slowmo = true; dashing = false; }
            }



            public void Keyboard_Input_On_Release(Keys key) 
            {
                if (key == controlKeys[4]) { dashing = false; }
                if (key == controlKeys[5]) { slowmo = false; }
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
