using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace piine.Memory
{
    /// <summary>
    /// Interface for working with unmanaged memory.
    /// </summary>
    public static unsafe class Unmanaged
    {
        /// <summary>
        /// Allocate unmanaged memory. Also informs the GC of the added memory pressure. The content is zero-filled depending on the value of <paramref name="fillWithDefault"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="fillWithDefault">If true, fills all the buffers with <c>default (T)</c></param>
        /// <returns></returns>
        [CLSCompliant (false)]
        public static T* AllocMemory<T> (long length, bool fillWithDefault = false) where T : unmanaged
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException (nameof (length), "length must be more than 0");

            long bytesToAlloc = length * sizeof (T);
            
            T* memory = (T*)Marshal.AllocHGlobal ((IntPtr)bytesToAlloc);
            GC.AddMemoryPressure (bytesToAlloc);

            if (fillWithDefault)
            {
                for (long i = 0; i < length; i++)
                {
                    memory[i] = default;
                }
            }

            return memory;
        }

        /// <summary>
        /// Allocate unmanaged memory. Also informs the GC of the added memory pressure. The content is zero-filled depending on the value of <paramref name="fillWithDefault"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="fillWithDefault">If true, fills all the buffers with <c>default (T)</c></param>
        /// <returns></returns>
        [CLSCompliant (false)]
        public static T* AllocMemory<T> (int length, bool fillWithDefault = false) where T : unmanaged => AllocMemory<T> ((long)length, fillWithDefault);

        /// <summary>
        /// Free unmanaged memory. Also informs the GC of the relieved memory pressure.
        /// </summary>
        /// <param name="memory">Pointer to memory. The pointer is set to <c>null</c> afterwards</param>
        /// <param name="length">Number of elements to free</param>
        [CLSCompliant (false)]
        public static void FreeMemory<T> (ref T* memory, long length) where T : unmanaged
        {
            if (memory == null)
                throw new ArgumentNullException (nameof (memory));
            if (length <= 0)
                throw new ArgumentOutOfRangeException (nameof (length), "length must be more than 0");

            long bytesToFree = length * sizeof (T);

            GC.RemoveMemoryPressure (bytesToFree);
            Marshal.FreeHGlobal ((IntPtr)memory);

            memory = null;
        }

        /// <summary>
        /// Free unmanaged memory. Also informs the GC of the relieved memory pressure.
        /// </summary>
        /// <param name="memory">Pointer to memory. The pointer is set to <c>null</c> afterwards</param>
        /// <param name="length">Number of elements to free</param>
        [CLSCompliant (false)]
        public static void FreeMemory<T> (ref T* memory, int length) where T : unmanaged => FreeMemory (ref memory, (long)length);

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
            if (memory == null)
                throw new ArgumentNullException (nameof (memory));
            if (currentLength <= 0)
                throw new ArgumentOutOfRangeException (nameof (currentLength), "currentLength must be more than 0");
            if (newLength <= 0)
                throw new ArgumentOutOfRangeException (nameof (newLength), "newLength must be more than 0");

            long currentSizeBytes = currentLength * sizeof (T);
            long newSizeBytes = newLength * sizeof (T);

            long difference = newSizeBytes - currentSizeBytes; 

            memory = (T*)Marshal.ReAllocHGlobal ((IntPtr)memory, (IntPtr)newSizeBytes);

            if (difference > 0)
                GC.AddMemoryPressure (difference);
            else if (difference < 0)
                GC.RemoveMemoryPressure (-difference);
        }

        /// <summary>
        /// Resizes already allocated memory
        /// </summary>
        /// <param name="memory">Pointer to memory</param>
        /// <param name="currentLength">Current number of elements</param>
        /// <param name="newLength">New number of elements</param>
        /// <returns></returns>
        [CLSCompliant (false)]
        public static void ReAllocMemory<T> (ref T* memory, int currentLength, int newLength) where T : unmanaged => ReAllocMemory (ref memory, (long)currentLength, newLength);

        /// <summary>
        /// Fills memory with a value
        /// </summary>
        /// <param name="memory">Pointer to memory</param>
        /// <param name="length">Number of elements</param>
        /// <param name="data">Value to fill with</param>
        [CLSCompliant (false)]
        public static void SetMemory<T> (T* memory, int length, T data) where T : unmanaged
        {
            if (memory == null)
                throw new ArgumentNullException (nameof (memory));

            if (sizeof (T) <= 8)
            {
                ulong packedData = 0;

                if (sizeof (T) == 1)
                {
                    byte dataAsByte = *(byte*)&data; //Read data as a byte

                    for (int i = 0; i < 8; i++)
                    {
                        packedData <<= 8;
                        packedData |= dataAsByte;
                    }
                } else if (sizeof (T) == 2)
                {
                    ushort dataAsShort = *(ushort*)&data; //Read data as a byte

                    for (int i = 0; i < 4; i++)
                    {
                        packedData <<= 16;
                        packedData |= dataAsShort;
                    }
                } else if (sizeof (T) == 4)
                {
                    uint dataAsInt = *(uint*)&data; //Read data as a byte

                    for (int i = 0; i < 2; i++)
                    {
                        packedData <<= 32;
                        packedData |= dataAsInt;
                    }
                } else //Size is 8
                    packedData = *(ulong*)&data;

                int memoryLengthInLongs = Math.DivRem (length * sizeof (T), 8, out int remainingElements);
                ulong* memoryAsLongs = (ulong*)memory;

                for (int i = 0; i < memoryLengthInLongs; i++)
                {
                    memoryAsLongs[i] = packedData;
                }

                if (remainingElements > 0)
                {
                    int start = memoryLengthInLongs * 8;

                    for (int i = 0; i < remainingElements; i++)
                    {
                        memory[start + i] = data;
                    }
                }
            } else
            {
                for (int i = 0; i < length; i++)
                {
                    memory[i] = default;
                }
            }
        }

        /// <summary>
        /// Fills memory with <c>default (T)</c>
        /// </summary>
        /// <param name="memory">Pointer to memory</param>
        /// <param name="length">Number of elements</param>
        [CLSCompliant (false)]
        public static void ZeroMemory<T> (T* memory, int length) where T : unmanaged
        {
            if (memory == null)
                throw new ArgumentNullException (nameof (memory));

            int memoryLengthInLongs = Math.DivRem (length * sizeof (T), 8, out int remainingElements);
            ulong* memoryAsLongs = (ulong*)memory;

            for (int i = 0; i < memoryLengthInLongs; i++)
            {
                memoryAsLongs[i] = 0;
            }

            if (remainingElements > 0)
            {
                int start = memoryLengthInLongs * 8;

                for (int i = 0; i < remainingElements; i++)
                {
                    memory[start + i] = default;
                }
            }
        }

        /// <summary>
        /// Wrapper for <c>Buffer.MemoryCopy</c>. Copies in elements instead of bytes.
        /// </summary>
        /// <param name="destinationLength">Number of elements at <paramref name="destination"/></param>
        /// <param name="elementsToCopy">Number of elements to copy</param>
        [CLSCompliant (false)]
        public static void MemoryCopy<T> (T* source, T* destination, long destinationLength, long elementsToCopy) where T : unmanaged => Buffer.MemoryCopy (source, destination, destinationLength * sizeof (T), elementsToCopy * sizeof (T));

        public static Span<T> AllocSpan<T> (int length) where T : unmanaged => new Span<T> (AllocMemory<T> (length), length);
        public static void FreeSpan<T> (Span<T> span) where T : unmanaged
        {
            fixed (T* spanPtr = &span.GetPinnableReference ())
            {
                T* tempPtr = spanPtr;
                FreeMemory (ref tempPtr, span.Length);
            }
        }
    }
}
