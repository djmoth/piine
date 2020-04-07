using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace piine
{
	
	[CLSCompliant (true)]
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
        private fixed float components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public float x;
        [FieldOffset (4)]
        public float y;
        [FieldOffset (8)]
        public float z;
		[FieldOffset (12)]
		public float w;
		#pragma warning restore CA1051

        public Float2 XY => new Float2 (x, y);
        public Float2 XZ => new Float2 (x, z);
        public Float2 YZ => new Float2 (y, z);
		
		public Float3 XYZ => new Float3 (x, y, z);
		public Float3 XZY => new Float3 (x, z, y);
		public Float3 YXZ => new Float3 (y, x, z);
		public Float3 YZX => new Float3 (y, z, x);
		public Float3 ZXY => new Float3 (z, x, y);
		public Float3 ZYX => new Float3 (z, y, x);

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

        public Float4 (float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public Float4 (Float2 leftPart, Float2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public Float4 (float all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public float GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, float value) => components[index] = value;

        public static Float4 Normalize (Float4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static Float4 Absolute (Float4 v) => new Float4 ((float)Math.Abs ((float)v.x), (float)Math.Abs ((float)v.y), (float)Math.Abs ((float)v.z), (float)Math.Abs ((float)v.w));

        public static float Distance (Float4 a, Float4 b) => (a - b).Length ();

        public static float Dot (Float4 a, Float4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static Float4 Reverse (Float4 v) => new Float4 (v.w, v.z, v.y, v.x);

		public float Sum () => x + y + z + w;

		public float Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (float value) => x == value || y == value || z == value || w == value;

        public static bool operator == (Float4 a, Float4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Float4 a, Float4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Float4 operator + (Float4 a, Float4 b) => new Float4 ((float)(a.x + b.x), (float)(a.y + b.y), (float)(a.z + b.z), (float)(a.w + b.w));

        public static Float4 operator - (Float4 a, Float4 b) => new Float4 ((float)(a.x - b.x), (float)(a.y - b.y), (float)(a.z - b.z), (float)(a.w - b.w));

		public static Float4 operator - (Float4 v) => new Float4 ((float)-v.x, (float)-v.y, (float)-v.z, (float)-v.w);
		
        public static Float4 operator * (Float4 a, float b) => new Float4 ((float)(a.x * b), (float)(a.y * b), (float)(a.z * b), (float)(a.w * b));

        public static Float4 operator / (Float4 a, float b) => new Float4 ((float)(a.x / b), (float)(a.y / b), (float)(a.z / b), (float)(a.w / b));

        public static bool operator > (Float4 a, Float4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Float4 a, Float4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Float4 a, Float4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Float4 a, Float4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out float x, out float y, out float z, out float w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

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

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (Float4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static implicit operator Float4 (Vector4 v) => new Float4 ((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);

		//Tuple
		public static implicit operator Float4 ((float x, float y, float z, float w) v) => new Float4 (v.x, v.y, v.z, v.w);

        public static implicit operator (float, float, float, float) (Float4 v) => (v.x, v.y, v.z, v.w);

		
		//Double4
		public static explicit operator Float4 (Double4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//Int4
		public static implicit operator Float4 (Int4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//UInt4
		public static implicit operator Float4 (UInt4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//Byte4
		public static implicit operator Float4 (Byte4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//SByte4
		public static implicit operator Float4 (SByte4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//Short4
		public static implicit operator Float4 (Short4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//UShort4
		public static implicit operator Float4 (UShort4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//Long4
		public static implicit operator Float4 (Long4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);

		//ULong4
		public static implicit operator Float4 (ULong4 v) => new Float4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
	}

	[CLSCompliant (true)]
    [StructLayout (LayoutKind.Explicit, Size = 32)]
    public unsafe struct Double4 : IEquatable<Double4>
    {
        public const int Size = 4;

        private static readonly Double4 zero = new Double4 (0);
        private static readonly Double4 one = new Double4 (1);
        private static readonly Double4 unitX = new Double4 (1, 0, 0, 0);
        private static readonly Double4 unitY = new Double4 (0, 1, 0, 0);
        private static readonly Double4 unitZ = new Double4 (0, 0, 1, 0);
		private static readonly Double4 unitW = new Double4 (0, 0, 0, 1);

        public static ref readonly Double4 Zero => ref zero;
        public static ref readonly Double4 One => ref one;
        public static ref readonly Double4 UnitX => ref unitX;
        public static ref readonly Double4 UnitY => ref unitY;
        public static ref readonly Double4 UnitZ => ref unitZ;
		public static ref readonly Double4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed double components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public double x;
        [FieldOffset (8)]
        public double y;
        [FieldOffset (16)]
        public double z;
		[FieldOffset (24)]
		public double w;
		#pragma warning restore CA1051

        public Double2 XY => new Double2 (x, y);
        public Double2 XZ => new Double2 (x, z);
        public Double2 YZ => new Double2 (y, z);
		
		public Double3 XYZ => new Double3 (x, y, z);
		public Double3 XZY => new Double3 (x, z, y);
		public Double3 YXZ => new Double3 (y, x, z);
		public Double3 YZX => new Double3 (y, z, x);
		public Double3 ZXY => new Double3 (z, x, y);
		public Double3 ZYX => new Double3 (z, y, x);

		public double this[int index]
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

        public Double4 (double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public Double4 (Double2 leftPart, Double2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public Double4 (double all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public double GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, double value) => components[index] = value;

        public static Double4 Normalize (Double4 v)
        {
            double length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (double)Math.Sqrt (length);

            return v / length;
        }

        public static Double4 Absolute (Double4 v) => new Double4 ((double)Math.Abs ((double)v.x), (double)Math.Abs ((double)v.y), (double)Math.Abs ((double)v.z), (double)Math.Abs ((double)v.w));

        public static double Distance (Double4 a, Double4 b) => (a - b).Length ();

        public static double Dot (Double4 a, Double4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static Double4 Reverse (Double4 v) => new Double4 (v.w, v.z, v.y, v.x);

		public double Sum () => x + y + z + w;

		public double Volume () => x * y * z * w;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y) + ((double)z * z) + ((double)w * w);

		public bool Contains (double value) => x == value || y == value || z == value || w == value;

        public static bool operator == (Double4 a, Double4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Double4 a, Double4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Double4 operator + (Double4 a, Double4 b) => new Double4 ((double)(a.x + b.x), (double)(a.y + b.y), (double)(a.z + b.z), (double)(a.w + b.w));

        public static Double4 operator - (Double4 a, Double4 b) => new Double4 ((double)(a.x - b.x), (double)(a.y - b.y), (double)(a.z - b.z), (double)(a.w - b.w));

		public static Double4 operator - (Double4 v) => new Double4 ((double)-v.x, (double)-v.y, (double)-v.z, (double)-v.w);
		
        public static Double4 operator * (Double4 a, double b) => new Double4 ((double)(a.x * b), (double)(a.y * b), (double)(a.z * b), (double)(a.w * b));

        public static Double4 operator / (Double4 a, double b) => new Double4 ((double)(a.x / b), (double)(a.y / b), (double)(a.z / b), (double)(a.w / b));

        public static bool operator > (Double4 a, Double4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Double4 a, Double4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Double4 a, Double4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Double4 a, Double4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out double x, out double y, out double z, out double w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (Double4 other) => this == other;

        public override bool Equals (object obj)
        {
            Double4? v = obj as Double4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static explicit operator Vector4 (Double4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static implicit operator Double4 (Vector4 v) => new Double4 ((double)v.X, (double)v.Y, (double)v.Z, (double)v.W);

		//Tuple
		public static implicit operator Double4 ((double x, double y, double z, double w) v) => new Double4 (v.x, v.y, v.z, v.w);

        public static implicit operator (double, double, double, double) (Double4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static implicit operator Double4 (Float4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//Int4
		public static implicit operator Double4 (Int4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//UInt4
		public static implicit operator Double4 (UInt4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//Byte4
		public static implicit operator Double4 (Byte4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//SByte4
		public static implicit operator Double4 (SByte4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//Short4
		public static implicit operator Double4 (Short4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//UShort4
		public static implicit operator Double4 (UShort4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//Long4
		public static implicit operator Double4 (Long4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);

		//ULong4
		public static implicit operator Double4 (ULong4 v) => new Double4 ((double)v.x, (double)v.y, (double)v.z, (double)v.w);
	}

	[CLSCompliant (true)]
    [StructLayout (LayoutKind.Explicit, Size = 16)]
    public unsafe struct Int4 : IEquatable<Int4>
    {
        public const int Size = 4;

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
        private fixed int components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;
        [FieldOffset (8)]
        public int z;
		[FieldOffset (12)]
		public int w;
		#pragma warning restore CA1051

        public Int2 XY => new Int2 (x, y);
        public Int2 XZ => new Int2 (x, z);
        public Int2 YZ => new Int2 (y, z);
		
		public Int3 XYZ => new Int3 (x, y, z);
		public Int3 XZY => new Int3 (x, z, y);
		public Int3 YXZ => new Int3 (y, x, z);
		public Int3 YZX => new Int3 (y, z, x);
		public Int3 ZXY => new Int3 (z, x, y);
		public Int3 ZYX => new Int3 (z, y, x);

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

        public Int4 (int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public Int4 (Int2 leftPart, Int2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public Int4 (int all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public int GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, int value) => components[index] = value;

        public static Int4 Normalize (Int4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static Int4 Absolute (Int4 v) => new Int4 ((int)Math.Abs ((float)v.x), (int)Math.Abs ((float)v.y), (int)Math.Abs ((float)v.z), (int)Math.Abs ((float)v.w));

        public static float Distance (Int4 a, Int4 b) => (a - b).Length ();

        public static float Dot (Int4 a, Int4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static Int4 Reverse (Int4 v) => new Int4 (v.w, v.z, v.y, v.x);

		public int Sum () => x + y + z + w;

		public int Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (int value) => x == value || y == value || z == value || w == value;

        public static bool operator == (Int4 a, Int4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Int4 a, Int4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Int4 operator + (Int4 a, Int4 b) => new Int4 ((int)(a.x + b.x), (int)(a.y + b.y), (int)(a.z + b.z), (int)(a.w + b.w));

        public static Int4 operator - (Int4 a, Int4 b) => new Int4 ((int)(a.x - b.x), (int)(a.y - b.y), (int)(a.z - b.z), (int)(a.w - b.w));

		public static Int4 operator - (Int4 v) => new Int4 ((int)-v.x, (int)-v.y, (int)-v.z, (int)-v.w);
		
        public static Int4 operator * (Int4 a, float b) => new Int4 ((int)(a.x * b), (int)(a.y * b), (int)(a.z * b), (int)(a.w * b));

        public static Int4 operator / (Int4 a, float b) => new Int4 ((int)(a.x / b), (int)(a.y / b), (int)(a.z / b), (int)(a.w / b));

        public static bool operator > (Int4 a, Int4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Int4 a, Int4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Int4 a, Int4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Int4 a, Int4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out int x, out int y, out int z, out int w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (Int4 other) => this == other;

        public override bool Equals (object obj)
        {
            Int4? v = obj as Int4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (Int4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static implicit operator Int4 (Vector4 v) => new Int4 ((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);

		//Tuple
		public static implicit operator Int4 ((int x, int y, int z, int w) v) => new Int4 (v.x, v.y, v.z, v.w);

        public static implicit operator (int, int, int, int) (Int4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static explicit operator Int4 (Float4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//Double4
		public static explicit operator Int4 (Double4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//UInt4
		public static explicit operator Int4 (UInt4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//Byte4
		public static implicit operator Int4 (Byte4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//SByte4
		public static implicit operator Int4 (SByte4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//Short4
		public static implicit operator Int4 (Short4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//UShort4
		public static implicit operator Int4 (UShort4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//Long4
		public static explicit operator Int4 (Long4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);

		//ULong4
		public static explicit operator Int4 (ULong4 v) => new Int4 ((int)v.x, (int)v.y, (int)v.z, (int)v.w);
	}

	[CLSCompliant (false)]
    [StructLayout (LayoutKind.Explicit, Size = 16)]
    public unsafe struct UInt4 : IEquatable<UInt4>
    {
        public const int Size = 4;

        private static readonly UInt4 zero = new UInt4 (0);
        private static readonly UInt4 one = new UInt4 (1);
        private static readonly UInt4 unitX = new UInt4 (1, 0, 0, 0);
        private static readonly UInt4 unitY = new UInt4 (0, 1, 0, 0);
        private static readonly UInt4 unitZ = new UInt4 (0, 0, 1, 0);
		private static readonly UInt4 unitW = new UInt4 (0, 0, 0, 1);

        public static ref readonly UInt4 Zero => ref zero;
        public static ref readonly UInt4 One => ref one;
        public static ref readonly UInt4 UnitX => ref unitX;
        public static ref readonly UInt4 UnitY => ref unitY;
        public static ref readonly UInt4 UnitZ => ref unitZ;
		public static ref readonly UInt4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed uint components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public uint x;
        [FieldOffset (4)]
        public uint y;
        [FieldOffset (8)]
        public uint z;
		[FieldOffset (12)]
		public uint w;
		#pragma warning restore CA1051

        public UInt2 XY => new UInt2 (x, y);
        public UInt2 XZ => new UInt2 (x, z);
        public UInt2 YZ => new UInt2 (y, z);
		
		public UInt3 XYZ => new UInt3 (x, y, z);
		public UInt3 XZY => new UInt3 (x, z, y);
		public UInt3 YXZ => new UInt3 (y, x, z);
		public UInt3 YZX => new UInt3 (y, z, x);
		public UInt3 ZXY => new UInt3 (z, x, y);
		public UInt3 ZYX => new UInt3 (z, y, x);

		public uint this[int index]
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

        public UInt4 (uint x, uint y, uint z, uint w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public UInt4 (UInt2 leftPart, UInt2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public UInt4 (uint all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public uint GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, uint value) => components[index] = value;

        public static UInt4 Normalize (UInt4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static UInt4 Absolute (UInt4 v) => new UInt4 ((uint)Math.Abs ((float)v.x), (uint)Math.Abs ((float)v.y), (uint)Math.Abs ((float)v.z), (uint)Math.Abs ((float)v.w));

        public static float Distance (UInt4 a, UInt4 b) => (a - b).Length ();

        public static float Dot (UInt4 a, UInt4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static UInt4 Reverse (UInt4 v) => new UInt4 (v.w, v.z, v.y, v.x);

		public uint Sum () => x + y + z + w;

		public uint Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (uint value) => x == value || y == value || z == value || w == value;

        public static bool operator == (UInt4 a, UInt4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (UInt4 a, UInt4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static UInt4 operator + (UInt4 a, UInt4 b) => new UInt4 ((uint)(a.x + b.x), (uint)(a.y + b.y), (uint)(a.z + b.z), (uint)(a.w + b.w));

        public static UInt4 operator - (UInt4 a, UInt4 b) => new UInt4 ((uint)(a.x - b.x), (uint)(a.y - b.y), (uint)(a.z - b.z), (uint)(a.w - b.w));

		
        public static UInt4 operator * (UInt4 a, float b) => new UInt4 ((uint)(a.x * b), (uint)(a.y * b), (uint)(a.z * b), (uint)(a.w * b));

        public static UInt4 operator / (UInt4 a, float b) => new UInt4 ((uint)(a.x / b), (uint)(a.y / b), (uint)(a.z / b), (uint)(a.w / b));

        public static bool operator > (UInt4 a, UInt4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (UInt4 a, UInt4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (UInt4 a, UInt4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (UInt4 a, UInt4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out uint x, out uint y, out uint z, out uint w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (UInt4 other) => this == other;

        public override bool Equals (object obj)
        {
            UInt4? v = obj as UInt4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (UInt4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static explicit operator UInt4 (Vector4 v) => new UInt4 ((uint)v.X, (uint)v.Y, (uint)v.Z, (uint)v.W);

		//Tuple
		public static implicit operator UInt4 ((uint x, uint y, uint z, uint w) v) => new UInt4 (v.x, v.y, v.z, v.w);

        public static implicit operator (uint, uint, uint, uint) (UInt4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static explicit operator UInt4 (Float4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//Double4
		public static explicit operator UInt4 (Double4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//Int4
		public static explicit operator UInt4 (Int4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//Byte4
		public static implicit operator UInt4 (Byte4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//SByte4
		public static implicit operator UInt4 (SByte4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//Short4
		public static implicit operator UInt4 (Short4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//UShort4
		public static implicit operator UInt4 (UShort4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//Long4
		public static explicit operator UInt4 (Long4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);

		//ULong4
		public static explicit operator UInt4 (ULong4 v) => new UInt4 ((uint)v.x, (uint)v.y, (uint)v.z, (uint)v.w);
	}

	[CLSCompliant (false)]
    [StructLayout (LayoutKind.Explicit, Size = 4)]
    public unsafe struct Byte4 : IEquatable<Byte4>
    {
        public const int Size = 4;

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
        private fixed byte components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public byte x;
        [FieldOffset (1)]
        public byte y;
        [FieldOffset (2)]
        public byte z;
		[FieldOffset (3)]
		public byte w;
		#pragma warning restore CA1051

        public Byte2 XY => new Byte2 (x, y);
        public Byte2 XZ => new Byte2 (x, z);
        public Byte2 YZ => new Byte2 (y, z);
		
		public Byte3 XYZ => new Byte3 (x, y, z);
		public Byte3 XZY => new Byte3 (x, z, y);
		public Byte3 YXZ => new Byte3 (y, x, z);
		public Byte3 YZX => new Byte3 (y, z, x);
		public Byte3 ZXY => new Byte3 (z, x, y);
		public Byte3 ZYX => new Byte3 (z, y, x);

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

        public Byte4 (byte x, byte y, byte z, byte w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public Byte4 (Byte2 leftPart, Byte2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public Byte4 (byte all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public byte GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, byte value) => components[index] = value;

        public static Byte4 Normalize (Byte4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static Byte4 Absolute (Byte4 v) => new Byte4 ((byte)Math.Abs ((float)v.x), (byte)Math.Abs ((float)v.y), (byte)Math.Abs ((float)v.z), (byte)Math.Abs ((float)v.w));

        public static float Distance (Byte4 a, Byte4 b) => (a - b).Length ();

        public static float Dot (Byte4 a, Byte4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static Byte4 Reverse (Byte4 v) => new Byte4 (v.w, v.z, v.y, v.x);

		public int Sum () => x + y + z + w;

		public int Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (byte value) => x == value || y == value || z == value || w == value;

        public static bool operator == (Byte4 a, Byte4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Byte4 a, Byte4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Byte4 operator + (Byte4 a, Byte4 b) => new Byte4 ((byte)(a.x + b.x), (byte)(a.y + b.y), (byte)(a.z + b.z), (byte)(a.w + b.w));

        public static Byte4 operator - (Byte4 a, Byte4 b) => new Byte4 ((byte)(a.x - b.x), (byte)(a.y - b.y), (byte)(a.z - b.z), (byte)(a.w - b.w));

		
        public static Byte4 operator * (Byte4 a, float b) => new Byte4 ((byte)(a.x * b), (byte)(a.y * b), (byte)(a.z * b), (byte)(a.w * b));

        public static Byte4 operator / (Byte4 a, float b) => new Byte4 ((byte)(a.x / b), (byte)(a.y / b), (byte)(a.z / b), (byte)(a.w / b));

        public static bool operator > (Byte4 a, Byte4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Byte4 a, Byte4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Byte4 a, Byte4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Byte4 a, Byte4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out byte x, out byte y, out byte z, out byte w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (Byte4 other) => this == other;

        public override bool Equals (object obj)
        {
            Byte4? v = obj as Byte4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (Byte4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static explicit operator Byte4 (Vector4 v) => new Byte4 ((byte)v.X, (byte)v.Y, (byte)v.Z, (byte)v.W);

		//Tuple
		public static implicit operator Byte4 ((byte x, byte y, byte z, byte w) v) => new Byte4 (v.x, v.y, v.z, v.w);

        public static implicit operator (byte, byte, byte, byte) (Byte4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static explicit operator Byte4 (Float4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//Double4
		public static explicit operator Byte4 (Double4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//Int4
		public static explicit operator Byte4 (Int4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//UInt4
		public static explicit operator Byte4 (UInt4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//SByte4
		public static explicit operator Byte4 (SByte4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//Short4
		public static explicit operator Byte4 (Short4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//UShort4
		public static explicit operator Byte4 (UShort4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//Long4
		public static explicit operator Byte4 (Long4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);

		//ULong4
		public static explicit operator Byte4 (ULong4 v) => new Byte4 ((byte)v.x, (byte)v.y, (byte)v.z, (byte)v.w);
	}

	[CLSCompliant (true)]
    [StructLayout (LayoutKind.Explicit, Size = 4)]
    public unsafe struct SByte4 : IEquatable<SByte4>
    {
        public const int Size = 4;

        private static readonly SByte4 zero = new SByte4 (0);
        private static readonly SByte4 one = new SByte4 (1);
        private static readonly SByte4 unitX = new SByte4 (1, 0, 0, 0);
        private static readonly SByte4 unitY = new SByte4 (0, 1, 0, 0);
        private static readonly SByte4 unitZ = new SByte4 (0, 0, 1, 0);
		private static readonly SByte4 unitW = new SByte4 (0, 0, 0, 1);

        public static ref readonly SByte4 Zero => ref zero;
        public static ref readonly SByte4 One => ref one;
        public static ref readonly SByte4 UnitX => ref unitX;
        public static ref readonly SByte4 UnitY => ref unitY;
        public static ref readonly SByte4 UnitZ => ref unitZ;
		public static ref readonly SByte4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed sbyte components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public sbyte x;
        [FieldOffset (1)]
        public sbyte y;
        [FieldOffset (2)]
        public sbyte z;
		[FieldOffset (3)]
		public sbyte w;
		#pragma warning restore CA1051

        public SByte2 XY => new SByte2 (x, y);
        public SByte2 XZ => new SByte2 (x, z);
        public SByte2 YZ => new SByte2 (y, z);
		
		public SByte3 XYZ => new SByte3 (x, y, z);
		public SByte3 XZY => new SByte3 (x, z, y);
		public SByte3 YXZ => new SByte3 (y, x, z);
		public SByte3 YZX => new SByte3 (y, z, x);
		public SByte3 ZXY => new SByte3 (z, x, y);
		public SByte3 ZYX => new SByte3 (z, y, x);

		public sbyte this[int index]
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

        public SByte4 (sbyte x, sbyte y, sbyte z, sbyte w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public SByte4 (SByte2 leftPart, SByte2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public SByte4 (sbyte all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public sbyte GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, sbyte value) => components[index] = value;

        public static SByte4 Normalize (SByte4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static SByte4 Absolute (SByte4 v) => new SByte4 ((sbyte)Math.Abs ((float)v.x), (sbyte)Math.Abs ((float)v.y), (sbyte)Math.Abs ((float)v.z), (sbyte)Math.Abs ((float)v.w));

        public static float Distance (SByte4 a, SByte4 b) => (a - b).Length ();

        public static float Dot (SByte4 a, SByte4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static SByte4 Reverse (SByte4 v) => new SByte4 (v.w, v.z, v.y, v.x);

		public int Sum () => x + y + z + w;

		public int Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (sbyte value) => x == value || y == value || z == value || w == value;

        public static bool operator == (SByte4 a, SByte4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (SByte4 a, SByte4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static SByte4 operator + (SByte4 a, SByte4 b) => new SByte4 ((sbyte)(a.x + b.x), (sbyte)(a.y + b.y), (sbyte)(a.z + b.z), (sbyte)(a.w + b.w));

        public static SByte4 operator - (SByte4 a, SByte4 b) => new SByte4 ((sbyte)(a.x - b.x), (sbyte)(a.y - b.y), (sbyte)(a.z - b.z), (sbyte)(a.w - b.w));

		public static SByte4 operator - (SByte4 v) => new SByte4 ((sbyte)-v.x, (sbyte)-v.y, (sbyte)-v.z, (sbyte)-v.w);
		
        public static SByte4 operator * (SByte4 a, float b) => new SByte4 ((sbyte)(a.x * b), (sbyte)(a.y * b), (sbyte)(a.z * b), (sbyte)(a.w * b));

        public static SByte4 operator / (SByte4 a, float b) => new SByte4 ((sbyte)(a.x / b), (sbyte)(a.y / b), (sbyte)(a.z / b), (sbyte)(a.w / b));

        public static bool operator > (SByte4 a, SByte4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (SByte4 a, SByte4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (SByte4 a, SByte4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (SByte4 a, SByte4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out sbyte x, out sbyte y, out sbyte z, out sbyte w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (SByte4 other) => this == other;

        public override bool Equals (object obj)
        {
            SByte4? v = obj as SByte4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (SByte4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static explicit operator SByte4 (Vector4 v) => new SByte4 ((sbyte)v.X, (sbyte)v.Y, (sbyte)v.Z, (sbyte)v.W);

		//Tuple
		public static implicit operator SByte4 ((sbyte x, sbyte y, sbyte z, sbyte w) v) => new SByte4 (v.x, v.y, v.z, v.w);

        public static implicit operator (sbyte, sbyte, sbyte, sbyte) (SByte4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static explicit operator SByte4 (Float4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//Double4
		public static explicit operator SByte4 (Double4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//Int4
		public static explicit operator SByte4 (Int4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//UInt4
		public static explicit operator SByte4 (UInt4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//Byte4
		public static explicit operator SByte4 (Byte4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//Short4
		public static explicit operator SByte4 (Short4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//UShort4
		public static explicit operator SByte4 (UShort4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//Long4
		public static explicit operator SByte4 (Long4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);

		//ULong4
		public static explicit operator SByte4 (ULong4 v) => new SByte4 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z, (sbyte)v.w);
	}

	[CLSCompliant (true)]
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct Short4 : IEquatable<Short4>
    {
        public const int Size = 4;

        private static readonly Short4 zero = new Short4 (0);
        private static readonly Short4 one = new Short4 (1);
        private static readonly Short4 unitX = new Short4 (1, 0, 0, 0);
        private static readonly Short4 unitY = new Short4 (0, 1, 0, 0);
        private static readonly Short4 unitZ = new Short4 (0, 0, 1, 0);
		private static readonly Short4 unitW = new Short4 (0, 0, 0, 1);

        public static ref readonly Short4 Zero => ref zero;
        public static ref readonly Short4 One => ref one;
        public static ref readonly Short4 UnitX => ref unitX;
        public static ref readonly Short4 UnitY => ref unitY;
        public static ref readonly Short4 UnitZ => ref unitZ;
		public static ref readonly Short4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed short components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public short x;
        [FieldOffset (2)]
        public short y;
        [FieldOffset (4)]
        public short z;
		[FieldOffset (6)]
		public short w;
		#pragma warning restore CA1051

        public Short2 XY => new Short2 (x, y);
        public Short2 XZ => new Short2 (x, z);
        public Short2 YZ => new Short2 (y, z);
		
		public Short3 XYZ => new Short3 (x, y, z);
		public Short3 XZY => new Short3 (x, z, y);
		public Short3 YXZ => new Short3 (y, x, z);
		public Short3 YZX => new Short3 (y, z, x);
		public Short3 ZXY => new Short3 (z, x, y);
		public Short3 ZYX => new Short3 (z, y, x);

		public short this[int index]
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

        public Short4 (short x, short y, short z, short w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public Short4 (Short2 leftPart, Short2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public Short4 (short all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public short GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, short value) => components[index] = value;

        public static Short4 Normalize (Short4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static Short4 Absolute (Short4 v) => new Short4 ((short)Math.Abs ((float)v.x), (short)Math.Abs ((float)v.y), (short)Math.Abs ((float)v.z), (short)Math.Abs ((float)v.w));

        public static float Distance (Short4 a, Short4 b) => (a - b).Length ();

        public static float Dot (Short4 a, Short4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static Short4 Reverse (Short4 v) => new Short4 (v.w, v.z, v.y, v.x);

		public int Sum () => x + y + z + w;

		public int Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (short value) => x == value || y == value || z == value || w == value;

        public static bool operator == (Short4 a, Short4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Short4 a, Short4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Short4 operator + (Short4 a, Short4 b) => new Short4 ((short)(a.x + b.x), (short)(a.y + b.y), (short)(a.z + b.z), (short)(a.w + b.w));

        public static Short4 operator - (Short4 a, Short4 b) => new Short4 ((short)(a.x - b.x), (short)(a.y - b.y), (short)(a.z - b.z), (short)(a.w - b.w));

		public static Short4 operator - (Short4 v) => new Short4 ((short)-v.x, (short)-v.y, (short)-v.z, (short)-v.w);
		
        public static Short4 operator * (Short4 a, float b) => new Short4 ((short)(a.x * b), (short)(a.y * b), (short)(a.z * b), (short)(a.w * b));

        public static Short4 operator / (Short4 a, float b) => new Short4 ((short)(a.x / b), (short)(a.y / b), (short)(a.z / b), (short)(a.w / b));

        public static bool operator > (Short4 a, Short4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Short4 a, Short4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Short4 a, Short4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Short4 a, Short4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out short x, out short y, out short z, out short w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (Short4 other) => this == other;

        public override bool Equals (object obj)
        {
            Short4? v = obj as Short4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (Short4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static explicit operator Short4 (Vector4 v) => new Short4 ((short)v.X, (short)v.Y, (short)v.Z, (short)v.W);

		//Tuple
		public static implicit operator Short4 ((short x, short y, short z, short w) v) => new Short4 (v.x, v.y, v.z, v.w);

        public static implicit operator (short, short, short, short) (Short4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static explicit operator Short4 (Float4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//Double4
		public static explicit operator Short4 (Double4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//Int4
		public static explicit operator Short4 (Int4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//UInt4
		public static explicit operator Short4 (UInt4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//Byte4
		public static implicit operator Short4 (Byte4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//SByte4
		public static implicit operator Short4 (SByte4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//UShort4
		public static explicit operator Short4 (UShort4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//Long4
		public static explicit operator Short4 (Long4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);

		//ULong4
		public static explicit operator Short4 (ULong4 v) => new Short4 ((short)v.x, (short)v.y, (short)v.z, (short)v.w);
	}

	[CLSCompliant (false)]
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct UShort4 : IEquatable<UShort4>
    {
        public const int Size = 4;

        private static readonly UShort4 zero = new UShort4 (0);
        private static readonly UShort4 one = new UShort4 (1);
        private static readonly UShort4 unitX = new UShort4 (1, 0, 0, 0);
        private static readonly UShort4 unitY = new UShort4 (0, 1, 0, 0);
        private static readonly UShort4 unitZ = new UShort4 (0, 0, 1, 0);
		private static readonly UShort4 unitW = new UShort4 (0, 0, 0, 1);

        public static ref readonly UShort4 Zero => ref zero;
        public static ref readonly UShort4 One => ref one;
        public static ref readonly UShort4 UnitX => ref unitX;
        public static ref readonly UShort4 UnitY => ref unitY;
        public static ref readonly UShort4 UnitZ => ref unitZ;
		public static ref readonly UShort4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed ushort components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public ushort x;
        [FieldOffset (2)]
        public ushort y;
        [FieldOffset (4)]
        public ushort z;
		[FieldOffset (6)]
		public ushort w;
		#pragma warning restore CA1051

        public UShort2 XY => new UShort2 (x, y);
        public UShort2 XZ => new UShort2 (x, z);
        public UShort2 YZ => new UShort2 (y, z);
		
		public UShort3 XYZ => new UShort3 (x, y, z);
		public UShort3 XZY => new UShort3 (x, z, y);
		public UShort3 YXZ => new UShort3 (y, x, z);
		public UShort3 YZX => new UShort3 (y, z, x);
		public UShort3 ZXY => new UShort3 (z, x, y);
		public UShort3 ZYX => new UShort3 (z, y, x);

		public ushort this[int index]
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

        public UShort4 (ushort x, ushort y, ushort z, ushort w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public UShort4 (UShort2 leftPart, UShort2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public UShort4 (ushort all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public ushort GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, ushort value) => components[index] = value;

        public static UShort4 Normalize (UShort4 v)
        {
            float length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (float)Math.Sqrt (length);

            return v / length;
        }

        public static UShort4 Absolute (UShort4 v) => new UShort4 ((ushort)Math.Abs ((float)v.x), (ushort)Math.Abs ((float)v.y), (ushort)Math.Abs ((float)v.z), (ushort)Math.Abs ((float)v.w));

        public static float Distance (UShort4 a, UShort4 b) => (a - b).Length ();

        public static float Dot (UShort4 a, UShort4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static UShort4 Reverse (UShort4 v) => new UShort4 (v.w, v.z, v.y, v.x);

		public int Sum () => x + y + z + w;

		public int Volume () => x * y * z * w;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z) + ((float)w * w);

		public bool Contains (ushort value) => x == value || y == value || z == value || w == value;

        public static bool operator == (UShort4 a, UShort4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (UShort4 a, UShort4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static UShort4 operator + (UShort4 a, UShort4 b) => new UShort4 ((ushort)(a.x + b.x), (ushort)(a.y + b.y), (ushort)(a.z + b.z), (ushort)(a.w + b.w));

        public static UShort4 operator - (UShort4 a, UShort4 b) => new UShort4 ((ushort)(a.x - b.x), (ushort)(a.y - b.y), (ushort)(a.z - b.z), (ushort)(a.w - b.w));

		
        public static UShort4 operator * (UShort4 a, float b) => new UShort4 ((ushort)(a.x * b), (ushort)(a.y * b), (ushort)(a.z * b), (ushort)(a.w * b));

        public static UShort4 operator / (UShort4 a, float b) => new UShort4 ((ushort)(a.x / b), (ushort)(a.y / b), (ushort)(a.z / b), (ushort)(a.w / b));

        public static bool operator > (UShort4 a, UShort4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (UShort4 a, UShort4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (UShort4 a, UShort4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (UShort4 a, UShort4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out ushort x, out ushort y, out ushort z, out ushort w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (UShort4 other) => this == other;

        public override bool Equals (object obj)
        {
            UShort4? v = obj as UShort4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (UShort4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static explicit operator UShort4 (Vector4 v) => new UShort4 ((ushort)v.X, (ushort)v.Y, (ushort)v.Z, (ushort)v.W);

		//Tuple
		public static implicit operator UShort4 ((ushort x, ushort y, ushort z, ushort w) v) => new UShort4 (v.x, v.y, v.z, v.w);

        public static implicit operator (ushort, ushort, ushort, ushort) (UShort4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static explicit operator UShort4 (Float4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//Double4
		public static explicit operator UShort4 (Double4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//Int4
		public static explicit operator UShort4 (Int4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//UInt4
		public static explicit operator UShort4 (UInt4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//Byte4
		public static implicit operator UShort4 (Byte4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//SByte4
		public static implicit operator UShort4 (SByte4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//Short4
		public static explicit operator UShort4 (Short4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//Long4
		public static explicit operator UShort4 (Long4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);

		//ULong4
		public static explicit operator UShort4 (ULong4 v) => new UShort4 ((ushort)v.x, (ushort)v.y, (ushort)v.z, (ushort)v.w);
	}

	[CLSCompliant (true)]
    [StructLayout (LayoutKind.Explicit, Size = 32)]
    public unsafe struct Long4 : IEquatable<Long4>
    {
        public const int Size = 4;

        private static readonly Long4 zero = new Long4 (0);
        private static readonly Long4 one = new Long4 (1);
        private static readonly Long4 unitX = new Long4 (1, 0, 0, 0);
        private static readonly Long4 unitY = new Long4 (0, 1, 0, 0);
        private static readonly Long4 unitZ = new Long4 (0, 0, 1, 0);
		private static readonly Long4 unitW = new Long4 (0, 0, 0, 1);

        public static ref readonly Long4 Zero => ref zero;
        public static ref readonly Long4 One => ref one;
        public static ref readonly Long4 UnitX => ref unitX;
        public static ref readonly Long4 UnitY => ref unitY;
        public static ref readonly Long4 UnitZ => ref unitZ;
		public static ref readonly Long4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed long components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public long x;
        [FieldOffset (8)]
        public long y;
        [FieldOffset (16)]
        public long z;
		[FieldOffset (24)]
		public long w;
		#pragma warning restore CA1051

        public Long2 XY => new Long2 (x, y);
        public Long2 XZ => new Long2 (x, z);
        public Long2 YZ => new Long2 (y, z);
		
		public Long3 XYZ => new Long3 (x, y, z);
		public Long3 XZY => new Long3 (x, z, y);
		public Long3 YXZ => new Long3 (y, x, z);
		public Long3 YZX => new Long3 (y, z, x);
		public Long3 ZXY => new Long3 (z, x, y);
		public Long3 ZYX => new Long3 (z, y, x);

		public long this[int index]
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

        public Long4 (long x, long y, long z, long w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public Long4 (Long2 leftPart, Long2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public Long4 (long all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public long GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, long value) => components[index] = value;

        public static Long4 Normalize (Long4 v)
        {
            double length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (double)Math.Sqrt (length);

            return v / length;
        }

        public static Long4 Absolute (Long4 v) => new Long4 ((long)Math.Abs ((double)v.x), (long)Math.Abs ((double)v.y), (long)Math.Abs ((double)v.z), (long)Math.Abs ((double)v.w));

        public static double Distance (Long4 a, Long4 b) => (a - b).Length ();

        public static double Dot (Long4 a, Long4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static Long4 Reverse (Long4 v) => new Long4 (v.w, v.z, v.y, v.x);

		public long Sum () => x + y + z + w;

		public long Volume () => x * y * z * w;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y) + ((double)z * z) + ((double)w * w);

		public bool Contains (long value) => x == value || y == value || z == value || w == value;

        public static bool operator == (Long4 a, Long4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (Long4 a, Long4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static Long4 operator + (Long4 a, Long4 b) => new Long4 ((long)(a.x + b.x), (long)(a.y + b.y), (long)(a.z + b.z), (long)(a.w + b.w));

        public static Long4 operator - (Long4 a, Long4 b) => new Long4 ((long)(a.x - b.x), (long)(a.y - b.y), (long)(a.z - b.z), (long)(a.w - b.w));

		public static Long4 operator - (Long4 v) => new Long4 ((long)-v.x, (long)-v.y, (long)-v.z, (long)-v.w);
		
        public static Long4 operator * (Long4 a, double b) => new Long4 ((long)(a.x * b), (long)(a.y * b), (long)(a.z * b), (long)(a.w * b));

        public static Long4 operator / (Long4 a, double b) => new Long4 ((long)(a.x / b), (long)(a.y / b), (long)(a.z / b), (long)(a.w / b));

        public static bool operator > (Long4 a, Long4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Long4 a, Long4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Long4 a, Long4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Long4 a, Long4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out long x, out long y, out long z, out long w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (Long4 other) => this == other;

        public override bool Equals (object obj)
        {
            Long4? v = obj as Long4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (Long4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static implicit operator Long4 (Vector4 v) => new Long4 ((long)v.X, (long)v.Y, (long)v.Z, (long)v.W);

		//Tuple
		public static implicit operator Long4 ((long x, long y, long z, long w) v) => new Long4 (v.x, v.y, v.z, v.w);

        public static implicit operator (long, long, long, long) (Long4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static implicit operator Long4 (Float4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//Double4
		public static explicit operator Long4 (Double4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//Int4
		public static implicit operator Long4 (Int4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//UInt4
		public static implicit operator Long4 (UInt4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//Byte4
		public static implicit operator Long4 (Byte4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//SByte4
		public static implicit operator Long4 (SByte4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//Short4
		public static implicit operator Long4 (Short4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//UShort4
		public static implicit operator Long4 (UShort4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);

		//ULong4
		public static explicit operator Long4 (ULong4 v) => new Long4 ((long)v.x, (long)v.y, (long)v.z, (long)v.w);
	}

	[CLSCompliant (false)]
    [StructLayout (LayoutKind.Explicit, Size = 32)]
    public unsafe struct ULong4 : IEquatable<ULong4>
    {
        public const int Size = 4;

        private static readonly ULong4 zero = new ULong4 (0);
        private static readonly ULong4 one = new ULong4 (1);
        private static readonly ULong4 unitX = new ULong4 (1, 0, 0, 0);
        private static readonly ULong4 unitY = new ULong4 (0, 1, 0, 0);
        private static readonly ULong4 unitZ = new ULong4 (0, 0, 1, 0);
		private static readonly ULong4 unitW = new ULong4 (0, 0, 0, 1);

        public static ref readonly ULong4 Zero => ref zero;
        public static ref readonly ULong4 One => ref one;
        public static ref readonly ULong4 UnitX => ref unitX;
        public static ref readonly ULong4 UnitY => ref unitY;
        public static ref readonly ULong4 UnitZ => ref unitZ;
		public static ref readonly ULong4 UnitW => ref unitW;

        [FieldOffset (0)]
        private fixed ulong components[Size];

		#pragma warning disable CA1051
        [FieldOffset (0)]
        public ulong x;
        [FieldOffset (8)]
        public ulong y;
        [FieldOffset (16)]
        public ulong z;
		[FieldOffset (24)]
		public ulong w;
		#pragma warning restore CA1051

        public ULong2 XY => new ULong2 (x, y);
        public ULong2 XZ => new ULong2 (x, z);
        public ULong2 YZ => new ULong2 (y, z);
		
		public ULong3 XYZ => new ULong3 (x, y, z);
		public ULong3 XZY => new ULong3 (x, z, y);
		public ULong3 YXZ => new ULong3 (y, x, z);
		public ULong3 YZX => new ULong3 (y, z, x);
		public ULong3 ZXY => new ULong3 (z, x, y);
		public ULong3 ZYX => new ULong3 (z, y, x);

		public ulong this[int index]
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

        public ULong4 (ulong x, ulong y, ulong z, ulong w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
			this.w = w;
        }

		public ULong4 (ULong2 leftPart, ULong2 rightPart)
		{
			this.x = leftPart.x;
			this.y = leftPart.y;
			this.z = rightPart.x;
			this.w = rightPart.y;
		}

        public ULong4 (ulong all)
        {
            x = all;
            y = all;
            z = all;
			w = all;
        }  

        public ulong GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, ulong value) => components[index] = value;

        public static ULong4 Normalize (ULong4 v)
        {
            double length = v.LengthSquared ();

			if (length == 0)
				return Zero;

			length = (double)Math.Sqrt (length);

            return v / length;
        }

        public static ULong4 Absolute (ULong4 v) => new ULong4 ((ulong)Math.Abs ((double)v.x), (ulong)Math.Abs ((double)v.y), (ulong)Math.Abs ((double)v.z), (ulong)Math.Abs ((double)v.w));

        public static double Distance (ULong4 a, ULong4 b) => (a - b).Length ();

        public static double Dot (ULong4 a, ULong4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

		public static ULong4 Reverse (ULong4 v) => new ULong4 (v.w, v.z, v.y, v.x);

		public ulong Sum () => x + y + z + w;

		public ulong Volume () => x * y * z * w;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y) + ((double)z * z) + ((double)w * w);

		public bool Contains (ulong value) => x == value || y == value || z == value || w == value;

        public static bool operator == (ULong4 a, ULong4 b) => a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;

        public static bool operator != (ULong4 a, ULong4 b) => a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;

        public unsafe static ULong4 operator + (ULong4 a, ULong4 b) => new ULong4 ((ulong)(a.x + b.x), (ulong)(a.y + b.y), (ulong)(a.z + b.z), (ulong)(a.w + b.w));

        public static ULong4 operator - (ULong4 a, ULong4 b) => new ULong4 ((ulong)(a.x - b.x), (ulong)(a.y - b.y), (ulong)(a.z - b.z), (ulong)(a.w - b.w));

		
        public static ULong4 operator * (ULong4 a, double b) => new ULong4 ((ulong)(a.x * b), (ulong)(a.y * b), (ulong)(a.z * b), (ulong)(a.w * b));

        public static ULong4 operator / (ULong4 a, double b) => new ULong4 ((ulong)(a.x / b), (ulong)(a.y / b), (ulong)(a.z / b), (ulong)(a.w / b));

        public static bool operator > (ULong4 a, ULong4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (ULong4 a, ULong4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (ULong4 a, ULong4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (ULong4 a, ULong4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

		public void Deconstruct (out ulong x, out ulong y, out ulong z, out ulong w)
		{
			x = this.x;
			y = this.y;
			z = this.z;
			w = this.w;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ", " + w + ")";

        public bool Equals (ULong4 other) => this == other;

        public override bool Equals (object obj)
        {
            ULong4? v = obj as ULong4?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791) ^ (int)(w * 39916801);

		//Conversion to other vectors
		//Vector4
		public static implicit operator Vector4 (ULong4 v) => new Vector4 ((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        public static explicit operator ULong4 (Vector4 v) => new ULong4 ((ulong)v.X, (ulong)v.Y, (ulong)v.Z, (ulong)v.W);

		//Tuple
		public static implicit operator ULong4 ((ulong x, ulong y, ulong z, ulong w) v) => new ULong4 (v.x, v.y, v.z, v.w);

        public static implicit operator (ulong, ulong, ulong, ulong) (ULong4 v) => (v.x, v.y, v.z, v.w);

		
		//Float4
		public static implicit operator ULong4 (Float4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//Double4
		public static explicit operator ULong4 (Double4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//Int4
		public static implicit operator ULong4 (Int4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//UInt4
		public static implicit operator ULong4 (UInt4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//Byte4
		public static implicit operator ULong4 (Byte4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//SByte4
		public static implicit operator ULong4 (SByte4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//Short4
		public static implicit operator ULong4 (Short4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//UShort4
		public static implicit operator ULong4 (UShort4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);

		//Long4
		public static explicit operator ULong4 (Long4 v) => new ULong4 ((ulong)v.x, (ulong)v.y, (ulong)v.z, (ulong)v.w);
	}
}