using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    interface IVoxelVolume<T>
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }

        void SetVoxel (IntVector3 position, T data);
        T GetVoxel (IntVector3 position);
    }
}
