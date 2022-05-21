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
            game.levels.Add( new Level(panel1) );
            game.levels[0].snakes.Add(new Snake(game) { level = game.levels[0]});
            game.players.Add(game.levels[0].snakes[0].player);
            game.players[0].snake = game.levels[0].snakes[0];
        }
        Game game = new Game(new GameSettings());
        class Game
        {
            
            public List<Level> levels = new List<Level>();
            public GameSettings settings = new GameSettings();
            public List<Player> players = new List<Player>();
            public bool paused = false;
            public Timer gameTickTimer;

            public Game(GameSettings settings)
            {
                this.settings = settings;
                gameTickTimer = new Timer() { Interval = 1000 / this.settings.TicksPerSecond, Enabled = true};
                gameTickTimer.Tick += GameTickTimer_Tick;
            }

            private void GameTickTimer_Tick(object sender, EventArgs e)
            {
                foreach (Player player in players)
                {
                    player.snake.OnGameTick();
                }
            }
        }
        class GameSettings
        {
            public int TicksPerSecond = 80;

        }

        class Profile
        {
            public string name = "Default";
            public string displayName = "bob";
            public List<Keys> keybinds = new List<Keys>() { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Shift, Keys.Space }; //
        }

        class Player
        {
            public Profile selectedProfile = new Profile(); // default profile
            public Snake snake;
        }
        class Level
        {
            public Panel panel;
            public List<Snake> snakes = new List<Snake>();
            public int xLen = 20; // in squares
            public int yLen = 20; // in squares
            public List<Square> squares = new List<Square>();
            public Color backColor = Color.Gray; // backColor of panel
            public Color squareBackColor = Color.Gray;
            public bool wrap = false;

            public Level(Panel panel)
            {
                int margin = 0;
                this.panel = panel;
                Size size = new Size(panel.Width / xLen - margin, panel.Height / yLen - margin);

                for (int iY = 0; iY < yLen; iY++)
                {
                    for (int iX = 0; iX < xLen; iX++)
                    {
                        Square square = new Square() { level = this, x = iX, y = iY, pictureBox = new PictureBox(), };
                        square.pictureBox.Size = size;
                        square.pictureBox.Location = new Point((size.Width + margin) * iX, (size.Height + margin) * iY);
                        square.pictureBox.BackColor = squareBackColor;
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
                        if (x < 0) { x += xLen * (x * -1 / xLen + 1); }
                        else { x -= xLen * (x / xLen); }
                    }
                    else { return null; }
                }
                if (y < 0 || y >= yLen)
                {
                    if (wrap) 
                    {
                        if (y < 0) { y += yLen * (y * -1 / yLen + 1); }
                        else { y -= yLen * (y / yLen); }
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
            public Level level;
            public Pickup pickup = null;
            public int x;
            public int y;
            public void RemoveSnake(Snake snake) 
            { 
                snakes.Remove(snake);
                if (snakes.Count() == 0) { Reset(); }
            }
            public void Reset() { snakes.Clear(); pictureBox.BackColor = level.squareBackColor; pickup = null; }
            //public Square getSegmentNr(Snake snake) { return new Square(); }
        }

        class Snake
        {
            
            public Player player = new Player(); // placeholder player
            public Level level;
            public Snakeskin skin;
            public int startingLength = 5;
            public bool dead;
            public int length = 5;
            public int headx = 2;
            public int heady = 2;
            public string facing = "right";
            public string previousSquareDirection = "left";
            public List<Square> occupiedSquares = new List<Square>();
            public List<Keys> controlKeys = new List<Keys>() { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Shift, Keys.Space }; // up, down, left, right, dash/slowmotion, use item
            public int speed = 6; // in squares/s
            int ticksPerMove;
            int currentTickNr = 0;

            public double dashSpeed = 2;
            public bool dashing = false;
            public double slowmoSpeed = 0.5;
            public bool slowmo = false;

            public Snake(Game game)
            {
                ticksPerMove = game.settings.TicksPerSecond / speed;
            }
            public void OnGameTick()
            {
                currentTickNr++;
                if (currentTickNr >= ticksPerMove)
                {
                    Move_Step();
                    currentTickNr -= ticksPerMove;
                }
            }

            public void Update_Render()
            {
                foreach (Square square in occupiedSquares)
                {
                    if (occupiedSquares.IndexOf(square) >= 0) { square.pictureBox.BackColor = Color.AliceBlue; if (dead) { square.pictureBox.BackColor = Color.Red; } }
                    else { square.pictureBox.BackColor = square.level.squareBackColor; }
                }
            }

            
            public void Move_Step()
            {
                if (!dead)
                {
                    Square nextSquare = null;
                    switch (facing)
                    {
                        case "up":
                            nextSquare = level.GetSquare(headx, heady - 1);
                            previousSquareDirection = "down";
                            break;

                        case "down":
                            nextSquare = level.GetSquare(headx, heady + 1);
                            previousSquareDirection = "up";
                            break;

                        case "left":
                            nextSquare = level.GetSquare(headx - 1, heady);
                            previousSquareDirection = "right";
                            break;

                        case "right":
                            nextSquare = level.GetSquare(headx + 1, heady);
                            previousSquareDirection = "left";
                            break;
                    }
                    if (nextSquare == null || nextSquare.snakes.Count() > 0) { dead = true; }
                    else { Try_Move_To_Square(nextSquare); }
                    if (occupiedSquares.Count() > length)
                    {
                        foreach (Square square in occupiedSquares.GetRange(length, occupiedSquares.Count() - length) )
                        {
                            square.RemoveSnake(this);
                        }
                        occupiedSquares.RemoveRange(length, occupiedSquares.Count() - length);
                    }
                    Update_Render();
                }

            }
            public void Try_Move_To_Square(Square square)
            {
                headx = square.x;
                heady = square.y;
                square.snakes.Insert(0, this);
                occupiedSquares.Insert(0, square);
            }
            
            public void Keyboard_Input_On_Press(Keys key)
            {
                if (key == player.selectedProfile.keybinds[0] && previousSquareDirection != "up")  { facing = "up"; }
                if (key == player.selectedProfile.keybinds[1] && previousSquareDirection != "down")    { facing = "down"; }
                if (key == player.selectedProfile.keybinds[2] && previousSquareDirection != "left") { facing = "left"; }
                if (key == player.selectedProfile.keybinds[3] && previousSquareDirection != "right")  { facing = "right"; }
                if (key == player.selectedProfile.keybinds[4]) { dashing = true; slowmo = false; }
                if (key == player.selectedProfile.keybinds[5]) { slowmo = true; dashing = false; }
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
            foreach (Level level in game.levels)
            {
                foreach (Snake snake in level.snakes)
                {
                    snake.Keyboard_Input_On_Press(e.KeyCode);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Level level in game.levels)
            {
                foreach (Snake snake in level.snakes)
                {
                    snake.Keyboard_Input_On_Release(e.KeyCode);
                }
            }
        }
    }
}
