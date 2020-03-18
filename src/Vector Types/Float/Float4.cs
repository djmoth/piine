using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 16)]
    public unsafe struct Float4 : IEquatable<Float4>
    {
        public const int Size = 4;

        private static readonly Float4 zero = new Float4 (0);
        private static readonly Float4 one = new Float4 (1);
        private static readonly Float4 unitX = new Float4 (1, 0, 0, 0);
        private static readonly Float4 unitY = new Float4 (0, 1, 0, 0);
        private static readonly Float4 unitZ = new Float4 (0, 0, 1, 0);
        private static readonly Float4 unitW = new Float4 (0, 0, 0, 1);

        public static ref readonly Float4 Zero => ref zero;
        public static ref readonly Float4 One => ref one;
        public static ref readonly Float4 UnitX => ref unitX;
        public static ref readonly Float4 UnitY => ref unitY;
        public static ref readonly Float4 UnitZ => ref unitZ;
        public static ref readonly Float4 UnitW => ref unitW;

        [FieldOffset (0)]
        internal fixed float components[Size];

        [FieldOffset (0)]
        public float x;
        [FieldOffset (4)]
        public float y;
        [FieldOffset (8)]
        public float z;
        [FieldOffset (12)]
        public float w;

        public Float2 XY => new Float2 (x, y);
        public Float2 XZ => new Float2 (x, z);
        public Float2 YZ => new Float2 (y, z);

        public float Length => (float)Math.Sqrt (LengthSquared);

        public float LengthSquared => (x * x) + (y * y) + (z * z) * (w * w);

        public Float4 (float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Float4 (float all)
        {
            x = all;
            y = all;
            z = all;
            w = all;
        }

        public float this[int index]
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

        public float GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, float value) => components[index] = value;

        public float CalculateVolume () => x * y * z * w;

        public static Float4 Normalize (Float4 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;

            return v / oldMagnitude;
        }

        public static Float4 Absolute (Float4 v) => new Float4 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z), Math.Abs (v.w));

        public static float Distance (Float4 a, Float4 b) => (a - b).Length;

        public static float Dot (Float4 a, Float4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

        public static explicit operator Float4 (Int4 v) => new Float4 (v.x, v.y, v.z, v.w);

        public static explicit operator Vector4 (Float4 v) => new Vector4 (v.x, v.y, v.z, v.w);

        public static explicit operator Float4 (Vector4 v) => new Float4 (v.X, v.Y, v.Z, v.W);

        public static implicit operator Float4 ((float x, float y, float z, float w) v) => new Float4 (v.x, v.y, v.z, v.w);

        public static implicit operator (float, float, float, float) (Float4 v) => (v.x, v.y, v.z, v.w);

        public static bool operator == (Float4 a, Float4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Float4 a, Float4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Float4 operator + (Float4 a, Float4 b) => new Float4 (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

        public static Float4 operator - (Float4 a, Float4 b) => new Float4 (a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

        public static Float4 operator - (Float4 v) => new Float4 (v.x * -1, v.y * -1, v.z * -1, v.w * -1);

        public static Float4 operator * (Float4 a, float b) => new Float4 (a.x * b, a.y * b, a.z * b, a.w * b);

        public static Float4 operator / (Float4 a, float b) => new Float4 (a.x / b, a.y / b, a.z / b, a.w / b);

        public static bool operator > (Float4 a, Float4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Float4 a, Float4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Float4 a, Float4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Float4 a, Float4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (Float4 other) => this == other;

        public override bool Equals (object obj)
        {
            Float4? v = obj as Float4?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}