using System.Collections.Generic;
using SquaresSolverTypes;

namespace SquaresServiceInterface
{
    public class SquareSolver
    {
        public List<bool[]> Map { get; set; }
        public int CostMargin { get; set; }
    }

    public class SquareSolverResponse
    {
        public List<Square> Solution { get; set; }
    }
}
