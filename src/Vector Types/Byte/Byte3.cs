using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 3)]
    public unsafe struct Byte3 : IEquatable<Byte3>
    {
        public const int Size = 3; //Number of dimensions

        private static readonly Byte3 zero = new Byte3 (0);
        private static readonly Byte3 one = new Byte3 (1);
        private static readonly Byte3 unitX = new Byte3 (1, 0, 0);
        private static readonly Byte3 unitY = new Byte3 (0, 1, 0);
        private static readonly Byte3 unitZ = new Byte3 (0, 0, 1);

        public static ref readonly Byte3 Zero => ref zero;
        public static ref readonly Byte3 One => ref one;
        public static ref readonly Byte3 UnitX => ref unitX;
        public static ref readonly Byte3 UnitY => ref unitY;
        public static ref readonly Byte3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        internal fixed byte components[Size];

        [FieldOffset (0)]
        public byte x;
        [FieldOffset (1)]
        public byte y;
        [FieldOffset (2)]
        public byte z;

        public Byte2 XY => new Byte2 (x, y);
        public Byte2 XZ => new Byte2 (x, z);
        public Byte2 YZ => new Byte2 (y, z);

        public float Length => (float)Math.Sqrt (LengthSquared);

        public float LengthSquared => (x * x) + (y * y) + (z * z);

        public Byte3 (byte x, byte y, byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Byte3 (byte all)
        {
            x = all;
            y = all;
            z = all;
        }

        public byte this[int index]
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

        public static Byte3 Normalize (Byte3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = (byte)Math.Round (v.x / oldMagnitude);
            v.y = (byte)Math.Round (v.y / oldMagnitude);
            v.z = (byte)Math.Round (v.z / oldMagnitude);

            return v;
        }

        public static Byte3 Absolute (Byte3 v) => new Byte3 ((byte)Math.Abs (v.x), (byte)Math.Abs (v.y), (byte)Math.Abs (v.z));

        public static float Distance (Byte3 a, Byte3 b) => (a - b).Length;

        public static explicit operator Byte3 (Float3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);

        public static explicit operator Vector3 (Byte3 v) => new Vector3 (v.x, v.y, v.z);

        public static explicit operator Byte3 (Vector3 v) => new Byte3 ((byte)v.X, (byte)v.Y, (byte)v.Z);

        public static implicit operator Byte3 ((byte x, byte y, byte z) v) => new Byte3 (v.x, v.y, v.z);

        public static implicit operator (byte, byte, byte) (Byte3 v) => (v.x, v.y, v.z);

        public static bool operator == (Byte3 a, Byte3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Byte3 a, Byte3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Byte3 operator + (Byte3 a, Byte3 b) => new Byte3 ((byte)(a.x + b.x), (byte)(a.y + b.y), (byte)(a.z + b.z));

        public static Byte3 operator - (Byte3 a, Byte3 b) => new Byte3 ((byte)(a.x - b.x), (byte)(a.y - b.y), (byte)(a.z - b.z));

        public static Byte3 operator - (Byte3 v) => new Byte3 ((byte)(v.x * -1), (byte)(v.y * -1), (byte)(v.z * -1));

        public static Byte3 operator * (Byte3 a, int b) => new Byte3 ((byte)(a.x * b), (byte)(a.y * b), (byte)(a.z * b));

        public static Byte3 operator / (Byte3 a, float b) => new Byte3 ((byte)(a.x / b), (byte)(a.y / b), (byte)(a.z / b));

        public static bool operator > (Byte3 a, Byte3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Byte3 a, Byte3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Byte3 a, Byte3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Byte3 a, Byte3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public override int GetHashCode () => (x << 16) | (y << 8) | z;

        public bool Equals (Byte3 other) => this == other;

        public override bool Equals (object obj)
        {
            Byte3? v = obj as Byte3?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
