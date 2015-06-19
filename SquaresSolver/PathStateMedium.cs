using System.Runtime.CompilerServices;

namespace SquaresSolver
{
    internal class PathStateMedium
    {
        public int cost;

        public ulong s1;
        public ulong s2;
        public ulong s3;
        public ulong s4;

        public PathStateMedium prev = null;

        public PathStateMedium() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathStateMedium(PathStateMedium p)
        {
            this.cost = p.cost;

            this.s1 = p.s1;
            this.s2 = p.s2;
            this.s3 = p.s3;
            this.s4 = p.s4;

            this.prev = p.prev;
        }

        public void Copy(PathStateMedium p)
        {
            this.cost = p.cost;

            this.s1 = p.s1;
            this.s2 = p.s2;
            this.s3 = p.s3;
            this.s4 = p.s4;

            this.prev = p.prev;
        }

        public int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 10)
                {
                    return (int)((s1 >> ((index << 2) + (index << 1))) % 64);
                }
                else if (index < 20)
                {
                    index -= 10;
                    return (int)((s2 >> ((index << 2) + (index << 1))) % 64);
                }
                else if (index < 30)
                {
                    index -= 20;
                    return (int)((s3 >> ((index << 2) + (index << 1))) % 64);
                }
                else
                {
                    index -= 30;
                    return (int)((s4 >> ((index << 2) + (index << 1))) % 64);
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 10)
                {
                    s1 &= ~(63UL << ((index << 2) + (index << 1)));
                    s1 |= ((ulong)value << ((index << 2) + (index << 1)));
                }
                else if (index < 20)
                {
                    index -= 10;
                    s2 &= ~(63UL << ((index << 2) + (index << 1)));
                    s2 |= ((ulong)value << ((index << 2) + (index << 1)));
                }
                else if (index < 30)
                {
                    index -= 20;
                    s3 &= ~(63UL << ((index << 2) + (index << 1)));
                    s3 |= ((ulong)value << ((index << 2) + (index << 1)));
                }
                else
                {
                    index -= 30;
                    s4 &= ~(63UL << ((index << 2) + (index << 1)));
                    s4 |= ((ulong)value << ((index << 2) + (index << 1)));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dec(int index)
        {
            if (index < 10)
            {
                s1 -= 1UL << ((index << 2) + (index << 1));
            }
            else if (index < 20)
            {
                index -= 10;
                s2 -= 1UL << ((index << 2) + (index << 1));
            }
            else if (index < 30)
            {
                index -= 20;
                s3 -= 1UL << ((index << 2) + (index << 1));
            }
            else
            {
                index -= 30;
                s4 -= 1UL << ((index << 2) + (index << 1));
            }
        }
    }
}
