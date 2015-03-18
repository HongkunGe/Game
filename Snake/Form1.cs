using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        public Form1()
        {
            InitializeComponent(); 
            // set the game to default.
            new Settings();
            // set speed and start timer.
            snakeMoveTimer.Interval = 1000 / Settings.Speed;
            snakeMoveTimer.Tick += moveSnake;
            snakeMoveTimer.Start();

            gameTimer.Interval = 1000 / (100); //Settings.Speed +
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            gameInfo.Parent = pbCanvas;
            gameInfo.BackColor = Color.Transparent;
            label1.Parent = pbCanvas;
            label1.BackColor = Color.Transparent;
            label2.Parent = pbCanvas;
            label2.BackColor = Color.Transparent;
            
            StartGame();
        }

        private void StartGame()
        {
            gameInfo.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            scoreNum.Text = Settings.Score.ToString();
            scoreNum.Parent = pbCanvas;
            scoreNum.BackColor = Color.Transparent;
            level.Text = Settings.Level.ToString();
            level.Parent = pbCanvas;
            level.BackColor = Color.Transparent;
            GenerateFood();
        }
        private void GenerateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXpos);
            food.Y = random.Next(0, maxYpos);
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.Keypress(Keys.Enter))
                {
                    StartGame();
                }
                if (Input.Keypress(Keys.Escape))
                {
                    this.Close();
                }
            }
            else if (Input.Keypress(Keys.Enter))
            {
                if (Settings.gamePauseSignal)
                {
                    ResumeGame();
                }
            }
            else if (Input.Keypress(Keys.P))
            {
                PauseGame();
            }
            else if (Input.Keypress(Keys.Escape))
            {
                this.Close();
            }
            else
            {
                if ((Input.Keypress(Keys.Right) || Input.Keypress(Keys.D)) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if ((Input.Keypress(Keys.Left) || Input.Keypress(Keys.A)) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if ((Input.Keypress(Keys.Up) || Input.Keypress(Keys.W)) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if ((Input.Keypress(Keys.Down) || Input.Keypress(Keys.S)) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;
                //MovePlayer();
            }
            pbCanvas.Invalidate();   
        }
        private void moveSnake(object sender, EventArgs e)
        {
            MovePlayer();
            pbCanvas.Invalidate();  // Invalidates the entire surface of the control and causes the control to be redrawn. 

        }     //  After moveSnake, jump to pbCanvas_Paint function, to draw the position that has already been changed.
        private void PauseGame()
        {
            snakeMoveTimer.Enabled = false;
            //gameTimer.Enabled = false;
            Settings.gamePauseSignal = true;
        }
        private void ResumeGame()
        {
            snakeMoveTimer.Enabled = true;
            //gameTimer.Enabled = true;
            Settings.gamePauseSignal = false;
        }
        private void pbCanvas_Paint(object sender, PaintEventArgs e)   // During each interval, the pbCanvas_Paint keep changing. 
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                //set color of snake
                Brush snakeColor;
                //DrawSnake
                for (int i = 0; i < Snake.Count; i++)
                {
                    // Draw head
                    if (i == 0)
                        snakeColor = Brushes.Black;
                    else
                        snakeColor = Brushes.Green;
                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height, 
                                      Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOver = "GAME OVER \nYour find score is: " + Settings.Score + "\n Enter - Play Again"
                                   + "\n Esc - Quit";
                gameInfo.Text = gameOver;
                gameInfo.Parent = pbCanvas;
                gameInfo.BackColor = Color.Transparent;
                gameInfo.Visible = true;
            }
        }

        // The following are member functions.
        private void MovePlayer()
        {
            for (int i = Snake.Count-1; i >= 0; i--)
            {
                // move haed
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;
                    // Detect the collision with wall
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXpos || Snake[i].Y >= maxYpos)
                    {
                        Die();
                    }
                    // Detect the collision with body.
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        fat();
                    }
                        
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                } 
            }
        }
        private void fat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;
            Snake.Add(food);
            Settings.Score += Settings.Points;
            scoreNum.Text = Settings.Score.ToString();

            GenerateFood();
            if (Settings.Score != 0 && Settings.Score % 500 == 0)
            {
                Settings.Level++;
                Settings.Speed += 2;
                level.Text = Settings.Level.ToString();
                level.BackColor = Color.Transparent;
                snakeMoveTimer.Interval = 1000 / Settings.Speed;
            }
        }
        private void Die()
        {
            Settings.GameOver = true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void scoreNum_Click(object sender, EventArgs e)
        {

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            scoreNum.Text = Settings.Score.ToString();
        }

    }
}
