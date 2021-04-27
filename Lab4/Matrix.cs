﻿using System;
using NUnit.Framework;

namespace Lab4
{
    public class Matrix
    {
        public double[,] Data { get; }

        public int M { get; }

        public int N { get; }

        public static double Count { get; private set; }

        public Matrix(int m, int n)
        {
            M = m;
            N = n;
            Data = new double[m, n];
        }

        public Matrix(int n)
        {
            M = n;
            N = n;
            Data = new double[n, n];
        }

        public Matrix(double[,] data)
        {
            Data = data;
            M = data.GetLength(0);
            N = data.GetLength(1);
            ProcessFunctionOverData((i, j) => Data[i, j] = data[i, j]);
        }

        private void ProcessFunctionOverData(Action<int, int> func)
        {
            for (var i = 0; i < M; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    func(i, j);
                }
            }
        }


        public double this[int x, int y]
        {
            get => Data[x, y];
            set => Data[x, y] = value;
        }

        public bool IsSquare => M == N;

        public static Matrix RedhefferMatrix(int n)
        {
            var m = new Matrix(n);
            m.ProcessFunctionOverData((i, j) =>
            {
                if ((j + 1) % (i + 1) == 0 || j == 0)
                {
                    m[i, j] = 1;
                }
            });
            return m;
        }

        public static double[] CramersRule(Matrix a, double[] b)
        {
            var x = new double[a.N];
            if (!a.IsSquare)
            {
                throw new ArgumentException("Matrix A must be squared");
            }

            if (b.Length != a.N)
            {
                throw new ArgumentException("B's size must be matrix's size");
            }

            var det = a.GetDeterminant();
            if (det == 0)
            {
                return null;
            }


            var temp = a.Copy();
            for (var j = 0; j < a.N; j++)
            {
                for (var i = 0; i < a.N; i++)
                    temp[i, j] = b[i];
                x[j] = temp.GetDeterminant() / det;

                for (var i = 0; i < a.N; i++)
                    temp[i, j] = a[i, j];
            }


            return x;
        }

        public override string ToString()
        {
            var str = "";
            for (var i = 0; i < M; i++)
            {
                str += "|";
                for (var j = 0; j < N; j++)
                {
                    str += Data[i, j];
                    if (j != N - 1)
                    {
                        str += "\t";
                    }
                }

                str += "|";
                str += "\n";
            }

            return str;
        }

        public Matrix Copy()
        {
            var newM = new Matrix(M, N);
            ProcessFunctionOverData((i, j) => newM[i, j] = Data[i, j]);
            return newM;
        }

        public double GetDeterminant()
        {
            if (!IsSquare)
            {
                throw new InvalidOperationException(
                    "Determinant can be calculated only for square matrix");
            }

            return DetRec(this);
        }

        private static double DetRec(Matrix matrix)
        {
            if (matrix.N==1)
            {
                return matrix[0, 0];
            }
            if (matrix.N == 2)
            {
                Count += 2;
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }

            double sign = 1, result = 0;
            for (var i = 0; i < matrix.N; i++)
            {
                var newM = CreateMatrixWithoutColumnAndRow(matrix, 0, i);
                Count += 2;
                result += sign * matrix[0, i] * DetRec(newM);
                sign = -sign;
            }

            return result;
        }


        private static Matrix CreateMatrixWithoutColumnAndRow(Matrix matrix, int p, int k)
        {
            var result = new Matrix(matrix.N - 1, matrix.N - 1);
            var row = 0;
            for (var i = 0; i < matrix.N; i++)
            {
                if (i == p)
                    continue;
                var col = 0;
                for (var j = 0; j < matrix.N; j++)
                {
                    if (j == k)
                        continue;
                    result[row, col] = matrix[i, j];
                    col++;
                }

                row++;
            }

            return result;
        }
        
        public Matrix GetInvertibleMatrix()
        {
            if (M != N)
                throw new ArgumentException("Matrix must be square");
            var determinant = GetDeterminant();
            if (determinant == 0.0)
            {
                throw new ArgumentException("The determinant is zero. Cannot find inverse matrix");

            }
            var result = new Matrix(M, M);
            result.ProcessFunctionOverData((i, j) => 
            {
                result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) * 
                    CreateMatrixWithoutColumnAndRow(this, i, j).GetDeterminant()/ determinant;
            });
            result = result.CreateTransposeMatrix();
            return result;
        }

        public Matrix CreateTransposeMatrix()
        {
            var result = new Matrix(N, M);
            result.ProcessFunctionOverData((i, j) =>
            {
                result[i, j] = this[j, i];
            });
            return result;
        }

        public double[] MultOnVector(double[] vector)
        {
            if (vector.Length != N)
            {
                throw new ArgumentException("Wrong b's size");
            }
            var result = new double[M];
            ProcessFunctionOverData((i, j) =>
            {
                result[i] += this[i, j] * vector[j];
            });
            return result;
        }

        public static double[] MethodOfMatrixInversion(Matrix a, double[] b)
        {
            var determinant = a.GetDeterminant();
            if (determinant == 0.0)
            {
                return null;
            }
            var inverse = a.GetInvertibleMatrix();
            return inverse.MultOnVector(b);
        }
    }
}