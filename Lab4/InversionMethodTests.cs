using System;
using NUnit.Framework;

namespace Lab4
{
    [TestFixture]
    public class InversionMethodTests
    {
        [Test]
        public void TestMethodOfMatrixInversionWithSolution()
        {
            var data = new double[,] {{506, 66}, {66, 11}};
            var answer = new[] {2315.1, 392.3};
            var m = new Matrix(data);
            var inversionMethod = new InversionMethod();
            inversionMethod.FindSolution(m, answer);
            Assert.True(inversionMethod.IsSolution);
            var x = inversionMethod.Solution;
            for (var i = 0; i < m.N; i++)
            {
                var t = 0.0;
                for (var j = 0; j < m.N; j++)
                {
                    t += Math.Round(m[i, j] * x[j], 1);
                }

                Assert.AreEqual(t, answer[i]);
            }
        }

        [Test]
        public void TestMethodOfMatrixInversionWithoutSolution()
        {
            var data = new double[,] {{1, 1}, {1, 1}};
            var answer = new[] {2315.1, 392.3};
            var m = new Matrix(data);
            var inversion = new InversionMethod();
            inversion.FindSolution(m, answer);
            Assert.False(inversion.IsSolution);
        }
        
        [Test]
        public void TestMethodOfMatrixInversionWithNonSquareMatrix()
        {
            var data = new double[,] {{506, 66}, {66, 11}, {23, 432}};
            var answer = new[] {2315.1, 392.3};
            var m = new Matrix(data);
            var exception = Assert.Throws<ArgumentException>(() => {new InversionMethod().FindSolution(m, answer); });
            if (exception != null)
                Assert.AreEqual("Matrix A must be square", exception.Message);
        }
        
        [Test]
        public void TestMethodOfMatrixInversionWithWrongBSize()
        {
            var data = new double[,] {{506, 66}, {66, 11}};
            var answer = new[] {2315.1, 392.3, 3432};
            var m = new Matrix(data);
            var exception = Assert.Throws<ArgumentException>(() => {new InversionMethod().FindSolution(m, answer); });
            if (exception != null)
                Assert.AreEqual("B's size must be matrix's size", exception.Message);
        }
    }
}