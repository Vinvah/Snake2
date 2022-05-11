﻿using System;
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
                panel = panel1
            };
            level.Setup();
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
            public bool wrap = false;

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
            public Level level;
            public Pickup pickup = null;
            public int x;
            public int y;
            public Square getSegmentNr(Snake snake) { return new Square(); }
        }

        class Snake
        {
            public Player player;
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
                    if (occupiedSquares.IndexOf(square) > 0) { square.pictureBox.BackColor = Color.AliceBlue; }
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
                if (nextSquare == null) { dead = true; }
                else { Try_Move_To_Square(nextSquare); }
                if (occupiedSquares.Count() > length)
                {
                    foreach (Square square in occupiedSquares.GetRange(length, occupiedSquares.Count() - 1 ) )
                    {
                        occupiedSquares.RemoveRange(length, occupiedSquares.Count - 1);
                    }
                }
            }
            public void Try_Move_To_Square(Square square)
            {
                headx = square.x;
                heady = square.y;
                occupiedSquares.Insert(0, square);
            }
            
            public void Keyboard_Input_On_Press(Keys key)
            {
                if (key == player.selectedProfile.keybinds[0] && facing != "down")  { facing = "up"; }
                if (key == player.selectedProfile.keybinds[1] && facing != "up")    { facing = "down"; }
                if (key == player.selectedProfile.keybinds[2] && facing != "right") { facing = "left"; }
                if (key == player.selectedProfile.keybinds[3] && facing != "left")  { facing = "right"; }
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

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
