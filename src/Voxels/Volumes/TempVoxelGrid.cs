using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public unsafe ref struct TempVoxelGrid<T>
    {
        private Span<T> voxels;

        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
        public int Volume => Width * Height * Depth;
        public Int3 Dimensions => new Int3 (Width, Height, Depth);

        /// <summary>
        /// Shorthand for GetVoxel (index) & SetVoxel (index, value).
        /// </summary>
        /// <param name="index">1D index of a voxel.</param>
        public T this[int index] { get => GetVoxel (index); set => SetVoxel (index, value); }

        /// <summary>
        /// Shorthand for GetVoxel (position) & SetVoxel (position, value).
        /// </summary>
        /// <param name="index">3D position of a voxel.</param>
        public T this[Int3 position] { get => GetVoxel (position); set => SetVoxel (position, value); }

        public TempVoxelGrid (Span<T> source, Int3 dimensions)
        {
            if (dimensions.CalculateVolume () > source.Length)
                throw new ArgumentOutOfRangeException ("Dimensions are larger than source.Length");
            else if (dimensions.x < 1 || dimensions.y < 1 || dimensions.z < 1)
                throw new ArgumentOutOfRangeException (nameof (dimensions));

            voxels = source;

            Width = dimensions.x;
            Height = dimensions.y;
            Depth = dimensions.z;
        }

        public T GetVoxel (int index) => voxels[index]; //Bounds check handled by Span

        public T GetVoxel (Int3 voxel)
        {
            if (voxel.x < 0 || voxel.x >= Width || voxel.y < 0 || voxel.y >= Height || voxel.z < 0 || voxel.z >= Depth)
                throw new ArgumentOutOfRangeException (nameof (voxel));

            return GetVoxel (CalculateIndex (voxel));
        }

        public void SetVoxel (int index, T data) => voxels[index] = data; //Bounds check handled by Span

        public void SetVoxel (Int3 voxel, T data)
        {
            if (voxel.x < 0 || voxel.x >= Width || voxel.y < 0 || voxel.y >= Height || voxel.z < 0 || voxel.z >= Depth)
                throw new ArgumentOutOfRangeException (nameof (voxel));

            SetVoxel (CalculateIndex (voxel), data);
        }

        public int CalculateIndex (Int3 voxel) => voxel.x + voxel.z * Width + voxel.y * (Width * Depth);
    }
}
