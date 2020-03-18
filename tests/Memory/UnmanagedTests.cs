using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using piine;
using piine.Memory;
using System.Diagnostics;

namespace Tests.Memory
{
    class UnmanagedTests
    {
        [Test]
        public unsafe void TestSetMemory ()
        {
            int* stuff = stackalloc int[200];

            XorShift rng = new XorShift (0);

            for (int i = 0; i < 200; i++)
            {
                stuff[i] = 0x000F_F000;
            }

            Unmanaged.SetMemory (stuff + 50, 100, 0x00F0_000F); //Fill the buffer from 50 to 150

            for (int i = 0; i < 50; i++)
            {
                Assert.IsTrue (stuff[i] == 0x000F_F000);
            }

            for (int i = 50; i < 150; i++)
            {
                Assert.IsTrue (stuff[i] == 0x00F0_000F);
            }

            for (int i = 150; i < 200; i++)
            {
                Assert.IsTrue (stuff[i] == 0x000F_F000);
            }
        }

        [Test]
        public unsafe void TestZeroMemory ()
        {
            int* stuff = stackalloc int[200];

            XorShift rng = new XorShift (0);

            for (int i = 0; i < 200; i++)
            {
                stuff[i] = rng.Next ();
            }

            Unmanaged.ZeroMemory (stuff, 200);

            for (int i = 0; i < 200; i++)
            {
                Assert.IsTrue (stuff[i] == 0);
            }
        }

        [Test]
        public unsafe void TestZeroMemoryPerformance ()
        {
            byte* stuff = stackalloc byte[4096];

            XorShift rng = new XorShift (0);

            for (int i = 0; i < 4096; i++)
            {
                stuff[i] = (byte)rng.Next ();
            }

            Stopwatch sw = Stopwatch.StartNew ();

            Unmanaged.ZeroMemory (stuff, 4096);

            sw.Stop ();

            double ms = sw.Elapsed.TotalMilliseconds;

            for (int i = 0; i < 4096; i++)
            {
                stuff[i] = (byte)rng.Next ();
            }

            sw.Restart ();

            for (int i = 0; i < 4096; i++)
            {
                stuff[i] = 0;
            }

            sw.Stop ();

            Assert.Pass ("ZeroMemory: " + ms + " ms, Simple: " + sw.Elapsed.TotalMilliseconds + " ms");
        }
    }
}
