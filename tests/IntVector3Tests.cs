using System;
using System.Numerics;
using NUnit.Framework;
using System.Diagnostics;
using piine;

namespace Tests
{
    public class IntVector3Tests
    {
        [Test]
        public void TestAdd ()
        {
            Int3 v1 = new Int3 (4, 2, 5);
            Int3 v2 = new Int3 (3, 7, -2);

            Int3 result = v1 + v2;

            Assert.IsTrue (result == new Int3 (3 + 4, 2 + 7, 5 + (-2)));
        }

        [Test]
        [Explicit]
        public unsafe void TestAddPerformanceNoSIMD ()
        {
            Int3 result = Int3.Zero;

            Stopwatch sw = Stopwatch.StartNew ();

            for (int i = 0; i < 10_000; i++)
            {
                result += new Int3 (1, 2, 3);
            }

            sw.Stop ();

            Assert.Pass ("No SIMD: Time = " + sw.Elapsed.TotalMilliseconds + " ms, result = " + result.ToString ());
        }

        [Test]
        [Explicit]
        public unsafe void TestAddPerformanceSIMD ()
        {
            Stopwatch sw = Stopwatch.StartNew ();

            Int3* vectors = stackalloc Int3[10_000];

            for (int i = 0; i < 10_000; i++)
            {
                vectors[i] = new Int3 (1, 2, 3);
            }

            Int3 result = Int3.Sum (vectors, 10_000);

            sw.Stop ();

            Assert.Pass ("SIMD (" + Vector.IsHardwareAccelerated + "): Time = " + sw.Elapsed.TotalMilliseconds + " ms, result = " + result.ToString () + ", Sizeof (IntVector3) = " + sizeof (Int3));
        }

        [Test]
        public void TestTupleCast ()
        {
            Int3 v1 = (2, 4, 6);
            Int3 v2 = v1 + (2, 1, -1);

            Assert.IsTrue (v2 == (4, 5, 5));
        }
    }
}
