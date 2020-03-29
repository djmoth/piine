using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 4)]
    public unsafe struct Byte4 : IEquatable<Byte4>
    {
        public const int Size = 4; //Number of dimensions

        private static readonly Byte4 zero = new Byte4 (0);
        private static readonly Byte4 one = new Byte4 (1);
        private static readonly Byte4 unitX = new Byte4 (1, 0, 0, 0);
        private static readonly Byte4 unitY = new Byte4 (0, 1, 0, 0);
        private static readonly Byte4 unitZ = new Byte4 (0, 0, 1, 0);
        private static readonly Byte4 unitW = new Byte4 (0, 0, 0, 1);

        public static ref readonly Byte4 Zero => ref zero;
        public static ref readonly Byte4 One => ref one;
        public static ref readonly Byte4 UnitX => ref unitX;
        public static ref readonly Byte4 UnitY => ref unitY;
        public static ref readonly Byte4 UnitZ => ref unitZ;
        public static ref readonly Byte4 UnitW => ref unitW;

        [FieldOffset (0)]
        internal fixed byte components[Size];

        [FieldOffset (0)]
        public byte x;
        [FieldOffset (1)]
        public byte y;
        [FieldOffset (2)]
        public byte z;
        [FieldOffset (3)]
        public byte w;

        [FieldOffset (0)]
        public uint uintValue;

        public float Length => (float)Math.Sqrt ((x * x) + (y * y) + (z * z) + (w * w));

        public float LengthSquared => (x * x) + (y * y) + (z * z) + (w * w);

        public Byte4 (byte x, byte y, byte z, byte w)
        {
            uintValue = 0;
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Byte4 (byte all)
        {
            uintValue = 0;
            x = all;
            y = all;
            z = all;
            w = all;
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-3, index was " + index);

                return components[index];
            }
            set
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-3, index was " + index);

                components[index] = value;
            }
        }

        public int CalculateVolume () => x * y * z * w;

        public static Byte4 Normalize (Byte4 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = (byte)Math.Round (v.x / oldMagnitude);
            v.y = (byte)Math.Round (v.y / oldMagnitude);
            v.z = (byte)Math.Round (v.z / oldMagnitude);
            v.w = (byte)Math.Round (v.w / oldMagnitude);

            return v;
        }

        public static Byte4 Absolute (Byte4 v) => new Byte4 ((byte)Math.Abs (v.x), (byte)Math.Abs (v.y), (byte)Math.Abs (v.z), (byte)Math.Abs (v.w));

        public static float Distance (Byte4 a, Byte4 b) => (a - b).Length;

        public static explicit operator Byte4 (Float4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

        public static implicit operator Vector4 (Byte4 iv) => new Vector4 (iv.x, iv.y, iv.z, iv.w);

        public static implicit operator Byte4 (Vector4 iv) => new Byte4 ((byte)iv.X, (byte)iv.Y, (byte)iv.Z, (byte)iv.W);

        public static implicit operator Byte4 ((byte x, byte y, byte z, byte w) v) => new Byte4 (v.x, v.y, v.z, v.w);

        public static implicit operator (byte, byte, byte, byte) (Byte4 v) => (v.x, v.y, v.z, v.w);

        public static bool operator == (Byte4 a, Byte4 b) => a.uintValue == b.uintValue;

        public static bool operator != (Byte4 a, Byte4 b) => a.uintValue != b.uintValue;

        public unsafe static Byte4 operator + (Byte4 a, Byte4 b) => new Byte4 ((byte)(a.x + b.x), (byte)(a.y + b.y), (byte)(a.z + b.z), (byte)(a.w + b.w));

        public static Byte4 operator - (Byte4 a, Byte4 b) => new Byte4 ((byte)(a.x - b.x), (byte)(a.y - b.y), (byte)(a.z - b.z), (byte)(a.w - b.w));

        public static Byte4 operator - (Byte4 v) => new Byte4 ((byte)(v.x * -1), (byte)(v.y * -1), (byte)(v.z * -1), (byte)(v.w * -1));

        public static Byte4 operator * (Byte4 a, float b) => new Byte4 ((byte)(a.x * b), (byte)(a.y * b), (byte)(a.z * b), (byte)(a.w * b));

        public static Byte4 operator / (Byte4 a, float b) => new Byte4 ((byte)(a.x / b), (byte)(a.y / b), (byte)(a.z / b), (byte)(a.w / b));

        public static bool operator > (Byte4 a, Byte4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Byte4 a, Byte4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Byte4 a, Byte4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Byte4 a, Byte4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public override int GetHashCode () => (int)uintValue;

        public bool Equals (Byte4 other) => this == other;

        public override bool Equals (object obj)
        {
            Byte4? v = obj as Byte4?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
