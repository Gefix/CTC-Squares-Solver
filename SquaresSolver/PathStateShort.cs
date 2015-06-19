using System.Runtime.CompilerServices;

namespace SquaresSolver
{
    internal class PathStateShort
    {
        public int cost;

        public ulong s1;

        public PathStateShort prev = null;

        public PathStateShort() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathStateShort(PathStateShort p)
        {
            this.cost = p.cost;

            this.s1 = p.s1;

            this.prev = p.prev;
        }

        public void Copy(PathStateShort p)
        {
            this.cost = p.cost;

            this.s1 = p.s1;

            this.prev = p.prev;
        }

        public int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (int)((s1 >> (index << 2)) % 16);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                s1 &= ~(15UL << (index << 2));
                s1 |= ((ulong)value << (index << 2));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dec(int index)
        {
            s1 -= 1UL << (index << 2);
        }

        public ulong Key
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return s1;
            }
        }
    }
}
