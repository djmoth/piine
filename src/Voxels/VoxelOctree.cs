using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public abstract class VoxelOctree<T> : IVoxelVolume<T>
    {
        public int Width => Size;
        public int Height => Size;
        public int Depth => Size;
        public virtual int Size { get; }

        public abstract T GetVoxel (IntVector3 position);

        public abstract void SetVoxel (IntVector3 position, T data);

        /// <summary>
        /// Calculates the 1D index of a node from it's 3D position.
        /// </summary>
        public static int CalculateNodeIndex (IntVector3 nodePos) => nodePos.x + nodePos.z * 2 + nodePos.y * 4;

        /// <summary>
        /// Calculates the 3D position of a node from it's 1D index.
        /// </summary>
        public static IntVector3 CalculateNodePosition (int index) => new IntVector3 (index % 2, index / 4, (index / 2) % 2);


    }
}
