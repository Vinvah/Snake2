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
        }


        class Square
        {
            public PictureBox pictureBox;
            public List<Snake> snakes = new List<Snake>();
            public List<int> snakeSegments = new List<int>();
            public Level level;
            public Pickup pickup = null;
            public int x;
            public int y;
        }

        class Snake
        {
            public int playerNr;
            public Snakeskin skin;
            public int startingLength = 5;
            public bool dead;
            public int length;
            public List<Square> occupiedSquares = new List<Square>();
            public List<Keys> controlKeys = new List<Keys>() { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Shift, Keys.Space }; // up, down, left, right, dash/slowmotion, use item
            public double speed = 10; // in squares/s
            public double dashSpeed = 2;
            public double slowmoSpeed = 0.5;
            
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
