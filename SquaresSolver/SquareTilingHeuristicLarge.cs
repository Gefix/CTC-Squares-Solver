using System;
using System.Collections.Generic;
using System.Linq;
using SquaresSolverTypes;

namespace SquaresSolver
{
    public class SquareTilingHeuristicLarge : SquareTilingHeuristic
    {
        public SquareTilingHeuristicLarge(bool[,] map, bool improve = false, int costMargin = 0, int sizeDeviation = 2) :
            base(map, improve, costMargin, sizeDeviation)
        {
        }

        protected override List<Square> SolveHeuristic()
        {
            timeStarted = DateTime.UtcNow;

            CalculateMaxSquares();

            PathStateLarge init = new PathStateLarge() { state = new int[M] };

            for (int i = 0; i < M; i++)
            {
                init[i] = 1;
            }

            var left = new PathStateLarge[1];
            var right = new Dictionary<ulong, PathStateLarge>(1 + m_costMargin * 100000);

            left[0] = init;

            int leftMinCost = 0;

            for (int x = 0; x < N; x++)
            {
                minnewcost = int.MaxValue;

                foreach(var path in left)
                {
                    if (path.cost - leftMinCost > m_costMargin) continue;

                    PathStateLarge p = new PathStateLarge(path);
                    p.prev = path;

                    for (int j = M - 1; j >= 0; j--)
                    {
                        if (p[j] <= 1)
                        {
                            MaxSize[j] = Math.Min(1 + MaxSize[j + 1], MaxSquare[x, j]);

                            if (MaxSize[j] > 0)
                            {
                                JumpTo[j] = 0;

                                if (JumpTo[j + 1] > 0)
                                {
                                    MinSquares[j + 1] = MinSquares[j + 1 + JumpTo[j + 1]];
                                }

                                MinSquares[j] = 1 + MinSquares[j + MaxSize[j]];
                            }
                            else
                            {
                                p[j] = 0;
                                JumpTo[j] = 1 + JumpTo[j + 1];
                            }
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

                left = right.Values.ToArray<PathStateLarge>();

                right = new Dictionary<ulong, PathStateLarge>(1 + m_costMargin * 100000);

                leftMinCost = minnewcost;

                int leftMinCostIndex = 0;

                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i].cost == leftMinCost)
                    {
                        leftMinCostIndex = i;
                        break;
                    }
                }

                var temp = left[0];
                left[0] = left[leftMinCostIndex];
                left[leftMinCostIndex] = temp;

                double time  = (DateTime.UtcNow - timeStarted).TotalMilliseconds;

                if (time > 7000) m_costMargin = 0;
                if (time > 6000) m_costMargin = 2;
                else if (x < 20 && time > 4000) m_costMargin = 3;
            }

            var solution = GetSolution(left);

            return solution;
        }

        private void Tile(int x, int y, PathStateLarge p, int cost, Dictionary<ulong, PathStateLarge> right)
        {
            y += JumpTo[y];

            if (y == M)
            {
                // store result
                ulong key = Fnv1a64.GetHash(p.state);

                minnewcost = Math.Min(cost, minnewcost);

                PathStateLarge existingPath;

                if (!right.TryGetValue(key, out existingPath))
                {
                    p.cost = cost;
                    right.Add(key, new PathStateLarge(p));
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

            int blockMaxSize = MaxSize[y];

            for (int i = blockMaxSize; i >= 1; i--)
            {
                if (cost + MinSquares[y + i] - minnewcost >= m_costMargin) break;

                for (int j = 0; j < i; j++)
                {
                    p[y + j] = i;
                }

                Tile(x, y + i, p, cost + 1, right);

                if (minnewcost < int.MaxValue && i <= blockMaxSize - m_sizeDeviation) break;
            }
        }

        private List<Square> GetSolution(PathStateLarge[] left)
        {
            List<Square> solution = new List<Square>();

            int bestI = 0;

            for (int i = 1; i < left.Length; i++)
            {
                if (left[i].cost < left[bestI].cost)
                {
                    bestI = i;
                }
            }

            PathStateLarge trace = left[bestI];

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
