using System;

namespace Lab4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var data = new double[,] {{2, 3, 1}, { -2, 1, 0}, {1, 2, -2}};
            var m = new Matrix(data);
            var b = new double[]{3, -2, -1};
            var x = Matrix.MethodOfMatrixInversion(m, b);
            foreach (var e in x)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}