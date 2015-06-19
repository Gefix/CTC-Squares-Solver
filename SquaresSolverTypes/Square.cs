using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquaresSolverTypes
{
    public struct Square
    {
        public int x;
        public int y;
        public int size;

        public Square(string json)
        {
            string[] parts = json.Split(',');

            x = 0;
            Int32.TryParse(parts[0], out x);

            y = 0;
            Int32.TryParse(parts[1], out y);

            size = 0;
            Int32.TryParse(parts[2], out size);
        }

        public override string ToString()
        {
            return x.ToString() + "," + y.ToString() + "," + size.ToString();
        }
    }
}
