
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
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
        private fixed float components[Size];

        [FieldOffset (0)]
        public float x;
        [FieldOffset (4)]
        public float y;  

		public float Area => x * y;

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

        public float GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, float value) => components[index] = value;

        public static Float2 Normalize (Float2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (float)(v.x / oldMagnitude);
            v.y = (float)(v.y / oldMagnitude);

            return v;
        }

        public static Float2 Absolute (Float2 v) => new Float2 ((float)Math.Abs ((float)v.x), (float)Math.Abs ((float)v.y));

        public static float Distance (Float2 a, Float2 b) => (a - b).Length ();

        public static float Dot (Float2 a, Float2 b) => a.x * b.x + a.y * b.y;

		public float Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (float value) => x == value || y == value;

        public static explicit operator Vector2 (Float2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Float2 (Vector2 v) => new Float2 ((float)v.X, (float)v.Y);

        public static implicit operator Float2 ((float x, float y) v) => new Float2 (v.x, v.y);

        public static implicit operator (float, float) (Float2 v) => (v.x, v.y);

        public static bool operator == (Float2 a, Float2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Float2 a, Float2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Float2 operator + (Float2 a, Float2 b) => new Float2 ((float)(a.x + b.x), (float)(a.y + b.y));

        public static Float2 operator - (Float2 a, Float2 b) => new Float2 ((float)(a.x - b.x), (float)(a.y - b.y));

		public static Float2 operator - (Float2 v) => new Float2 ((float)-v.x, (float)-v.y);
		
        public static Float2 operator * (Float2 a, float b) => new Float2 ((float)(a.x * b), (float)(a.y * b));

        public static Float2 operator / (Float2 a, float b) => new Float2 ((float)(a.x / b), (float)(a.y / b));

        public static bool operator > (Float2 a, Float2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Float2 a, Float2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Float2 a, Float2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Float2 a, Float2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out float x, out float y)
		{
			x = this.x;
			y = this.y;
		}

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

		//Conversion to other vectors
		
		//Double2
public static explicit operator Float2 (Double2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//Int2
public static explicit operator Float2 (Int2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//UInt2
public static explicit operator Float2 (UInt2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//Byte2
public static explicit operator Float2 (Byte2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//SByte2
public static explicit operator Float2 (SByte2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//Short2
public static explicit operator Float2 (Short2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//UShort2
public static explicit operator Float2 (UShort2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//Long2
public static explicit operator Float2 (Long2 v) => new Float2 ((float)v.x, (float)v.y);

		
		//ULong2
public static explicit operator Float2 (ULong2 v) => new Float2 ((float)v.x, (float)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 16)]
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

        [FieldOffset (0)]
        private fixed double components[Size];

        [FieldOffset (0)]
        public double x;
        [FieldOffset (8)]
        public double y;  

		public double Area => x * y;

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

        public static Double2 Normalize (Double2 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length ();
            v.x = (double)(v.x / oldMagnitude);
            v.y = (double)(v.y / oldMagnitude);

            return v;
        }

        public static Double2 Absolute (Double2 v) => new Double2 ((double)Math.Abs ((double)v.x), (double)Math.Abs ((double)v.y));

        public static double Distance (Double2 a, Double2 b) => (a - b).Length ();

        public static double Dot (Double2 a, Double2 b) => a.x * b.x + a.y * b.y;

		public double Sum () => x + y;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y);

		public bool Contains (double value) => x == value || y == value;

        public static explicit operator Vector2 (Double2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Double2 (Vector2 v) => new Double2 ((double)v.X, (double)v.Y);

        public static implicit operator Double2 ((double x, double y) v) => new Double2 (v.x, v.y);

        public static implicit operator (double, double) (Double2 v) => (v.x, v.y);

        public static bool operator == (Double2 a, Double2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Double2 a, Double2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Double2 operator + (Double2 a, Double2 b) => new Double2 ((double)(a.x + b.x), (double)(a.y + b.y));

        public static Double2 operator - (Double2 a, Double2 b) => new Double2 ((double)(a.x - b.x), (double)(a.y - b.y));

		public static Double2 operator - (Double2 v) => new Double2 ((double)-v.x, (double)-v.y);
		
        public static Double2 operator * (Double2 a, double b) => new Double2 ((double)(a.x * b), (double)(a.y * b));

        public static Double2 operator / (Double2 a, double b) => new Double2 ((double)(a.x / b), (double)(a.y / b));

        public static bool operator > (Double2 a, Double2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Double2 a, Double2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Double2 a, Double2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Double2 a, Double2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out double x, out double y)
		{
			x = this.x;
			y = this.y;
		}

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

		//Conversion to other vectors
		
		//Float2
public static explicit operator Double2 (Float2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//Int2
public static explicit operator Double2 (Int2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//UInt2
public static explicit operator Double2 (UInt2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//Byte2
public static explicit operator Double2 (Byte2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//SByte2
public static explicit operator Double2 (SByte2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//Short2
public static explicit operator Double2 (Short2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//UShort2
public static explicit operator Double2 (UShort2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//Long2
public static explicit operator Double2 (Long2 v) => new Double2 ((double)v.x, (double)v.y);

		
		//ULong2
public static explicit operator Double2 (ULong2 v) => new Double2 ((double)v.x, (double)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct Int2 : IEquatable<Int2>
    {
        public const int Size = 2;

        private static readonly Int2 zero = new Int2 (0);
        private static readonly Int2 one = new Int2 (1);
        private static readonly Int2 unitX = new Int2 (1, 0);
        private static readonly Int2 unitY = new Int2 (0, 1);

        public static ref readonly Int2 Zero => ref zero;
        public static ref readonly Int2 One => ref one;
        public static ref readonly Int2 UnitX => ref unitX;
        public static ref readonly Int2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed int components[Size];

        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;  

		public int Area => x * y;

        public Int2 (int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Int2 (int all)
        {
            x = all;
            y = all;
        }

        public int this[int index]
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

        public int GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, int value) => components[index] = value;

        public static Int2 Normalize (Int2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (int)(v.x / oldMagnitude);
            v.y = (int)(v.y / oldMagnitude);

            return v;
        }

        public static Int2 Absolute (Int2 v) => new Int2 ((int)Math.Abs ((float)v.x), (int)Math.Abs ((float)v.y));

        public static float Distance (Int2 a, Int2 b) => (a - b).Length ();

        public static float Dot (Int2 a, Int2 b) => a.x * b.x + a.y * b.y;

		public int Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (int value) => x == value || y == value;

        public static explicit operator Vector2 (Int2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Int2 (Vector2 v) => new Int2 ((int)v.X, (int)v.Y);

        public static implicit operator Int2 ((int x, int y) v) => new Int2 (v.x, v.y);

        public static implicit operator (int, int) (Int2 v) => (v.x, v.y);

        public static bool operator == (Int2 a, Int2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Int2 a, Int2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Int2 operator + (Int2 a, Int2 b) => new Int2 ((int)(a.x + b.x), (int)(a.y + b.y));

        public static Int2 operator - (Int2 a, Int2 b) => new Int2 ((int)(a.x - b.x), (int)(a.y - b.y));

		public static Int2 operator - (Int2 v) => new Int2 ((int)-v.x, (int)-v.y);
		
        public static Int2 operator * (Int2 a, float b) => new Int2 ((int)(a.x * b), (int)(a.y * b));

        public static Int2 operator / (Int2 a, float b) => new Int2 ((int)(a.x / b), (int)(a.y / b));

        public static bool operator > (Int2 a, Int2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Int2 a, Int2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Int2 a, Int2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Int2 a, Int2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out int x, out int y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Int2 other) => this == other;

        public override bool Equals (object obj)
        {
            Int2? v = obj as Int2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator Int2 (Float2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//Double2
public static explicit operator Int2 (Double2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//UInt2
public static explicit operator Int2 (UInt2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//Byte2
public static explicit operator Int2 (Byte2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//SByte2
public static explicit operator Int2 (SByte2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//Short2
public static explicit operator Int2 (Short2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//UShort2
public static explicit operator Int2 (UShort2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//Long2
public static explicit operator Int2 (Long2 v) => new Int2 ((int)v.x, (int)v.y);

		
		//ULong2
public static explicit operator Int2 (ULong2 v) => new Int2 ((int)v.x, (int)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 8)]
    public unsafe struct UInt2 : IEquatable<UInt2>
    {
        public const int Size = 2;

        private static readonly UInt2 zero = new UInt2 (0);
        private static readonly UInt2 one = new UInt2 (1);
        private static readonly UInt2 unitX = new UInt2 (1, 0);
        private static readonly UInt2 unitY = new UInt2 (0, 1);

        public static ref readonly UInt2 Zero => ref zero;
        public static ref readonly UInt2 One => ref one;
        public static ref readonly UInt2 UnitX => ref unitX;
        public static ref readonly UInt2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed uint components[Size];

        [FieldOffset (0)]
        public uint x;
        [FieldOffset (4)]
        public uint y;  

		public uint Area => x * y;

        public UInt2 (uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }

        public UInt2 (uint all)
        {
            x = all;
            y = all;
        }

        public uint this[int index]
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

        public uint GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, uint value) => components[index] = value;

        public static UInt2 Normalize (UInt2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (uint)(v.x / oldMagnitude);
            v.y = (uint)(v.y / oldMagnitude);

            return v;
        }

        public static UInt2 Absolute (UInt2 v) => new UInt2 ((uint)Math.Abs ((float)v.x), (uint)Math.Abs ((float)v.y));

        public static float Distance (UInt2 a, UInt2 b) => (a - b).Length ();

        public static float Dot (UInt2 a, UInt2 b) => a.x * b.x + a.y * b.y;

		public uint Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (uint value) => x == value || y == value;

        public static explicit operator Vector2 (UInt2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator UInt2 (Vector2 v) => new UInt2 ((uint)v.X, (uint)v.Y);

        public static implicit operator UInt2 ((uint x, uint y) v) => new UInt2 (v.x, v.y);

        public static implicit operator (uint, uint) (UInt2 v) => (v.x, v.y);

        public static bool operator == (UInt2 a, UInt2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (UInt2 a, UInt2 b) => a.x != b.x || a.y != b.y;

        public unsafe static UInt2 operator + (UInt2 a, UInt2 b) => new UInt2 ((uint)(a.x + b.x), (uint)(a.y + b.y));

        public static UInt2 operator - (UInt2 a, UInt2 b) => new UInt2 ((uint)(a.x - b.x), (uint)(a.y - b.y));

		
        public static UInt2 operator * (UInt2 a, float b) => new UInt2 ((uint)(a.x * b), (uint)(a.y * b));

        public static UInt2 operator / (UInt2 a, float b) => new UInt2 ((uint)(a.x / b), (uint)(a.y / b));

        public static bool operator > (UInt2 a, UInt2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (UInt2 a, UInt2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (UInt2 a, UInt2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (UInt2 a, UInt2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out uint x, out uint y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (UInt2 other) => this == other;

        public override bool Equals (object obj)
        {
            UInt2? v = obj as UInt2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator UInt2 (Float2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//Double2
public static explicit operator UInt2 (Double2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//Int2
public static explicit operator UInt2 (Int2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//Byte2
public static explicit operator UInt2 (Byte2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//SByte2
public static explicit operator UInt2 (SByte2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//Short2
public static explicit operator UInt2 (Short2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//UShort2
public static explicit operator UInt2 (UShort2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//Long2
public static explicit operator UInt2 (Long2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		
		//ULong2
public static explicit operator UInt2 (ULong2 v) => new UInt2 ((uint)v.x, (uint)v.y);

		    }
	
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
        private fixed byte components[Size];

        [FieldOffset (0)]
        public byte x;
        [FieldOffset (1)]
        public byte y;  

		public int Area => x * y;

        public Byte2 (byte x, byte y)
        {
            this.x = x;
            this.y = y;
        }

        public Byte2 (byte all)
        {
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

        public static Byte2 Normalize (Byte2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (byte)(v.x / oldMagnitude);
            v.y = (byte)(v.y / oldMagnitude);

            return v;
        }

        public static Byte2 Absolute (Byte2 v) => new Byte2 ((byte)Math.Abs ((float)v.x), (byte)Math.Abs ((float)v.y));

        public static float Distance (Byte2 a, Byte2 b) => (a - b).Length ();

        public static float Dot (Byte2 a, Byte2 b) => a.x * b.x + a.y * b.y;

		public int Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (byte value) => x == value || y == value;

        public static explicit operator Vector2 (Byte2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Byte2 (Vector2 v) => new Byte2 ((byte)v.X, (byte)v.Y);

        public static implicit operator Byte2 ((byte x, byte y) v) => new Byte2 (v.x, v.y);

        public static implicit operator (byte, byte) (Byte2 v) => (v.x, v.y);

        public static bool operator == (Byte2 a, Byte2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Byte2 a, Byte2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Byte2 operator + (Byte2 a, Byte2 b) => new Byte2 ((byte)(a.x + b.x), (byte)(a.y + b.y));

        public static Byte2 operator - (Byte2 a, Byte2 b) => new Byte2 ((byte)(a.x - b.x), (byte)(a.y - b.y));

		
        public static Byte2 operator * (Byte2 a, float b) => new Byte2 ((byte)(a.x * b), (byte)(a.y * b));

        public static Byte2 operator / (Byte2 a, float b) => new Byte2 ((byte)(a.x / b), (byte)(a.y / b));

        public static bool operator > (Byte2 a, Byte2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Byte2 a, Byte2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Byte2 a, Byte2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Byte2 a, Byte2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out byte x, out byte y)
		{
			x = this.x;
			y = this.y;
		}

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

		//Conversion to other vectors
		
		//Float2
public static explicit operator Byte2 (Float2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//Double2
public static explicit operator Byte2 (Double2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//Int2
public static explicit operator Byte2 (Int2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//UInt2
public static explicit operator Byte2 (UInt2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//SByte2
public static explicit operator Byte2 (SByte2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//Short2
public static explicit operator Byte2 (Short2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//UShort2
public static explicit operator Byte2 (UShort2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//Long2
public static explicit operator Byte2 (Long2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		
		//ULong2
public static explicit operator Byte2 (ULong2 v) => new Byte2 ((byte)v.x, (byte)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 2)]
    public unsafe struct SByte2 : IEquatable<SByte2>
    {
        public const int Size = 2;

        private static readonly SByte2 zero = new SByte2 (0);
        private static readonly SByte2 one = new SByte2 (1);
        private static readonly SByte2 unitX = new SByte2 (1, 0);
        private static readonly SByte2 unitY = new SByte2 (0, 1);

        public static ref readonly SByte2 Zero => ref zero;
        public static ref readonly SByte2 One => ref one;
        public static ref readonly SByte2 UnitX => ref unitX;
        public static ref readonly SByte2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed sbyte components[Size];

        [FieldOffset (0)]
        public sbyte x;
        [FieldOffset (1)]
        public sbyte y;  

		public int Area => x * y;

        public SByte2 (sbyte x, sbyte y)
        {
            this.x = x;
            this.y = y;
        }

        public SByte2 (sbyte all)
        {
            x = all;
            y = all;
        }

        public sbyte this[int index]
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

        public sbyte GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, sbyte value) => components[index] = value;

        public static SByte2 Normalize (SByte2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (sbyte)(v.x / oldMagnitude);
            v.y = (sbyte)(v.y / oldMagnitude);

            return v;
        }

        public static SByte2 Absolute (SByte2 v) => new SByte2 ((sbyte)Math.Abs ((float)v.x), (sbyte)Math.Abs ((float)v.y));

        public static float Distance (SByte2 a, SByte2 b) => (a - b).Length ();

        public static float Dot (SByte2 a, SByte2 b) => a.x * b.x + a.y * b.y;

		public int Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (sbyte value) => x == value || y == value;

        public static explicit operator Vector2 (SByte2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator SByte2 (Vector2 v) => new SByte2 ((sbyte)v.X, (sbyte)v.Y);

        public static implicit operator SByte2 ((sbyte x, sbyte y) v) => new SByte2 (v.x, v.y);

        public static implicit operator (sbyte, sbyte) (SByte2 v) => (v.x, v.y);

        public static bool operator == (SByte2 a, SByte2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (SByte2 a, SByte2 b) => a.x != b.x || a.y != b.y;

        public unsafe static SByte2 operator + (SByte2 a, SByte2 b) => new SByte2 ((sbyte)(a.x + b.x), (sbyte)(a.y + b.y));

        public static SByte2 operator - (SByte2 a, SByte2 b) => new SByte2 ((sbyte)(a.x - b.x), (sbyte)(a.y - b.y));

		public static SByte2 operator - (SByte2 v) => new SByte2 ((sbyte)-v.x, (sbyte)-v.y);
		
        public static SByte2 operator * (SByte2 a, float b) => new SByte2 ((sbyte)(a.x * b), (sbyte)(a.y * b));

        public static SByte2 operator / (SByte2 a, float b) => new SByte2 ((sbyte)(a.x / b), (sbyte)(a.y / b));

        public static bool operator > (SByte2 a, SByte2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (SByte2 a, SByte2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (SByte2 a, SByte2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (SByte2 a, SByte2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out sbyte x, out sbyte y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (SByte2 other) => this == other;

        public override bool Equals (object obj)
        {
            SByte2? v = obj as SByte2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator SByte2 (Float2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//Double2
public static explicit operator SByte2 (Double2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//Int2
public static explicit operator SByte2 (Int2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//UInt2
public static explicit operator SByte2 (UInt2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//Byte2
public static explicit operator SByte2 (Byte2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//Short2
public static explicit operator SByte2 (Short2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//UShort2
public static explicit operator SByte2 (UShort2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//Long2
public static explicit operator SByte2 (Long2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		
		//ULong2
public static explicit operator SByte2 (ULong2 v) => new SByte2 ((sbyte)v.x, (sbyte)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 4)]
    public unsafe struct Short2 : IEquatable<Short2>
    {
        public const int Size = 2;

        private static readonly Short2 zero = new Short2 (0);
        private static readonly Short2 one = new Short2 (1);
        private static readonly Short2 unitX = new Short2 (1, 0);
        private static readonly Short2 unitY = new Short2 (0, 1);

        public static ref readonly Short2 Zero => ref zero;
        public static ref readonly Short2 One => ref one;
        public static ref readonly Short2 UnitX => ref unitX;
        public static ref readonly Short2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed short components[Size];

        [FieldOffset (0)]
        public short x;
        [FieldOffset (2)]
        public short y;  

		public int Area => x * y;

        public Short2 (short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        public Short2 (short all)
        {
            x = all;
            y = all;
        }

        public short this[int index]
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

        public short GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, short value) => components[index] = value;

        public static Short2 Normalize (Short2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (short)(v.x / oldMagnitude);
            v.y = (short)(v.y / oldMagnitude);

            return v;
        }

        public static Short2 Absolute (Short2 v) => new Short2 ((short)Math.Abs ((float)v.x), (short)Math.Abs ((float)v.y));

        public static float Distance (Short2 a, Short2 b) => (a - b).Length ();

        public static float Dot (Short2 a, Short2 b) => a.x * b.x + a.y * b.y;

		public int Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (short value) => x == value || y == value;

        public static explicit operator Vector2 (Short2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Short2 (Vector2 v) => new Short2 ((short)v.X, (short)v.Y);

        public static implicit operator Short2 ((short x, short y) v) => new Short2 (v.x, v.y);

        public static implicit operator (short, short) (Short2 v) => (v.x, v.y);

        public static bool operator == (Short2 a, Short2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Short2 a, Short2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Short2 operator + (Short2 a, Short2 b) => new Short2 ((short)(a.x + b.x), (short)(a.y + b.y));

        public static Short2 operator - (Short2 a, Short2 b) => new Short2 ((short)(a.x - b.x), (short)(a.y - b.y));

		public static Short2 operator - (Short2 v) => new Short2 ((short)-v.x, (short)-v.y);
		
        public static Short2 operator * (Short2 a, float b) => new Short2 ((short)(a.x * b), (short)(a.y * b));

        public static Short2 operator / (Short2 a, float b) => new Short2 ((short)(a.x / b), (short)(a.y / b));

        public static bool operator > (Short2 a, Short2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Short2 a, Short2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Short2 a, Short2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Short2 a, Short2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out short x, out short y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Short2 other) => this == other;

        public override bool Equals (object obj)
        {
            Short2? v = obj as Short2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator Short2 (Float2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//Double2
public static explicit operator Short2 (Double2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//Int2
public static explicit operator Short2 (Int2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//UInt2
public static explicit operator Short2 (UInt2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//Byte2
public static explicit operator Short2 (Byte2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//SByte2
public static explicit operator Short2 (SByte2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//UShort2
public static explicit operator Short2 (UShort2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//Long2
public static explicit operator Short2 (Long2 v) => new Short2 ((short)v.x, (short)v.y);

		
		//ULong2
public static explicit operator Short2 (ULong2 v) => new Short2 ((short)v.x, (short)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 4)]
    public unsafe struct UShort2 : IEquatable<UShort2>
    {
        public const int Size = 2;

        private static readonly UShort2 zero = new UShort2 (0);
        private static readonly UShort2 one = new UShort2 (1);
        private static readonly UShort2 unitX = new UShort2 (1, 0);
        private static readonly UShort2 unitY = new UShort2 (0, 1);

        public static ref readonly UShort2 Zero => ref zero;
        public static ref readonly UShort2 One => ref one;
        public static ref readonly UShort2 UnitX => ref unitX;
        public static ref readonly UShort2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed ushort components[Size];

        [FieldOffset (0)]
        public ushort x;
        [FieldOffset (2)]
        public ushort y;  

		public int Area => x * y;

        public UShort2 (ushort x, ushort y)
        {
            this.x = x;
            this.y = y;
        }

        public UShort2 (ushort all)
        {
            x = all;
            y = all;
        }

        public ushort this[int index]
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

        public ushort GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, ushort value) => components[index] = value;

        public static UShort2 Normalize (UShort2 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length ();
            v.x = (ushort)(v.x / oldMagnitude);
            v.y = (ushort)(v.y / oldMagnitude);

            return v;
        }

        public static UShort2 Absolute (UShort2 v) => new UShort2 ((ushort)Math.Abs ((float)v.x), (ushort)Math.Abs ((float)v.y));

        public static float Distance (UShort2 a, UShort2 b) => (a - b).Length ();

        public static float Dot (UShort2 a, UShort2 b) => a.x * b.x + a.y * b.y;

		public int Sum () => x + y;

		public float Length () => (float)Math.Sqrt (LengthSquared ());

        public float LengthSquared () => ((float)x * x) + ((float)y * y);

		public bool Contains (ushort value) => x == value || y == value;

        public static explicit operator Vector2 (UShort2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator UShort2 (Vector2 v) => new UShort2 ((ushort)v.X, (ushort)v.Y);

        public static implicit operator UShort2 ((ushort x, ushort y) v) => new UShort2 (v.x, v.y);

        public static implicit operator (ushort, ushort) (UShort2 v) => (v.x, v.y);

        public static bool operator == (UShort2 a, UShort2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (UShort2 a, UShort2 b) => a.x != b.x || a.y != b.y;

        public unsafe static UShort2 operator + (UShort2 a, UShort2 b) => new UShort2 ((ushort)(a.x + b.x), (ushort)(a.y + b.y));

        public static UShort2 operator - (UShort2 a, UShort2 b) => new UShort2 ((ushort)(a.x - b.x), (ushort)(a.y - b.y));

		
        public static UShort2 operator * (UShort2 a, float b) => new UShort2 ((ushort)(a.x * b), (ushort)(a.y * b));

        public static UShort2 operator / (UShort2 a, float b) => new UShort2 ((ushort)(a.x / b), (ushort)(a.y / b));

        public static bool operator > (UShort2 a, UShort2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (UShort2 a, UShort2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (UShort2 a, UShort2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (UShort2 a, UShort2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out ushort x, out ushort y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (UShort2 other) => this == other;

        public override bool Equals (object obj)
        {
            UShort2? v = obj as UShort2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator UShort2 (Float2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//Double2
public static explicit operator UShort2 (Double2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//Int2
public static explicit operator UShort2 (Int2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//UInt2
public static explicit operator UShort2 (UInt2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//Byte2
public static explicit operator UShort2 (Byte2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//SByte2
public static explicit operator UShort2 (SByte2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//Short2
public static explicit operator UShort2 (Short2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//Long2
public static explicit operator UShort2 (Long2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		
		//ULong2
public static explicit operator UShort2 (ULong2 v) => new UShort2 ((ushort)v.x, (ushort)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 16)]
    public unsafe struct Long2 : IEquatable<Long2>
    {
        public const int Size = 2;

        private static readonly Long2 zero = new Long2 (0);
        private static readonly Long2 one = new Long2 (1);
        private static readonly Long2 unitX = new Long2 (1, 0);
        private static readonly Long2 unitY = new Long2 (0, 1);

        public static ref readonly Long2 Zero => ref zero;
        public static ref readonly Long2 One => ref one;
        public static ref readonly Long2 UnitX => ref unitX;
        public static ref readonly Long2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed long components[Size];

        [FieldOffset (0)]
        public long x;
        [FieldOffset (8)]
        public long y;  

		public long Area => x * y;

        public Long2 (long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public Long2 (long all)
        {
            x = all;
            y = all;
        }

        public long this[int index]
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

        public long GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, long value) => components[index] = value;

        public static Long2 Normalize (Long2 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length ();
            v.x = (long)(v.x / oldMagnitude);
            v.y = (long)(v.y / oldMagnitude);

            return v;
        }

        public static Long2 Absolute (Long2 v) => new Long2 ((long)Math.Abs ((double)v.x), (long)Math.Abs ((double)v.y));

        public static double Distance (Long2 a, Long2 b) => (a - b).Length ();

        public static double Dot (Long2 a, Long2 b) => a.x * b.x + a.y * b.y;

		public long Sum () => x + y;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y);

		public bool Contains (long value) => x == value || y == value;

        public static explicit operator Vector2 (Long2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator Long2 (Vector2 v) => new Long2 ((long)v.X, (long)v.Y);

        public static implicit operator Long2 ((long x, long y) v) => new Long2 (v.x, v.y);

        public static implicit operator (long, long) (Long2 v) => (v.x, v.y);

        public static bool operator == (Long2 a, Long2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (Long2 a, Long2 b) => a.x != b.x || a.y != b.y;

        public unsafe static Long2 operator + (Long2 a, Long2 b) => new Long2 ((long)(a.x + b.x), (long)(a.y + b.y));

        public static Long2 operator - (Long2 a, Long2 b) => new Long2 ((long)(a.x - b.x), (long)(a.y - b.y));

		public static Long2 operator - (Long2 v) => new Long2 ((long)-v.x, (long)-v.y);
		
        public static Long2 operator * (Long2 a, double b) => new Long2 ((long)(a.x * b), (long)(a.y * b));

        public static Long2 operator / (Long2 a, double b) => new Long2 ((long)(a.x / b), (long)(a.y / b));

        public static bool operator > (Long2 a, Long2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (Long2 a, Long2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (Long2 a, Long2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (Long2 a, Long2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out long x, out long y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (Long2 other) => this == other;

        public override bool Equals (object obj)
        {
            Long2? v = obj as Long2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator Long2 (Float2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//Double2
public static explicit operator Long2 (Double2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//Int2
public static explicit operator Long2 (Int2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//UInt2
public static explicit operator Long2 (UInt2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//Byte2
public static explicit operator Long2 (Byte2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//SByte2
public static explicit operator Long2 (SByte2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//Short2
public static explicit operator Long2 (Short2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//UShort2
public static explicit operator Long2 (UShort2 v) => new Long2 ((long)v.x, (long)v.y);

		
		//ULong2
public static explicit operator Long2 (ULong2 v) => new Long2 ((long)v.x, (long)v.y);

		    }
	
    [StructLayout (LayoutKind.Explicit, Size = 16)]
    public unsafe struct ULong2 : IEquatable<ULong2>
    {
        public const int Size = 2;

        private static readonly ULong2 zero = new ULong2 (0);
        private static readonly ULong2 one = new ULong2 (1);
        private static readonly ULong2 unitX = new ULong2 (1, 0);
        private static readonly ULong2 unitY = new ULong2 (0, 1);

        public static ref readonly ULong2 Zero => ref zero;
        public static ref readonly ULong2 One => ref one;
        public static ref readonly ULong2 UnitX => ref unitX;
        public static ref readonly ULong2 UnitY => ref unitY;

        [FieldOffset (0)]
        private fixed ulong components[Size];

        [FieldOffset (0)]
        public ulong x;
        [FieldOffset (8)]
        public ulong y;  

		public ulong Area => x * y;

        public ULong2 (ulong x, ulong y)
        {
            this.x = x;
            this.y = y;
        }

        public ULong2 (ulong all)
        {
            x = all;
            y = all;
        }

        public ulong this[int index]
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

        public ulong GetUnsafe (int index) => components[index];

        public void SetUnsafe (int index, ulong value) => components[index] = value;

        public static ULong2 Normalize (ULong2 v)
        {
            if (v == Zero)
                return Zero;

            double oldMagnitude = v.Length ();
            v.x = (ulong)(v.x / oldMagnitude);
            v.y = (ulong)(v.y / oldMagnitude);

            return v;
        }

        public static ULong2 Absolute (ULong2 v) => new ULong2 ((ulong)Math.Abs ((double)v.x), (ulong)Math.Abs ((double)v.y));

        public static double Distance (ULong2 a, ULong2 b) => (a - b).Length ();

        public static double Dot (ULong2 a, ULong2 b) => a.x * b.x + a.y * b.y;

		public ulong Sum () => x + y;

		public double Length () => (double)Math.Sqrt (LengthSquared ());

        public double LengthSquared () => ((double)x * x) + ((double)y * y);

		public bool Contains (ulong value) => x == value || y == value;

        public static explicit operator Vector2 (ULong2 v) => new Vector2 ((float)v.x, (float)v.y);

        public static explicit operator ULong2 (Vector2 v) => new ULong2 ((ulong)v.X, (ulong)v.Y);

        public static implicit operator ULong2 ((ulong x, ulong y) v) => new ULong2 (v.x, v.y);

        public static implicit operator (ulong, ulong) (ULong2 v) => (v.x, v.y);

        public static bool operator == (ULong2 a, ULong2 b) => a.x == b.x && a.y == b.y;

        public static bool operator != (ULong2 a, ULong2 b) => a.x != b.x || a.y != b.y;

        public unsafe static ULong2 operator + (ULong2 a, ULong2 b) => new ULong2 ((ulong)(a.x + b.x), (ulong)(a.y + b.y));

        public static ULong2 operator - (ULong2 a, ULong2 b) => new ULong2 ((ulong)(a.x - b.x), (ulong)(a.y - b.y));

		
        public static ULong2 operator * (ULong2 a, double b) => new ULong2 ((ulong)(a.x * b), (ulong)(a.y * b));

        public static ULong2 operator / (ULong2 a, double b) => new ULong2 ((ulong)(a.x / b), (ulong)(a.y / b));

        public static bool operator > (ULong2 a, ULong2 b) => a.x > b.x && a.y > b.y;

        public static bool operator < (ULong2 a, ULong2 b) => a.x < b.x && a.y < b.y;

        public static bool operator >= (ULong2 a, ULong2 b) => a.x >= b.x && a.y >= b.y;

        public static bool operator <= (ULong2 a, ULong2 b) => a.x <= b.x && a.y <= b.y;

		public void Deconstruct (out ulong x, out ulong y)
		{
			x = this.x;
			y = this.y;
		}

        public override string ToString () => "(" + x + ", " + y + ")";

        public bool Equals (ULong2 other) => this == other;

        public override bool Equals (object obj)
        {
            ULong2? v = obj as ULong2?;

            if (v != null)
                return v == this;
            else
                return false;
        }

		//Conversion to other vectors
		
		//Float2
public static explicit operator ULong2 (Float2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//Double2
public static explicit operator ULong2 (Double2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//Int2
public static explicit operator ULong2 (Int2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//UInt2
public static explicit operator ULong2 (UInt2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//Byte2
public static explicit operator ULong2 (Byte2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//SByte2
public static explicit operator ULong2 (SByte2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//Short2
public static explicit operator ULong2 (Short2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//UShort2
public static explicit operator ULong2 (UShort2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		
		//Long2
public static explicit operator ULong2 (Long2 v) => new ULong2 ((ulong)v.x, (ulong)v.y);

		    }
	}