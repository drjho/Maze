using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver.SearchAlgorithm.Tests
{
    [TestClass()]
    public class NodeTests
    {
        [TestMethod()]
        public void CompareTest()
        {
            var x = new Node { F = 10 };
            var y = new Node { F = 1 };
            var expected = 1;
            var actual = Node.CompareF(x, y);

            Assert.AreEqual(expected, actual);
        }

    }
}