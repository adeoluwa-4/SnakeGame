/*
 * Author: Josh Weese
 */
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace KSU.CIS300.Snake
{
    /// <summary>
    /// THE UserInterface
    /// </summary>
    public partial class UserInterface : Form
    {
        /// <summary>
        /// Width of each square in pixels
        /// </summary>
        private int _squareWidth;

        /// <summary>
        /// Grid size (NxN)
        /// </summary>
        private int _size;

        /// <summary>
        /// Current game instance
        /// </summary>
        private Game _game;

        /// <summary>
        /// Brush used to draw snake body
        /// </summary>
        private SolidBrush _bodyBrush = new SolidBrush(Color.DarkViolet);

        

        /// <summary>
        /// Brush used to draw food
        /// </summary>
        private SolidBrush _foodBrush = new SolidBrush(Color.Cyan);

        /// <summary>
        /// Pen used to draw borders
        /// </summary>
        private Pen _pen = new Pen(Color.Black, 2);

        /// <summary>
        /// Allows canceling of async movement loop
        /// </summary>
        private CancellationTokenSource _cancelSource = new CancellationTokenSource();

        public UserInterface()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        /// <summary>
        /// Starts a new game with the specified grid size and speed.
        /// </summary>
        /// <param name="size">Grid size</param>
        /// <param name="speed">Delay in milliseconds between moves</param>
        private void NewGame(int size, int speed)
        {
            _cancelSource.Cancel();
            _cancelSource = new CancellationTokenSource();

            _size = size;
            _game = new Game(size, speed, uxIsAI.Checked);

            uxPictureBox.Width = 600;
            uxPictureBox.Height = 600;
            this.Width = uxPictureBox.Width + 40;
            this.Height = uxPictureBox.Height + 120;

            _squareWidth = uxPictureBox.Width / size;

            uxScore.DataBindings.Clear();
            uxScore.DataBindings.Add("Text", _game, "Score");

           

            IProgress<SnakeStatus> progress = new Progress<SnakeStatus>(CheckProgress);

           
           _= _game.StartMoving(progress, _cancelSource.Token);




        }
        /// <summary>
        /// Responds to status updates from the game loop.
        /// </summary>
        /// <param name="status">The current status of the snake.</param>
        private void CheckProgress(SnakeStatus status)
        {
            Refresh();

            if (status == SnakeStatus.Collision)
            {
                MessageBox.Show("Game over!");
            }
            else if (status == SnakeStatus.Win)
            {
                MessageBox.Show("Game Completed!");
            }
        }



        /// <summary>
        /// Handles the Paint event for the PictureBox.
        /// Draws the snake body and food on the game grid.
        /// </summary>
        /// <param name="sender">The PictureBox control triggering the paint event.</param>
        /// <param name="e">Paint event arguments containing the Graphics context.</param>
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if(_game == null) return;
            Graphics g = e.Graphics;

            foreach (GameNode node in _game.GetSnakePath())
            {
                Rectangle rect = new Rectangle(node.X * _squareWidth, node.Y * _squareWidth, _squareWidth, _squareWidth);
                g.FillRectangle(_bodyBrush, rect);
                g.DrawRectangle(_pen, rect);
            }

            GameNode food = _game.GetFood();
            if (food != null)
            {
                Rectangle foodRect = new Rectangle(food.X * _squareWidth, food.Y * _squareWidth, _squareWidth, _squareWidth);
                g.FillEllipse(_foodBrush, foodRect);
                g.DrawEllipse(_pen, foodRect);
            }

        }
        /// <summary>
        /// Handles arrow key input from the user to manually control the snake
        /// when the AI is disabled.
        /// </summary>
        /// <param name="sender">The form receiving the key press.</param>
        /// <param name="e">Key event arguments, including which key was pressed.</param>
        private void UserInterface_KeyDown(object sender, KeyEventArgs e)
            {
                if (_game != null && _game.Play && !uxIsAI.Checked)
                {
                    if (e.KeyCode == Keys.Up) _game.MoveUp();
                    else if (e.KeyCode == Keys.Down) _game.MoveDown();
                    else if (e.KeyCode == Keys.Left) _game.MoveLeft();
                    else if (e.KeyCode == Keys.Right) _game.MoveRight();

                    uxPictureBox.Refresh();
                }
            }

        /// <summary>
        /// Starts a new easy game 
        /// </summary>
        /// <param name="sender">The Easy menu item.</param>
        /// <param name="e">Click event arguments.</param>
        private void EasyGame_Click(object sender, EventArgs e)
            {
            NewGame(10, 250);
        }

        /// <summary>
        /// Starts a new normal game 
        /// </summary>
        /// <param name="sender">The Normal menu item.</param>
        /// <param name="e">Click event arguments.</param>
        private void NormalGame_Click(object sender, EventArgs e)
            {
            NewGame(20, 150);
        }
        /// <summary>
        /// Starts a new hard game 
        /// </summary>
        /// <param name="sender">The Hard menu item.</param>
        /// <param name="e">Click event arguments.</param>
        private void HardGame_Click(object sender, EventArgs e)
            {
            int speed;
            if (uxIsAI.Checked)
            {
                speed = (int)uxAIspeed.Value;
            }
            else
            {
                speed = 100;
            }

            NewGame(30, speed);
        }

        /// <summary>
        /// Ensures arrow keys are treated as input even when focus is on another control.
        /// </summary>
        /// <param name="sender">The control requesting preview.</param>
        /// <param name="e">Preview key event arguments.</param>
        private void UserInterface_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
            {
            e.IsInputKey = true;
        }
        }
    }
