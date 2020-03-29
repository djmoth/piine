using System;
using System.Collections.Generic;
using System.Text;
using piine;
using piine.Memory;
using NUnit.Framework;

namespace Tests.Memory
{
    class FixedCapacityListTests
    {

        [Test]
        public void TestCreateNew ()
        {
            int[] storage = new int[25];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);
        }

        [Test]
        public void TestAdd ()
        {
            int[] storage = new int[100];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);

            for (int i = 0; i < 100; i++)
            {
                list.Add (i);
            }

            Assert.IsTrue (list.Count == 100);

            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue (list[i] == i);
            }
        }

        [Test]
        public void TestInsert ()
        {
            int[] storage = new int[100];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);

            for (int i = 0; i < 99; i++)
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

        [Test]
        public void TestRemove ()
        {
            int[] storage = new int[100];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);

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

        [Test]
        public void TestRemoveAt ()
        {
            int[] storage = new int[100];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);

            for (int i = 0; i < 100; i++)
            {
                list.Add (i);
            }

            list.RemoveAt (20);

            Assert.IsTrue (list.Count == 99);
            Assert.IsTrue (list[20] == 21);
        }

        //Tests list element indexing
        [Test]
        public void TestAccess ()
        {
            int[] storage = new int[50];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);

            Random rng = new Random ();

            for (int i = 0; i < 50; i++)
            {
                int value = rng.Next (int.MinValue, int.MaxValue);

                list.Add (value);

                Assert.IsTrue (list[i] == value);
            }
        }


        [Test]
        public void TestIndexOf ()
        {
            FixedCapacityList<int> list = GetRandomizedlist (50);

            int valueToFind = list[30];

            int index = list.IndexOf (valueToFind);

            Assert.IsTrue (list[index] == valueToFind);
        }

        [Test]
        public void TestForeach ()
        {
            long totalForeach = 0;
            long totalFor = 0;

            FixedCapacityList<int> list = GetRandomizedlist (50);

            foreach (int i in list)
            {
                totalForeach += i;
            }

            for (int i = 0; i < list.Count; i++)
            {
                totalFor += list[i];
            }

            Assert.IsTrue (totalForeach == totalFor);
        }

        public FixedCapacityList<int> GetRandomizedlist (int size)
        {
            int[] storage = new int[100];

            FixedCapacityList<int> list = new FixedCapacityList<int> (storage);

            Random rng = new Random ();

            for (int i = 0; i < size; i++)
            {
                list.Add (rng.Next (int.MinValue, int.MaxValue));
            }

            return list;
        }
    }
}
