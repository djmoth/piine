using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 2)]
    public unsafe struct Byte2 : IEquatable<Byte2>
    {
        public const int Size = 2;

        private static readonly Byte2 zero = new Byte2 (0);
        private static readonly Byte2 one = new Byte2 (1);
        private static readonly Byte2 unitX = new Byte2 (1, 0);
        private static readonly Byte2 unitY = new Byte2 (0, 1);

        public static ref readonly Byte2 Zero => ref zero;
        public static ref readonly Byte2 One => ref one;
        public static ref readonly Byte2 UnitX => ref unitX;
        public static ref readonly Byte2 UnitY => ref unitY;

        
        [FieldOffset (0)]
        internal fixed byte components[Size];

        [FieldOffset (0)]
        public byte x;
        [FieldOffset (1)]
        public byte y;

        [FieldOffset (0)]
        private ushort ushortValue;

        public float Length => (float)Math.Sqrt (LengthSquared);

        public int LengthSquared => (x * x) + (y * y);

        public ushort AsUInt16 => ushortValue;

        public Byte2 (byte x, byte y)
        {
            ushortValue = 0;
            this.x = x;
            this.y = y;          
        }

        public Byte2 (byte all)
        {
            ushortValue = 0;
            x = all;
            y = all;
        }

        public byte this[int index]
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

        public byte GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, byte value) => components[index] = value;

        public int CalculateArea () => x * y;

        public static Byte2 Absolute (Byte2 v) => new Byte2 ((byte)Math.Abs (v.x), (byte)Math.Abs (v.y));

        public static float Distance (Byte2 a, Byte2 b) => (a - b).Length;

        public static explicit operator Byte2 (Float2 v) => new Byte2 ((byte)v.x, (byte)v.y);

        public static explicit operator Vector2 (Byte2 v) => new Vector2 (v.x, v.y);

        public static explicit operator Byte2 (Vector2 v) => new Byte2 ((byte)v.X, (byte)v.Y);

        public static implicit operator Byte2 ((byte x, byte y) v) => new Byte2 (v.x, v.y);

        public static implicit operator (byte, byte) (Byte2 v) => (v.x, v.y);

        public static bool operator == (Byte2 a, Byte2 b) => *(ushort*)a.components == *(ushort*)b.components;

        public static bool operator != (Byte2 a, Byte2 b) => *(ushort*)a.components != *(ushort*)b.components;

        public unsafe static Byte2 operator + (Byte2 a, Byte2 b) => new Byte2 ((byte)(a.x + b.x), (byte)(a.y + b.y));

        public static Byte2 operator - (Byte2 a, Byte2 b) => new Byte2 ((byte)(a.x - b.x), (byte)(a.y - b.y));

        public static Byte2 operator - (Byte2 v) => new Byte2 ((byte)(v.x * -1), (byte)(v.y * -1));

        public static Byte2 operator * (Byte2 a, byte b) => new Byte2 ((byte)(a.x * b), (byte)(a.y * b));

        public static Byte2 operator / (Byte2 a, float b) => new Byte2 ((byte)(a.x / b), (byte)(a.y / b));

        public static bool operator > (Byte2 a, Byte2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Byte2 a, Byte2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Byte2 a, Byte2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Byte2 a, Byte2 b) => a.x <= b.x && a.y <= b.y;

        public override int GetHashCode () => ushortValue;

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Byte2 other) => this == other;

        public override bool Equals (object obj)
        {
            Byte2? v = obj as Byte2?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}