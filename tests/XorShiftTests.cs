using NUnit.Framework;
using System;
using System.Collections.Generic;
using piine;

namespace tests
{
    class XorShiftTests
    {
        [Test]
        public void TestNext ()
        {
            XorShift rng = new XorShift (1234);

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine (rng.Next ());
            }
        }

        [Test]
        public void TestNextRange ()
        {
            XorShift rng = new XorShift (1234);

            for (int i = 0; i < 10000; i++)
            {
                int v = rng.Next (0, 10);

                Assert.IsTrue (v >= 0 && v <= 10);
            }
        }
    }
}
