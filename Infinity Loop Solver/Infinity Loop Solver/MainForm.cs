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

                // Make the problem easier
                FixSideTiles(tileSet);



                // Finalize the solution and transfer it to the TILE_SET
                GrowTileSet(tileSet, upLevel, westLevel);

                // Draw the solution
                DrawTiles();

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

                tile = TILE_SET[row, col];

                pictureBox.Image = tile.Image;

                CAN_BE_CLICKED = true;
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

        // Fixes the available side tiles according to a specific strategy for each tile type
        private void FixSideTiles(Tile[,] tileSet)
        {
            var rowNum = tileSet.GetLength(0);
            var colNum = tileSet.GetLength(1);

            // Iterate side tiles for Line and Junction
            for (int r = 0; r < rowNum; r++)
            {

            }

            for (int c = 0; c < colNum; c++)
            {

            }

            // Iterate side tiles for Turn
            for (int r = 1; r < rowNum - 1; r++)
            {

            }

            for (int c = 1; c < colNum - 1; c++)
            {

            }

            // Iterate side tiles for Oneway
            for (int r = 0; r < rowNum; r++)
            {
                var westTile = tileSet[r, 0];

                if (westTile.GetType().Name == ONEWAY)
                {
                    if (r == 0 && r + 1 < rowNum && IsFinalizedAndNotEmpty(tileSet[r + 1, 0]))
                    {
                        if (tileSet[r + 1, 0].North)
                        {
                            westTile.Rotate(2);
                            westTile.Finalized = true;
                        }
                    }
                    else if (r == rowNum - 1 && r - 1 >= 0 && IsFinalizedAndNotEmpty(tileSet[r - 1, 0]))
                    {
                        if (tileSet[r - 1, 0].South)
                        {
                            westTile.Rotate(0);
                            westTile.Finalized = true;
                        }
                    }
                    else if (0 < r && r < rowNum - 1)
                    {
                        if (IsFinalizedAndNotEmpty(tileSet[r - 1, 0]) && tileSet[r - 1, 0].South)
                        {
                            westTile.Rotate(0);
                            westTile.Finalized = true;
                        }
                        if (IsFinalizedAndNotEmpty(tileSet[r + 1, 0]) && tileSet[r + 1, 0].North)
                        {
                            westTile.Rotate(2);
                            westTile.Finalized = true;
                        }
                    }
                }

                var eastTile = tileSet[r, colNum - 1];

                if (eastTile.GetType().Name == ONEWAY)
                {
                    if (r == 0 && r + 1 < rowNum && IsFinalizedAndNotEmpty(tileSet[r + 1, colNum - 1]))
                    {
                        if (tileSet[r + 1, colNum - 1].North)
                        {
                            eastTile.Rotate(2);
                            eastTile.Finalized = true;
                        }
                    }
                    else if (r == rowNum - 1 && r - 1 >= 0 && IsFinalizedAndNotEmpty(tileSet[r - 1, colNum - 1]))
                    {
                        if (tileSet[r - 1, colNum - 1].South)
                        {
                            eastTile.Rotate(0);
                            eastTile.Finalized = true;
                        }
                    }
                    else if (0 < r && r < rowNum - 1)
                    {
                        if (IsFinalizedAndNotEmpty(tileSet[r - 1, colNum - 1]) && tileSet[r - 1, colNum - 1].South)
                        {
                            eastTile.Rotate(0);
                            eastTile.Finalized = true;
                        }
                        if (IsFinalizedAndNotEmpty(tileSet[r + 1, colNum - 1]) && tileSet[r + 1, colNum - 1].North)
                        {
                            eastTile.Rotate(2);
                            eastTile.Finalized = true;
                        }
                    }
                }
            }

            for (int c = 0; c < colNum; c++)
            {
                var upTile = tileSet[0, c];

                if (upTile.GetType().Name == ONEWAY)
                {
                    if (c == 0 && c + 1 < colNum && IsFinalizedAndNotEmpty(tileSet[0, c + 1]))
                    {
                        if(tileSet[0, c + 1].West)
                        {
                            upTile.Rotate(1);
                            upTile.Finalized = true;
                        }
                    }
                    else if (c == colNum - 1 && c - 1 >= 0 && IsFinalizedAndNotEmpty(tileSet[0, c - 1]))
                    {
                        if (tileSet[0, c - 1].East)
                        {
                            upTile.Rotate(3);
                            upTile.Finalized = true;
                        }
                    }
                    else if (0 < c && c < colNum - 1)
                    {
                        if(IsFinalizedAndNotEmpty(tileSet[0, c - 1]) && tileSet[0, c - 1].East)
                        {
                            upTile.Rotate(3);
                            upTile.Finalized = true;
                        }
                        if(IsFinalizedAndNotEmpty(tileSet[0, c + 1]) && tileSet[0, c - 1].West)
                        {
                            upTile.Rotate(1);
                            upTile.Finalized = true;
                        }
                    }
                }

                var downTile = tileSet[rowNum - 1, c];

                if (downTile.GetType().Name == ONEWAY)
                {
                    if (c == 0 && c + 1 < colNum && IsFinalizedAndNotEmpty(tileSet[rowNum - 1, c + 1]))
                    {
                        if (tileSet[rowNum - 1, c + 1].West)
                        {
                            downTile.Rotate(1);
                            downTile.Finalized = true;
                        }
                    }
                    else if (c == colNum - 1 && c - 1 >= 0 && IsFinalizedAndNotEmpty(tileSet[rowNum - 1, c - 1]))
                    {
                        if (tileSet[rowNum - 1, c - 1].East)
                        {
                            upTile.Rotate(3);
                            upTile.Finalized = true;
                        }
                    }
                    else if (0 < c && c < colNum - 1)
                    {
                        if (IsFinalizedAndNotEmpty(tileSet[rowNum - 1, c - 1]) && tileSet[rowNum - 1, c - 1].East)
                        {
                            upTile.Rotate(3);
                            upTile.Finalized = true;
                        }
                        if (IsFinalizedAndNotEmpty(tileSet[rowNum - 1, c + 1]) && tileSet[rowNum - 1, c - 1].West)
                        {
                            upTile.Rotate(1);
                            upTile.Finalized = true;
                        }
                    }
                }
            }
        }

        // Returns true if the given tile is Finalized and its type is not Empty
        private bool IsFinalizedAndNotEmpty(Tile tile)
        {
            return tile.Finalized && tile.GetType().Name != EMPTY;
        }
    }
}
