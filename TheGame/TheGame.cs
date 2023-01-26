using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Media;

namespace TheGame
{
    public partial class TheGame : Form
    {
        const int SquareSide = 25;
        int pos = 0;
        int ros = 0;
        int score = 0;
        List<List<int>> PlayingField = new List<List<int>>() { };
        Random r = new Random();
        SolidBrush brush = new SolidBrush(Color.DarkOrange);
        SolidBrush playerbrush = new SolidBrush(Color.DarkRed);
        SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\trosf\Downloads\music.wav");
        public TheGame()
        {
            InitializeComponent();
            for (int i = 0; i < MainPanel.Height / SquareSide; i++)
            {
                PlayingField.Add(new List<int>());
                for (int j = 0; j < MainPanel.Width / SquareSide; j++)
                {
                    PlayingField[i].Add(0);
                }
            }
            label1.Text = score.ToString();
            pos = MainPanel.Width / SquareSide / 2;
            ros = MainPanel.Height / SquareSide - 1;
            PlayingField[ros][pos] = -1;
        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            simpleSound.Play();
            MainTimer.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            simpleSound.Stop();
            MainTimer.Stop();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            label1.Text = (++score).ToString();
            PlayingFieldMove();
            MainPanel.Invalidate();
        }

        private void PlayingFieldMove()
        {
            PlayingField.Insert(0, new List<int> { });
            for (int i = 0; i < MainPanel.Width / SquareSide; i++)
            {
                PlayingField[0].Add(r.Next(0, 5) > 3 ? 1 : 0);
            }
            if (PlayingField.Count == MainPanel.Height / SquareSide + 1)
            {
                if (CheckPosition(0, 0, 0, 0))
                {
                    GameOver();
                }
                PlayingField[ros + 1][pos] = 0;
                PlayingField[ros][pos] = -1;
                PlayingField.RemoveAt(MainPanel.Height / SquareSide);
            }
        }
        private void reset()
        {
            for (int i = 0; i < MainPanel.Height / SquareSide; i++)
            {
                for (int j = 0; j < MainPanel.Width / SquareSide; j++)
                {
                    PlayingField[i][j] = 0;
                }
            }
            pos = MainPanel.Width / SquareSide / 2;
            ros = MainPanel.Height / SquareSide - 1;
            PlayingField[ros][pos] = -1;
            score = 0;
        }
        //
        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < MainPanel.Height / SquareSide; i++)
            {
                for (int j = 0; j < MainPanel.Width / SquareSide; j++)
                {
                    if (PlayingField[i][j] == 1)
                    {
                        g.FillRectangle(brush, SquareSide * j, SquareSide * i, SquareSide, SquareSide);
                    }
                    else if (PlayingField[i][j] == -1)
                    {
                        g.FillRectangle(playerbrush, SquareSide * j, SquareSide * i, SquareSide, SquareSide);
                    }
                }
            }
        }
        private bool CheckPosition(int w, int a, int s, int d)
        {
            if (PlayingField[ros - w + s][pos - a + d] == 1 || (ros - w + s) >= MainPanel.Height / SquareSide || (pos - a + d) >= MainPanel.Width / SquareSide)
            {
                return true;
            }
            return false;
        }

        private void GameOver()
        {
            simpleSound.Stop();
            MainTimer.Stop();
            MessageBox.Show("GAME OVER \n Your score: " + score.ToString(), "game over", MessageBoxButtons.OK, MessageBoxIcon.Error);
            reset();

        }

        private void PlayerMove(int w, int a, int s, int d)
        {
            if (CheckPosition(w, a, s, d) == true)
            {
                GameOver();
            }
            PlayingField[ros][pos] = 0;
            ros = ros - w + s;
            pos = pos - a + d;
            PlayingField[ros][pos] = -1;
        }
        private void TheGame_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    PlayerMove(1, 0, 0, 0);
                    MainPanel.Invalidate();
                    break;
                case Keys.S:
                    PlayerMove(0, 0, 1, 0);
                    MainPanel.Invalidate();
                    break;
                case Keys.A:
                    PlayerMove(0, 1, 0, 0);
                    MainPanel.Invalidate();
                    break;
                case Keys.D:
                    PlayerMove(0, 0, 0, 1);
                    MainPanel.Invalidate();
                    break;
            }
        }
    }
}