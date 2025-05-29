using KSU.CIS300.Snake;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KSU.CIS300.SnakeTests
{
    /// <summary>
    /// This class contains unit tests for the GameBoard class.
    /// </summary>
    /// 
    [TestFixture]
    public class BGameBoardTests
    {
        /// <summary>
        /// The longest and shortest timeouts for the tests.
        /// </summary>
        private const int _longestMaxTimeout = 1000;
        /// <summary>
        /// The shortest timeout for the tests.
        /// </summary>
        private const int _shortestMaxTimeout = 1000;

        /// <summary>
        /// Sets up the game board with the specified size.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        /// <returns>a new game board of the given size</returns>
        public GameBoard SetBoard(int boardSize)
        {
            GameBoard gb = new GameBoard(boardSize);
            return gb;
        }

        #region test for the GameBoard constructor

        /// <summary>
        /// Tests the GameBoard constructor to ensure it initializes the properties correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("B-Constructor")]
        public void TestBB1GameBoard_ConstructBase(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            Assert.That(gb.Grid.GetLength(0), Is.EqualTo(boardSize));
            Assert.That(gb.Grid.GetLength(1), Is.EqualTo(boardSize));
            int count = 0;

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    Assert.That(gb.Grid[x, y].X, Is.EqualTo(x));
                    Assert.That(gb.Grid[x, y].Y, Is.EqualTo(y));
                    if (GridData.Empty == gb.Grid[x, y].Data)
                    {
                        count++;
                    }
                }
            }
            Assert.That(count, Is.GreaterThanOrEqualTo(boardSize * boardSize - 2), "Board should be empty except one food and the snake head");
        }

        /// <summary>
        /// Tests the GameBoard constructor to ensure it initializes the snake head and tail correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("B-Constructor")]
        public void TestBB2GameBoard_ConstructHeadsNTails(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            Assert.That(gb.Grid[5, 5].Data, Is.EqualTo(GridData.SnakeHead));
            Assert.That(gb.Head, Is.SameAs(gb.Grid[5, 5]));
            Assert.That(gb.Tail, Is.SameAs(gb.Grid[5, 5]));
        }

        /// <summary>
        /// Tests the GameBoard constructor to ensure it initializes the food correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("B-Constructor")]
        [Category("C-Food")]
        public void TestBC3GameBoard_ConstructFood(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            int food = 0;
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (gb.Grid[x, y].Data == GridData.SnakeFood)
                    {
                        food++;
                    }
                }
            }
            Assert.That(food, Is.EqualTo(1), "Board should have only one piece of food");
        }

        #endregion

        /// <summary>
        /// Tests the GameBoard constructor to ensure it initializes the food correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("C-Food")]
        public void TestBC1GameBoard_AddFood(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            for (int i = 0; i < 90; i++)
            {
                gb.AddFood();
            }
            int food = 0;
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (gb.Grid[x, y].Data == GridData.SnakeFood)
                    {
                        food++;
                    }
                }
            }
            Assert.That(food, Is.EqualTo(91));
        }

        #region tests for the GetNextNode method

        /// <summary>
        /// Tests the GetNextNode method to ensure it returns the correct node in the given direction.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD1GameBoard_GetNextNodeDown(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[0, 8];
            GameNode next = gb.GetNextNode(Direction.Down, gb.Head);
            Assert.That(next, Is.EqualTo(gb.Grid[0, 9]));
        }

        /// <summary>
        /// Tests the GetNextNode method to ensure it returns the correct node in the given direction.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD2GameBoard_GetNextNodeUp(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[0, 1];
            GameNode next = gb.GetNextNode(Direction.Up, gb.Head);
            Assert.That(next, Is.EqualTo(gb.Grid[0, 0]));
        }

        /// <summary>
        ///     Tests the GetNextNode method to ensure it returns the correct node in the given direction.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD3GameBoard_GetNextNodeRight(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[8, 1];
            GameNode next = gb.GetNextNode(Direction.Right, gb.Head);
            Assert.That(next, Is.EqualTo(gb.Grid[9, 1]));
        }

        /// <summary>
        /// Tests the GetNextNode method to ensure it returns the correct node in the given direction.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD4GameBoard_GetNextNodeLeft(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[1, 0];
            GameNode next = gb.GetNextNode(Direction.Left, gb.Head);
            Assert.That(next, Is.EqualTo(gb.Grid[0, 0]));
        }

        /// <summary>
        ///     Tests the GetNextNode method to ensure it returns null when the next node is out of bounds.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD5GameBoard_GetNextNodeFailDown(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[0, 9];
            GameNode next = gb.GetNextNode(Direction.Down, gb.Head);
            Assert.That(next, Is.Null);
        }

        /// <summary>
        /// No node above the head
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD6GameBoard_GetNextNodeFailUp(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[0, 0];
            GameNode next = gb.GetNextNode(Direction.Up, gb.Head);
            Assert.That(next, Is.Null);
        }

        /// <summary>
        /// No node to the right of the head
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD7GameBoard_GetNextNodeFailRight(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[9, 1];
            GameNode next = gb.GetNextNode(Direction.Right, gb.Head);
            Assert.That(next, Is.Null);
        }

        /// <summary>
        /// No node to the left of the head
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("D-GetNextNode")]
        public void TestBD8GameBoard_GetNextNodeFailLeft(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Head = gb.Grid[0, 0];
            GameNode next = gb.GetNextNode(Direction.Left, gb.Head);
            Assert.That(next, Is.Null);
        }

        #endregion

        #region Tests for getting the path of the snake

        /// <summary>
        /// Tests the GetSnakePath method to ensure it returns the correct path.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("E-GetSnakePath")]
        public void TestBE1GameBoard_Base(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            VerifyPathLength(1, gb);
        }

        /// <summary>
        /// Tests the GetSnakePath method to ensure it returns the correct path.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("E-GetSnakePath")]
        public void TestBE2GameBoard_Simple(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            gb.Grid[5, 5].Data = GridData.SnakeBody;
            gb.Grid[5, 5].SnakeEdge = gb.Grid[5, 6];
            gb.Grid[5, 6].Data = GridData.SnakeHead;
            gb.Tail = gb.Grid[5, 5];
            gb.Head = gb.Grid[5, 6];
            VerifyPathLength(2, gb);
        }

        /// <summary>
        /// Tests the GetSnakePath method to ensure it returns the correct path.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("E-GetSnakePath")]
        public void TestBE2GameBoard_Long(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            for (int i = 5; i >= 1; i--)
            {
                gb.Grid[i, 5].Data = GridData.SnakeBody;
                gb.Grid[i, 5].SnakeEdge = gb.Grid[i - 1, 5];
            }
            gb.Grid[0, 5].Data = GridData.SnakeBody;
            gb.Grid[0, 5].SnakeEdge = gb.Grid[0, 4];

            gb.Grid[0, 4].Data = GridData.SnakeBody;
            gb.Grid[0, 4].SnakeEdge = gb.Grid[1, 4];

            gb.Grid[1, 4].Data = GridData.SnakeBody;
            gb.Grid[1, 4].SnakeEdge = gb.Grid[2, 4];

            gb.Grid[2, 4].Data = GridData.SnakeHead;
            gb.Head = gb.Grid[2, 4];
            gb.Tail = gb.Grid[5, 5];
            VerifyPathLength(9, gb);
        }
        #endregion

        #region Test basic snake movements

        /// <summary>
        /// Tests the MoveSnake method to ensure it moves the snake correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("F-MoveSnake")]
        public void TestBF1GameBoard_MoveSnakeWallCollision(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);

            gb.Head = gb.Grid[0, 0];
            SnakeStatus s = gb.MoveSnake(Direction.Up);
            Assert.That(s, Is.EqualTo(SnakeStatus.Collision));
        }

        /// <summary>
        /// Tests the MoveSnake method to ensure it moves the snake correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("F-MoveSnake")]
        public void TestBF2GameBoard_MoveSnakeNoCut(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);
            Assert.That(gb.MoveSnake(Direction.Right), Is.EqualTo(SnakeStatus.Moving));
            Assert.That(gb.Head, Is.EqualTo(gb.Grid[boardSize / 2 + 1, boardSize / 2]));
            Assert.That(gb.Head, Is.Not.EqualTo(gb.Tail));
            VerifyPathLength(2, gb);
        }

        /// <summary>
        /// Tests the MoveSnake method to ensure it moves the snake correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("F-MoveSnake")]
        public void TestBF3GameBoard_MoveSnakeCutTail(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);
            Assert.That(gb.MoveSnake(Direction.Right), Is.EqualTo(SnakeStatus.Moving));
            Assert.That(gb.MoveSnake(Direction.Up), Is.EqualTo(SnakeStatus.Moving));
            Assert.That(gb.Head, Is.Not.EqualTo(gb.Tail));
            VerifyPathLength(2, gb);
        }

        /// <summary>
        /// Tests the MoveSnake method to ensure it moves the snake correctly.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("F-MoveSnake")]
        public void TestBF4GameBoard_MoveSnakeInvalid(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);
            Assert.That(gb.MoveSnake(Direction.Right), Is.EqualTo(SnakeStatus.Moving));
            Assert.That(gb.MoveSnake(Direction.Right), Is.EqualTo(SnakeStatus.Moving));
            Assert.That(gb.MoveSnake(Direction.Left), Is.EqualTo(SnakeStatus.InvalidDirection));
            VerifyPathLength(2, gb);
        }

        /// <summary>
        /// Tests the MoveSnake method to ensure it moves the snake correctly. Eats food.
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("F-MoveSnake")]
        public void TestBF5GameBoard_MoveSnakeEatFood(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            Assert.That(gb.MoveSnake(Direction.Left), Is.EqualTo(SnakeStatus.Moving));
            Assert.That(gb.MoveSnake(Direction.Up), Is.EqualTo(SnakeStatus.Moving));
            ResetFood(4, 3, gb);
            Assert.That(gb.MoveSnake(Direction.Up), Is.EqualTo(SnakeStatus.Eating));
            Assert.That(gb.Food.Data, Is.EqualTo(GridData.SnakeFood));
            Assert.That(gb.Food, Is.Not.SameAs(gb.Grid[4, 3]));
            VerifyPathLength(3, gb);
        }

        /// <summary>
        /// Tests the MoveSnake method to ensure it moves the snake correctly. Body collision
        /// </summary>
        /// <param name="boardSize">size of board</param>
        [TestCase(10)]
        [Category("F-MoveSnake")]
        public void TestBF6GameBoard_MoveSnakeBodyCollision(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(4, 5, gb);

            Assert.That(gb.MoveSnake(Direction.Left), Is.EqualTo(SnakeStatus.Eating));

            gb.Grid[3, 5].Data = GridData.SnakeFood;
            gb.Food = gb.Grid[3, 5];
            Assert.That(gb.MoveSnake(Direction.Left), Is.EqualTo(SnakeStatus.Eating));

            gb.Grid[3, 6].Data = GridData.SnakeFood;
            gb.Food = gb.Grid[3, 6];
            Assert.That(gb.MoveSnake(Direction.Down), Is.EqualTo(SnakeStatus.Eating));

            gb.Grid[3, 7].Data = GridData.SnakeFood;
            gb.Food = gb.Grid[3, 7];
            Assert.That(gb.MoveSnake(Direction.Down), Is.EqualTo(SnakeStatus.Eating));

            gb.Grid[4, 7].Data = GridData.SnakeFood;
            gb.Food = gb.Grid[4, 7];
            Assert.That(gb.MoveSnake(Direction.Right), Is.EqualTo(SnakeStatus.Eating));

            gb.Grid[4, 6].Data = GridData.SnakeFood;
            gb.Food = gb.Grid[4, 6];
            Assert.That(gb.MoveSnake(Direction.Up), Is.EqualTo(SnakeStatus.Eating));
            Assert.That(gb.MoveSnake(Direction.Left), Is.EqualTo(SnakeStatus.Collision));
            VerifyPathLength(7, gb);
        }

        #endregion

        #region Tests for finding the path for the AI
        /// <summary>
        /// Tests the FindLongestAiPath method to ensure it finds the correct path.
        /// </summary>
        /// <param name="boardSize">siaze of board</param>
        [TestCase(4), TestCase(6), TestCase(8),
            TestCase(10), TestCase(12), TestCase(16), TestCase(26)]
        [Timeout(_longestMaxTimeout)]
        [Category("H-AIPath")]
        public void TestBH1GameBoard_LongestAIpath(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);
            gb.MoveSnake(Direction.Up);

            Queue<Direction> path = gb.FindLongestAiPath();
            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(gb.Grid.Length));

            VerifyPath(gb.Head, new List<Direction>(path.ToArray()), gb);
        }

        /// <summary>
        /// Tests the FindShortestAiPath method to ensure it finds the correct path.
        /// </summary>
        /// <param name="boardSize">board size</param>
        [TestCase(3), TestCase(4), TestCase(5), TestCase(7),
            TestCase(10), TestCase(20), TestCase(30)]
        [Timeout(_shortestMaxTimeout)]
        [Category("H-AIPath")]
        public void TestBH3GameBoard_ShortestAIpath(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);
            gb.MoveSnake(Direction.Up);

            List<Direction> path = gb.FindShortestAiPath(gb.Food);
            Assert.That(path, Is.Not.Null);

            Assert.That(path.Count, Is.GreaterThanOrEqualTo(1));
            VerifyPath(gb.Food, path, gb);
        }

        /// <summary>
        /// Tests the FindShortestAiPath method to ensure it finds the correct path.
        /// </summary>
        /// <param name="boardSize">board size</param>
        [TestCase(3), TestCase(4), TestCase(5), TestCase(7),
            TestCase(10), TestCase(20), TestCase(30)]
        [Timeout(_shortestMaxTimeout)]
        [Category("H-AIPath")]
        public void TestBH4GameBoard_ShortestAIpath_Snake2(int boardSize)
        {
            GameBoard gb = SetBoard(boardSize);
            ResetFood(0, 0, gb);
            gb.MoveSnake(Direction.Up);

            List<Direction> path = gb.FindShortestAiPath(gb.Food);
            Assert.That(path, Is.Not.Null);

            Assert.That(path.Count, Is.GreaterThanOrEqualTo(1));
            VerifyPath(gb.Food, path, gb);
        }
        #endregion

        /// <summary>
        /// Verifies that the given path leads to the destination from the snake's head
        /// </summary>
        /// <param name="destination">goal node</param>
        /// <param name="path">path</param>
        /// <param name="gb">game board</param>
        private void VerifyPath(GameNode destination, List<Direction> path, GameBoard gb)
        {
            bool[,] visited = new bool[gb.Grid.GetLength(0), gb.Grid.GetLength(0)];
            foreach (Direction dir in path)
            {
                gb.MoveSnake(dir);
                Assert.That(visited[gb.Head.X, gb.Head.Y], Is.False);
                visited[gb.Head.X, gb.Head.Y] = true;
            }
            Assert.That(gb.Head, Is.EqualTo(destination));
        }

        /// <summary>
        /// Verifies that the snake is of the specified length
        /// </summary>
        /// <param name="length">expected length</param>
        /// <param name="gb">game board</param>
        private void VerifyPathLength(int length, GameBoard gb)
        {
            int x = gb.Tail.X;
            int y = gb.Tail.Y;
            List<GameNode> path = gb.GetSnakePath();

            Assert.That(path.Count, Is.EqualTo(length));
            Assert.That(path[0].X, Is.EqualTo(x));
            Assert.That(path[0].Y, Is.EqualTo(y));
            for (int i = 1; i < path.Count; i++)
            {
                GameNode node = path[i];
                Assert.That(x == node.X && y == node.Y, Is.False);
                Assert.That(Math.Abs(x - node.X), Is.LessThanOrEqualTo(1));
                Assert.That(Math.Abs(y - node.Y), Is.LessThanOrEqualTo(1));
                x = node.X;
                y = node.Y;
            }
            Assert.That(path[0], Is.SameAs(gb.Tail));
            Assert.That(path[path.Count - 1], Is.SameAs(gb.Head));
        }

        /// <summary>
        /// Forces the food to the given location
        /// </summary>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        /// <param name="gb">game board</param>
        private void ResetFood(int x, int y, GameBoard gb)
        {
            gb.Food.Data = GridData.Empty;
            gb.Food = gb.Grid[x, y];
            gb.Food.Data = GridData.SnakeFood;
        }
    }
}
