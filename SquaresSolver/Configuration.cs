using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresSolver
{
    public class RunTimeConfiguration
    {
        // Total time in seconds we have to solve the task
        private const int TotalTimeThreshold = 10;

        // Max number of unique states when generating derived tiles in heuristic mode
        public const int HeuristicsMaxUniqueTiles = 1000000;

        // Time gates and optimizations in milliseconds - derived from the total time
        public const int HeuristicsCostReductionGate1 = TotalTimeThreshold * 400;
        public const int HeuristicsCostReductionGate2 = TotalTimeThreshold * 600;
        public const int HeuristicsCostReductionGate3 = TotalTimeThreshold * 700;

        public const int HeuristicsTurnOnGreedy = TotalTimeThreshold * 750;

        public const int HeuristicsStopOptimizing = TotalTimeThreshold * 990 - 1400;

        public const int TaskTimeout = TotalTimeThreshold * 1000 - 500;
    }
}
