using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace piine
{
    public static class Simd
    {
        public static unsafe Int3 Sum (Span<Int3> vectors)
        {
            fixed (Int3* vectorPtr = vectors)
            {
                Vector<int> result = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr);

                Vector<int> vector;
                for (int i = 1; i < vectors.Length; i++) //Start at 1 because result already contains the first element
                {
                    vector = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr + i);

                    result += vector;
                }

                return new Int3 (result);

                /*int fullVectorsInSIMD = Vector<int>.Count / Size; //The number of IntVector3's that fit in one Vector<int>
                int simdLength = fullVectorsInSIMD * Size; //Number of ints that fit in fullVectorsInSIMD

                Vector<int> wideResult = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr);

                Vector<int> tempVector;
                for (int i = fullVectorsInSIMD; i < length; i += fullVectorsInSIMD) //Start at simdLength because result already contains the first elements
                {
                    tempVector = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr + i);

                    wideResult += tempVector;
                }

                //Sum the resulting fullVectorsInSIMD together
                IntVector3 v;
                for (int i = Size; i < simdLength; i += Size) //Start at Size because result already contains the first full IntVector3
                {
                    v = new IntVector3 (wideResult, i);
                    tempVector = Unsafe.ReadUnaligned<Vector<int>> (&v);

                    wideResult += tempVector;
                }

                return new IntVector3 (wideResult);*/
            }
        }
    }
}
