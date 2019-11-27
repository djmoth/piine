using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace piine.Memory
{
    [CLSCompliant (false)]
    public static unsafe class Unmanaged
    {
        /// <summary>
        /// Allocate unmanaged memory. Also informs the GC of the added memory pressure.
        /// </summary>
        /// <typeparam name="T">Unmanaged Type to allocate for.</typeparam>
        /// <param name="length">Number of elements to allocate.</param>
        /// <returns></returns>
        public static T* AllocMemory<T> (long length) where T : unmanaged
        {
            long bytesToAlloc = length * sizeof (T);

            GC.AddMemoryPressure (bytesToAlloc);
            return (T*)Marshal.AllocHGlobal ((IntPtr)bytesToAlloc);
        }

        /// <summary>
        /// Free unmanaged memory. Also informs the GC of the relieved memory pressure.
        /// </summary>
        /// <param name="memory">Pointer to memory</param>
        /// <param name="length">Number of elements to free</param>
        public static void FreeMemory<T> (T* memory, long length) where T : unmanaged
        {
            long bytesToFree = length * sizeof (T);

            GC.RemoveMemoryPressure (bytesToFree);
            Marshal.FreeHGlobal ((IntPtr)memory);
        }

        /// <summary>
        /// Resizes already allocated memory
        /// </summary>
        /// <param name="memory">Pointer to memory</param>
        /// <param name="currentLength">Current number of elements</param>
        /// <param name="newLength">New number of elements</param>
        /// <returns></returns>
        public static void ReAllocMemory<T> (ref T* memory, long currentLength, long newLength) where T : unmanaged
        {
            long currentSizeBytes = currentLength * sizeof (T);
            long newSizeBytes = newLength * sizeof (T);

            long difference = newSizeBytes - currentSizeBytes;

            if (difference > 0)
                GC.AddMemoryPressure (difference);
            else if (difference < 0)
                GC.RemoveMemoryPressure (difference);

            memory = (T*)Marshal.ReAllocHGlobal ((IntPtr)memory, (IntPtr)newSizeBytes);
        }
    }
}
