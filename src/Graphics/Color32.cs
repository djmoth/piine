using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace piine.Graphics
{
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

        public int AsInt32 => rawInt32;

        public Color32 (byte r, byte g, byte b, byte a)
        {
            rawInt32 = 0;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public Color32 (byte r, byte g, byte b)
        {
            rawInt32 = 0;
            this.r = r;
            this.g = g;
            this.b = b;
            a = 255;
        }

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

        //Colors
        public static Color32 Black => new Color32 (0, 0, 0);
        public static Color32 White => new Color32 (255, 255, 255);
        public static Color32 Red => new Color32 (255, 0, 0);
        public static Color32 Green => new Color32 (0, 255, 0);
        public static Color32 Blue => new Color32 (0, 0, 255);
    }
}
