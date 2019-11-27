using System;
using NUnit.Framework;
using piine.Memory;

namespace Tests.Memory
{
    public class UnmanagedArrayTests
    {
        [Test]
        public void TestCreateNew ()
        {
            //With default values (zero-ed out)
            using (UnmanagedArray<int> array = new UnmanagedArray<int> (50))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Assert.IsTrue (array[i] == 0);
                }
            }
        }

        [Test]
        public void TestCreateFromSpan ()
        {
            Span<int> span = stackalloc int[50];

            for (int i = 0; i < span.Length; i++)
            {
                span[i] = i;
            }

            using (UnmanagedArray<int> array = new UnmanagedArray<int> (span))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Assert.IsTrue (array[i] == i);
                }
            }
        }

        //Tests array element indexing
        [Test]
        public void TestAccess ()
        {
            using (UnmanagedArray<int> array = new UnmanagedArray<int> (50))
            {
                Random rng = new Random ();

                for (int i = 0; i < array.Length; i++)
                {
                    int value = rng.Next (int.MinValue, int.MaxValue);

                    array[i] = value;

                    Assert.IsTrue (array[i] == value);
                }
            }
        }

        [Test]
        public void TestDispose ()
        {
            UnmanagedArray<int> array = GetRandomizedArray (50);

            array.Dispose ();

            Assert.Throws<InvalidOperationException> (() => array[0] = 10, "Should throw InvalidOperationException");
        }

        [Test]
        public void TestIndexOf ()
        {
            using (UnmanagedArray<int> array = GetRandomizedArray (50))
            {
                int valueToFind = array[30];

                int index = array.IndexOf (valueToFind);

                Assert.IsTrue (array[index] == valueToFind);
            }
        }

        [Test]
        public void TestForeach ()
        {
            long totalForeach = 0;
            long totalFor = 0;

            using (UnmanagedArray<int> array = GetRandomizedArray (50))
            {
                foreach (int i in array)
                {
                    totalForeach += i;
                }

                for (int i = 0; i < array.Length; i++)
                {
                    totalFor += array[i];
                }
            }

            Assert.IsTrue (totalForeach == totalFor);
        }

        public UnmanagedArray<int> GetRandomizedArray (int size)
        {
            UnmanagedArray<int> array = new UnmanagedArray<int> (size);

            Random rng = new Random ();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rng.Next (int.MinValue, int.MaxValue);
            }

            return array;
        }
    }
}