namespace Core
{
    public class Utils
    {
        public static int Coords2DTo1D(int x, int y, int gridSize)
        {
            return y * gridSize + x;
        }
        
        public static (int, int) Coords1DTo2D(int index, int gridSize)
        {
            var x = index % gridSize;
            var y = index / gridSize;
            return (x, y);
        }
    }
}