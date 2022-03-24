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
            public int xLen;
            public int yLen;
            public List<Square> squares;
            public Color backColor;
            public Color squareBackColor;

        }


        class Square
        {
            public PictureBox pictureBox;
            public List<Snake> snakes = new List<Snake>();
            public List<int> snakeSegments = new List<int>();
            public Level level;
            public Pickup pickup = null;
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
