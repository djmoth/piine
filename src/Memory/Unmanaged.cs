using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace piine.Memory
{
    public static unsafe class Unmanaged
    {
        /// <summary>
        /// Allocate unmanaged memory. Also informs the GC of the added memory pressure.
        /// </summary>
        /// <typeparam name="T">Unmanaged Type to allocate for.</typeparam>
        /// <param name="length">Number of elements to allocate.</param>
        /// <returns></returns>
        [CLSCompliant (false)]
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
        [CLSCompliant (false)]
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
        [CLSCompliant (false)]
        public static void ReAllocMemory<T> (ref T* memory, long currentLength, long newLength) where T : unmanaged
        {
            long currentSizeBytes = currentLength * sizeof (T);
            long newSizeBytes = newLength * sizeof (T);

            long difference = newSizeBytes - currentSizeBytes;

            if (difference > 0)
                GC.AddMemoryPressure (difference);
            else if (difference < 0)
                GC.RemoveMemoryPressure (-difference);

            memory = (T*)Marshal.ReAllocHGlobal ((IntPtr)memory, (IntPtr)newSizeBytes);
        }

        public static Span<T> AllocSpan<T> (int length) where T : unmanaged => new Span<T> (AllocMemory<T> (length), length);
        public static unsafe void FreeSpan<T> (Span<T> span) where T : unmanaged
        {
            fixed (T* spanPtr = &span.GetPinnableReference ())
                FreeMemory (spanPtr, span.Length);
        }
    }
}
