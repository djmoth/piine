using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using piine.Graphics;

namespace piine.Voxels
{
    /// <summary>
    /// Base-class for voxel data.
    /// </summary>
    /// <typeparam name="T">Type of voxels</typeparam>
    public abstract class VoxelGrid<T> : IVoxelVolume<T>
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract int Depth { get; }
        public int Volume => Width * Height * Depth;
        public IntVector3 Dimensions => new IntVector3 (Width, Height, Depth);

        /// <summary>
        /// Shorthand for GetVoxel (index) & SetVoxel (index, value).
        /// </summary>
        /// <param name="index">1D index of a voxel.</param>
        public T this[int index] { get => GetVoxel (index); set => SetVoxel (index, value); }

        /// <summary>
        /// Shorthand for GetVoxel (position) & SetVoxel (position, value).
        /// </summary>
        /// <param name="index">3D position of a voxel.</param>
        public T this[IntVector3 position] { get => GetVoxel (position); set => SetVoxel (position, value); }

        public virtual void SetVoxel (IntVector3 position, T data)
        {
            if (position.x < 0 || position.x >= Width || position.y < 0 || position.y >= Height || position.z < 0 || position.z >= Depth)
                throw new ArgumentOutOfRangeException (nameof (position));

            SetVoxel (CalculateIndex (position), data);
        }
        public abstract void SetVoxel (int index, T data);

        public virtual T GetVoxel (IntVector3 position)
        {
            if (position.x < 0 || position.x >= Width || position.y < 0 || position.y >= Height || position.z < 0 || position.z >= Depth)
                throw new ArgumentOutOfRangeException (nameof (position));

            return GetVoxel (CalculateIndex (position));
        }

        public abstract T GetVoxel (int index);

        /// <summary>
        /// Calculates a 1D index from a 3D position.
        /// </summary>
        public int CalculateIndex (IntVector3 voxel) => voxel.x + voxel.z * Width + voxel.y * (Width * Depth);

        /// <summary>
        /// Calculates a 3D position from a 1D index.
        /// </summary>
        public IntVector3 CalculatePosition (int index) => new IntVector3 (index % Width, index / (Width * Depth), (index / Depth) % Depth);

        public static int CalculateIndex (IntVector3 dimensions, IntVector3 voxel) => voxel.x + voxel.z * dimensions.x + voxel.y * (dimensions.x * dimensions.z);
        public static IntVector3 CalculatePosition (IntVector3 dimensions, int index) => new IntVector3 (index % dimensions.x, index / (dimensions.x * dimensions.z), (index / dimensions.x) % dimensions.z);

        private Enumerator GetEnumerator () => new Enumerator (this);

        private struct Enumerator
        {
            private readonly VoxelGrid<T> voxels;
            public T Current => voxels[position];

            private int position;

            public Enumerator (VoxelGrid<T> voxels)
            {
                this.voxels = voxels;
                position = -1;
            }

            public bool MoveNext ()
            {
                position++;

                return position < voxels.Volume;
            }

            public void Reset ()
            {
                position = -1;
            }

            public void Dispose () { }
        }
    }
}
