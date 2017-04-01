namespace Infinity_Loop_Solver
{
    public class Tile
    {
        // Tile's position is finalized
        protected bool Finalized;

        // Edges exist
        protected bool North;
        protected bool East;
        protected bool South;
        protected bool West;

        // Edges connected
        protected bool NorthC;
        protected bool EastC;
        protected bool SouthC;
        protected bool WestC;

        // Tile image
        protected string Image;

        // Rotates the tile in clockwise direction
        public void Rotate() { }
    }

    // Empty Tile without any edges
    public class Empty : Tile
    {
        public Empty()
        {
            Finalized = true;
            North = East = South = West = false;
            NorthC = EastC = SouthC = WestC = true;
            Image = "../Resources/E.png";
        }
    }
}
