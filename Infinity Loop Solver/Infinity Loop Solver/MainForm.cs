using System;
using System.Windows.Forms;

namespace Infinity_Loop_Solver
{
    public partial class MainForm : Form
    {
        public Tile[,] TILE_SET;
        public bool CAN_BE_CLICKED;

        public MainForm()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TILE_SET = new Tile[13, 8];
            CAN_BE_CLICKED = true;

            InitializeTiles();
            DrawTiles();
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.ShowDialog();
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            if(CAN_BE_CLICKED)
            {
                CAN_BE_CLICKED = false;

                InitializeTiles();
                DrawTiles();

                CAN_BE_CLICKED = true;
            }
        }

        private void BtnSolve_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (CAN_BE_CLICKED)
            {
                CAN_BE_CLICKED = false;

                PictureBox pictureBox = sender as PictureBox;

                var name = pictureBox.Name;
                var number = int.Parse(name.Substring(name.IndexOf('x') + 1)) - 1;

                int row = number / TILE_SET.GetLength(1);
                int col = number % TILE_SET.GetLength(1);
                Tile tile = TILE_SET[row, col];

                switch (tile.GetType().Name)
                {
                    case "Empty":
                        TILE_SET[row, col] = new Line();
                        break;
                    case "Line":
                        TILE_SET[row, col] = new Turn();
                        break;
                    case "Turn":
                        TILE_SET[row, col] = new OneWay();
                        break;
                    case "OneWay":
                        TILE_SET[row, col] = new Junction();
                        break;
                    case "Junction":
                        TILE_SET[row, col] = new Roundabout();
                        break;
                    case "Roundabout":
                        TILE_SET[row, col] = new Empty();
                        break;
                    default:
                        break;
                }

                CAN_BE_CLICKED = true;            

                DrawTiles();
            }
        }

        /*********************************************************************************************************/

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

        // Assign TILE_SET images to PictureBoxes
        private void DrawTiles()
        {
            var count = TilePanel.Controls.Count;

            for (int i = 0; i < count; i++)
            {
                int row = i / TILE_SET.GetLength(1);
                int col = i % TILE_SET.GetLength(1);
                Tile tile = TILE_SET[row, col];

                PictureBox pictureBox = TilePanel.Controls[count - 1 - i] as PictureBox;

                pictureBox.Image = tile.Image;
            }
        }        
    }
}
