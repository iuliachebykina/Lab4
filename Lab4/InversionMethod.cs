using System;

namespace Lab4
{
    public class InversionMethod
    {
        public bool IsSolution { get; private set; }
        public double[] Solution { get; private set; }
        public void FindSolution(Matrix a, double[] b)
        {
            if (!a.IsSquare)
            {
                throw new ArgumentException("Matrix A must be square");
            }
            if (b.Length != a.N)
            {
                throw new ArgumentException("B's size must be matrix's size");
            }
            IsSolution = true;
            var determinant = a.GetDeterminant();
            if (determinant == 0.0)
            {
                IsSolution = false;
                return;
                
            }
            
            var inverse = a.GetInvertibleMatrix();
            Solution = inverse.MultOnVector(b);
        }
    }
}