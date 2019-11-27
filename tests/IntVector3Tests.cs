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
            IntVector3 v1 = new IntVector3 (4, 2, 5);
            IntVector3 v2 = new IntVector3 (3, 7, -2);

            IntVector3 result = v1 + v2;

            Assert.IsTrue (result == new IntVector3 (3 + 4, 2 + 7, 5 + (-2)));
        }

        [Test]
        [Explicit]
        public unsafe void TestAddPerformanceNoSIMD ()
        {
            IntVector3 result = IntVector3.Zero;

            Stopwatch sw = Stopwatch.StartNew ();

            for (int i = 0; i < 10_000; i++)
            {
                result += new IntVector3 (1, 2, 3);
            }

            sw.Stop ();

            Assert.Pass ("No SIMD: Time = " + sw.Elapsed.TotalMilliseconds + " ms, result = " + result.ToString ());
        }

        [Test]
        [Explicit]
        public unsafe void TestAddPerformanceSIMD ()
        {
            Stopwatch sw = Stopwatch.StartNew ();

            IntVector3* vectors = stackalloc IntVector3[10_000];

            for (int i = 0; i < 10_000; i++)
            {
                vectors[i] = new IntVector3 (1, 2, 3);
            }

            IntVector3 result = IntVector3.Sum (vectors, 10_000);

            sw.Stop ();

            Assert.Pass ("SIMD (" + Vector.IsHardwareAccelerated + "): Time = " + sw.Elapsed.TotalMilliseconds + " ms, result = " + result.ToString () + ", Sizeof (IntVector3) = " + sizeof (IntVector3));
        }
    }
}
