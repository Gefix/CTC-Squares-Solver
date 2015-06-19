using System;
using System.Collections.Generic;
using System.Linq;
using SquaresSolverTypes;

namespace SquaresSolver
{
    public class SquareTilingHeuristic : SquareTilingBase
    {
        // The maximum difference between the best and the worst cost paths to keep
        protected int m_costMargin = 0;

        // The maximum deviation from the optimal square size for a given block
        protected int m_sizeDeviation = 0;

        // Should we improve the result with optimal stripes
        protected bool m_improve = false;

        protected int[] MinSquares;

        public SquareTilingHeuristic(bool[,] map, bool improve = false, int costMargin = 0, int sizeDeviation = 2) :
            base(map)
        {
            m_costMargin = costMargin;
            m_sizeDeviation = sizeDeviation;
            m_improve = improve;

            MinSquares = new int[M + 1];
        }

        protected DateTime timeStarted;

        protected int minnewcost = 0;

        public override List<Square> Solve()
        {
            var solution = SolveHeuristic();

            Console.WriteLine("Phase 1 (Heuristic) Complete: " + ((int)(DateTime.UtcNow - timeStarted).TotalMilliseconds).ToString() + " ms.");

            if (m_improve)
            {
                int subWidth = 15;
                int subStep = 10;

                SubSolveHorizontal(ref solution, 0, subStep, N, subWidth);

                SubSolveVertical(ref solution, 0, subStep, subWidth, M);

                SubSolveHorizontal(ref solution, M - subWidth, -subStep, N, subWidth);

                SubSolveVertical(ref solution, N - subWidth, -subStep, subWidth, M);

                Console.WriteLine("Phase 2 (Stripe Optimal) Complete: " + ((int)(DateTime.UtcNow - timeStarted).TotalMilliseconds).ToString() + " ms.");
            }

            return solution;
        }

        protected virtual List<Square> SolveHeuristic()
        {
            timeStarted = DateTime.UtcNow;

            CalculateMaxSquares();

            PathStateMedium init = new PathStateMedium();

            for (int i = 0; i < M; i++)
            {
                init[i] = 1;
            }

            var left = new PathStateMedium[1];
            var right = new Dictionary<ulong, PathStateMedium>(1 + m_costMargin * 100000);

            left[0] = init;

            int leftMinCost = 0;

            for (int x = 0; x < N; x++)
            {
                minnewcost = int.MaxValue;

                foreach(var path in left)
                {
                    if (path.cost - leftMinCost > m_costMargin) continue;

                    PathStateMedium p = new PathStateMedium(path);
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

                left = right.Values.ToArray<PathStateMedium>();

                right = new Dictionary<ulong, PathStateMedium>(1 + m_costMargin * 100000);

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

        private void Tile(int x, int y, PathStateMedium p, int cost, Dictionary<ulong, PathStateMedium> right)
        {
            y += JumpTo[y];

            if (y == M)
            {
                // store result
                ulong key = 0;

                int prevY = 0;
                int prevPos = p[prevY];

                for (int i = 1; i < M; i++)
                {
                    int currentPos = p[i];

                    if (currentPos == prevPos)
                    {
                        continue;
                    }

                    key += SquareTilingCombinatorics.CombinationsBefore(M - prevY, i - prevY, prevPos);

                    prevY = i;
                    prevPos = currentPos;
                }

                key += SquareTilingCombinatorics.CombinationsBefore(M - prevY, M - prevY, prevPos);

                minnewcost = Math.Min(cost, minnewcost);

                PathStateMedium existingPath;

                if (!right.TryGetValue(key, out existingPath))
                {
                    p.cost = cost;
                    right.Add(key, new PathStateMedium(p));
                }
                else
                {
                    //collissions++;

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

        void SubSolveHorizontal(ref List<Square> solution, int sY, int vY, int dX, int dY)
        {
            int sX = 0;

            for (; sY >= 0 && sY + dY <= M; sY += vY)
            {
                if ((DateTime.UtcNow - timeStarted).TotalMilliseconds > 8500) return;

                bool[,] subMap = new bool[N, dY];

                for (int x = 0; x < dX; x++)
                {
                    for (int y = 0; y < dY; y++)
                    {
                        subMap[x, y] = Map[sX + x, sY + y];
                    }
                }

                int originalMin = 0;

                var newSolution = new List<Square>();

                foreach (var square in solution)
                {
                    if (square.x >= sX && square.x + square.size <= sX + dX &&
                        square.y >= sY && square.y + square.size <= sY + dY)
                    {
                        // completely inside the sub-map - skip
                        originalMin++;
                        continue;
                    }

                    newSolution.Add(square);

                    for (int x = Math.Max(0, square.x - sX); x < Math.Min(dX, square.x + square.size - sX); x++)
                    {
                        for (int y = Math.Max(0, square.y - sY); y < Math.Min(dY, square.y + square.size - sY); y++)
                        {
                            subMap[x, y] = false;
                        }
                    }
                }

                var subTiling = new SquareTilingOptimal(subMap);

                var subSolution = subTiling.Solve();

                if (subSolution.Count >= originalMin)
                {
                    continue;
                }

                for (int i = 0; i < subSolution.Count; i++)
                {
                    newSolution.Add(new Square()
                    {
                        x = subSolution[i].x + sX,
                        y = subSolution[i].y + sY,
                        size = subSolution[i].size
                    });
                }

                solution = newSolution;
            }
        }

        void SubSolveVertical(ref List<Square> solution, int sX, int vX, int dX, int dY)
        {
            int sY = 0;

            for (; sX >= 0 && sX + dX <= N; sX += vX)
            {
                if ((DateTime.UtcNow - timeStarted).TotalMilliseconds > 8500) return;

                bool[,] subMap = new bool[M, dX];

                for (int x = 0; x < dX; x++)
                {
                    for (int y = 0; y < dY; y++)
                    {
                        subMap[y, x] = Map[sX + x, sY + y];
                    }
                }

                int originalMin = 0;

                var newSolution = new List<Square>();

                foreach (var square in solution)
                {
                    if (square.x >= sX && square.x + square.size <= sX + dX &&
                        square.y >= sY && square.y + square.size <= sY + dY)
                    {
                        // completely inside the sub-map - skip
                        originalMin++;
                        continue;
                    }

                    newSolution.Add(square);

                    for (int x = Math.Max(0, square.x - sX); x < Math.Min(dX, square.x + square.size - sX); x++)
                    {
                        for (int y = Math.Max(0, square.y - sY); y < Math.Min(dY, square.y + square.size - sY); y++)
                        {
                            subMap[y, x] = false;
                        }
                    }
                }

                var subTiling = new SquareTilingOptimal(subMap);

                var subSolution = subTiling.Solve();

                if (subSolution.Count >= originalMin)
                {
                    continue;
                }

                for (int i = 0; i < subSolution.Count; i++)
                {
                    newSolution.Add(new Square()
                    {
                        x = subSolution[i].y + sX,
                        y = subSolution[i].x + sY,
                        size = subSolution[i].size
                    });
                }

                solution = newSolution;
            }
        }

        private List<Square> GetSolution(PathStateMedium[] left)
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

            PathStateMedium trace = left[bestI];

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
