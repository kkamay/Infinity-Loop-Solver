using System;
using System.Windows.Forms;

namespace Infinity_Loop_Solver
{
    public partial class Form1 : Form
    {
        public Tile[,] TILE_SET = new Tile[8, 13];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeTiles();
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            InitializeTiles();
        }

        private void BtnSolve_Click(object sender, EventArgs e)
        {

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////

        // Initialize all tiles to Empty
        private void InitializeTiles()
        {
            for (int r = 0; r < TILE_SET.GetLength(0); r++)
            {
                for (int c = 0; c < TILE_SET.GetLength(1); c++)
                {
                    TILE_SET[r, c] = new Empty();
                }
            }
        }        
    }
}
