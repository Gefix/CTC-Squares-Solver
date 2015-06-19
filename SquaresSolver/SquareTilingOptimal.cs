using System;
using System.Collections.Generic;
using System.Linq;
using SquaresSolverTypes;

namespace SquaresSolver
{
    public class SquareTilingOptimal : SquareTilingBase
    {
        public SquareTilingOptimal(bool[,] map) :
            base(map)
        {
        }

        public override List<Square> Solve()
        {
            CalculateMaxSquares();

            PathStateShort init = new PathStateShort();

            for (int i = 0; i < M; i++)
            {
                init[i] = 1;
            }

            var left = new List<PathStateShort>();
            var right = new Dictionary<ulong, PathStateShort>(25000);

            left.Add(init);

            for (int x = 0; x < N; x++)
            {
                for(int i = 0; i < left.Count; i++)
                {
                    PathStateShort p = new PathStateShort(left[i]);
                    p.prev = left[i];

                    for (int j = M - 1; j >= 0; j--)
                    {
                        if (p[j] <= 1)
                        {
                            p[j] = 0;

                            MaxSize[j] = Math.Min(1 + MaxSize[j + 1], MaxSquare[x, j]);
                            JumpTo[j] = MaxSize[j] > 0 ? 0 : 1 + JumpTo[j + 1];
                        }
                        else
                        {
                            p.Dec(j);

                            MaxSize[j] = 0;
                            JumpTo[j] = 1 + JumpTo[j + 1];
                        }
                    }

                    Tile(x, 0, p, p.cost, right);
                }

                left = right.Values.ToList<PathStateShort>();

                right = new Dictionary<ulong, PathStateShort>(25000);
            }

            var solution = GetSolution(left);

            return solution;
        }

        private void Tile(int x, int y, PathStateShort p, int cost, Dictionary<ulong, PathStateShort> right)
        {
            y += JumpTo[y];

            if (y == M)
            {
                // store result
                PathStateShort existingPath;

                if (!right.TryGetValue(p.Key, out existingPath))
                {
                    p.cost = cost;
                    right.Add(p.Key, new PathStateShort(p));
                }
                else
                {
                    if (existingPath.cost > cost)
                    {
                        p.cost = cost;
                        existingPath.Copy(p);
                    }
                }

                return;
            }

            for (int i = MaxSize[y]; i >= 1; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    p[y + j] = i;
                }

                Tile(x, y + i, p, cost + 1, right);
            }
        }

        private List<Square> GetSolution(List<PathStateShort> left)
        {
            List<Square> solution = new List<Square>();

            int bestI = 0;

            for (int i = 1; i < left.Count; i++)
            {
                if (left[i].cost < left[bestI].cost)
                {
                    bestI = i;
                }
            }

            PathStateShort trace = left[bestI];

            int x = N - 1;
            while (x >= 0 && trace != null)
            {
                int currentnew = 0;
                for (int i = 0; i < M; i++)
                {
                    if (trace[i] == 0) continue;

                    if (x == 0 || trace.prev[i] <= trace[i])
                    {
                        solution.Add(new Square()
                        {
                            x = x,
                            y = i,
                            size = trace[i]
                        });
                        currentnew++;

                        i += trace[i] - 1;
                    }
                }

                x--;
                trace = trace.prev;
            }

            return solution;
        }

    }
}
