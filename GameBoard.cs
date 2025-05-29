using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSU.CIS300.Snake
{
    /// <summary>
    /// Game Board
    /// </summary>
    public class GameBoard
    {
        /// <summary>
        /// The node containing the food.
        /// </summary>
        public GameNode Food { get; set; }

        /// <summary>
        /// 2D grid of GameNodes representing the board.
        /// </summary>
        public GameNode[,] Grid { get; private set; }

        /// <summary>
        /// Node where the snake's head is currently located.
        /// </summary>
        public GameNode Head { get; set; }

        /// <summary>
        /// Node where the snake's tail is currently located.
        /// </summary>
        public GameNode Tail { get; set; }

        /// <summary>
        /// Current size of the snake.
        /// </summary>
        public int SnakeSize { get; private set; }
        /// <summary>
        /// Size 
        /// </summary>
        private int _size;

        /// <summary>
        /// Directions used by the AI to check movement
        /// </summary>
        private Direction[] _aiDirection = new Direction[]
        {
        Direction.Up, Direction.Left, Direction.Right, Direction.Down
        };

        /// <summary>
        /// Directions used when current movement is vertical Left and Right.
        /// </summary>
        private Direction[] _leftRight = new Direction[] { Direction.Left, Direction.Right };

        /// <summary>
        /// Directions used when current movement is horizontal Up and Down.
        /// </summary>
        private Direction[] _upDown = new Direction[] { Direction.Up, Direction.Down };

        /// <summary>
        /// Random number generator used for food placement.
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// Initializes a new game board of given size.
        /// </summary>
        /// <param name="size">Size of the board (n x n).</param>
        public GameBoard(int size)
        {
            _size = size;
            Grid = new GameNode[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Grid[x, y] = new GameNode(x, y);
                }
            }

            int center = size / 2;

            Head = Grid[center, center];
            Head.Data = GridData.SnakeHead;
            Tail = Grid[center , center];
            SnakeSize = 1;

            AddFood();
        }

        /// <summary>
        /// Randomly places a new food on an empty node.
        /// </summary>
        public void AddFood()
        {
            List<GameNode> emptyNodes = new List<GameNode>();
            foreach (GameNode node in Grid)
            {
                if (node.Data == GridData.Empty)
                {
                    emptyNodes.Add(node);
                }
            }

            if (emptyNodes.Count > 0)
            {
                GameNode foodNode = emptyNodes[_random.Next(emptyNodes.Count)];
                foodNode.Data = GridData.SnakeFood;
                Food = foodNode;
            }
            else
            {
                Food = null;
            }
        }

        /// <summary>
        /// Gets the next node in the given direction from the current node.
        /// </summary>
        /// <param name="dir">Direction to move.</param>
        /// <param name="current">Current node.</param>
        /// <returns>The next GameNode or null if out of bounds.</returns>
        public GameNode GetNextNode(Direction dir, GameNode current)
        {
            int x = current.X;
            int y = current.Y;

            switch (dir)
            {
                case Direction.Up: y--; break;
                case Direction.Down: y++; break;
                case Direction.Left: x--; break;
                case Direction.Right: x++; break;
            }

            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return null;

            return Grid[x, y];
        }

        /// <summary>
        /// Moves the snake in the given direction.
        /// </summary>
        /// <param name="dir">Direction to move.</param>
        /// <returns>Status of the snake after moving.</returns>
        public SnakeStatus MoveSnake(Direction dir)
        {
            GameNode next = GetNextNode(dir, Head);

            if (next == null)
                return SnakeStatus.Collision;

            if (next.SnakeEdge == Head)
                return SnakeStatus.InvalidDirection;

            if (next.Data == GridData.SnakeBody )
                return SnakeStatus.Collision;

            GridData temp = next.Data;

           
            Head.Data = GridData.SnakeBody;
            next.Data = GridData.SnakeHead;
         
            Head.SnakeEdge = next;
           

            if (temp == GridData.SnakeFood)
            {
                Head = next;
                SnakeSize++;
                if (SnakeSize == _size * _size)
                    return SnakeStatus.Win;

                AddFood();
                return SnakeStatus.Eating;
            }
            else
            {
                if (Head != Tail)
                {
                    Tail.Data = GridData.Empty;
                    GameNode oldTail = Tail.SnakeEdge;
                 Tail.SnakeEdge = null;
                    Tail= oldTail;
                   
                }
                else
                {
                    SnakeSize++;
                   
                }
                Head = next;

                return SnakeStatus.Moving;
            }

        }

        /// <summary>
        /// Gets the full snake body path from tail to head.
        /// </summary>
        /// <returns>List of GameNodes representing the snake's body.</returns>
        public List<GameNode> GetSnakePath()
        {
            List<GameNode> path = new List<GameNode>();
            GameNode current = Tail;
            while (current != null)
            {
                path.Add(current);
                current = current.SnakeEdge;
            }
            return path;
        }

        /// <summary>
        /// Reconstructs the path from a dictionary of visited nodes.
        /// </summary>
        /// <param name="path">Dictionary of reverse paths.</param>
        /// <param name="dest">Destination node.</param>
        /// <returns>List of directions from head to destination.</returns>
        private List<Direction> BuildPath(Dictionary<GameNode, (GameNode, Direction)> path, GameNode dest)
        {
            List<Direction> result = new List<Direction>();
            GameNode current = dest;
            while (current!= Head)
            {
                GameNode previous = path[current].Item1;
                Direction direction = path[current].Item2;
                result.Insert(0, direction);
                current = previous;
            }
            return result;
        }
        /// <summary>
        /// Helper Method to Find Adjacent
        /// </summary>
        /// <param name="node">c</param>
        /// <param name="alTail">c</param>
        /// <returns>c</returns>
        public List<(GameNode, GameNode, Direction )> FindAdjacent( GameNode node, bool alTail )
        {
            List<(GameNode, GameNode, Direction)> result= new List<(GameNode, GameNode, Direction)>();
            GameNode temp;
            foreach (Direction dir in _aiDirection)
            {
                temp  = GetNextNode(dir, node);
                if (temp != null && ((alTail&& temp == Tail && Tail.SnakeEdge!= node) || temp.Data != GridData.SnakeBody)&& 
                    temp.Data!= GridData.SnakeHead)
                {
                  result.Add((node, temp,  dir));
                }
            }
            return result;
        }

        /// <summary>
        /// Finds the shortest valid path from the head to the given destination.
        /// </summary>
        /// <param name="dest">Destination node.</param>
        /// <returns>List of directions from head to destination.</returns>
        public List<Direction> FindShortestAiPath(GameNode dest)
        {
            Queue<(GameNode from, GameNode to, Direction dir)> queue = new Queue<(GameNode, GameNode, Direction)>();
            Dictionary<GameNode, (GameNode, Direction)> visited = new Dictionary<GameNode, (GameNode, Direction)>();
            visited[Head] = (Head, Direction.None);

            bool tailCheck = dest== Tail;

            foreach ((GameNode, GameNode,Direction) Nodee in FindAdjacent(Head, tailCheck) )
            {
                queue.Enqueue(Nodee);
               
            }

            while (queue.Count > 0)
            {
                (GameNode from, GameNode to, Direction dir) = queue.Dequeue();

                if (!visited.ContainsKey(to))
                {
                    visited[to] = (from, dir);
                   

                    if (to == dest)
                        return BuildPath(visited, dest);

                    foreach ((GameNode, GameNode, Direction) Nodee in FindAdjacent(to, tailCheck))
                    {
                        queue.Enqueue(Nodee);

                    }
                }
                    

               
            }

            return new List<Direction>();
        }

        /// <summary>
        /// Builds a Hamiltonian path from head to tail by extending shortest path.
        /// </summary>
        /// <returns>Queue of directions representing the path.</returns>
        public Queue<Direction> FindLongestAiPath()
        {
            List<Direction> path = FindShortestAiPath(Tail);
            if (path.Count == 0) return null;
            bool[,] visited= new bool[_size, _size];
            GameNode current = Head;
            Direction[] extension = null;
            visited[Head.X, Head.Y] = true;

            List<GameNode> nodes = new List<GameNode> { current };
            GameNode temp = Head;


            foreach (Direction dir in path)
            {
                
                temp  = GetNextNode(dir, temp);
                visited[temp.X, temp.Y] = true;
                
            }

            int i = 0;
            while (i < path.Count )
            {
                Direction dir = path[i];
                GameNode next = GetNextNode(dir, current);
                extension = (dir == Direction.Up || dir == Direction.Down) ? _leftRight : _upDown;
                bool pathExtended = false;



                foreach (Direction ext in extension)
                {
                    GameNode extA = GetNextNode(ext, current);
                    GameNode extB = GetNextNode(ext, next);

                    if (extA != null && extB != null && !visited[extA.X,extA.Y] && !visited[extB.X, extB.Y])
                    {
                        path.Insert(i, ext);
                        path.Insert(i + 2, Opposite(ext));

                        visited[extA.X, extA.Y] = true;
                        visited[extB.X, extB.Y] = true;
                        pathExtended = true;
                        break;
                    }
                }
                if(pathExtended== false)
                {
                    i++;
                    current = next;
                }
               
            }
            while (current != Head)
            {
                if(current.SnakeEdge.X- current.X==0)
                {
                    if (current.SnakeEdge.Y - current.Y == -1)
                    {
                        path.Add(Direction.Up);
                    }
                    else
                    {
                        path.Add(Direction.Down);
                    }

                }
                else
                {
                    if (current.SnakeEdge.X - current.X == -1)
                    {
                        path.Add(Direction.Right);
                    }
                    else
                    {
                        path.Add(Direction.Left);
                    }
                }
                current= current.SnakeEdge;

            }

            return new Queue<Direction>(path);
        }

        /// <summary>
        /// Gets the opposite direction.
        /// </summary>
        /// <param name="dir">Original direction.</param>
        /// <returns>Opposite direction.</returns>
        private Direction Opposite(Direction dir)
        {
            return dir switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => dir
            };
        }
    }

}
