using System;
using piine;
using piine.Voxels;
using piine.Graphics;
using System.Runtime.InteropServices;

namespace Demo
{
    class Program
    {
        static void Main (string[] args)
        {
            
        }

        public struct CustomVoxel : IVoxel
        {
            private Color32 color;

            public Color32 Color => color;
            public float Density => color.a - 127;
            public float Specular => 0;
            public Float3 Normal => Float3.Zero;
            public float Emission => 0;
        }

        [StructLayout (LayoutKind.Explicit, Size = 8)]
        public struct FullVoxel : IVoxel
        {
            [FieldOffset (0)]
            private Color32 color;
            [FieldOffset (4)]
            private byte normalX;
            [FieldOffset (5)]
            private byte normalY;
            [FieldOffset (6)]
            private byte normalZ;
            [FieldOffset (7)]
            private byte specularEmission;

            public Color32 Color => color;
            public float Density => color.a - 127;
            public float Specular => ExtractSpecular ();
            public Float3 Normal => ExtractNormal ();
            public float Emission => ExtractEmission ();

            private Float3 ExtractNormal ()
            {
                Float3 normal = new Float3 ();

                normal.x = (normalX / 255f) - 0.5f;
                normal.y = (normalY / 255f) - 0.5f;
                normal.z = (normalZ / 255f) - 0.5f;

                return normal * 2;
            }

            private float ExtractSpecular () => (specularEmission >> 4) / 15f;

            private float ExtractEmission () => (specularEmission & 0b0000_1111) / 15f;
        }
    }
}
