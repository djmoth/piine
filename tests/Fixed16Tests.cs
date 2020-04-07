using System;
using System.Collections.Generic;
using System.Text;
using piine;
using NUnit.Framework;

namespace Tests
{
    class Fixed16Tests
    {
        [Test]
        public void TestFromFloat ()
        {
            float start = 0.125f;

            Fixed16 fixed16 = start;

            float result = fixed16;

            Assert.AreEqual (start, result);
        }

        [Test]
        public void TestAdd ()
        {
            Fixed16 a = 0.125f;
            Fixed16 b = 0.875f;

            Fixed16 result = a + b;
            Assert.IsTrue (result == 1);
        }

        [Test]
        public void TestSubtract ()
        {
            Fixed16 a = 3f;
            Fixed16 b = 4.5f;

            Fixed16 result = a - b;
            Assert.IsTrue (result == -1.5f);
        }

        [Test]
        public void TestMultiply ()
        {
            Fixed16 a = 0.125f;
            Fixed16 b = 8;

            Fixed16 result = a * b;
            Assert.IsTrue (result == 1);

            result = a * 8;
            Assert.IsTrue (result == 1);
        }

        [Test]
        public void TestDivision ()
        {
            Fixed16 a = 10;
            Fixed16 b = 5;

            Fixed16 result = a / b;
            Assert.IsTrue (result == 2);
        }
    }
}
