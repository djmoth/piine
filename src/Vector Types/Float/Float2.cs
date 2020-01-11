using System;
//using System.Numerics;
using UnityEngine;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct Float2 : IEquatable<Float2>
    {
        public const int Size = 2;

        private static readonly Float2 zero = new Float2 (0);
        private static readonly Float2 one = new Float2 (1);
        private static readonly Float2 unitX = new Float2 (1, 0);
        private static readonly Float2 unitY = new Float2 (0, 1);

        public static ref readonly Float2 Zero => ref zero;
        public static ref readonly Float2 One => ref one;
        public static ref readonly Float2 UnitX => ref unitX;
        public static ref readonly Float2 UnitY => ref unitY;

        [FieldOffset (0)]
        internal fixed float components[Size];

        [FieldOffset (0)]
        public float x;
        [FieldOffset (4)]
        public float y;

        public float Length => (float)Math.Sqrt (LengthSquared);

        public float LengthSquared => (x * x) + (y * y);

        public Float2 (float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Float2 (float all)
        {
            x = all;
            y = all;
        }

        public float this[int index]
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

        /// <summary>
        /// Get the value of a component without any bounds checking. Be mindful when working with this.
        /// </summary>
        /// <param name="index">The 0-based index of the component</param>
        public float GetUnsafe (int index) => components[index];

        /// <summary>
        /// Set the value of a component without any bounds checking. Be mindful when working with this.
        /// </summary>
        /// <param name="index">The 0-based index of the component</param>
        /// <param name="value">The value to write</param>
        public void SetUnsafe (int index, float value) => components[index] = value;    

        public float CalculateArea () => x * y;

        public static Float2 Normalize (Float2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = v.x / oldMagnitude;
            v.y = v.y / oldMagnitude;

            return v;
        }

        public static Float2 Absolute (Float2 v) => new Float2 (Math.Abs (v.x), Math.Abs (v.y));

        public static float Distance (Float2 a, Float2 b) => (a - b).Length;

        public static explicit operator Float2 (Int2 v) => new Float2 (v.x, v.y);

        public static explicit operator Vector2 (Float2 v) => new Vector2 (v.x, v.y);

        public static explicit operator Float2 (Vector2 v) => new Float2 (v.x, v.y);

        public static implicit operator Float2 ((float x, float y) v) => new Float2 (v.x, v.y);

        public static implicit operator (float, float) (Float2 v) => (v.x, v.y);

        public static bool operator == (Float2 a, Float2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Float2 a, Float2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Float2 operator + (Float2 a, Float2 b) => new Float2 (a.x + b.x, a.y + b.y);

        public static Float2 operator - (Float2 a, Float2 b) => new Float2 (a.x - b.x, a.y - b.y);

        public static Float2 operator - (Float2 v) => new Float2 (v.x * -1, v.y * -1);

        public static Float2 operator * (Float2 a, float b) => new Float2 (a.x * b, a.y * b);

        public static Float2 operator / (Float2 a, float b) => new Float2 (a.x / b, a.y / b);
        
        public static bool operator > (Float2 a, Float2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Float2 a, Float2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Float2 a, Float2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Float2 a, Float2 b) => a.x <= b.x && a.y <= b.y;

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Float2 other) => this == other;

        public override bool Equals (object obj)
        {
            Float2? v = obj as Float2?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}