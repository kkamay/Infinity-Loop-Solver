using System.Drawing;

namespace Infinity_Loop_Solver
{
    #region Tile

    public class Tile
    {
        // Tile's position is finalized
        protected bool Finalized { get; set; }

        // Edges exist
        protected bool North { get; set; }
        protected bool East { get; set; }
        protected bool South { get; set; }
        protected bool West { get; set; }

        // Edges connected
        protected bool NorthC { get; set; }
        protected bool EastC { get; set; }
        protected bool SouthC { get; set; }
        protected bool WestC { get; set; }

        // Tile image
        public Bitmap Image { get; set; }

        // Type that determines the orientation of the tile
        public int? Type { get; set; }

        // Rotates the tile in clockwise direction
        public void Rotate() { }
    }

    #endregion

    #region Empty

    // Empty Tile without any edges
    public class Empty : Tile
    {
        public Empty()
        {
            Finalized = true;
            North = East = South = West = false;
            NorthC = EastC = SouthC = WestC = true;
            Image = Resource.E;
            Type = null;
        }
    }

    #endregion

    #region Line

    // Line Tile with 2 direct edges
    public class Line : Tile
    {
        public Line()
        {
            Finalized = false;
            North = South = true;
            East = West = false;
            NorthC = EastC = SouthC = WestC = false;
            Image = Resource.L0;
            Type = 0;
        }

        public new void Rotate()
        {
            if (Type == 0)
            {
                North = South = false;
                East = West = true;
                Image = Resource.L1;
                Type = 1;
            }
            else
            {
                North = South = true;
                East = West = false;
                Image = Resource.L0;
                Type = 0;
            }
        }
    }

    #endregion

    #region Turn

    // Turn Tile with 2 consecutive edges i.e. not straight, but curvy
    public class Turn : Tile
    {
        public Turn()
        {
            Finalized = false;
            North = East = true;
            South = West = false;
            NorthC = EastC = SouthC = WestC = false;
            Image = Resource.T0;
            Type = 0;
        }

        public new void Rotate()
        {
            switch (Type)
            {
                case 0:
                    East = South = true;
                    West = North = false;
                    Image = Resource.T1;
                    Type = 1;
                    break;
                case 1:
                    South = West = true;
                    North = East = false;
                    Image = Resource.T2;
                    Type = 2;
                    break;
                case 2:
                    West = North = true;
                    East = South = false;
                    Image = Resource.T3;
                    Type = 3;
                    break;
                case 3:
                    North = East = true;
                    South = West = false;
                    Image = Resource.T0;
                    Type = 0;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    #region One Way

    // One Way Tile that has only 1 edge
    public class OneWay : Tile
    {
        public OneWay()
        {
            Finalized = false;
            North = true;
            East = South = West = false;
            NorthC = EastC = SouthC = WestC = false;
            Image = Resource.O0;
            Type = 0;
        }

        public new void Rotate()
        {
            switch (Type)
            {
                case 0:
                    East = true;
                    North = South = West = false;
                    Image = Resource.O1;
                    Type = 1;
                    break;
                case 1:
                    South = true;
                    North = East = West = false;
                    Image = Resource.O2;
                    Type = 2;
                    break;
                case 2:
                    West = true;
                    North = South = East = false;
                    Image = Resource.O3;
                    Type = 3;
                    break;
                case 3:
                    North = true;
                    East = South = West = false;
                    Image = Resource.O0;
                    Type = 0;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    #region Junction

    // 3-Way Junction Tile that has 3 edges
    public class Junction : Tile
    {
        public Junction()
        {
            Finalized = false;
            North = East = South = true;
            West = false;
            NorthC = EastC = SouthC = WestC = false;
            Image = Resource.J0;
            Type = 0;
        }

        public new void Rotate()
        {
            switch (Type)
            {
                case 0:
                    East = South = West = true;
                    North = false;
                    Image = Resource.J1;
                    Type = 1;
                    break;
                case 1:
                    North = West = South = true;
                    East = false;
                    Image = Resource.J2;
                    Type = 2;
                    break;
                case 2:
                    North = East = West = true;
                    South = false;
                    Image = Resource.J3;
                    Type = 3;
                    break;
                case 3:
                    North = East = South = true;
                    West = false;
                    Image = Resource.J0;
                    Type = 0;
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    #region Roundabout

    // Roundabout Tile that has 4 edges in all directions
    public class Roundabout : Tile
    {
        public Roundabout()
        {
            Finalized = true;
            North = East = South = West = true;
            NorthC = EastC = SouthC = WestC = false;
            Image = Resource.R;
            Type = null;
        }
    }

    #endregion
}
