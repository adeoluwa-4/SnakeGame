using System;
using NUnit.Framework;
using KSU.CIS300.Snake;

namespace KSU.CIS300.SnakeTests
{
    /// <summary>
    /// This class contains unit tests for the Game class, which represents the main game logic for the Snake game.
    /// </summary>
    [TestFixture]
    public class CGameTests
    {
        private Game g;

        /// <summary>
        /// Sets up a new Game instance before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            g = new Game(10, 150, false);
        }

        /// <summary>
        /// Tests the Game constructor to ensure it initializes properties correctly.
        /// </summary>
        [Test]
        [Category("B-Constructor")]
        public void TestCB4Game_Construct()
        {
            Assert.That(g.Size, Is.EqualTo(10), "Game size was not initialized correctly.");
            Assert.That(g.Play, Is.True, "Game play state was not initialized correctly.");
            Assert.That(g.Score, Is.EqualTo(2), "Game score was not initialized correctly.");
            Assert.That(g.Board.Grid.Length, Is.EqualTo(100), "Game board grid size was not initialized correctly.");
        }

        /// <summary>
        /// Tests the GetSnakePath method to ensure it returns the correct snake path length.
        /// </summary>
        [Test]
        [Category("G-BasicGameMethods")]
        public void TestCG1Game_GetSnakePath()
        {
            Assert.That(g.GetSnakePath().Count, Is.EqualTo(2), "Snake path length is incorrect.");
        }

        /// <summary>
        /// Tests the GetFood method to ensure it returns the correct food node.
        /// </summary>
        [Test]
        [Category("G-BasicGameMethods")]
        public void TestCG2Game_GetFood()
        {
            Assert.That(g.GetFood(), Is.SameAs(g.Board.Food), "GetFood did not return the correct food node.");
        }

        /// <summary>
        /// Tests the MoveUp method to ensure it sets the KeyPress property to Direction.Up.
        /// </summary>
        [Test]
        [Category("G-BasicGameMethods")]
        public void TestCG3Game_MoveUp()
        {
            g.MoveUp();
            Assert.That(g.KeyPress, Is.EqualTo(Direction.Up), "KeyPress was not set to Direction.Up.");
        }

        /// <summary>
        /// Tests the MoveDown method to ensure it sets the KeyPress property to Direction.Down.
        /// </summary>
        [Test]
        [Category("G-BasicGameMethods")]
        public void TestCG4Game_MoveDown()
        {
            g.MoveDown();
            Assert.That(g.KeyPress, Is.EqualTo(Direction.Down), "KeyPress was not set to Direction.Down.");
        }

        /// <summary>
        /// Tests the MoveLeft method to ensure it sets the KeyPress property to Direction.Left.
        /// </summary>
        [Test]
        [Category("G-BasicGameMethods")]
        public void TestCG5Game_MoveLeft()
        {
            g.MoveLeft();
            Assert.That(g.KeyPress, Is.EqualTo(Direction.Left), "KeyPress was not set to Direction.Left.");
        }

        /// <summary>
        /// Tests the MoveRight method to ensure it sets the KeyPress property to Direction.Right.
        /// </summary>
        [Test]
        [Category("G-BasicGameMethods")]
        public void TestCG6Game_MoveRight()
        {
            g.MoveRight();
            Assert.That(g.KeyPress, Is.EqualTo(Direction.Right), "KeyPress was not set to Direction.Right.");
        }
    }
}
