﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication
{
    public unsafe struct Matrix
    {
        private readonly int[] data;

        public Matrix(int m, int n)
        {
            data = new int[m * n];
            M = m;
            N = n;
        }

        public int this[int i, int j]
        {
            set => data[i + M * j] = value;
            get => data[i + M * j];
        }

        public readonly int M;
        public readonly int N;

        public static Matrix CreateRandomMatrix(int m, int n)
        {
            Matrix matrix = new Matrix(m, n);
            Random random = new Random();

            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    matrix[i, j] = random.Next(10);
                }
            }

            return matrix;
        }

        public static Matrix Multiply(Matrix a, Matrix b, bool multiThreaded = false)
        {
            int aM = a.M;
            int bM = b.M;
            int bN = b.N;
            int aN = a.N;

            Matrix c = new Matrix(aN, bN);

            if (multiThreaded)
            {
                Parallel.For(0, aM, i =>
                {
                    fixed (int* aData = a.data)
                    fixed (int* bData = b.data)
                    fixed (int* cData = c.data)
                    {
                        for (int k = 0; k < bN; ++k)
                        {
                            int sum = 0;

                            for (int j = 0; j < aN; ++j)
                            {
                                sum += aData[i + aM * j] * bData[j + bM * k];
                            }

                            cData[i + aN * k] = sum;
                        }
                    }
                });
            }
            else
            {
                fixed (int* aData = a.data)
                fixed (int* bData = b.data)
                fixed (int* cData = c.data)
                {
                    for (int i = 0; i < aM; ++i)
                    {
                        for (int k = 0; k < bN; ++k)
                        {
                            int sum = 0;

                            for (int j = 0; j < aN; ++j)
                            {
                                sum += aData[i + aM * j] * bData[j + bM * k];
                            }

                            cData[i + aN * k] = sum;
                        }
                    }
                }
                
            }

            return c;
        }
    }
}
