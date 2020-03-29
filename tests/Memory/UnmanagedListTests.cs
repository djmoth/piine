using System;
using NUnit.Framework;
using piine.Memory;

namespace Tests.Memory
{
    public class UnmanagedListTests
    {
        [Test]
        public void TestCreateNew ()
        {
            using UnmanagedList<int> list = new UnmanagedList<int> ();
        }

        [Test]
        public void TestAdd ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> ())
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add (i);
                }

                Assert.IsTrue (list.Count == 100);

                for (int i = 0; i < 100; i++)
                {
                    Assert.IsTrue (list[i] == i);
                }

                Assert.Pass ("Final Capacity = " + list.Capacity.ToString ());
            }
        }

        [Test]
        public void TestInsert ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> ())
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add (i);
                }

                list.Insert (20, 123);

                Assert.IsTrue (list[20] == 123);

                //All elements below index 20 should be untouched
                for (int i = 0; i < 20; i++)
                {
                    Assert.IsTrue (list[i] == i);
                }

                //All elements above index 20 should be moved 1 index up
                for (int i = 21; i < list.Count; i++)
                {
                    Assert.IsTrue (list[i] == i - 1);
                }
            }
        }

        [Test]
        public void TestInsertAtMaxCapacity ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> (100))
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add (i);
                }

                list.Insert (20, 123);

                //All elements below index 20 should be untouched
                for (int i = 0; i < 20; i++)
                {
                    Assert.IsTrue (list[i] == i);
                }

                //All elements above index 20 should be moved 1 index up
                for (int i = 21; i < list.Count; i++)
                {
                    Assert.IsTrue (list[i] == i - 1);
                }
            }
        }

        [Test]
        public void TestRemove ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> ())
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add (i);
                }

                for (int i = 0; i < 100; i++)
                {
                    if (i != 20)
                        Assert.IsTrue (list.Remove (i));
                }

                Assert.IsFalse (list.Remove (-1));

                Assert.IsTrue (list.Count == 1);
                Assert.IsTrue (list.IndexOf (20) == 0);
            }
        }

        [Test]
        public void TestRemoveAt ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> ())
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add (i);
                }

                list.RemoveAt (20);

                Assert.IsTrue (list.Count == 99);
                Assert.IsTrue (list[20] == 21);
            }
        }

        [Test]
        public void TestOutOfRangeAccess ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> ())
            {
                for (int i = 0; i < 100; i++)
                {
                    list.Add (i);
                }

                Assert.Throws<ArgumentOutOfRangeException> (() => list[-1] = 123);
                Assert.Throws<ArgumentOutOfRangeException> (() => list[list.Count] = 123);
                Assert.Throws<ArgumentOutOfRangeException> (() => list.RemoveAt (-1));
            }
        }

        //Tests list element indexing
        [Test]
        public void TestAccess ()
        {
            using (UnmanagedList<int> list = new UnmanagedList<int> ())
            {
                Random rng = new Random ();

                for (int i = 0; i < 50; i++)
                {
                    int value = rng.Next (int.MinValue, int.MaxValue);

                    list.Add (value);

                    Assert.IsTrue (list[i] == value);
                }
            }
        }

        [Test]
        public void TestDispose ()
        {
            UnmanagedList<int> list = GetRandomizedlist (50);

            list.Dispose ();

            Assert.Throws<ObjectDisposedException> (() => list[0] = 10, "Should throw ObjectDisposedException");
        }

        [Test]
        public void TestIndexOf ()
        {
            using (UnmanagedList<int> list = GetRandomizedlist (50))
            {
                int valueToFind = list[30];

                int index = list.IndexOf (valueToFind);

                Assert.IsTrue (list[index] == valueToFind);
            }
        }

        [Test]
        public void TestForeach ()
        {
            long totalForeach = 0;
            long totalFor = 0;

            using (UnmanagedList<int> list = GetRandomizedlist (50))
            {
                foreach (int i in list)
                {
                    totalForeach += i;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    totalFor += list[i];
                }
            }

            Assert.IsTrue (totalForeach == totalFor);
        }

        public UnmanagedList<int> GetRandomizedlist (int size)
        {
            UnmanagedList<int> list = new UnmanagedList<int> (size);

            Random rng = new Random ();

            for (int i = 0; i < size; i++)
            {
                list.Add (rng.Next (int.MinValue, int.MaxValue));
            }

            return list;
        }
    }
}