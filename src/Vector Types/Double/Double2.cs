using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct Double2 : IEquatable<Double2>
    {
        public const int Size = 2;

        private static readonly Double2 zero = new Double2 (0);
        private static readonly Double2 one = new Double2 (1);
        private static readonly Double2 unitX = new Double2 (1, 0);
        private static readonly Double2 unitY = new Double2 (0, 1);

        public static ref readonly Double2 Zero => ref zero;
        public static ref readonly Double2 One => ref one;
        public static ref readonly Double2 UnitX => ref unitX;
        public static ref readonly Double2 UnitY => ref unitY;

        [CLSCompliant (false)]
        [FieldOffset (0)]
        public fixed double components[Size];

        [FieldOffset (0)]
        public double x;
        [FieldOffset (4)]
        public double y;

        public double Length => (double)Math.Sqrt (LengthSquared);

        public double LengthSquared => (x * x) + (y * y);

        public Double2 (double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Double2 (double all)
        {
            x = all;
            y = all;
        }

        public double this[int index]
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

        public double GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, double value) => components[index] = value;
        
        public double CalculateArea () => x * y;

        public static Double2 Normalize (Double2 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length;
            v.x = v.x / oldMagnitude;
            v.y = v.y / oldMagnitude;

            return v;
        }

        public static Double2 Absolute (Double2 v) => new Double2 (Math.Abs (v.x), Math.Abs (v.y));

        public static double Distance (Double2 a, Double2 b) => (a - b).Length;


        public static explicit operator Double2 (Int2 v) => new Double2 (v.x, v.y);

        public static explicit operator Vector2 (Double2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Double2 (Vector2 v) => new Double2 (v.X, v.Y);

        public static implicit operator Double2 ((double x, double y) v) => new Double2 (v.x, v.y);

        public static implicit operator (double, double) (Double2 v) => (v.x, v.y);

        public static bool operator == (Double2 a, Double2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Double2 a, Double2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Double2 operator + (Double2 a, Double2 b) => new Double2 (a.x + b.x, a.y + b.y);

        public static Double2 operator - (Double2 a, Double2 b) => new Double2 (a.x - b.x, a.y - b.y);

        public static Double2 operator - (Double2 v) => new Double2 (v.x * -1, v.y * -1);

        public static Double2 operator * (Double2 a, double b) => new Double2 (a.x * b, a.y * b);

        public static Double2 operator / (Double2 a, double b) => new Double2 (a.x / b, a.y / b);

        public static bool operator > (Double2 a, Double2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Double2 a, Double2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Double2 a, Double2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Double2 a, Double2 b) => a.x <= b.x && a.y <= b.y;

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Double2 other) => this == other;

        public override bool Equals (object obj)
        {
            Double2? v = obj as Double2?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}