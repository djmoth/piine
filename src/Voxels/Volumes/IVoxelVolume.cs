using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public interface IVoxelVolume<T>
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }

        void SetVoxel (Int3 voxel, T data);
        T GetVoxel (Int3 voxel);
    }
}
