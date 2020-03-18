using System;
using piine;
using piine.Graphics;
using piine.Memory;
using System.Runtime.InteropServices;

namespace Demo
{
    class Program
    {
        static unsafe void Main (string[] args)
        {
            Console.WriteLine ("sizeof: " + sizeof (Test));
            Console.WriteLine ("Marshal.SizeOf: " + Marshal.SizeOf<Test> ());

            Test* testArray = Unmanaged.AllocMemory<Test> (10, true);

            for (int i = 0; i < 10; i++)
            {
                testArray[i].x = i;
                testArray[i].y = (byte)i;
                testArray[i].z = i;
                testArray[i].w = (short)i;
            }

            Console.WriteLine ((long)(testArray));
            Console.WriteLine ((long)(&testArray[0].x));
            Console.WriteLine ((long)(&testArray[0].y));
            Console.WriteLine ((long)(&testArray[0].z));
            Console.WriteLine ((long)(&testArray[0].w));

            for (int i = 0; i < 10; i++)
            {
                if (testArray[i].x != i)
                    Console.WriteLine ("Error with x at i " + i);
                if (testArray[i].y != i)
                    Console.WriteLine ("Error with y at i " + i);
                if (testArray[i].z != i)
                    Console.WriteLine ("Error with z at i " + i);
                if (testArray[i].w != i)
                    Console.WriteLine ("Error with w at i " + i);
            }

            Console.ReadKey ();
        }

        [StructLayout (LayoutKind.Explicit)]
        struct Test
        {
            [FieldOffset (0)]
            public int x;
            [FieldOffset (10)]
            public byte y;
            [FieldOffset (4)]
            public int z;
            [FieldOffset (8)]
            public short w;
        }
    }
}
