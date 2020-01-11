using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using piine;

namespace tests
{
    class Float2Tests
    {
        [Test]
        void TestCtor ()
        {
            Float2 f = new Float2 (-0.5f, 2);
            Assert.IsTrue (f.x == -0.5f && f.y == 2);

            Float2 f2 = new Float2 (10);
            Assert.IsTrue (f.x == 10 && f.y == 10);
        }

        [Test]
        void TestIndexer ()
        {
            Float2 f = new Float2 (5f, -7.21f);

            //Test normal accesses
            Assert.IsTrue (f[0] == 5f && f[1] == -7.21f);

            //Test out of bound accesses
            Assert.Throws<ArgumentOutOfRangeException> (() => f[-1] = 0);
            Assert.Throws<ArgumentOutOfRangeException> (() => Console.Write (f[-1]));

            Assert.Throws<ArgumentOutOfRangeException> (() => f[Float2.Size] = 0);
            Assert.Throws<ArgumentOutOfRangeException> (() => Console.Write (f[Float2.Size]));
        }

        [Test]
        void TestNormalize ()
        {
            Float2 f = new Float2 (10, -5);
            f = Float2.Normalize (f);

            float length = f.Length;

            Assert.IsTrue (length > 0.999f && length <= 1.001f);
        }

        [Test]
        void TestEquals ()
        {
            Assert.IsTrue (new Float2 (3, 8) == new Float2 (3, 8));
        }

        [Test]
        void TestNotEquals () => Assert.IsTrue (new Float2 (3, 8) != new Float2 (8, 3));
    }
}
