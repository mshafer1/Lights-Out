using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GRID_OFFSET = 25; // Distance from upper-left side of window
  
        private int GRID_LENGTH = 200; // Size in pixels of grid
        private const int MAX_NUM_CELLS = 5;
        private int CELL_LENGTH = 60;
        private bool[,] grid; // Stores on/off state of cells in grid
        private Random rand; // Used to generate random numbers
        private int NUM_CELLS; // Number of cells in grid
        private int margin;
        public MainForm()
        {
            InitializeComponent();
            NUM_CELLS = 3;
            rand = new Random();
            grid = new bool[MAX_NUM_CELLS, MAX_NUM_CELLS];
            CELL_LENGTH = GRID_LENGTH / NUM_CELLS;
            for (int x = 0; x < MAX_NUM_CELLS; x++)
            {
                for (int y = 0; y < MAX_NUM_CELLS; y++)
                {
                    grid[y, x] = true;
                }
            }
            margin = Height - btnExit.Bottom;
            btnExit.Left = margin/2 + btnNewGame.Right;
            resize();
            x3ToolStripMenuItem.Checked = true;
            x4ToolStripMenuItem.Checked = x5ToolStripMenuItem.Checked = false;
        }

       

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < NUM_CELLS; r++)
            {
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    //get proper pen and brush for on/off
                    //grid section
                    Brush brush;
                    Pen pen;

                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    //determine (x.y) coord of row and col to draw rectangle
                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;

                    //draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CELL_LENGTH, CELL_LENGTH);
                    g.FillRectangle(brush, x + 1, y + 1, CELL_LENGTH - 2, CELL_LENGTH - 2);

                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < GRID_OFFSET || e.X > CELL_LENGTH * NUM_CELLS + GRID_OFFSET ||
                e.Y < GRID_OFFSET || e.Y > CELL_LENGTH * NUM_CELLS + GRID_OFFSET)
            {
                return;
            }
            else
            {
                // find row, col, and mouse press;
                int r = (e.Y - GRID_OFFSET) / CELL_LENGTH;
                int c = (e.X - GRID_OFFSET) / CELL_LENGTH;

                //invert selected box and all surrounding boxes
                for (int i = r - 1; i <= r + 1; i++)
                {
                    for (int j = c - 1; j <= c + 1; j++)
                    {
                        if (i >= 0 && i < NUM_CELLS && j >= 0 && j < NUM_CELLS)
                        {
                            grid[i, j] = !grid[i, j];
                        }
                    }
                }

                //redraw grid
                this.Invalidate();

                //check to see if puzzle has been solved
                if(PlayerWon())
                {
                    MessageBox.Show(this, "Congratualtions! You've Won!", "Lights Out!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool PlayerWon()
        {
            bool result = true;
            for (int i = 0; i < NUM_CELLS; i++)
            {
                for (int j = 0; j < NUM_CELLS; j++)
                {
                    result = result && !grid[i, j];
                }
            }
            return result;
        }

        private void NewGame(object sender, EventArgs e)
        {
            for (int r = 0; r < NUM_CELLS; r++)
            {
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }
            this.Invalidate();
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog(this);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void resize()
        {
            btnExit.Top = btnNewGame.Top = this.Height - (margin + btnExit.Height);
            int center = (Width - (btnExit.Right - btnNewGame.Left)) / 2;
            btnNewGame.Left = center;
            btnExit.Left = btnNewGame.Right + margin/2;
            //set new grid size
            GRID_LENGTH = Math.Min(this.Width - 4 * GRID_OFFSET, btnExit.Top - 2 * GRID_OFFSET);
            CELL_LENGTH = GRID_LENGTH / NUM_CELLS;



            this.Invalidate();
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = true;
            x4ToolStripMenuItem.Checked = x5ToolStripMenuItem.Checked = false;
            NUM_CELLS = 3;
            resize();
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x4ToolStripMenuItem.Checked = true;
            x3ToolStripMenuItem.Checked = x5ToolStripMenuItem.Checked = false;
            NUM_CELLS = 4;
            resize();
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x5ToolStripMenuItem.Checked = true;
            x3ToolStripMenuItem.Checked = x4ToolStripMenuItem.Checked = false;
            NUM_CELLS = 5;
            resize();
        }
    }
}
