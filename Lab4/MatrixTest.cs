using System;
using NUnit.Framework;

namespace Lab4
{
     [TestFixture]
    public class MatrixTest
    {
        [Test]
        public void TestEmptyMatrixConstructor()
        {
            var m = new Matrix(10, 20);
            var s = new double[10, 20];
            Assert.AreEqual(s, m.Data);
            Assert.AreEqual(20, m.N);
            Assert.AreEqual(10, m.M);
        }

        [Test]
        public void TestEmptySquareMatrixConstructor()
        {
            var m = new Matrix(10);
            var s = new double[10, 10];
            Assert.AreEqual(s, m.Data);
            Assert.AreEqual(10, m.N);
            Assert.AreEqual(10, m.M);
        }

        [Test]
        public void TestMatrixDataConstructor()
        {
            var data = new double[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            var m = new Matrix(data);
            Assert.AreEqual(data, m.Data);
            Assert.AreEqual(3, m.N);
            Assert.AreEqual(3, m.M);
        }

        [Test]
        public void TestIndex()
        {
            var data = new double[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            var d = data[1, 2];
            var m = new Matrix(data);
            Assert.AreEqual(d, m[1, 2]);
        }

        [Test]
        public void TestIsSquare()
        {
            var data1 = new double[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            var m1 = new Matrix(data1);
            var data2 = new double[,] {{1, 2, 3}, {4, 5, 6}};
            var m2 = new Matrix(data2);
            Assert.AreEqual(true, m1.IsSquare);
            Assert.AreEqual(false, m2.IsSquare);
        }

        [Test]
        public void TestDeterminant()
        {
            var data = new double[,] {{1, -2, 3}, {4, 0, 6}, {-7, 8, 9}};
            var m = new Matrix(data);
            Assert.AreEqual(204, m.GetDeterminant());
        }

        [Test]
        public void TestDeterminantOnNonSquareMatrix()
        {
            var data = new double[,] {{1, -2, 3}, {4, 0, 6}};
            var m = new Matrix(data);
            var exception = Assert.Throws<InvalidOperationException>(() => { m.GetDeterminant(); });
            if (exception != null)
                Assert.AreEqual("Determinant can be calculated only for square matrix", exception.Message);
        }

        [Test]
        public void TestCreateRedhefferMatrix()
        {
            var m = Matrix.RedhefferMatrix(5);
            var data = new double[,]
                {{1, 1, 1, 1, 1}, {1, 1, 0, 1, 0}, {1, 0, 1, 0, 0}, {1, 0, 0, 1, 0}, {1, 0, 0, 0, 1}};
            Assert.AreEqual(data, m.Data);
        }

        [Test]
        public void TestDeterminantOnRedhefferMatrix()
        {
            var m1 = Matrix.RedhefferMatrix(5);
            Assert.AreEqual(-2, m1.GetDeterminant());
            var m2 = Matrix.RedhefferMatrix(8);
            Assert.AreEqual(-2, m2.GetDeterminant());
            var m3 = Matrix.RedhefferMatrix(10);
            Assert.AreEqual(-1, m3.GetDeterminant());
        }

        [Test]
        public void TestCopy()
        {
            var data = new double[,] {{1, -2, 3}, {4, 0, 6}, {-7, 8, 9}};
            var m = new Matrix(data);
            var copy = m.Copy();
            Assert.AreEqual(m.Data, copy.Data);
            m[2, 2] = 123456789;
            Assert.AreNotEqual(m.Data, copy.Data);
        }


        [Test]
        public void TestCramersRuleWithSolution()
        {
            var data = new double[,] {{506, 66}, {66, 11}};
            var answer = new[] {2315.1, 392.3};
            var m = new Matrix(data);
            var x = Matrix.CramersRule(m, answer);
            Assert.False(x==null);
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
        public void TestCramersRuleWithoutSolution()
        {
            var data = new double[,] {{1, 1}, {1, 1}};
            var answer = new double[] {2, 2};
            var m = new Matrix(data);
            var x = Matrix.CramersRule(m, answer);
            Assert.True(x==null);
            
        }
        
        [Test]
        public void TestCramersRuleWithNonSquaredMatrix()
        {
            var data = new double[,] {{1, 1}, {1, 1}, {2, 2}};
            var answer = new double[] {2, 2};
            var m = new Matrix(data);
            var exception = Assert.Throws<ArgumentException>(() => { Matrix.CramersRule(m, answer); });
            if (exception != null)
                Assert.AreEqual("Matrix A must be squared", exception.Message);
        }
        
        [Test]
        public void TestCramersRuleWithWrongBSize()
        {
            var data = new double[,] {{1, 1}, {1, 1}};
            var answer = new double[] {2, 2, 423, 34};
            var m = new Matrix(data);
            var exception = Assert.Throws<ArgumentException>(() => { Matrix.CramersRule(m, answer); });
            if (exception != null)
                Assert.AreEqual("B's size must be matrix's size", exception.Message);
        }

        public void TestInverse()
        {
            
        }
        
        [Test]
        public void TestMethodOfMatrixInversionWithSolution()
        {
            var data = new double[,] {{506, 66}, {66, 11}};
            var answer = new[] {2315.1, 392.3};
            var m = new Matrix(data);
            var x = Matrix.MethodOfMatrixInversion(m, answer);
            Assert.False(x==null);
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
    }
}