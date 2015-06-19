using ServiceStack;
using SquaresServiceInterface;
using SquaresSolver;

namespace SquaresService
{
    public class SquareSolverService : Service
    {
        public object Any(SquareSolver request)
        {
            int N = request.Map[0].Length;
            int M = request.Map.Count;
            var map = new bool[N, M];

            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < M; y++)
                {
                    map[x, y] = request.Map[y][x];
                }
            }

            SquareTilingBase solver;

            if (M <= 15)
            {
                solver = new SquareTilingOptimal(map);
            }
            else if (M <= 40)
            {
                solver = new SquareTilingHeuristic(map, true, request.CostMargin);
            }
            else
            {
                solver = new SquareTilingHeuristicLarge(map, true, request.CostMargin);
            }

            return new SquareSolverResponse { Solution = solver.Solve() };
        }
    }
}
