using System;
using System.IO;
using System.Linq;

namespace Lab4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Client();
            /*Experiment();*/
        }

        private static void Experiment()
        {
            var streamWriter = new StreamWriter("res.txt");
            for (var i = 1; i <= 10; i++)
            {
                var m = new Matrix(i);
                var random = new Random();
                var flag = true;
                while (flag)
                {
                    try
                    {
                        m.GetInvertibleMatrix();
                        flag = false;
                    }
                    catch
                    {
                        var x = random.Next() % i;
                        var y = random.Next() % i;
                        m[x, y] += 1;
                        m.Count = 0;
                    }
                }

                Console.WriteLine(i + " - " + m.Count);
                streamWriter.WriteLine(i + " - " + m.Count);
            }
            Console.WriteLine("Эксперимент завершен");
        }

        private static void Client()
        {
            Console.WriteLine("Решение системы линейных уравнений A*x=B методом обратной матрицы");
            Console.WriteLine("Введите размер квадратной матрицы A: ");
            int n;
            try
            {
                n = Convert.ToInt32(Console.ReadLine());
                if (n <= 0)
                    throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine("Размер матрицы должен быть целым положительным числом");
                Console.ReadLine();
                return;
            }

            var data = new double[n, n];
            Console.WriteLine("Вводите построчно элементы матрицы A через пробел: ");
            try
            {
                for (var i = 0; i < n; i++)
                {
                    var numbers = Console.ReadLine()?.Split(' ');
                    for (var j = 0; j < n; j++)
                    {
                        if (numbers != null) data[i, j] = Convert.ToDouble(numbers[j]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Матрица была введена неправильно");
                Console.ReadLine();
                return;
            }

            var m = new Matrix(data);

            Console.WriteLine("Введите через пробел элементы вектора B:");
            double[] b;
            try
            {
                b = Console.ReadLine()?.Split(' ').Select(Convert.ToDouble).ToArray();
            }
            catch (Exception)
            {
                Console.WriteLine("Вектор B был введен неправильно");
                Console.ReadLine();
                return;
            }

            var inversionMethod = new InversionMethod();
            inversionMethod.FindSolution(m, b);
            if (inversionMethod.IsSolution)
            {
                Console.WriteLine("Решение СЛУ:");
                var result = inversionMethod.Solution;
                Console.Write("X = ");
                Console.Write("(");

                for (var i = 0; i < result.Length; i++)
                {
                    Console.Write(result[i]);
                    if (i != result.Length - 1)
                        Console.Write(" ");
                }

                Console.WriteLine(")");
            }
            else
            {
                Console.WriteLine("СЛУ не имеет решений, так как определитель матрицы равен 0");
            }

            Console.ReadLine();
        }
    }
}