using System;
using System.Collections.Generic;
using System.Text;

namespace piine
{
    public readonly struct Fixed8 : IEquatable<Fixed8>
    {
        private const int PRECISION_BITS = 6;

        public const int Precision = 1 << PRECISION_BITS;
        private const float PrecisionF = Precision;
        public const float MinFraction = 1 / PrecisionF;
        public const float MaxValue = sbyte.MaxValue / PrecisionF;
        public const float MinValue = sbyte.MinValue / PrecisionF;

        public readonly sbyte raw;

        public Fixed8 (float value) => raw = (sbyte)(value * Precision);
        public Fixed8 (int value) => raw = (sbyte)(value << PRECISION_BITS);

        private Fixed8 (sbyte rawValue) => raw = rawValue;

        public static Fixed8 FromRawValue (sbyte rawValue) => new Fixed8 (rawValue);

        public static implicit operator Fixed8 (float value) => new Fixed8 (value);

        public static implicit operator float (Fixed8 value) => value.raw / PrecisionF;

        public static implicit operator Fixed8 (int value) => new Fixed8 (value);

        public static explicit operator int (Fixed8 value) => value.raw / Precision;

        public static Fixed8 operator + (Fixed8 a, Fixed8 b) => new Fixed8 ((sbyte)(a.raw + b.raw));
        public static Fixed8 operator - (Fixed8 a, Fixed8 b) => new Fixed8 ((sbyte)(a.raw - b.raw));
        public static Fixed8 operator * (Fixed8 a, Fixed8 b) => new Fixed8 ((sbyte)((a.raw * b.raw) >> PRECISION_BITS));
        public static Fixed8 operator * (Fixed8 a, int b) => new Fixed8 ((sbyte)(a.raw * b));
        public static Fixed8 operator / (Fixed8 a, Fixed8 b) => new Fixed8 ((sbyte)((a.raw << PRECISION_BITS) / b.raw));

        public static bool operator == (Fixed8 a, Fixed8 b) => a.raw == b.raw;
        public static bool operator != (Fixed8 a, Fixed8 b) => a.raw != b.raw;

        public bool Equals (Fixed8 other) => this == other;

        public override bool Equals (object obj) => obj is Fixed8 && Equals ((Fixed8)obj);

        public override int GetHashCode () => raw;

        public override string ToString () => ((float)this).ToString ();
    }
}
