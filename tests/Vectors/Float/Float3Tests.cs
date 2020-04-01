using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using NUnit.Framework;
using piine;

namespace Tests
{
    class Float3Tests
    {
        [Test]
        public void TestCtor ()
        {
            Float3 f = new Float3 (-0.5f, 2, 4);
            Assert.IsTrue (f.x == -0.5f && f.y == 2 && f.z == 4);

            Float3 f2 = new Float3 (10);
            Assert.IsTrue (f2.x == 10 && f2.y == 10 && f2.z == 10);
        }

        [Test]
        public void TestIndexer ()
        {
            Float3 f = new Float3 (5f, -7.21f, 4);

            //Test normal accesses
            Assert.IsTrue (f[0] == 5f && f[1] == -7.21f && f[2] == 4);

            //Test out of bound accesses
            Assert.Throws<ArgumentOutOfRangeException> (() => f[-1] = 0);
            Assert.Throws<ArgumentOutOfRangeException> (() => Console.Write (f[-1]));

            Assert.Throws<ArgumentOutOfRangeException> (() => f[Float3.Size] = 0);
            Assert.Throws<ArgumentOutOfRangeException> (() => Console.Write (f[Float3.Size]));
        }

        [Test]
        public void TestNormalize ()
        {
            Float3 f = new Float3 (10, -5, 4);
            f = Float3.Normalize (f);

            float length = f.Length ();

            Assert.IsTrue (length > 0.999f && length <= 1.001f);
        }

        [Test]
        public void TestEquals ()
        {
            Assert.IsTrue (new Float3 (3, 8, -4) == new Float3 (3, 8, -4));
        }

        [Test]
        public void TestNotEquals () => Assert.IsTrue (new Float3 (3, 8, 4) != new Float3 (8, 3, -4));

        [Test]
        public unsafe void TestMath ()
        {

            TestFloat3Math ();
            TestVector3Math ();

            //(float x, float y, float z) = new Float3 (2, 3, 4);
            

            double float3Time = TestFloat3Math ();
            double vector3Time = TestVector3Math ();
            

            Assert.Pass ("Addition of " + 1000 + " Float3: " + float3Time + " ms, Vector3: " + vector3Time + " ms");
        }

        private unsafe double TestFloat3Math ()
        {
            const int fLength = 1_000;

            Float3* f = stackalloc Float3[fLength];

            Float3 result = Float3.Zero;

            for (int i = 0; i < fLength; i++)
            {
                f[i] = new Float3 (i, fLength - i, i / 200);
            }

            Stopwatch sw = new Stopwatch ();

            sw.Start ();

            for (int i = 0; i < fLength; i++)
            {
                result += f[i];
            }

            sw.Stop ();

            return sw.Elapsed.TotalMilliseconds;
        }

        private unsafe double TestVector3Math ()
        {
            const int fLength = 1_000;

            Vector3* v = stackalloc Vector3[fLength];

            for (int i = 0; i < fLength; i++)
            {
                v[i] = new Vector3 (i, fLength - i, i / 200);
            }

            Vector3 vResult = new Vector3 ();

            Stopwatch sw = new Stopwatch ();

            sw.Start ();

            for (int i = 0; i < fLength; i++)
            {
                vResult += v[i];
            }

            sw.Stop ();

            return sw.Elapsed.TotalMilliseconds;
        }
    }
}
