using System;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 36)]
    public unsafe struct Float3x3
    {
        private const int ROWS = 3;
        private const int COLUMNS = 3;

        [CLSCompliant (false)]
        [FieldOffset (0)]
        public fixed float data[ROWS * COLUMNS];

        public float this[int x, int y]
        {
            get => data[x * COLUMNS + y];
            set => data[x * COLUMNS + y] = value;
        }

        public static int TransformIndex (int x, int y) => x * COLUMNS + y;
    }
}
