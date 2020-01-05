using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 12)]
    public unsafe struct Int3 : IEquatable<Int3>
    {
        public const int Size = 3; //Number of dimensions

        private static readonly Int3 zero = new Int3 (0);
        private static readonly Int3 one = new Int3 (1);
        private static readonly Int3 unitX = new Int3 (1, 0, 0);
        private static readonly Int3 unitY = new Int3 (0, 1, 0);
        private static readonly Int3 unitZ = new Int3 (0, 0, 1);

        public static ref readonly Int3 Zero => ref zero;
        public static ref readonly Int3 One => ref one;
        public static ref readonly Int3 UnitX => ref unitX;
        public static ref readonly Int3 UnitY => ref unitY;
        public static ref readonly Int3 UnitZ => ref unitZ;

        [CLSCompliant (false)]
        [FieldOffset (0)]
        public fixed int components[Size];

        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;
        [FieldOffset (8)]
        public int z;

        public Int2 XY => new Int2 (x, y);
        public Int2 XZ => new Int2 (x, z);
        public Int2 YZ => new Int2 (y, z);

        public float Length => (float)Math.Sqrt (LengthSquared);

        public float LengthSquared => (x * x) + (y * y) + (z * z);

        public Int3 (int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Int3 (int all)
        {
            x = all;
            y = all;
            z = all;
        }

        public Int3 (Vector<int> v, int startIndex = 0)
        {
            x = v[startIndex];
            y = v[startIndex + 1];
            z = v[startIndex + 2];
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-2, index was " + index);

                return components[index];
            }
            set
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-2, index was " + index);

                components[index] = value;
            }
        }

        public int CalculateVolume () => x * y * z;

        public static Int3 Normalize (Int3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = (int)Math.Round (v.x / oldMagnitude);
            v.y = (int)Math.Round (v.y / oldMagnitude);
            v.z = (int)Math.Round (v.z / oldMagnitude);

            return v;
        }     

        public static Int3 Absolute (Int3 v) => new Int3 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z));

        public static float Distance (Int3 a, Int3 b) => (a - b).Length;

        public static Int3 Cross (Int3 a, Int3 b)
        {
            Int3 r = new Int3 ();
            r.x = a.y * b.z - a.z * b.y;
            r.y = a.x * b.z - a.z * b.x;
            r.z = a.y * b.x - a.x * b.y;

            return r;
        }

        public static explicit operator Int3 (Float3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);

        public static explicit operator Vector3 (Int3 v) => new Vector3 (v.x, v.y, v.z);

        public static explicit operator Int3 (Vector3 v) => new Int3 ((int)v.X, (int)v.Y, (int)v.Z);

        public static implicit operator Int3 ((int x, int y, int z) v) => new Int3 (v.x, v.y, v.z);

        public static implicit operator (int, int, int) (Int3 v) => (v.x, v.y, v.z);

        public static bool operator == (Int3 a, Int3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Int3 a, Int3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Int3 operator + (Int3 a, Int3 b) => new Int3 (a.x + b.x, a.y + b.y, a.z + b.z);

        public static Int3 operator - (Int3 a, Int3 b) => new Int3 (a.x - b.x, a.y - b.y, a.z - b.z);

        public static Int3 operator - (Int3 v) => new Int3 (v.x * -1, v.y * -1, v.z * -1);

        public static Int3 operator * (Int3 a, int b) => new Int3 (a.x * b, a.y * b, a.z * b);

        public static Int3 operator / (Int3 a, float b) => new Int3 ((int)(a.x / b), (int)(a.y / b), (int)(a.z / b));

        public static bool operator > (Int3 a, Int3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Int3 a, Int3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Int3 a, Int3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Int3 a, Int3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

        public override string ToString () =>  "(" + x + ", " + y + ", " + z + ")";

        public override int GetHashCode () => (x * 73856093) ^ (y * 19349663) ^ (z * 83492791);

        public bool Equals (Int3 other) => this == other;

        public override bool Equals (object obj)
        {
            Int3? v = obj as Int3?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
