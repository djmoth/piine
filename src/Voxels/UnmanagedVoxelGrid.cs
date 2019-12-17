using System;
using System.Collections.Generic;
using piine.Graphics;
using piine.Memory;

namespace piine.Voxels
{
    /// <summary>
    /// A VoxelGrid where voxel data is stored in an unmanaged array.
    /// </summary>
    /// <typeparam name="T">Unmanaged Type of voxels</typeparam>
    public unsafe class UnmanagedVoxelGrid<T> : VoxelGrid<T>, IDisposable where T : unmanaged
    {
        private UnmanagedArray<T> voxels;

        public override int Width { get; }
        public override int Height { get; }
        public override int Depth { get; }

        public UnmanagedVoxelGrid (int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;

            voxels = new UnmanagedArray<T> (Volume);
        }

        public override void SetVoxel (int index, T data)
        {
            if (voxels == null)
                throw new ObjectDisposedException (nameof (UnmanagedVoxelGrid<T>));
            if (index < 0 || index >= Volume)
                throw new ArgumentOutOfRangeException (nameof (index));

            voxels.ArrayPointer[index] = data;
        }

        /// <summary>
        /// Sets a voxel without doing any bounds checking
        /// </summary>
        /// <param name="index">Index of the voxel to set. You must ensure this is in the range [0;Volume[</param>
        /// <param name="data">Data to write to the voxel</param>
        public void SetVoxelUnsafe (int index, T data) => voxels.ArrayPointer[index] = data;

        public override T GetVoxel (int index)
        {
            if (voxels == null)
                throw new ObjectDisposedException (nameof (UnmanagedVoxelGrid<T>));
            if (index < 0 || index >= Volume)
                throw new ArgumentOutOfRangeException (nameof (index));

            return voxels.ArrayPointer[index];
        }

        /// <summary>
        /// Gets a voxel without doing any bounds checking.
        /// </summary>
        /// <param name="index">Index of the voxel to get. You must ensure this is in the range [0;Volume[</param>
        public T GetVoxelUnsafe (int index) => voxels.ArrayPointer[index];

        ~UnmanagedVoxelGrid () => Dispose (false);

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        /// <summary>
        /// Free the unmanaged memory. The object will be unusable afterwards.
        /// </summary>
        protected virtual void Dispose (bool disposing)
        {
            if (voxels != null)
            {
                voxels.Dispose ();
                voxels = null;
            }
        }

        public override int GetHashCode ()
        {
            const int prime = 31;

            int hash = 0;

            for (int i = 0; i < Volume; i++)
            {
                hash ^= voxels.ArrayPointer[i].GetHashCode ();
                hash *= prime;
            }

            return hash;
        }
    }
}
