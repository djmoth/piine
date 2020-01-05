using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Explicit, Size = 12)]
    public unsafe struct Int3 : IEquatable<Int3>
    {
        public const int Size = 3; //Number of dimensions

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

        [CLSCompliant (false)]
        [FieldOffset (0)]
        public fixed int data[3];

        [FieldOffset (0)]
        public int x;
        [FieldOffset (4)]
        public int y;
        [FieldOffset (8)]
        public int z;

        public float Length => (float)Math.Sqrt (LengthSquared);

        public float LengthSquared => (x * x) + (y * y) + (z * z);

        public Int3 (int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Int3 (float x, float y, float z)
        {
            this.x = (int)Math.Round (x);
            this.y = (int)Math.Round (y);
            this.z = (int)Math.Round (z);
        }

        public Int3 (int all)
        {
            x = all;
            y = all;
            z = all;
        }

        public Int3 (Vector<int> v, int startIndex = 0)
        {
            x = v[startIndex];
            y = v[startIndex + 1];
            z = v[startIndex + 2];
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-2, index was " + index);

                return data[0];
            }
            set
            {
                if (index < 0 || index >= Size)
                    throw new ArgumentOutOfRangeException ("Index must be in the range 0-2, index was " + index);

                data[0] = value;
            }
        }

        public static Int3 Normalize (Int3 v)
        {
            if (v == Zero)
                return Zero;

            float oldMagnitude = v.Length;
            v.x = (int)Math.Round (v.x / oldMagnitude);
            v.y = (int)Math.Round (v.y / oldMagnitude);
            v.z = (int)Math.Round (v.z / oldMagnitude);

            return v;
        }

        public static Vector3 NormalizeToVector3 (Int3 v) => Vector3.Normalize (v);

        public int GetLongestComponent ()
        {
            int xAbsolute = Math.Abs (x);
            int yAbsolute = Math.Abs (y);
            int zAbsolute = Math.Abs (z);

            return Math.Max (xAbsolute, Math.Max (yAbsolute, zAbsolute));
        }
        public int CalculateVolume () => x * y * z;

        public static Int3 Absolute (Int3 v)
        {
            return new Int3 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z));
        }

        public static float Distance (Int3 a, Int3 b)
        {
            return (a - b).Length;
        }

        public static Vector3 RoundVector3 (Vector3 v)
        {
            return (Int3)v;
        }   

        public static Int3 Cross (Int3 a, Int3 b)
        {
            Int3 r = new Int3 ();
            r.x = a.y * b.z - a.z * b.y;
            r.y = a.x * b.z - a.z * b.x;
            r.z = a.y * b.x - a.x * b.y;

            return r;
        }

        [CLSCompliant (false)]
        public static unsafe Int3 Sum (Int3* vectorPtr, int length)
        {
            Vector<int> result = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr);

            Vector<int> vector;
            for (int i = 1; i < length; i++) //Start at 1 because result already contains the first element
            {
                vector = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr + i);

                result += vector;
            }

            return new Int3 (result);

            /*int fullVectorsInSIMD = Vector<int>.Count / Size; //The number of IntVector3's that fit in one Vector<int>
            int simdLength = fullVectorsInSIMD * Size; //Number of ints that fit in fullVectorsInSIMD

            Vector<int> wideResult = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr);

            Vector<int> tempVector;
            for (int i = fullVectorsInSIMD; i < length; i += fullVectorsInSIMD) //Start at simdLength because result already contains the first elements
            {
                tempVector = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr + i);

                wideResult += tempVector;
            }

            //Sum the resulting fullVectorsInSIMD together
            IntVector3 v;
            for (int i = Size; i < simdLength; i += Size) //Start at Size because result already contains the first full IntVector3
            {
                v = new IntVector3 (wideResult, i);
                tempVector = Unsafe.ReadUnaligned<Vector<int>> (&v);

                wideResult += tempVector;
            }

            return new IntVector3 (wideResult);*/
        }

        public static implicit operator Vector3 (Int3 iv)
        {
            return new Vector3 (iv.x, iv.y, iv.z);
        }

        public static implicit operator Int3 (Vector3 iv)
        {
            return new Int3 (iv.X, iv.Y, iv.Z);
        }

        public static implicit operator Int3 ((int x, int y, int z) v)
        {
            return new Int3 (v.x, v.y, v.z);
        }

        public static implicit operator (int, int, int) (Int3 v)
        {
            return (v.x, v.y, v.z);
        }

        public static bool operator == (Int3 a, Int3 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z)
                return true;
            else
                return false;
        }

        public static bool operator != (Int3 a, Int3 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z)
                return false;
            else
                return true;
        }

        public static bool operator == (Int3 a, Vector3 b)
        {
            if (a.x == b.X && a.y == b.Y && a.z == b.Z)
                return true;
            else
                return false;
        }

        public static bool operator != (Int3 a, Vector3 b)
        {
            if (a.x == b.X && a.y == b.Y && a.z == b.Z)
                return false;
            else
                return true;
        }

        public unsafe static Int3 operator + (Int3 a, Int3 b)
        {
            return new Int3 (a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Int3 operator - (Int3 a, Int3 b)
        {
            return new Int3 (a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Int3 operator - (Int3 v)
        {
            return new Int3 (v.x * -1, v.y * -1, v.z * -1);
        }

        public static Int3 operator * (Int3 a, int b)
        {
            return new Int3 (a.x * b, a.y * b, a.z * b);
        }

        public static Int3 operator / (Int3 a, float b)
        {
            return new Int3 (a.x / b, a.y / b, a.z / b);
        }

        public static bool operator > (Int3 a, Int3 b) => a.x > b.x && a.y > b.y && a.z > b.z;

        public static bool operator < (Int3 a, Int3 b) => a.x < b.x && a.y < b.y && a.z < b.z;

        public static bool operator >= (Int3 a, Int3 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z;

        public static bool operator <= (Int3 a, Int3 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z;

        public override string ToString ()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        public override int GetHashCode ()
        {
            return (x * 73856093) ^ (y * 19349663) ^ (z * 83492791);
        }

        public bool Equals (Int3 other) => this == other;

        public override bool Equals (object obj)
        {
            Int3? v = obj as Int3?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
