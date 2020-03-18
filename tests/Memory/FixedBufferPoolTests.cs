using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using piine.Memory;

namespace Tests.Memory
{
    public class FixedBufferPoolTests
    {
        [Test]
        public void TestConstructor ()
        {
            FixedBufferPool<int> pool = new FixedBufferPool<int> (20, 100);

            pool.Dispose ();
        }

        [Test]
        public unsafe void TestAllocation ()
        {
            using (FixedBufferPool<int> pool = new FixedBufferPool<int> (100, 250))
            {
                int* lastPtr = null;

                for (int i = 1; i <= 100; i++)
                {
                    int* ptr = pool.Allocate (false);

                    Assert.IsTrue (ptr != lastPtr);
                    Assert.IsTrue (pool.AllocatedBufferCount == i);
                }
            }
        }

        [Test]
        public unsafe void TestDeallocation ()
        {
            using (FixedBufferPool<int> pool = new FixedBufferPool<int> (100, 250))
            {
                int** buffers = stackalloc int*[100];

                for (int i = 0; i < 100; i++)
                {
                    buffers[i] = pool.Allocate (false);
                }

                for (int i = 0; i < 100; i++)
                {
                    pool.Free (ref buffers[i]);
                }

                Assert.IsTrue (pool.AllocatedBufferCount == 0);
            }
        }

        [Test]
        public unsafe void TestAllocationMemory ()
        {
            using (FixedBufferPool<int> pool = new FixedBufferPool<int> (100, 250))
            {
                int** buffers = stackalloc int*[100];

                for (int i = 0; i < 100; i++)
                {
                    int* buffer = pool.Allocate (false);
                    
                    for (int j = 0; j < 250; j++)
                    {
                        buffer[j] = i;
                    }

                    buffers[i] = buffer;
                }

                for (int i = 0; i < 100; i++)
                {
                    int* buffer = buffers[i];

                    for (int j = 0; j < 250; j++)
                    {
                        Assert.IsTrue (buffer[j] == i);
                    }
                }

                const int bufferToTest = 30;

                pool.Free (ref buffers[bufferToTest]);

                int* newBuffer = pool.Allocate (false);

                for (int i = 0; i < 250; i++)
                {
                    Assert.IsTrue (newBuffer[i] == 30);
                }
            }
        }
    }
}
