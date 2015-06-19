using System;
using System.Runtime.CompilerServices;

namespace SquaresSolver
{
    internal class PathStateLarge
    {
        public int cost;

        public int[] state;

        public PathStateLarge prev = null;

        public PathStateLarge() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathStateLarge(PathStateLarge p)
        {
            this.cost = p.cost;

            var l = p.state.Length;
            this.state = new int[l];
            Array.Copy(p.state, this.state, l);

            this.prev = p.prev;
        }

        public void Copy(PathStateLarge p)
        {
            this.cost = p.cost;

            var l = p.state.Length;
            this.state = new int[l];
            Array.Copy(p.state, this.state, l);

            this.prev = p.prev;
        }

        public int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return state[index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                state[index] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dec(int index)
        {
            state[index]--;
        }
    }
}
