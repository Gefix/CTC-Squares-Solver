// A modified and stripped-down FNV-1a 64-bit hashing implementation for computing a 64-bit hash of a 32-bit integer array.
// This class is used internally by the SquareTilingHeuristicLarge square tiling algorithm and does not implement the HashAlgorithm interface.
// Inspired by https://github.com/jslicer/FNV-1a

namespace SquaresSolver
{
    public sealed class Fnv1a64
    {
        private const ulong FnvPrime = unchecked(1099511628211);

        private const ulong FnvOffsetBasis = unchecked(14695981039346656037);

        public static ulong GetHash(int[] array)
        {
            ulong hash = FnvOffsetBasis;

            int end = array.Length;
            for (var i = 0; i < end; i++)
            {
                unchecked
                {
                    hash ^= (ulong)(array[i] >> 24);
                    hash *= FnvPrime;
                    hash ^= (ulong)((array[i] >> 16) % 256);
                    hash *= FnvPrime;
                    hash ^= (ulong)((array[i] >> 8) % 256);
                    hash *= FnvPrime;
                    hash ^= (ulong)(array[i] % 256);
                    hash *= FnvPrime;
                }
            }

            return hash;
        }
    }
}
