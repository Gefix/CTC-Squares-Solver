using System;
using System.Runtime.CompilerServices;

namespace SquaresSolver
{
    public class SquareTilingCombinatorics
    {
        public const int MaxN = 41;

        public static void Init()
        {
            for (int n = 2; n <= MaxN; n++)
            {
                for (int w = 2; w <= n; w++)
                {
                    for (int p = 2; p <= w; p++)
                    {
                        CombinationsBefore(n, w, p);
                    }
                }
            }
        }

        // How many combinations for a given height before we reach a top square of a given size and position offset

        static ulong[] m_combinationsBeforeCache = new ulong[MaxN * MaxN * MaxN];
        
        // Since the cache is pre-initialized we can inline the cached result function

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong CombinationsBefore(int height, int topSquareSize, int topSquarePosition)
        {
            if (topSquarePosition <= 1 || height <= 1)
            {
                return 0;
            }

            int index = height * MaxN * MaxN + topSquareSize * MaxN + topSquarePosition - MaxN * MaxN - MaxN - 1;

            if (m_combinationsBeforeCache[index] > 0) return m_combinationsBeforeCache[index];

            return CombinationsBeforeCalculator(index, height, topSquareSize, topSquarePosition);
        }

        private static ulong CombinationsBeforeCalculator(int index, int height, int topSquareSize, int topSquarePosition)
        {
            ulong result = UniqueTilings(height - 1, 1);

            for (int i = 2; i < topSquareSize; i++)
            {
                for (int j = 2; j <= i; j++)
                {
                    result += UniqueTilings(height - i, j);
                }
            }

            for (int j = 2; j < topSquarePosition; j++)
            {
                result += UniqueTilings(height - topSquareSize, j);
            }

            m_combinationsBeforeCache[index] = result;

            return result;
        }


        // How many unique tiling combinations are there given the remaining height of the puzzle and the previous block width

        static ulong[] m_uniqueTilingsCache = new ulong[MaxN * MaxN];

        public static ulong UniqueTilings(int height, int previous = 0)
        {
            if (height <= 1)
            {
                return 1;
            }

            int index = height * MaxN + previous;

            if (m_uniqueTilingsCache[index] > 0) return m_uniqueTilingsCache[index];

            ulong result = UniqueTilings(height - 1, 1);

            for (int i = 2; i <= height; i++)
            {
                for (int j = 2; j <= i; j++)
                {
                    if (j == previous) continue;

                    result += UniqueTilings(height - i, j);
                }
            }

            m_uniqueTilingsCache[index] = result;

            return result;
        }

        // How many derived tiling combinations are there given the remaining height of the puzzle and the previous block width

        static ulong[] m_derivedTilingsCache = new ulong[MaxN * MaxN * MaxN];

        public static ulong DerivedTilings(int height, int previous = 0, int length = 0)
        {
            if (height <= 1)
            {
                return UniqueTilings(length);
            }

            int index = height * MaxN * MaxN + previous * MaxN + length;

            if (m_derivedTilingsCache[index] > 0) return m_derivedTilingsCache[index];

            ulong result = DerivedTilings(height - 1, 1, length + 1);

            for (int i = 2; i <= height; i++)
            {
                for (int j = 2; j <= i; j++)
                {
                    if (j == previous) continue;

                    result += UniqueTilings(length) * DerivedTilings(height - i, j, 0);
                }
            }

            m_derivedTilingsCache[index] = result;

            return result;
        }
    }
}
