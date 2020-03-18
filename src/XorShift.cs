using System;
using System.Collections.Generic;
using System.Text;

namespace piine
{
    /// <summary>
    /// A simple Pseudo-Random Number Generator, derived from George Marsaglia's work
    /// </summary>
    public struct XorShift
    {
        private int state;

        public XorShift (int seed)
        {
            if (seed == 0)
                state = (int)DateTime.Now.ToBinary ();
            else
                state = seed;
        }

        /// <summary>
        /// Calculates a new random number in the range [Int32.MinValue; Int32.MaxValue]
        /// </summary>
        public int Next ()
        {
            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;
            return state;
        }

        /// <summary>
        /// Calculates a new random number in the range [<paramref name="min"/>; <paramref name="max"/>]
        /// </summary>
        /// <param name="min">Inclusive lower bound</param>
        /// <param name="max">Inclusive upper bound</param>
        public int Next (int min, int max)
        {
            int rng = Next ();
            return Mod (rng, max + 1 - min) + min;
        }

        private static int Mod (int n, int modulus) =>  (n % modulus + modulus) % modulus;

        public override int GetHashCode () => state;

        public static bool operator == (XorShift left, XorShift right) => left.state == right.state;

        public static bool operator != (XorShift left, XorShift right) => left.state != right.state;

        public override bool Equals (object obj)
        {
            XorShift? v = obj as XorShift?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
