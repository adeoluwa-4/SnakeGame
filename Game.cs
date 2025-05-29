using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KSU.CIS300.Snake
{
    /// <summary>
    /// Game Class
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The main game board.
        /// </summary>
        public GameBoard Board { get; private set; }

        /// <summary>
        /// Indicates whether the game is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Delay in milliseconds between each snake move.
        /// </summary>
        private int _delay;

        /// <summary>
        /// Indicates whether AI is enabled.
        /// </summary>
        private bool _useAi;
        /// <summary>
        /// Whether AI is controlling the snake
        /// </summary>
        public bool ISAI;

        /// <summary>
        /// Queue of directions for AI to follow.
        /// </summary>
        private Queue<Direction> _aiPath;

        /// <summary>
        /// Stores the most recent key press direction.
        /// </summary>
        public Direction KeyPress { get; private set; }

        /// <summary>
        /// Gets the size of the board.
        /// </summary>
        public int Size => Board.Grid.GetLength(0);

        /// <summary>
        /// Gets whether the game is currently active.
        /// </summary>
        public bool Play => IsRunning;

        /// <summary>
        /// Gets the current snake size (score).
        /// </summary>
        public int Score => Board.SnakeSize;

        /// <summary>
        /// Event triggered when the game board updates.
        /// </summary>
        public event Action Updated;

        /// <summary>
        /// Event triggered when the score changes.
        /// </summary>
        public event Action<int> ScoreChanged;

        /// <summary>
        /// Event triggered when the game ends.
        /// </summary>
        public event Action<string> GameEnded;

        /// <summary>
        /// Initializes a new Game instance.
        /// </summary>
        /// <param name="size">Size of the game board.</param>
        /// <param name="delay">Delay between moves in milliseconds.</param>
        /// <param name="useAi">Whether to use AI mode.</param>
        public Game(int size, int delay, bool useAi)
        {
            ISAI = useAi;
           
            Board = new GameBoard(size);
            _delay = delay;
          
            _aiPath = new Queue<Direction>();
            IsRunning = true;
            Board.MoveSnake(Direction.Up);
        }

        /// <summary>
        /// Starts the asynchronous game clock. Each tick moves the snake,
        /// </summary>
        /// <param name="progress">d</param>
        /// <param name="cancelToken">d</param>
        /// <returns>d</returns>
        public async Task StartMoving(IProgress<SnakeStatus> progress, CancellationToken cancelToken)
        {
            while (Play && !cancelToken.IsCancellationRequested)
            {
                SnakeStatus status;

                if (ISAI)
                {
                    // Refill AI path if empty
                    if (_aiPath.Count == 0)
                    {
                        _aiPath = Board.FindLongestAiPath();
                    }
                    // Dequeue next AI move, use it, then enqueue it again
                    Direction nextMove = _aiPath.Dequeue();
                    KeyPress = nextMove;
                    _aiPath.Enqueue(nextMove);
                }

                // Move the snake in whatever KeyPress currently is
                status = Board.MoveSnake(KeyPress);

                // Tell the UI what happened
                progress.Report(status);

                // Handle collision  stop game
                if (status == SnakeStatus.Collision)
                {
                    GameEnded?.Invoke("Game Over");
                    IsRunning = false;
                }
                // Handle eating score grew by one in MoveSnake
                else if (status == SnakeStatus.Eating)
                {
                    ScoreChanged?.Invoke(Score);
                }
                // Handle win  score grew, then end game
                else if (status == SnakeStatus.Win)
                {
                    ScoreChanged?.Invoke(Score);
                    GameEnded?.Invoke("You Win!");
                    IsRunning = false;
                }

                // Let the UI redraw
                Updated?.Invoke();

                // Wait for the next tick, or break early if cancelled
                try
                {
                    await Task.Delay(_delay, cancelToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Moves the snake in the given direction in manual mode.
        /// </summary>
        /// <param name="dir">The direction to move the snake.</param>
        public void Move(Direction dir)
        {
            if (!ISAI && IsRunning)
            {
                KeyPress = dir;
                SnakeStatus result = Board.MoveSnake(dir);
                HandleStatus(result);
                Updated?.Invoke();
            }
        }

        /// <summary>
        /// Moves the snake up 
        /// </summary>
        public void MoveUp() => Move(Direction.Up);

        /// <summary>
        /// Moves the snake down (used for testing or input handling).
        /// </summary>
        public void MoveDown()
        {
            if (Play && !ISAI)
            {
                KeyPress = Direction.Down;
            }
        }


        /// <summary>
        /// Moves the snake left 
        /// </summary>
        public void MoveLeft()
        {
            if (Play && !ISAI)
            {
                KeyPress = Direction.Left;
            }
        }

        /// <summary>
        /// Moves the snake right
        /// </summary>
        public void MoveRight()
        {
            if(Play&& !ISAI)
            {
                KeyPress= Direction.Right;
            }
        }

        /// <summary>
        /// Gets the snake's current path from tail to head.
        /// </summary>
        /// <returns>List of GameNodes representing the snake path.</returns>
        public List<GameNode> GetSnakePath()
        {
            return Board.GetSnakePath();
        }

        /// <summary>
        /// Gets the current food node on the board.
        /// </summary>
        /// <returns>The GameNode where the food is located.</returns>
        public GameNode GetFood()
        {
            return Board.Food;
        }

        /// <summary>
        /// Processes the result of a snake movement.
        /// </summary>
        /// <param name="status">The result of the snake move.</param>
        private void HandleStatus(SnakeStatus status)
        {
            switch (status)
            {
                case SnakeStatus.Eating:
                    ScoreChanged?.Invoke(Board.SnakeSize);
                    break;
                case SnakeStatus.Collision:
                    GameEnded?.Invoke("Game Over");
                    IsRunning = false;
                    break;
                case SnakeStatus.Win:
                    GameEnded?.Invoke("You Win!");
                    IsRunning = false;
                    break;
            }
        }
    }
}
