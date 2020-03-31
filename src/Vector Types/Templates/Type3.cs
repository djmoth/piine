
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
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
        private fixed float components[Size];

        [FieldOffset (0)]
        public float x;
        [FieldOffset (4)]
        public float y;
        [FieldOffset (8)]
        public float z;

        public Float2 XY => new Float2 (x, y);
        public Float2 XZ => new Float2 (x, z);
        public Float2 YZ => new Float2 (y, z);

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

        public static Float3 Normalize (Float3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (float)(v.x / oldMagnitude);
            v.y = (float)(v.y / oldMagnitude);
            v.z = (float)(v.z / oldMagnitude);

            return v;
        }
        public static Float3 Absolute (Float3 v) => new Float3 ((float)Math.Abs ((float)v.x), (float)Math.Abs ((float)v.y), (float)Math.Abs ((float)v.z));

        public static float Distance (Float3 a, Float3 b) => (a - b).Length ();

        public static Float3 Cross (Float3 a, Float3 b)
        {
            Float3 r = new Float3 ();
            r.x = (float)(a.y * b.z - a.z * b.y);
            r.y = (float)(a.x * b.z - a.z * b.x);
            r.z = (float)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (Float3 a, Float3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public float Sum () => x + y + z;

		public float Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (float value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (Float3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator Float3 (Vector3 v) => new Float3 ((float)v.X, (float)v.Y, (float)v.Z);

        public static implicit operator Float3 ((float x, float y, float z) v) => new Float3 (v.x, v.y, v.z);

        public static implicit operator (float, float, float) (Float3 v) => (v.x, v.y, v.z);

        public static bool operator == (Float3 a, Float3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Float3 a, Float3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Float3 operator + (Float3 a, Float3 b) => new Float3 ((float)(a.x + b.x), (float)(a.y + b.y), (float)(a.z + b.z));

        public static Float3 operator - (Float3 a, Float3 b) => new Float3 ((float)(a.x - b.x), (float)(a.y - b.y), (float)(a.z - b.z));

		public static Float3 operator - (Float3 v) => new Float3 ((float)-v.x, (float)-v.y, (float)-v.z);
		
        public static Float3 operator * (Float3 a, float b) => new Float3 ((float)(a.x * b), (float)(a.y * b), (float)(a.z * b));

        public static Float3 operator / (Float3 a, float b) => new Float3 ((float)(a.x / b), (float)(a.y / b), (float)(a.z / b));

        public static bool operator > (Float3 a, Float3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Float3 a, Float3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Float3 a, Float3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Float3 a, Float3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out float x, out float y, out float z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

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

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Double3
		public static explicit operator Float3 (Double3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//Int3
		public static explicit operator Float3 (Int3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//UInt3
		public static explicit operator Float3 (UInt3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//Byte3
		public static implicit operator Float3 (Byte3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//SByte3
		public static implicit operator Float3 (SByte3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//Short3
		public static implicit operator Float3 (Short3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//UShort3
		public static implicit operator Float3 (UShort3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//Long3
		public static explicit operator Float3 (Long3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);


		//ULong3
		public static explicit operator Float3 (ULong3 v) => new Float3 ((float)v.x, (float)v.y, (float)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 24)]
    public unsafe struct Double3 : IEquatable<Double3>
    {
        public const int Size = 3;

        private static readonly Double3 zero = new Double3 (0);
        private static readonly Double3 one = new Double3 (1);
        private static readonly Double3 unitX = new Double3 (1, 0, 0);
        private static readonly Double3 unitY = new Double3 (0, 1, 0);
        private static readonly Double3 unitZ = new Double3 (0, 0, 1);

        public static ref readonly Double3 Zero => ref zero;
        public static ref readonly Double3 One => ref one;
        public static ref readonly Double3 UnitX => ref unitX;
        public static ref readonly Double3 UnitY => ref unitY;
        public static ref readonly Double3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed double components[Size];

        [FieldOffset (0)]
        public double x;
        [FieldOffset (8)]
        public double y;
        [FieldOffset (16)]
        public double z;

        public Double2 XY => new Double2 (x, y);
        public Double2 XZ => new Double2 (x, z);
        public Double2 YZ => new Double2 (y, z);

        public Double3 (double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Double3 (double all)
        {
            x = all;
            y = all;
            z = all;
        }

        public double this[int index]
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

        public double GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, double value) => components[index] = value;

        public static Double3 Normalize (Double3 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length ();
            v.x = (double)(v.x / oldMagnitude);
            v.y = (double)(v.y / oldMagnitude);
            v.z = (double)(v.z / oldMagnitude);

            return v;
        }
        public static Double3 Absolute (Double3 v) => new Double3 ((double)Math.Abs ((double)v.x), (double)Math.Abs ((double)v.y), (double)Math.Abs ((double)v.z));

        public static double Distance (Double3 a, Double3 b) => (a - b).Length ();

        public static Double3 Cross (Double3 a, Double3 b)
        {
            Double3 r = new Double3 ();
            r.x = (double)(a.y * b.z - a.z * b.y);
            r.y = (double)(a.x * b.z - a.z * b.x);
            r.z = (double)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static double Dot (Double3 a, Double3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public double Sum () => x + y + z;

		public double Volume () => x * y * z;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y) + ((double)z * z);

		public bool Contains (double value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (Double3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator Double3 (Vector3 v) => new Double3 ((double)v.X, (double)v.Y, (double)v.Z);

        public static implicit operator Double3 ((double x, double y, double z) v) => new Double3 (v.x, v.y, v.z);

        public static implicit operator (double, double, double) (Double3 v) => (v.x, v.y, v.z);

        public static bool operator == (Double3 a, Double3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Double3 a, Double3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Double3 operator + (Double3 a, Double3 b) => new Double3 ((double)(a.x + b.x), (double)(a.y + b.y), (double)(a.z + b.z));

        public static Double3 operator - (Double3 a, Double3 b) => new Double3 ((double)(a.x - b.x), (double)(a.y - b.y), (double)(a.z - b.z));

		public static Double3 operator - (Double3 v) => new Double3 ((double)-v.x, (double)-v.y, (double)-v.z);
		
        public static Double3 operator * (Double3 a, double b) => new Double3 ((double)(a.x * b), (double)(a.y * b), (double)(a.z * b));

        public static Double3 operator / (Double3 a, double b) => new Double3 ((double)(a.x / b), (double)(a.y / b), (double)(a.z / b));

        public static bool operator > (Double3 a, Double3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Double3 a, Double3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Double3 a, Double3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Double3 a, Double3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out double x, out double y, out double z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (Double3 other) => this == other;

        public override bool Equals (object obj)
        {
            Double3? v = obj as Double3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static implicit operator Double3 (Float3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//Int3
		public static implicit operator Double3 (Int3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//UInt3
		public static implicit operator Double3 (UInt3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//Byte3
		public static implicit operator Double3 (Byte3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//SByte3
		public static implicit operator Double3 (SByte3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//Short3
		public static implicit operator Double3 (Short3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//UShort3
		public static implicit operator Double3 (UShort3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//Long3
		public static explicit operator Double3 (Long3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);


		//ULong3
		public static explicit operator Double3 (ULong3 v) => new Double3 ((double)v.x, (double)v.y, (double)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 12)]
    public unsafe struct Int3 : IEquatable<Int3>
    {
        public const int Size = 3;

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

        [FieldOffset (0)]
        private fixed int components[Size];

        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;
        [FieldOffset (8)]
        public int z;

        public Int2 XY => new Int2 (x, y);
        public Int2 XZ => new Int2 (x, z);
        public Int2 YZ => new Int2 (y, z);

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

        public int GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, int value) => components[index] = value;

        public static Int3 Normalize (Int3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (int)(v.x / oldMagnitude);
            v.y = (int)(v.y / oldMagnitude);
            v.z = (int)(v.z / oldMagnitude);

            return v;
        }
        public static Int3 Absolute (Int3 v) => new Int3 ((int)Math.Abs ((float)v.x), (int)Math.Abs ((float)v.y), (int)Math.Abs ((float)v.z));

        public static float Distance (Int3 a, Int3 b) => (a - b).Length ();

        public static Int3 Cross (Int3 a, Int3 b)
        {
            Int3 r = new Int3 ();
            r.x = (int)(a.y * b.z - a.z * b.y);
            r.y = (int)(a.x * b.z - a.z * b.x);
            r.z = (int)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (Int3 a, Int3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public int Sum () => x + y + z;

		public int Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (int value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (Int3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator Int3 (Vector3 v) => new Int3 ((int)v.X, (int)v.Y, (int)v.Z);

        public static implicit operator Int3 ((int x, int y, int z) v) => new Int3 (v.x, v.y, v.z);

        public static implicit operator (int, int, int) (Int3 v) => (v.x, v.y, v.z);

        public static bool operator == (Int3 a, Int3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Int3 a, Int3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Int3 operator + (Int3 a, Int3 b) => new Int3 ((int)(a.x + b.x), (int)(a.y + b.y), (int)(a.z + b.z));

        public static Int3 operator - (Int3 a, Int3 b) => new Int3 ((int)(a.x - b.x), (int)(a.y - b.y), (int)(a.z - b.z));

		public static Int3 operator - (Int3 v) => new Int3 ((int)-v.x, (int)-v.y, (int)-v.z);
		
        public static Int3 operator * (Int3 a, float b) => new Int3 ((int)(a.x * b), (int)(a.y * b), (int)(a.z * b));

        public static Int3 operator / (Int3 a, float b) => new Int3 ((int)(a.x / b), (int)(a.y / b), (int)(a.z / b));

        public static bool operator > (Int3 a, Int3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Int3 a, Int3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Int3 a, Int3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Int3 a, Int3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out int x, out int y, out int z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (Int3 other) => this == other;

        public override bool Equals (object obj)
        {
            Int3? v = obj as Int3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static explicit operator Int3 (Float3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//Double3
		public static explicit operator Int3 (Double3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//UInt3
		public static explicit operator Int3 (UInt3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//Byte3
		public static implicit operator Int3 (Byte3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//SByte3
		public static implicit operator Int3 (SByte3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//Short3
		public static implicit operator Int3 (Short3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//UShort3
		public static implicit operator Int3 (UShort3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//Long3
		public static explicit operator Int3 (Long3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);


		//ULong3
		public static explicit operator Int3 (ULong3 v) => new Int3 ((int)v.x, (int)v.y, (int)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 12)]
    public unsafe struct UInt3 : IEquatable<UInt3>
    {
        public const int Size = 3;

        private static readonly UInt3 zero = new UInt3 (0);
        private static readonly UInt3 one = new UInt3 (1);
        private static readonly UInt3 unitX = new UInt3 (1, 0, 0);
        private static readonly UInt3 unitY = new UInt3 (0, 1, 0);
        private static readonly UInt3 unitZ = new UInt3 (0, 0, 1);

        public static ref readonly UInt3 Zero => ref zero;
        public static ref readonly UInt3 One => ref one;
        public static ref readonly UInt3 UnitX => ref unitX;
        public static ref readonly UInt3 UnitY => ref unitY;
        public static ref readonly UInt3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed uint components[Size];

        [FieldOffset (0)]
        public uint x;
        [FieldOffset (4)]
        public uint y;
        [FieldOffset (8)]
        public uint z;

        public UInt2 XY => new UInt2 (x, y);
        public UInt2 XZ => new UInt2 (x, z);
        public UInt2 YZ => new UInt2 (y, z);

        public UInt3 (uint x, uint y, uint z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public UInt3 (uint all)
        {
            x = all;
            y = all;
            z = all;
        }

        public uint this[int index]
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

        public uint GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, uint value) => components[index] = value;

        public static UInt3 Normalize (UInt3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (uint)(v.x / oldMagnitude);
            v.y = (uint)(v.y / oldMagnitude);
            v.z = (uint)(v.z / oldMagnitude);

            return v;
        }
        public static UInt3 Absolute (UInt3 v) => new UInt3 ((uint)Math.Abs ((float)v.x), (uint)Math.Abs ((float)v.y), (uint)Math.Abs ((float)v.z));

        public static float Distance (UInt3 a, UInt3 b) => (a - b).Length ();

        public static UInt3 Cross (UInt3 a, UInt3 b)
        {
            UInt3 r = new UInt3 ();
            r.x = (uint)(a.y * b.z - a.z * b.y);
            r.y = (uint)(a.x * b.z - a.z * b.x);
            r.z = (uint)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (UInt3 a, UInt3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public uint Sum () => x + y + z;

		public uint Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (uint value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (UInt3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator UInt3 (Vector3 v) => new UInt3 ((uint)v.X, (uint)v.Y, (uint)v.Z);

        public static implicit operator UInt3 ((uint x, uint y, uint z) v) => new UInt3 (v.x, v.y, v.z);

        public static implicit operator (uint, uint, uint) (UInt3 v) => (v.x, v.y, v.z);

        public static bool operator == (UInt3 a, UInt3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (UInt3 a, UInt3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static UInt3 operator + (UInt3 a, UInt3 b) => new UInt3 ((uint)(a.x + b.x), (uint)(a.y + b.y), (uint)(a.z + b.z));

        public static UInt3 operator - (UInt3 a, UInt3 b) => new UInt3 ((uint)(a.x - b.x), (uint)(a.y - b.y), (uint)(a.z - b.z));

		
        public static UInt3 operator * (UInt3 a, float b) => new UInt3 ((uint)(a.x * b), (uint)(a.y * b), (uint)(a.z * b));

        public static UInt3 operator / (UInt3 a, float b) => new UInt3 ((uint)(a.x / b), (uint)(a.y / b), (uint)(a.z / b));

        public static bool operator > (UInt3 a, UInt3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (UInt3 a, UInt3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (UInt3 a, UInt3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (UInt3 a, UInt3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out uint x, out uint y, out uint z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (UInt3 other) => this == other;

        public override bool Equals (object obj)
        {
            UInt3? v = obj as UInt3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static explicit operator UInt3 (Float3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//Double3
		public static explicit operator UInt3 (Double3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//Int3
		public static explicit operator UInt3 (Int3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//Byte3
		public static implicit operator UInt3 (Byte3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//SByte3
		public static implicit operator UInt3 (SByte3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//Short3
		public static implicit operator UInt3 (Short3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//UShort3
		public static implicit operator UInt3 (UShort3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//Long3
		public static explicit operator UInt3 (Long3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);


		//ULong3
		public static explicit operator UInt3 (ULong3 v) => new UInt3 ((uint)v.x, (uint)v.y, (uint)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 3)]
    public unsafe struct Byte3 : IEquatable<Byte3>
    {
        public const int Size = 3;

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
        private fixed byte components[Size];

        [FieldOffset (0)]
        public byte x;
        [FieldOffset (1)]
        public byte y;
        [FieldOffset (2)]
        public byte z;

        public Byte2 XY => new Byte2 (x, y);
        public Byte2 XZ => new Byte2 (x, z);
        public Byte2 YZ => new Byte2 (y, z);

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

        public byte GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, byte value) => components[index] = value;

        public static Byte3 Normalize (Byte3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (byte)(v.x / oldMagnitude);
            v.y = (byte)(v.y / oldMagnitude);
            v.z = (byte)(v.z / oldMagnitude);

            return v;
        }
        public static Byte3 Absolute (Byte3 v) => new Byte3 ((byte)Math.Abs ((float)v.x), (byte)Math.Abs ((float)v.y), (byte)Math.Abs ((float)v.z));

        public static float Distance (Byte3 a, Byte3 b) => (a - b).Length ();

        public static Byte3 Cross (Byte3 a, Byte3 b)
        {
            Byte3 r = new Byte3 ();
            r.x = (byte)(a.y * b.z - a.z * b.y);
            r.y = (byte)(a.x * b.z - a.z * b.x);
            r.z = (byte)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (Byte3 a, Byte3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public int Sum () => x + y + z;

		public int Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (byte value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (Byte3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator Byte3 (Vector3 v) => new Byte3 ((byte)v.X, (byte)v.Y, (byte)v.Z);

        public static implicit operator Byte3 ((byte x, byte y, byte z) v) => new Byte3 (v.x, v.y, v.z);

        public static implicit operator (byte, byte, byte) (Byte3 v) => (v.x, v.y, v.z);

        public static bool operator == (Byte3 a, Byte3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Byte3 a, Byte3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Byte3 operator + (Byte3 a, Byte3 b) => new Byte3 ((byte)(a.x + b.x), (byte)(a.y + b.y), (byte)(a.z + b.z));

        public static Byte3 operator - (Byte3 a, Byte3 b) => new Byte3 ((byte)(a.x - b.x), (byte)(a.y - b.y), (byte)(a.z - b.z));

		
        public static Byte3 operator * (Byte3 a, float b) => new Byte3 ((byte)(a.x * b), (byte)(a.y * b), (byte)(a.z * b));

        public static Byte3 operator / (Byte3 a, float b) => new Byte3 ((byte)(a.x / b), (byte)(a.y / b), (byte)(a.z / b));

        public static bool operator > (Byte3 a, Byte3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Byte3 a, Byte3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Byte3 a, Byte3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Byte3 a, Byte3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out byte x, out byte y, out byte z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (Byte3 other) => this == other;

        public override bool Equals (object obj)
        {
            Byte3? v = obj as Byte3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static explicit operator Byte3 (Float3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//Double3
		public static explicit operator Byte3 (Double3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//Int3
		public static explicit operator Byte3 (Int3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//UInt3
		public static explicit operator Byte3 (UInt3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//SByte3
		public static explicit operator Byte3 (SByte3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//Short3
		public static explicit operator Byte3 (Short3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//UShort3
		public static explicit operator Byte3 (UShort3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//Long3
		public static explicit operator Byte3 (Long3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);


		//ULong3
		public static explicit operator Byte3 (ULong3 v) => new Byte3 ((byte)v.x, (byte)v.y, (byte)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 3)]
    public unsafe struct SByte3 : IEquatable<SByte3>
    {
        public const int Size = 3;

        private static readonly SByte3 zero = new SByte3 (0);
        private static readonly SByte3 one = new SByte3 (1);
        private static readonly SByte3 unitX = new SByte3 (1, 0, 0);
        private static readonly SByte3 unitY = new SByte3 (0, 1, 0);
        private static readonly SByte3 unitZ = new SByte3 (0, 0, 1);

        public static ref readonly SByte3 Zero => ref zero;
        public static ref readonly SByte3 One => ref one;
        public static ref readonly SByte3 UnitX => ref unitX;
        public static ref readonly SByte3 UnitY => ref unitY;
        public static ref readonly SByte3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed sbyte components[Size];

        [FieldOffset (0)]
        public sbyte x;
        [FieldOffset (1)]
        public sbyte y;
        [FieldOffset (2)]
        public sbyte z;

        public SByte2 XY => new SByte2 (x, y);
        public SByte2 XZ => new SByte2 (x, z);
        public SByte2 YZ => new SByte2 (y, z);

        public SByte3 (sbyte x, sbyte y, sbyte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SByte3 (sbyte all)
        {
            x = all;
            y = all;
            z = all;
        }

        public sbyte this[int index]
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

        public sbyte GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, sbyte value) => components[index] = value;

        public static SByte3 Normalize (SByte3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (sbyte)(v.x / oldMagnitude);
            v.y = (sbyte)(v.y / oldMagnitude);
            v.z = (sbyte)(v.z / oldMagnitude);

            return v;
        }
        public static SByte3 Absolute (SByte3 v) => new SByte3 ((sbyte)Math.Abs ((float)v.x), (sbyte)Math.Abs ((float)v.y), (sbyte)Math.Abs ((float)v.z));

        public static float Distance (SByte3 a, SByte3 b) => (a - b).Length ();

        public static SByte3 Cross (SByte3 a, SByte3 b)
        {
            SByte3 r = new SByte3 ();
            r.x = (sbyte)(a.y * b.z - a.z * b.y);
            r.y = (sbyte)(a.x * b.z - a.z * b.x);
            r.z = (sbyte)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (SByte3 a, SByte3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public int Sum () => x + y + z;

		public int Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (sbyte value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (SByte3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator SByte3 (Vector3 v) => new SByte3 ((sbyte)v.X, (sbyte)v.Y, (sbyte)v.Z);

        public static implicit operator SByte3 ((sbyte x, sbyte y, sbyte z) v) => new SByte3 (v.x, v.y, v.z);

        public static implicit operator (sbyte, sbyte, sbyte) (SByte3 v) => (v.x, v.y, v.z);

        public static bool operator == (SByte3 a, SByte3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (SByte3 a, SByte3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static SByte3 operator + (SByte3 a, SByte3 b) => new SByte3 ((sbyte)(a.x + b.x), (sbyte)(a.y + b.y), (sbyte)(a.z + b.z));

        public static SByte3 operator - (SByte3 a, SByte3 b) => new SByte3 ((sbyte)(a.x - b.x), (sbyte)(a.y - b.y), (sbyte)(a.z - b.z));

		public static SByte3 operator - (SByte3 v) => new SByte3 ((sbyte)-v.x, (sbyte)-v.y, (sbyte)-v.z);
		
        public static SByte3 operator * (SByte3 a, float b) => new SByte3 ((sbyte)(a.x * b), (sbyte)(a.y * b), (sbyte)(a.z * b));

        public static SByte3 operator / (SByte3 a, float b) => new SByte3 ((sbyte)(a.x / b), (sbyte)(a.y / b), (sbyte)(a.z / b));

        public static bool operator > (SByte3 a, SByte3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (SByte3 a, SByte3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (SByte3 a, SByte3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (SByte3 a, SByte3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out sbyte x, out sbyte y, out sbyte z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (SByte3 other) => this == other;

        public override bool Equals (object obj)
        {
            SByte3? v = obj as SByte3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static explicit operator SByte3 (Float3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//Double3
		public static explicit operator SByte3 (Double3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//Int3
		public static explicit operator SByte3 (Int3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//UInt3
		public static explicit operator SByte3 (UInt3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//Byte3
		public static explicit operator SByte3 (Byte3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//Short3
		public static explicit operator SByte3 (Short3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//UShort3
		public static explicit operator SByte3 (UShort3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//Long3
		public static explicit operator SByte3 (Long3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);


		//ULong3
		public static explicit operator SByte3 (ULong3 v) => new SByte3 ((sbyte)v.x, (sbyte)v.y, (sbyte)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 6)]
    public unsafe struct Short3 : IEquatable<Short3>
    {
        public const int Size = 3;

        private static readonly Short3 zero = new Short3 (0);
        private static readonly Short3 one = new Short3 (1);
        private static readonly Short3 unitX = new Short3 (1, 0, 0);
        private static readonly Short3 unitY = new Short3 (0, 1, 0);
        private static readonly Short3 unitZ = new Short3 (0, 0, 1);

        public static ref readonly Short3 Zero => ref zero;
        public static ref readonly Short3 One => ref one;
        public static ref readonly Short3 UnitX => ref unitX;
        public static ref readonly Short3 UnitY => ref unitY;
        public static ref readonly Short3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed short components[Size];

        [FieldOffset (0)]
        public short x;
        [FieldOffset (2)]
        public short y;
        [FieldOffset (4)]
        public short z;

        public Short2 XY => new Short2 (x, y);
        public Short2 XZ => new Short2 (x, z);
        public Short2 YZ => new Short2 (y, z);

        public Short3 (short x, short y, short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Short3 (short all)
        {
            x = all;
            y = all;
            z = all;
        }

        public short this[int index]
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

        public short GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, short value) => components[index] = value;

        public static Short3 Normalize (Short3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (short)(v.x / oldMagnitude);
            v.y = (short)(v.y / oldMagnitude);
            v.z = (short)(v.z / oldMagnitude);

            return v;
        }
        public static Short3 Absolute (Short3 v) => new Short3 ((short)Math.Abs ((float)v.x), (short)Math.Abs ((float)v.y), (short)Math.Abs ((float)v.z));

        public static float Distance (Short3 a, Short3 b) => (a - b).Length ();

        public static Short3 Cross (Short3 a, Short3 b)
        {
            Short3 r = new Short3 ();
            r.x = (short)(a.y * b.z - a.z * b.y);
            r.y = (short)(a.x * b.z - a.z * b.x);
            r.z = (short)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (Short3 a, Short3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public int Sum () => x + y + z;

		public int Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (short value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (Short3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator Short3 (Vector3 v) => new Short3 ((short)v.X, (short)v.Y, (short)v.Z);

        public static implicit operator Short3 ((short x, short y, short z) v) => new Short3 (v.x, v.y, v.z);

        public static implicit operator (short, short, short) (Short3 v) => (v.x, v.y, v.z);

        public static bool operator == (Short3 a, Short3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Short3 a, Short3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Short3 operator + (Short3 a, Short3 b) => new Short3 ((short)(a.x + b.x), (short)(a.y + b.y), (short)(a.z + b.z));

        public static Short3 operator - (Short3 a, Short3 b) => new Short3 ((short)(a.x - b.x), (short)(a.y - b.y), (short)(a.z - b.z));

		public static Short3 operator - (Short3 v) => new Short3 ((short)-v.x, (short)-v.y, (short)-v.z);
		
        public static Short3 operator * (Short3 a, float b) => new Short3 ((short)(a.x * b), (short)(a.y * b), (short)(a.z * b));

        public static Short3 operator / (Short3 a, float b) => new Short3 ((short)(a.x / b), (short)(a.y / b), (short)(a.z / b));

        public static bool operator > (Short3 a, Short3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Short3 a, Short3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Short3 a, Short3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Short3 a, Short3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out short x, out short y, out short z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (Short3 other) => this == other;

        public override bool Equals (object obj)
        {
            Short3? v = obj as Short3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static explicit operator Short3 (Float3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//Double3
		public static explicit operator Short3 (Double3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//Int3
		public static explicit operator Short3 (Int3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//UInt3
		public static explicit operator Short3 (UInt3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//Byte3
		public static implicit operator Short3 (Byte3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//SByte3
		public static implicit operator Short3 (SByte3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//UShort3
		public static explicit operator Short3 (UShort3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//Long3
		public static explicit operator Short3 (Long3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);


		//ULong3
		public static explicit operator Short3 (ULong3 v) => new Short3 ((short)v.x, (short)v.y, (short)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 6)]
    public unsafe struct UShort3 : IEquatable<UShort3>
    {
        public const int Size = 3;

        private static readonly UShort3 zero = new UShort3 (0);
        private static readonly UShort3 one = new UShort3 (1);
        private static readonly UShort3 unitX = new UShort3 (1, 0, 0);
        private static readonly UShort3 unitY = new UShort3 (0, 1, 0);
        private static readonly UShort3 unitZ = new UShort3 (0, 0, 1);

        public static ref readonly UShort3 Zero => ref zero;
        public static ref readonly UShort3 One => ref one;
        public static ref readonly UShort3 UnitX => ref unitX;
        public static ref readonly UShort3 UnitY => ref unitY;
        public static ref readonly UShort3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed ushort components[Size];

        [FieldOffset (0)]
        public ushort x;
        [FieldOffset (2)]
        public ushort y;
        [FieldOffset (4)]
        public ushort z;

        public UShort2 XY => new UShort2 (x, y);
        public UShort2 XZ => new UShort2 (x, z);
        public UShort2 YZ => new UShort2 (y, z);

        public UShort3 (ushort x, ushort y, ushort z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public UShort3 (ushort all)
        {
            x = all;
            y = all;
            z = all;
        }

        public ushort this[int index]
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

        public ushort GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, ushort value) => components[index] = value;

        public static UShort3 Normalize (UShort3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (ushort)(v.x / oldMagnitude);
            v.y = (ushort)(v.y / oldMagnitude);
            v.z = (ushort)(v.z / oldMagnitude);

            return v;
        }
        public static UShort3 Absolute (UShort3 v) => new UShort3 ((ushort)Math.Abs ((float)v.x), (ushort)Math.Abs ((float)v.y), (ushort)Math.Abs ((float)v.z));

        public static float Distance (UShort3 a, UShort3 b) => (a - b).Length ();

        public static UShort3 Cross (UShort3 a, UShort3 b)
        {
            UShort3 r = new UShort3 ();
            r.x = (ushort)(a.y * b.z - a.z * b.y);
            r.y = (ushort)(a.x * b.z - a.z * b.x);
            r.z = (ushort)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static float Dot (UShort3 a, UShort3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public int Sum () => x + y + z;

		public int Volume () => x * y * z;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y) + ((float)z * z);

		public bool Contains (ushort value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (UShort3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator UShort3 (Vector3 v) => new UShort3 ((ushort)v.X, (ushort)v.Y, (ushort)v.Z);

        public static implicit operator UShort3 ((ushort x, ushort y, ushort z) v) => new UShort3 (v.x, v.y, v.z);

        public static implicit operator (ushort, ushort, ushort) (UShort3 v) => (v.x, v.y, v.z);

        public static bool operator == (UShort3 a, UShort3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (UShort3 a, UShort3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static UShort3 operator + (UShort3 a, UShort3 b) => new UShort3 ((ushort)(a.x + b.x), (ushort)(a.y + b.y), (ushort)(a.z + b.z));

        public static UShort3 operator - (UShort3 a, UShort3 b) => new UShort3 ((ushort)(a.x - b.x), (ushort)(a.y - b.y), (ushort)(a.z - b.z));

		
        public static UShort3 operator * (UShort3 a, float b) => new UShort3 ((ushort)(a.x * b), (ushort)(a.y * b), (ushort)(a.z * b));

        public static UShort3 operator / (UShort3 a, float b) => new UShort3 ((ushort)(a.x / b), (ushort)(a.y / b), (ushort)(a.z / b));

        public static bool operator > (UShort3 a, UShort3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (UShort3 a, UShort3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (UShort3 a, UShort3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (UShort3 a, UShort3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out ushort x, out ushort y, out ushort z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (UShort3 other) => this == other;

        public override bool Equals (object obj)
        {
            UShort3? v = obj as UShort3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static explicit operator UShort3 (Float3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//Double3
		public static explicit operator UShort3 (Double3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//Int3
		public static explicit operator UShort3 (Int3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//UInt3
		public static explicit operator UShort3 (UInt3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//Byte3
		public static implicit operator UShort3 (Byte3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//SByte3
		public static implicit operator UShort3 (SByte3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//Short3
		public static explicit operator UShort3 (Short3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//Long3
		public static explicit operator UShort3 (Long3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);


		//ULong3
		public static explicit operator UShort3 (ULong3 v) => new UShort3 ((ushort)v.x, (ushort)v.y, (ushort)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 24)]
    public unsafe struct Long3 : IEquatable<Long3>
    {
        public const int Size = 3;

        private static readonly Long3 zero = new Long3 (0);
        private static readonly Long3 one = new Long3 (1);
        private static readonly Long3 unitX = new Long3 (1, 0, 0);
        private static readonly Long3 unitY = new Long3 (0, 1, 0);
        private static readonly Long3 unitZ = new Long3 (0, 0, 1);

        public static ref readonly Long3 Zero => ref zero;
        public static ref readonly Long3 One => ref one;
        public static ref readonly Long3 UnitX => ref unitX;
        public static ref readonly Long3 UnitY => ref unitY;
        public static ref readonly Long3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed long components[Size];

        [FieldOffset (0)]
        public long x;
        [FieldOffset (8)]
        public long y;
        [FieldOffset (16)]
        public long z;

        public Long2 XY => new Long2 (x, y);
        public Long2 XZ => new Long2 (x, z);
        public Long2 YZ => new Long2 (y, z);

        public Long3 (long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Long3 (long all)
        {
            x = all;
            y = all;
            z = all;
        }

        public long this[int index]
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

        public long GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, long value) => components[index] = value;

        public static Long3 Normalize (Long3 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length ();
            v.x = (long)(v.x / oldMagnitude);
            v.y = (long)(v.y / oldMagnitude);
            v.z = (long)(v.z / oldMagnitude);

            return v;
        }
        public static Long3 Absolute (Long3 v) => new Long3 ((long)Math.Abs ((double)v.x), (long)Math.Abs ((double)v.y), (long)Math.Abs ((double)v.z));

        public static double Distance (Long3 a, Long3 b) => (a - b).Length ();

        public static Long3 Cross (Long3 a, Long3 b)
        {
            Long3 r = new Long3 ();
            r.x = (long)(a.y * b.z - a.z * b.y);
            r.y = (long)(a.x * b.z - a.z * b.x);
            r.z = (long)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static double Dot (Long3 a, Long3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public long Sum () => x + y + z;

		public long Volume () => x * y * z;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y) + ((double)z * z);

		public bool Contains (long value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (Long3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator Long3 (Vector3 v) => new Long3 ((long)v.X, (long)v.Y, (long)v.Z);

        public static implicit operator Long3 ((long x, long y, long z) v) => new Long3 (v.x, v.y, v.z);

        public static implicit operator (long, long, long) (Long3 v) => (v.x, v.y, v.z);

        public static bool operator == (Long3 a, Long3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (Long3 a, Long3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static Long3 operator + (Long3 a, Long3 b) => new Long3 ((long)(a.x + b.x), (long)(a.y + b.y), (long)(a.z + b.z));

        public static Long3 operator - (Long3 a, Long3 b) => new Long3 ((long)(a.x - b.x), (long)(a.y - b.y), (long)(a.z - b.z));

		public static Long3 operator - (Long3 v) => new Long3 ((long)-v.x, (long)-v.y, (long)-v.z);
		
        public static Long3 operator * (Long3 a, double b) => new Long3 ((long)(a.x * b), (long)(a.y * b), (long)(a.z * b));

        public static Long3 operator / (Long3 a, double b) => new Long3 ((long)(a.x / b), (long)(a.y / b), (long)(a.z / b));

        public static bool operator > (Long3 a, Long3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Long3 a, Long3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Long3 a, Long3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Long3 a, Long3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out long x, out long y, out long z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (Long3 other) => this == other;

        public override bool Equals (object obj)
        {
            Long3? v = obj as Long3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static implicit operator Long3 (Float3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//Double3
		public static explicit operator Long3 (Double3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//Int3
		public static implicit operator Long3 (Int3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//UInt3
		public static implicit operator Long3 (UInt3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//Byte3
		public static implicit operator Long3 (Byte3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//SByte3
		public static implicit operator Long3 (SByte3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//Short3
		public static implicit operator Long3 (Short3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//UShort3
		public static implicit operator Long3 (UShort3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);


		//ULong3
		public static explicit operator Long3 (ULong3 v) => new Long3 ((long)v.x, (long)v.y, (long)v.z);

	}
	
    [StructLayout (LayoutKind.Explicit, Size = 24)]
    public unsafe struct ULong3 : IEquatable<ULong3>
    {
        public const int Size = 3;

        private static readonly ULong3 zero = new ULong3 (0);
        private static readonly ULong3 one = new ULong3 (1);
        private static readonly ULong3 unitX = new ULong3 (1, 0, 0);
        private static readonly ULong3 unitY = new ULong3 (0, 1, 0);
        private static readonly ULong3 unitZ = new ULong3 (0, 0, 1);

        public static ref readonly ULong3 Zero => ref zero;
        public static ref readonly ULong3 One => ref one;
        public static ref readonly ULong3 UnitX => ref unitX;
        public static ref readonly ULong3 UnitY => ref unitY;
        public static ref readonly ULong3 UnitZ => ref unitZ;

        [FieldOffset (0)]
        private fixed ulong components[Size];

        [FieldOffset (0)]
        public ulong x;
        [FieldOffset (8)]
        public ulong y;
        [FieldOffset (16)]
        public ulong z;

        public ULong2 XY => new ULong2 (x, y);
        public ULong2 XZ => new ULong2 (x, z);
        public ULong2 YZ => new ULong2 (y, z);

        public ULong3 (ulong x, ulong y, ulong z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public ULong3 (ulong all)
        {
            x = all;
            y = all;
            z = all;
        }

        public ulong this[int index]
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

        public ulong GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, ulong value) => components[index] = value;

        public static ULong3 Normalize (ULong3 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length ();
            v.x = (ulong)(v.x / oldMagnitude);
            v.y = (ulong)(v.y / oldMagnitude);
            v.z = (ulong)(v.z / oldMagnitude);

            return v;
        }
        public static ULong3 Absolute (ULong3 v) => new ULong3 ((ulong)Math.Abs ((double)v.x), (ulong)Math.Abs ((double)v.y), (ulong)Math.Abs ((double)v.z));

        public static double Distance (ULong3 a, ULong3 b) => (a - b).Length ();

        public static ULong3 Cross (ULong3 a, ULong3 b)
        {
            ULong3 r = new ULong3 ();
            r.x = (ulong)(a.y * b.z - a.z * b.y);
            r.y = (ulong)(a.x * b.z - a.z * b.x);
            r.z = (ulong)(a.y * b.x - a.x * b.y);
            return r;
        }

        public static double Dot (ULong3 a, ULong3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

		public ulong Sum () => x + y + z;

		public ulong Volume () => x * y * z;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y) + ((double)z * z);

		public bool Contains (ulong value) => x == value || y == value || z == value;

        public static explicit operator Vector3 (ULong3 v) => new Vector3 ((float)v.x, (float)v.y, (float)v.z);

        public static explicit operator ULong3 (Vector3 v) => new ULong3 ((ulong)v.X, (ulong)v.Y, (ulong)v.Z);

        public static implicit operator ULong3 ((ulong x, ulong y, ulong z) v) => new ULong3 (v.x, v.y, v.z);

        public static implicit operator (ulong, ulong, ulong) (ULong3 v) => (v.x, v.y, v.z);

        public static bool operator == (ULong3 a, ULong3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        public static bool operator != (ULong3 a, ULong3 b) => a.x != b.x || a.y != b.y || a.z != b.z;

        public unsafe static ULong3 operator + (ULong3 a, ULong3 b) => new ULong3 ((ulong)(a.x + b.x), (ulong)(a.y + b.y), (ulong)(a.z + b.z));

        public static ULong3 operator - (ULong3 a, ULong3 b) => new ULong3 ((ulong)(a.x - b.x), (ulong)(a.y - b.y), (ulong)(a.z - b.z));

		
        public static ULong3 operator * (ULong3 a, double b) => new ULong3 ((ulong)(a.x * b), (ulong)(a.y * b), (ulong)(a.z * b));

        public static ULong3 operator / (ULong3 a, double b) => new ULong3 ((ulong)(a.x / b), (ulong)(a.y / b), (ulong)(a.z / b));

        public static bool operator > (ULong3 a, ULong3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (ULong3 a, ULong3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (ULong3 a, ULong3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (ULong3 a, ULong3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

		public void Deconstruct (out ulong x, out ulong y, out ulong z)
		{
			x = this.x;
			y = this.y;
			z = this.z;
		}

        public override string ToString () => "(" + x + ", " + y + ", " + z + ")";

        public bool Equals (ULong3 other) => this == other;

        public override bool Equals (object obj)
        {
            ULong3? v = obj as ULong3?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		public override int GetHashCode () => (int)(x * 73856093) ^ (int)(y * 19349663) ^ (int)(z * 83492791);

		//Conversion to other vectors
		
		//Float3
		public static implicit operator ULong3 (Float3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//Double3
		public static explicit operator ULong3 (Double3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//Int3
		public static implicit operator ULong3 (Int3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//UInt3
		public static implicit operator ULong3 (UInt3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//Byte3
		public static implicit operator ULong3 (Byte3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//SByte3
		public static implicit operator ULong3 (SByte3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//Short3
		public static implicit operator ULong3 (Short3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//UShort3
		public static implicit operator ULong3 (UShort3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);


		//Long3
		public static explicit operator ULong3 (Long3 v) => new ULong3 ((ulong)v.x, (ulong)v.y, (ulong)v.z);

	}
	}