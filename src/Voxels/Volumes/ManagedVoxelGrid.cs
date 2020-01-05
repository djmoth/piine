using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    /// <summary>
    /// A VoxelGrid where voxel data is stored in a managed array.
    /// </summary>
    /// <typeparam name="T">Type of voxels</typeparam>
    public class ManagedVoxelGrid<T> : VoxelGrid<T>
    {
        private readonly T[] voxels;

        public override int Width { get; }
        public override int Height { get; }
        public override int Depth { get; }

        public ManagedVoxelGrid (int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;

            voxels = new T[Volume];
        }

        public override void SetVoxel (int index, T data) => voxels[index] = data; //Array will handle out of bounds errors

        public override T GetVoxel (int index) => voxels[index]; //Array will handle out of bounds errors
    }
}
