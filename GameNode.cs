namespace KSU.CIS300.Snake
{
    /// <summary>
    /// Game Node Class
    /// </summary>
    public class GameNode
    {

        /// <summary>
        /// X-coordinate on the grid
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///  // Y-coordinate on the grid
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// currently in this cell
        /// </summary>
        public GridData Data { get; set; }

        /// <summary>
        ///  Link to the next piece in the snake 
        /// </summary>
        public GameNode SnakeEdge { get; set; }

        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GameNode(int x, int y)
        {
            X = x;
            Y = y;
            Data = GridData.Empty;
            SnakeEdge = null;
        }


    }
}
