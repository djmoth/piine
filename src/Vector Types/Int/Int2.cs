using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct Int2 : IEquatable<Int2>
    {
        public const int Size = 2;

        private static readonly Int2 zero = new Int2 (0);
        private static readonly Int2 one = new Int2 (1);
        private static readonly Int2 unitX = new Int2 (1, 0);
        private static readonly Int2 unitY = new Int2 (0, 1);

        public static ref readonly Int2 Zero => ref zero;
        public static ref readonly Int2 One => ref one;
        public static ref readonly Int2 UnitX => ref unitX;
        public static ref readonly Int2 UnitY => ref unitY;

        [CLSCompliant (false)]
        [FieldOffset (0)]
        public fixed int components[Size];

        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;

        public int Length => (int)Math.Sqrt (LengthSquared);

        public int LengthSquared => (x * x) + (y * y);

        public Int2 (int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Int2 (int all)
        {
            x = all;
            y = all;
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-1, index was " + index);

                return components[index];
            }
            set
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-1, index was " + index);

                components[index] = value;
            }
        }

        public int GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, int value) => components[index] = value;

        public int CalculateArea () => x * y;

        public static Int2 Normalize (Int2 v)
        {
            if (v == Zero)
                return Zero;

            int oldMagnitude = v.Length;
            v.x = v.x / oldMagnitude;
            v.y = v.y / oldMagnitude;

            return v;
        }

        

        public static Int2 Absolute (Int2 v) => new Int2 (Math.Abs (v.x), Math.Abs (v.y));

        public static int Distance (Int2 a, Int2 b) =>  (a - b).Length;

        public static explicit operator Int2 (Float2 v) => new Int2 ((int)v.x, (int)v.y);

        public static explicit operator Vector2 (Int2 v) => new Vector2 (v.x, v.y);

        public static explicit operator Int2 (Vector2 v) => new Int2 ((int)v.X, (int)v.Y);

        public static implicit operator Int2 ((int x, int y) v) => new Int2 (v.x, v.y);

        public static implicit operator (int, int) (Int2 v) => (v.x, v.y);

        public static bool operator == (Int2 a, Int2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Int2 a, Int2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Int2 operator + (Int2 a, Int2 b) => new Int2 (a.x + b.x, a.y + b.y);

        public static Int2 operator - (Int2 a, Int2 b) => new Int2 (a.x - b.x, a.y - b.y);

        public static Int2 operator - (Int2 v) => new Int2 (v.x * -1, v.y * -1);

        public static Int2 operator * (Int2 a, int b) => new Int2 (a.x * b, a.y * b);

        public static Int2 operator / (Int2 a, float b) => new Int2 ((int)(a.x / b), (int)(a.y / b));

        public static bool operator > (Int2 a, Int2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Int2 a, Int2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Int2 a, Int2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Int2 a, Int2 b) => a.x <= b.x && a.y <= b.y;

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Int2 other) => this == other;

        public override bool Equals (object obj)
        {
            Int2? v = obj as Int2?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}