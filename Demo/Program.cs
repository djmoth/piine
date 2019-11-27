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
            
        }
    }

    public class ColorVoxelVolume : UnmanagedVoxelGrid<Color32>
    {
        public ColorVoxelVolume (int width, int height, int depth) : base (width, height, depth)
        {

        }

        public override Color32 GetColor (int index) => GetVoxel (index);
    }

    public class Color8VoxelVolume : UnmanagedVoxelGrid<byte>
    {
        public Color8VoxelVolume (int width, int height, int depth) : base (width, height, depth)
        {

        }

        public override Color32
    }
}
