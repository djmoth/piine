using System;
//using System.Numerics;
using UnityEngine;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 12)]
    public unsafe struct Float3 : IEquatable<Float3>
    {
        public const int Size = 3;

        private static readonly Float3 zero = new Float3 (0);
        private static readonly Float3 one = new Float3 (1);
        private static readonly Float3 unitX = new Float3 (1, 0, 0);
        private static readonly Float3 unitY = new Float3 (0, 1, 0);
        private static readonly Float3 unitZ = new Float3 (0, 0, 1);

        public static ref readonly Float3 Zero => ref zero;
        public static ref readonly Float3 One => ref one;
        public static ref readonly Float3 UnitX => ref unitX;
        public static ref readonly Float3 UnitY => ref unitY;
        public static ref readonly Float3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        internal fixed float components[Size];

        [FieldOffset (0)]
        public float x;
        [FieldOffset (4)]
        public float y;
        [FieldOffset (8)]
        public float z;

        public Float2 XY => new Float2 (x, y);
        public Float2 XZ => new Float2 (x, z);
        public Float2 YZ => new Float2 (y, z);

        public float Length => (float)Math.Sqrt (LengthSquared);

        public float LengthSquared => (x * x) + (y * y) + (z * z);

        public Float3 (float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Float3 (float all)
        {
            x = all;
            y = all;
            z = all;
        }

        public float this[int index]
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

        public float GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, float value) => components[index] = value;     

        public float CalculateVolume () => x * y * z;

        public static Float3 Normalize (Float3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = v.x / oldMagnitude;
            v.y = v.y / oldMagnitude;
            v.z = v.z / oldMagnitude;

            return v;
        }

        public static Float3 Absolute (Float3 v)
        {
            return new Float3 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z));
        }

        public static float Distance (Float3 a, Float3 b)
        {
            return (a - b).Length;
        }

        public static Float3 Cross (Float3 a, Float3 b)
        {
            Float3 r = new Float3 ();
            r.x = a.y * b.z - a.z * b.y;
            r.y = a.x * b.z - a.z * b.x;
            r.z = a.y * b.x - a.x * b.y;

            return r;
        }

        public static explicit operator Float3 (Int3 v) => new Float3 (v.x, v.y, v.z);

        public static explicit operator Vector3 (Float3 v) => new Vector3 (v.x, v.y, v.z);

        public static explicit operator Float3 (Vector3 v) => new Float3 (v.x, v.y, v.z);

        public static implicit operator Float3 ((float x, float y, float z) v) => new Float3 (v.x, v.y, v.z);

        public static implicit operator (float, float, float) (Float3 v) => (v.x, v.y, v.z);

        public static bool operator == (Float3 a, Float3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Float3 a, Float3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Float3 operator + (Float3 a, Float3 b) => new Float3 (a.x + b.x, a.y + b.y, a.z + b.z);

        public static Float3 operator - (Float3 a, Float3 b) => new Float3 (a.x - b.x, a.y - b.y, a.z - b.z);

        public static Float3 operator - (Float3 v) => new Float3 (v.x * -1, v.y * -1, v.z * -1);

        public static Float3 operator * (Float3 a, float b) => new Float3 (a.x * b, a.y * b, a.z * b);

        public static Float3 operator / (Float3 a, float b) => new Float3 (a.x / b, a.y / b, a.z / b);

        public static bool operator > (Float3 a, Float3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Float3 a, Float3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Float3 a, Float3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Float3 a, Float3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (Float3 other) => this == other;

        public override bool Equals (object obj)
        {
            Float3? v = obj as Float3?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}