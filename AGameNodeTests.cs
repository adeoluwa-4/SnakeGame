using System;
using NUnit.Framework;
using KSU.CIS300.Snake;

namespace KSU.CIS300.SnakeTests
{
    /// <summary>
    /// This class contains unit tests for the GameNode class.
    /// </summary>
    [TestFixture]
    public class AGameNodeTests
    {

        /// <summary>
        /// Tests the GameNode constructor to ensure it initializes the properties correctly.
        /// </summary>
        [Test]
        [Category("B-Constructor")]
        public void TestBA1GameNodeConstruct()
        {
            GameNode gn = new GameNode(1, 2);

            Assert.That(gn.X, Is.EqualTo(1), "X property was not initialized correctly.");
            Assert.That(gn.Y, Is.EqualTo(2), "Y property was not initialized correctly.");
            Assert.That(gn.Data, Is.EqualTo(GridData.Empty), "Data property was not initialized correctly.");
            Assert.That(gn.SnakeEdge, Is.Null, "SnakeEdge property was not initialized correctly.");
        }
    }
}
