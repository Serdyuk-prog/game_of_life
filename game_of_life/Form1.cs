using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_of_life
{
    public partial class Form1 : Form
    {

        private Graphics graphics;
        private int resolution;
        private bool isSimRanning = false;
        private bool[,] map;
        private int cols;
        private int rows;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void startSimulation()
        {
            if (isSimRanning) return;

            resolution = 8;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            map = new bool[cols, rows];

            //Random random = new Random();

            for (int x = 0; x < cols; x++)
            {
                for(int y=0; y < rows; y++)
                {
                    map[x, y] = false;
                    //map[x, y] = random.Next(2) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // новая карина в pictureBox
            graphics = Graphics.FromImage(pictureBox1.Image); // создаем графику
            timer1.Start();
            isSimRanning = true;
            
        }

        private void drawNextGen()
        {
            graphics.Clear(Color.DodgerBlue);

            var newMap = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighboursCount = countNeighbours(x,y);
                    bool hasLife = map[x, y];

                    if (hasLife && neighboursCount < 2)
                    {
                        newMap[x, y] = false;
                    }
                    if (hasLife && (neighboursCount == 2 || neighboursCount == 3))
                    {
                        newMap[x, y] = true;
                    }
                    if (hasLife && neighboursCount > 3)
                    {
                        newMap[x, y] = false;
                    }
                    if (!hasLife && neighboursCount == 3)
                    {
                        newMap[x, y] = true;
                    }


                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.LightYellow, x*resolution, y*resolution, resolution-1, resolution-1);
                    }
                }
            }

            map = newMap;
            pictureBox1.Refresh();
        }

        private int countNeighbours(int x, int y)
        {
            int result = 0;

            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;
                    bool isSelfChecking = col == x && row == y;

                    if (!isSelfChecking && map[col, row])
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private void stopSimulation()
        {
            if (!isSimRanning) return;
            timer1.Stop();
            isSimRanning = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            drawNextGen();
        }
        
        private void btStart_Click(object sender, EventArgs e)
        {
            startSimulation();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            stopSimulation();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isSimRanning) return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;

                if (validateMousePosition(x, y)) map[x, y] = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                if (validateMousePosition(x, y)) map[x, y] = false;
            }
        }

        private bool validateMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }
    }
}
