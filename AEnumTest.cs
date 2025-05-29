using System;
using NUnit.Framework;
using KSU.CIS300.Snake;

namespace KSU.CIS300.SnakeTests
{
    /// <summary>
    /// This class contains unit tests for the enums used in the Snake game.
    /// </summary>
    [TestFixture]
    class AEnumTest
    {
        /// <summary>
        /// Tests the GridData enum to ensure it contains all expected values.
        /// </summary>
        [Test]
        [Category("A-Enum")]
        public void TestA1GridDataEnum()
        {
            try
            {
                GridData temp = GridData.Empty;
                temp = GridData.SnakeBody;
                temp = GridData.SnakeHead;
                temp = GridData.SnakeFood;
            }
            catch (Exception e)
            {
                Assert.Fail("Enum does not contain all values");
            }
        }

        /// <summary>
        /// Tests the SnakeStatus enum to ensure it contains all expected values.
        /// </summary>
        [Test]
        [Category("A-Enum")]
        public void TestBA1SnakeStatusEnum()
        {
            try
            {
                SnakeStatus temp = SnakeStatus.Moving;
                temp = SnakeStatus.InvalidDirection;
                temp = SnakeStatus.Eating;
                temp = SnakeStatus.Collision;
            }
            catch (Exception e)
            {
                Assert.Fail("Enum does not contain all values");
            }
        }

        /// <summary>
        /// Tests the Direction enum to ensure it contains all expected values.
        /// </summary>
        [Test]
        [Category("A-Enum")]
        public void TestCA2Game_DirectionEnum()
        {
            try
            {
                Direction temp = Direction.Up;
                temp = Direction.Down;
                temp = Direction.Left;
                temp = Direction.Right;
            }
            catch (Exception e)
            {
                Assert.Fail("Enum does not contain all values");
            }
        }
    }
}
