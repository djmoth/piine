using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace piine.Memory
{
    public static unsafe class Unmanaged
    {
        /// <summary>
        /// Allocate unmanaged memory. Also informs the GC of the added memory pressure. The content is not zero-filled.
        /// </summary>
        /// <typeparam name="T">Unmanaged Type to allocate for.</typeparam>
        /// <param name="length">Number of elements to allocate.</param>
        /// <returns></returns>
        [CLSCompliant (false)]
        public static T* AllocMemory<T> (long length) where T : unmanaged
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException ("length must be more than 0", nameof (length));

            long bytesToAlloc = length * sizeof (T);

            GC.AddMemoryPressure (bytesToAlloc);
            return (T*)Marshal.AllocHGlobal ((IntPtr)bytesToAlloc);
        }

        /// <summary>
        /// Allocate unmanaged memory. Also informs the GC of the added memory pressure. The content is zero-filled depending on the value of <paramref name="fillWithDefault"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="fillWithDefault">If true, fills all the buffers with <c>default (T)</c></param>
        /// <returns></returns>
        [CLSCompliant (false)]
        public static T* AllocMemory<T> (long length, bool fillWithDefault) where T : unmanaged
        {
            if (!fillWithDefault)
                return AllocMemory<T> (length);

            if (length <= 0)
                throw new ArgumentOutOfRangeException (nameof (length), "length must be more than 0");

            long bytesToAlloc = length * sizeof (T);

            GC.AddMemoryPressure (bytesToAlloc);
            T* memory = (T*)Marshal.AllocHGlobal ((IntPtr)bytesToAlloc);

            for (int i = 0; i < length; i++)
            {
                memory[i] = default;
            }

            return memory;
        }
        /// <summary>
        /// Free unmanaged memory. Also informs the GC of the relieved memory pressure.
        /// </summary>
        /// <param name="memory">Pointer to memory. The pointer is set to <c>null</c> afterwards</param>
        /// <param name="length">Number of elements to free</param>
        [CLSCompliant (false)]
        public static void FreeMemory<T> (ref T* memory, long length) where T : unmanaged
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException (nameof (length), "length must be more than 0");

            long bytesToFree = length * sizeof (T);

            GC.RemoveMemoryPressure (bytesToFree);
            Marshal.FreeHGlobal ((IntPtr)memory);

            memory = null;
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
            if (currentLength <= 0)
                throw new ArgumentOutOfRangeException (nameof (currentLength), "currentLength must be more than 0");
            if (newLength <= 0)
                throw new ArgumentOutOfRangeException (nameof (newLength), "newLength must be more than 0");

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
            {
                T* tempPtr = spanPtr;
                FreeMemory (ref tempPtr, span.Length);
            }
        }
    }
}
