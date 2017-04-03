using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Infinity_Loop_Solver
{
    public partial class MainForm : Form
    {
        public Tile[,] TILE_SET;

        public bool CAN_BE_CLICKED;

        // Global Tile Names
        public const string EMPTY = "Empty";
        public const string LINE = "Line";
        public const string TURN = "Turn";
        public const string ONEWAY = "OneWay";
        public const string JUNCTION = "Junction";
        public const string ROUNDABOUT = "Roundabout";

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
            if (CAN_BE_CLICKED)
            {
                CAN_BE_CLICKED = false;

                InitializeTiles();
                DrawTiles();

                CAN_BE_CLICKED = true;
            }
        }

        private void BtnSolve_Click(object sender, EventArgs e)
        {
            if (CAN_BE_CLICKED)
            {
                CAN_BE_CLICKED = false;

                // Shrinked level
                var upLevel = 0;
                var westLevel = 0;

                // Get rid of unnecessary edges
                var tileSet = ShrinkTileSet(ref upLevel, ref westLevel);



                // Finalize the solution and transfer it to the TILE_SET
                GrowTileSet(tileSet, upLevel, westLevel);

                for (int r = 0; r < TILE_SET.GetLength(0); r++)
                {
                    for (int c = 0; c < TILE_SET.GetLength(1); c++)
                    {
                        Console.Write(TILE_SET[r, c].GetType().Name + " ");
                    }

                    Console.WriteLine();
                }

                CAN_BE_CLICKED = true;
            }
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
                    case EMPTY:
                        TILE_SET[row, col] = new Line();
                        break;
                    case LINE:
                        TILE_SET[row, col] = new Turn();
                        break;
                    case TURN:
                        TILE_SET[row, col] = new OneWay();
                        break;
                    case ONEWAY:
                        TILE_SET[row, col] = new Junction();
                        break;
                    case JUNCTION:
                        TILE_SET[row, col] = new Roundabout();
                        break;
                    case ROUNDABOUT:
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

        // Shrink the TILE_SET from all directions to get rid of empty tiles
        private Tile[,] ShrinkTileSet(ref int upLevel, ref int westLevel)
        {
            // Convert to 2D linkedlist
            var tileSet = new LinkedList<LinkedList<Tile>>();

            for (int r = 0; r < TILE_SET.GetLength(0); r++)
            {
                tileSet.AddLast(new LinkedList<Tile>());

                for (int c = 0; c < TILE_SET.GetLength(1); c++)
                {
                    tileSet.Last.Value.AddLast(TILE_SET[r, c]);
                }
            }

            // Shrink from up
            var stop = false;

            for (int r = 0; r < tileSet.Count; r++)
            {
                foreach (var item in tileSet.First.Value)
                {
                    if (item.GetType().Name != EMPTY)
                        stop = true;
                }

                if (stop)
                    break;
                else
                {
                    tileSet.RemoveFirst();
                    upLevel++;
                }
            }

            // Shrink from down
            stop = false;

            for (int r = tileSet.Count - 1; r >= 0; r--)
            {
                foreach (var item in tileSet.Last.Value)
                {
                    if (item.GetType().Name != EMPTY)
                        stop = true;
                }

                if (stop)
                    break;
                else
                    tileSet.RemoveLast();
            }

            // Shrink from east
            stop = false;

            for (int c = 0; c < tileSet.First.Value.Count; c++)
            {
                foreach (var item in tileSet)
                {
                    if (item.First.Value.GetType().Name != EMPTY)
                        stop = true;
                }

                if (stop)
                    break;
                else
                {
                    foreach (var item in tileSet)
                    {
                        item.RemoveFirst();
                    }

                    westLevel++;
                }
            }

            // Shrink from west
            stop = false;

            for (int c = tileSet.First.Value.Count - 1; c >= 0; c--)
            {
                foreach (var item in tileSet)
                {
                    if (item.Last.Value.GetType().Name != EMPTY)
                        stop = true;
                }

                if (stop)
                    break;
                else
                {
                    foreach (var item in tileSet)
                    {
                        item.RemoveLast();
                    }
                }
            }

            // Convert to 2D array
            var res = new Tile[tileSet.Count, tileSet.First.Value.Count];

            var list = tileSet.First;

            for (int r = 0; r < res.GetLength(0); r++)
            {
                var tile = list.Value.First;

                for (int c = 0; c < res.GetLength(1); c++)
                {
                    res[r, c] = tile.Value;

                    tile = tile.Next;
                }

                list = list.Next;
            }

            return res;
        }

        // "Grow" the shrinked grid from all directions to achieve the final form of the TILE_SET i.e transfer
        // the solution to the TILE_SET
        private void GrowTileSet(Tile[,] tileSet, int upLevel, int westLevel)
        {
            for (int r = 0; r < tileSet.GetLength(0); r++)
            {
                for (int c = 0; c < tileSet.GetLength(1); c++)
                {
                    TILE_SET[upLevel + r, westLevel + c] = tileSet[r, c];
                }
            }
        }
    }
}
