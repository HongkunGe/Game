using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallGame
{
    public partial class Form1 : Form
    {
        //public int speed_left = 8;
        //public int speed_top = 8;
        public struct ball_speed
        {
            public int speed_left, speed_top;
            public ball_speed(int p1=8, int p2=8)
            {
                speed_left = p1;
                speed_top = p2;
            }
        }
        ball_speed bs1 = new ball_speed(8,8);
        ball_speed bs2 = new ball_speed(8,8);

        public int point = 0;
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
            

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.TopMost = true;
            //this.Bounds = Screen.PrimaryScreen.Bounds;

            racket.Top = playground.Bottom - (playground.Bottom / 10);
            gameover_lbl.Left = (playground.Width / 2) - (gameover_lbl.Width / 2);
            gameover_lbl.Top = (playground.Height / 2) - (gameover_lbl.Height / 2);
            gameover_lbl.Visible = false;
        }

      

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.F1)
            {
                ball.Top = 50;
                ball.Left = 50;
                ball2.Top = 150;
                ball2.Left = 50;
                bs1.speed_top = 8;
                bs1.speed_left = 8;
                bs2.speed_top = 8;
                bs2.speed_left = 8;
                point = 0;
                point_lbl.Text = "0";
                timer1.Enabled = true;
                gameover_lbl.Visible = false;
                //playground.BackColor = Color.White;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Cursor.Hide(); 
            racket.Left = Cursor.Position.X - (racket.Width / 2);
            
            ball.Left = ball.Left + bs1.speed_left;
            ball.Top = ball.Top + bs1.speed_top;

            if (ball.Bottom >= racket.Top && ball.Bottom <= racket.Bottom && ball.Left >= racket.Left && ball.Right <= racket.Right)
            {
                bs1.speed_left += 2;
                bs1.speed_top += 2;
                bs1.speed_top = -bs1.speed_top;
                point += 1;
                point_lbl.Text = point.ToString();
                //Random r = new Random();
               // playground.BackColor = Color.FromArgb(r.Next(150, 255), r.Next(150, 255), r.Next(150, 255));

            }
            if (ball.Left <= playground.Left)
            {
                bs1.speed_left = -bs1.speed_left;
            }
            if (ball.Right >= playground.Right)
            {
                bs1.speed_left = -bs1.speed_left;
            }
            if (ball.Top <= playground.Top)
            {
                bs1.speed_top = -bs1.speed_top;
            }
            if (ball.Bottom >= playground.Bottom)
            {
                timer1.Enabled = false;
                gameover_lbl.Visible = true;
            }

            ball2.Left = ball2.Left + bs2.speed_left;
            ball2.Top = ball2.Top + bs2.speed_top;

            if (ball2.Bottom >= racket.Top && ball2.Bottom <= racket.Bottom && ball2.Left >= racket.Left && ball2.Right <= racket.Right)
            {
                bs2.speed_left += 2;
                bs2.speed_top += 2;
                bs2.speed_top = -bs2.speed_top;
                point += 1;
                point_lbl.Text = point.ToString();
                //Random r = new Random();
                //playground.BackColor = Color.FromArgb(r.Next(150, 255), r.Next(150, 255), r.Next(150, 255));

            }
            if (ball2.Left <= playground.Left)
            {
                bs2.speed_left = -bs2.speed_left;
            }
            if (ball2.Right >= playground.Right)
            {
                bs2.speed_left = -bs2.speed_left;
            }
            if (ball2.Top <= playground.Top)
            {
                bs2.speed_top = -bs2.speed_top;
            }
            if (ball2.Bottom >= playground.Bottom)
            {
                timer1.Enabled = false;
                gameover_lbl.Visible = true;
            }
        }

        private void racket_Click(object sender, EventArgs e)
        {
                
        }

        private void gameover_lbl_Click(object sender, EventArgs e)
        {

        }

       
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    e.Graphics.DrawLine(Pens.Yellow, 0, 0, 100, 100);
        //}
        //private void playground_Paint(object sender, PaintEventArgs e)
        //{
        //    this.BackColor = Color.White;
        //    playground.BackColor = Color.FromArgb(25, Color.Black);
        //}
    }
}
