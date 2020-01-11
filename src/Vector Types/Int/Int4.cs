using System;
//using System.Numerics;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 16)]
    public unsafe struct Int4 : IEquatable<Int4>
    {
        public const int Size = 4; //Number of dimensions

        private static readonly Int4 zero = new Int4 (0);
        private static readonly Int4 one = new Int4 (1);
        private static readonly Int4 unitX = new Int4 (1, 0, 0, 0);
        private static readonly Int4 unitY = new Int4 (0, 1, 0, 0);
        private static readonly Int4 unitZ = new Int4 (0, 0, 1, 0);
        private static readonly Int4 unitW = new Int4 (0, 0, 0, 1);

        public static ref readonly Int4 Zero => ref zero;
        public static ref readonly Int4 One => ref one;
        public static ref readonly Int4 UnitX => ref unitX;
        public static ref readonly Int4 UnitY => ref unitY;
        public static ref readonly Int4 UnitZ => ref unitZ;
        public static ref readonly Int4 UnitW => ref unitW;

        [FieldOffset (0)]
        internal fixed int components[Size];

        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;
        [FieldOffset (8)]
        public int z;
        [FieldOffset (12)]
        public int w;

        public float Length => (float)Math.Sqrt ((x * x) + (y * y) + (z * z) + (w * w));

        public float LengthSquared => (x * x) + (y * y) + (z * z) + (w * w);

        public Int4 (int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Int4 (int all)
        {
            x = all;
            y = all;
            z = all;
            w = all;
        }

        public int this[int index]
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

        public static Int4 Normalize (Int4 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = (int)Math.Round (v.x / oldMagnitude);
            v.y = (int)Math.Round (v.y / oldMagnitude);
            v.z = (int)Math.Round (v.z / oldMagnitude);
            v.w = (int)Math.Round (v.w / oldMagnitude);

            return v;
        }

        public static Int4 Absolute (Int4 v)
        {
            return new Int4 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z), Math.Abs (v.w));
        }

        public static float Distance (Int4 a, Int4 b)
        {
            return (a - b).Length;
        }

        public static explicit operator Int4 (Float4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

        public static implicit operator Vector4 (Int4 iv) => new Vector4 (iv.x, iv.y, iv.z, iv.w);

        public static implicit operator Int4 (Vector4 iv) => new Int4 ((int)iv.x, (int)iv.y, (int)iv.z, (int)iv.w);

        public static implicit operator Int4 ((int x, int y, int z, int w) v) => new Int4 (v.x, v.y, v.z, v.w);

        public static implicit operator (int, int, int, int) (Int4 v) => (v.x, v.y, v.z, v.w);

        public static bool operator == (Int4 a, Int4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Int4 a, Int4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Int4 operator + (Int4 a, Int4 b) => new Int4 (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

        public static Int4 operator - (Int4 a, Int4 b) => new Int4 (a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

        public static Int4 operator - (Int4 v) => new Int4 (v.x * -1, v.y * -1, v.z * -1, v.w * -1);

        public static Int4 operator * (Int4 a, float b) => new Int4 ((int)(a.x * b), (int)(a.y * b), (int)(a.z * b), (int)(a.w * b));

        public static Int4 operator / (Int4 a, float b) => new Int4 ((int)(a.x / b), (int)(a.y / b), (int)(a.z / b), (int)(a.w / b));

        public static bool operator > (Int4 a, Int4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Int4 a, Int4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Int4 a, Int4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Int4 a, Int4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

        public override string ToString () =>  "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public override int GetHashCode () =>  (x * 74856094) ^ (y * 19449664) ^ (z * 84492791) ^ (w * 21016934);

        public bool Equals (Int4 other) => this == other;

        public override bool Equals (object obj)
        {
            Int4? v = obj as Int4?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
