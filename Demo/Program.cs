using System;
using piine;
using piine.Voxels;
using piine.Graphics;

namespace Demo
{
    class Program
    {
        static void Main (string[] args)
        {
            Color8VoxelVolume volume = new Color8VoxelVolume (4, 4, 4);

            IColorVolume volumeAsInterface = volume;

            Random rng = new Random ();

            for (int x = 0; x < volume.Width; x++)
            {
                for (int y = 0; y < volume.Height; y++)
                {
                    for (int z = 0; z < volume.Depth; z++)
                    {
                        byte color = (byte)rng.Next (byte.MinValue, byte.MaxValue);

                        volume.SetVoxel (new Int3 (x, y, z), color);

                        Console.WriteLine (volumeAsInterface.GetColor (new Int3 (x, y, z)));
                    }
                }
            }

            Console.ReadKey ();
        }
    }

    public class ColorVoxelVolume : UnmanagedVoxelGrid<Color32>, IColorVolume
    {
        public ColorVoxelVolume (int width, int height, int depth) : base (width, height, depth)
        {

        }

        public Color32 GetColor (Int3 voxel) => GetVoxel (voxel);
    }

    public class Color8VoxelVolume : UnmanagedVoxelGrid<byte>, IColorVolume
    {
        public Color8VoxelVolume (int width, int height, int depth) : base (width, height, depth)
        {

        }

        public Color32 GetColor (Int3 voxel)
        {
            byte raw = GetVoxel (voxel);

            Color32 color = new Color32 ();
            color.r = (byte)((raw >> 6) * 85);
            color.g = (byte)(((raw >> 4) & 0b00_00_00_11) * 85);
            color.b = (byte)(((raw >> 2) & 0b00_00_00_11) * 85);
            color.a = (byte)((raw & 0b00_00_00_11) * 85);

            return color;
        }
    }
}
