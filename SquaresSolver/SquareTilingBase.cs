using SquaresSolverTypes;
using System;
using System.Collections.Generic;

namespace SquaresSolver
{
    public abstract class SquareTilingBase
    {
        protected int N;
        protected int M;

        protected bool[,] Map;

        protected int[,] MaxSquare;

        protected int[] MaxSize;
        protected int[] JumpTo;

        public SquareTilingBase(bool[,] map)
        {
            N = map.GetLength(0);
            M = map.GetLength(1);
            Map = map;

            MaxSquare = new int[N + 1, M + 1];
            MaxSize = new int[M + 1];
            JumpTo = new int[M + 1];
        }

        protected void CalculateMaxSquares()
        {
            MaxSquare = new int[N, M];

            for (int x = N - 1; x >= 0; x--)
            {
                for (int y = M - 1; y >= 0; y--)
                {
                    if (!Map[x, y])
                    {
                        MaxSquare[x, y] = 0;
                    }
                    else if (x == N - 1 || y == M - 1)
                    {
                        MaxSquare[x, y] = 1;
                    }
                    else
                    {
                        MaxSquare[x, y] = 1 + Math.Min(MaxSquare[x + 1, y + 1], Math.Min(MaxSquare[x + 1, y], MaxSquare[x, y + 1]));
                    }
                }
            }
        }

        public abstract List<Square> Solve();
    }
}
