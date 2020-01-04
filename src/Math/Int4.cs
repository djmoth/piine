using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace piine
{
    [StructLayout (LayoutKind.Sequential, Size = 16)]
    public struct Int4 : IEquatable<Int4>
    {
        public const int Size = 4; //Number of dimensions

        public static Int4 Zero { get; } = new Int4 (0);
        public static Int4 One { get; } = new Int4 (1);
        public static Int4 UnitX { get; } = new Int4 (1, 0, 0, 0);
        public static Int4 UnitY { get; } = new Int4 (0, 1, 0, 0);
        public static Int4 UnitZ { get; } = new Int4 (0, 0, 1, 0);
        public static Int4 UnitW { get; } = new Int4 (0, 0, 0, 1);

        public int x;
        public int y;
        public int z;
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

        public Int4 (float x, float y, float z, float w)
        {
            this.x = (int)Math.Round (x);
            this.y = (int)Math.Round (y);
            this.z = (int)Math.Round (z);
            this.w = (int)Math.Round (w);
        }

        public Int4 (int all)
        {
            x = all;
            y = all;
            z = all;
            w = all;
        }

        public Int4 (Vector<int> v, int startIndex = 0)
        {
            x = v[startIndex];
            y = v[startIndex + 1];
            z = v[startIndex + 2];
            w = v[startIndex + 3];
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                }

                throw new ArgumentOutOfRangeException ("Index must be in the range 0-3, index was " + index);
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; return;
                    case 1: y = value; return;
                    case 2: z = value; return;
                    case 3: w = value; return;
                }

                throw new ArgumentOutOfRangeException ("Index must be in the range 0-3, index was " + index);
            }
        }

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

        public static Vector4 NormalizeToVector4 (Int4 v) => Vector4.Normalize (v);

        public int GetLongestComponent ()
        {
            int xAbsolute = Math.Abs (x);
            int yAbsolute = Math.Abs (y);
            int zAbsolute = Math.Abs (z);
            int wAbsolute = Math.Abs (w);

            return Math.Max (xAbsolute, Math.Max (Math.Max (yAbsolute, zAbsolute), wAbsolute));
        }
        public int CalculateVolume () => x * y * z * w;

        public static Int4 Absolute (Int4 v)
        {
            return new Int4 (Math.Abs (v.x), Math.Abs (v.y), Math.Abs (v.z), Math.Abs (v.w));
        }

        public static float Distance (Int4 a, Int4 b)
        {
            return (a - b).Length;
        }

        public static Vector4 RoundVector4 (Vector4 v)
        {
            return (Int4)v;
        }

        [CLSCompliant (false)]
        public static unsafe Int4 Sum (Int4* vectorPtr, int length)
        {
            Vector<int> result = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr);

            Vector<int> vector;
            for (int i = 1; i < length; i++) //Start at 1 because result already contains the first element
            {
                vector = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr + i);

                result += vector;
            }

            return new Int4 (result);

            /*int fullVectorsInSIMD = Vector<int>.Count / Size; //The number of IntVector4's that fit in one Vector<int>
            int simdLength = fullVectorsInSIMD * Size; //Number of ints that fit in fullVectorsInSIMD

            Vector<int> wideResult = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr);

            Vector<int> tempVector;
            for (int i = fullVectorsInSIMD; i < length; i += fullVectorsInSIMD) //Start at simdLength because result already contains the first elements
            {
                tempVector = Unsafe.ReadUnaligned<Vector<int>> (vectorPtr + i);

                wideResult += tempVector;
            }

            //Sum the resulting fullVectorsInSIMD together
            IntVector4 v;
            for (int i = Size; i < simdLength; i += Size) //Start at Size because result already contains the first full IntVector4
            {
                v = new IntVector4 (wideResult, i);
                tempVector = Unsafe.ReadUnaligned<Vector<int>> (&v);

                wideResult += tempVector;
            }

            return new IntVector4 (wideResult);*/
        }

        public static implicit operator Vector4 (Int4 iv)
        {
            return new Vector4 (iv.x, iv.y, iv.z, iv.w);
        }

        public static implicit operator Int4 (Vector4 iv)
        {
            return new Int4 (iv.X, iv.Y, iv.Z, iv.W);
        }

        public static implicit operator Int4 ((int x, int y, int z, int w) v)
        {
            return new Int4 (v.x, v.y, v.z, v.w);
        }

        public static implicit operator (int, int, int, int) (Int4 v)
        {
            return (v.x, v.y, v.z, v.w);
        }

        public static bool operator == (Int4 a, Int4 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w)
                return true;
            else
                return false;
        }

        public static bool operator != (Int4 a, Int4 b)
        {
            if (a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w)
                return false;
            else
                return true;
        }

        public static bool operator == (Int4 a, Vector4 b)
        {
            if (a.x == b.X && a.y == b.Y && a.z == b.Z && a.w == b.W)
                return true;
            else
                return false;
        }

        public static bool operator != (Int4 a, Vector4 b)
        {
            if (a.x == b.X && a.y == b.Y && a.z == b.Z && a.w == b.W)
                return false;
            else
                return true;
        }

        public unsafe static Int4 operator + (Int4 a, Int4 b)
        {
            return new Int4 (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Int4 operator - (Int4 a, Int4 b)
        {
            return new Int4 (a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Int4 operator - (Int4 v)
        {
            return new Int4 (v.x * -1, v.y * -1, v.z * -1, v.w * -1);
        }

        public static Int4 operator * (Int4 a, int b)
        {
            return new Int4 (a.x * b, a.y * b, a.z * b, a.w * b);
        }

        public static Int4 operator / (Int4 a, float b)
        {
            return new Int4 (a.x / b, a.y / b, a.z / b, a.w / b);
        }

        public static bool operator > (Int4 a, Int4 b) => a.x > b.x && a.y > b.y && a.z > b.z && a.w > b.w;

        public static bool operator < (Int4 a, Int4 b) => a.x < b.x && a.y < b.y && a.z < b.z && a.w < b.w;

        public static bool operator >= (Int4 a, Int4 b) => a.x >= b.x && a.y >= b.y && a.z >= b.z && a.w >= b.w;

        public static bool operator <= (Int4 a, Int4 b) => a.x <= b.x && a.y <= b.y && a.z <= b.z && a.w <= b.w;

        public override string ToString ()
        {
            return "(" + x + ", " + y + ", " + z + ", " + w + ")";
        }

        public override int GetHashCode ()
        {
            return (x * 74856094) ^ (y * 19449664) ^ (z * 84492791) ^ (w * 21016934);
        }

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
