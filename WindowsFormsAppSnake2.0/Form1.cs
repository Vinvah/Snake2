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
            
        }

        class Level
        {
            public Panel panel;
            public List<Snake> snakes;
            public int xLen = 20; // in squares
            public int yLen = 20; // in squares
            public List<Square> squares;
            public Color backColor = Color.Gray; // backColor of panel
            public Color squareBackColor = Color.Transparent;
            Level(Panel panel, int xLen, int yLen )
            {
                this.panel = panel;
                this.xLen = xLen;
                this.yLen = yLen;
                Size size = new Size( this.panel.Width/xLen, this.panel.Height/yLen );
                
                for (int iY = 0; iY < yLen; iY++)
                {
                    for (int iX = 0; iX < xLen; iX++)
                    {
                        Square square = new Square() { level = this, x = iX, y = iY, pictureBox = new PictureBox(), };
                        square.pictureBox.Size = size;
                        squares.Add(square);
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
            public Size size;
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
