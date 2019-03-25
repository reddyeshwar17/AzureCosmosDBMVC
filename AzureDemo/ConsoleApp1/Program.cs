using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        //Merge two arrays and sort
        static void Main(string[] args)
        {            
            float[,] arr1 = new float[3, 3] { { 0, 12, 3 }, { 1, 25, 5 }, { 2, 45, 8 } };
            float[,] arr2 = new float[3, 3] { { 0, 9, 10 }, { 1, 17, 19 }, { 2, 29, 20 } };

            int d1 = arr1.GetLength(0) + arr2.GetLength(0);
            int d2 = arr1.GetLength(1) > arr2.GetLength(1) ? arr1.GetLength(1) : arr2.GetLength(1);
            float[,] result = new float[d1, d2];

            AddToArray(result, arr1);
            AddToArray(result, arr2, arr1.GetLength(0));
            
            //sorting
            for (int i = 0; i < result.GetLength(0); i++) // Array Sorting
            {
                for (int j = result.GetLength(1) - 1; j > 0; j--)
                {

                    for (int k = 0; k < j; k++)
                    {
                        if (result[i, k] > result[i, k + 1])
                        {
                            float temp = result[i, k];
                            result[i, k] = result[i, k + 1];
                            result[i, k + 1] = temp;
                        }
                    }
                }              
            }

            for (int i = 0; i < result.GetLength(0); ++i)
            {
                for (int j = 0; j < result.GetLength(1); ++j)
                {
                    Console.Write(result[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        static void AddToArray(float[,] result, float[,] array, int start = 0)
        {
            for (int i = 0; i < array.GetLength(0); ++i)
            {
                for (int j = 0; j < array.GetLength(1); ++j)
                {
                    result[i + start, j] = array[i, j];
                }
            }
        }

    }
}

