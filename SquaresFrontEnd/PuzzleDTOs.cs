using SquaresSolverTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squares
{
    public class PuzzleRequest
    {
        public int width { get; set; }
        public int height { get; set; }

        public List<bool[]> puzzle { get; set; }

        public string id { get; set; }
    }

    public class PuzzleResponse
    {
        public string id { get; set; }

        public List<PuzzleResponseSquare> squares { get; set; }

        public void SetSquares(List<Square> p_squares)
        {
            squares = new List<PuzzleResponseSquare>();
            foreach (var square in p_squares)
            {
                squares.Add(new PuzzleResponseSquare(square));
            }
        }
    }

    public class PuzzleResponseSquare
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        
        public PuzzleResponseSquare(Square square)
        {
            X = square.x;
            Y = square.y;
            Size = square.size;
        }
    }

}
