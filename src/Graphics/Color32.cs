using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace piine.Graphics
{
    /// <summary>
    /// A 32 bit RGBA color value. 1 byte for each component.
    /// </summary>
    [StructLayout (LayoutKind.Explicit, Size = 4)]
    public struct Color32 : IEquatable<Color32>
    {
        [FieldOffset(0)]
        public byte r;
        [FieldOffset (1)]
        public byte g;
        [FieldOffset (2)]
        public byte b;
        [FieldOffset (3)]
        public byte a;

        [FieldOffset (0)]
        private int rawInt32;

        /// <summary>
        /// Get the 4 components packed into a 32bit integer in the order RGBA. The most significant byte is R and the least is A.
        /// </summary>
        public int AsInt32 => rawInt32;

        /// <summary>
        /// Constructs a new color value
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <param name="a">Alpha</param>
        public Color32 (byte r, byte g, byte b, byte a)
        {
            rawInt32 = 0;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// Constructs a new color value. Alpha will be set to 255
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        public Color32 (byte r, byte g, byte b)
        {
            rawInt32 = 0;
            this.r = r;
            this.g = g;
            this.b = b;
            a = 255;
        }

        /// <summary>
        /// Constructs a new color value from a 32 bit integer
        /// </summary>
        /// <param name="rawRGBA">RGBA components packed into an integer</param>
        public Color32 (int rawRGBA)
        {
            r = 0;
            g = 0;
            b = 0;
            a = 0;
            rawInt32 = rawRGBA;
        }

        public static bool operator == (Color32 left, Color32 right) => left.AsInt32 == right.AsInt32;

        public static bool operator != (Color32 left, Color32 right) => left.AsInt32 != right.AsInt32;

        public bool Equals (Color32 other) => this == other;

        public override bool Equals (object obj)
        {
            Color32? c = obj as Color32?;

            if (c != null)
                return c == this;
            else
                return false;
        }

        public override int GetHashCode () => AsInt32;

        public override string ToString () => "#" + r.ToString ("X2") + g.ToString ("X2") + b.ToString ("X2") + a.ToString ("X2");

        //Colors
        public static Color32 Black => new Color32 (0, 0, 0);
        public static Color32 White => new Color32 (255, 255, 255);
        public static Color32 Red => new Color32 (255, 0, 0);
        public static Color32 Green => new Color32 (0, 255, 0);
        public static Color32 Blue => new Color32 (0, 0, 255);
    }
}
