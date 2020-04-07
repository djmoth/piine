using System;
using System.Collections.Generic;
using System.Text;

namespace piine
{
    public readonly struct Fixed16 : IEquatable<Fixed16>
    {    
        private const int PRECISION_BITS = 6;

        public const int Precision = 1 << PRECISION_BITS;
        private const float PrecisionF = Precision;
        public const float MinFraction = 1 / PrecisionF;
        public const float MaxValue = short.MaxValue / PrecisionF;
        public const float MinValue = short.MinValue / PrecisionF;

        public readonly short raw;

        public Fixed16 (float value) => raw = (short)(value * Precision);
        public Fixed16 (int value) => raw = (short)(value << PRECISION_BITS);

        private Fixed16 (short rawValue) => raw = rawValue;

        public static Fixed16 FromRawValue (short rawValue) => new Fixed16 (rawValue);

        public static implicit operator Fixed16 (float value) => new Fixed16 (value);

        public static implicit operator float (Fixed16 value) => value.raw / PrecisionF;

        public static implicit operator Fixed16 (int value) => new Fixed16 (value);

        public static explicit operator int (Fixed16 value) => value.raw / Precision;

        public static Fixed16 operator + (Fixed16 a, Fixed16 b) => new Fixed16 ((short)(a.raw + b.raw));
        public static Fixed16 operator - (Fixed16 a, Fixed16 b) => new Fixed16 ((short)(a.raw - b.raw));
        public static Fixed16 operator * (Fixed16 a, Fixed16 b) => new Fixed16 ((short)((a.raw * b.raw) >> PRECISION_BITS));
        public static Fixed16 operator * (Fixed16 a, int b) => new Fixed16 ((short)(a.raw * b));
        public static Fixed16 operator / (Fixed16 a, Fixed16 b) => new Fixed16 ((short)((a.raw << PRECISION_BITS) / b.raw));

        public static bool operator == (Fixed16 a, Fixed16 b) => a.raw == b.raw;
        public static bool operator != (Fixed16 a, Fixed16 b) => a.raw != b.raw;

        public bool Equals (Fixed16 other) => this == other;

        public override bool Equals (object obj) => obj is Fixed16 && Equals ((Fixed16)obj);

        public override int GetHashCode () => raw;

        public override string ToString () => ((float)this).ToString ();
    }
}
