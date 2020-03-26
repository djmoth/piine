using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using piine.Memory;

namespace Tests.Memory
{
    class UnmanagedDictionaryTests
    {
        [Test]
        public void TestAccess ()
        {
            //With default values (zero-ed out)
            using (UnmanagedDictionary<int, int> dictionary = new UnmanagedDictionary<int, int> ())
            {
                for (int i = 0; i < 50; i++)
                {
                    dictionary.Add (i, i);
                }

                for (int i = 0; i < 50; i++)
                {
                    Assert.IsTrue (dictionary[i] == i);
                }
            }
        }
    }
}
