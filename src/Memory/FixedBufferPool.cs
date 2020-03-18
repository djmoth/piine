using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace piine.Memory
{
    
    public unsafe class FixedBufferPool<T> : IDisposable where T : unmanaged
    {
        private T* buffers;
        private ulong* freeBuffers;
        private int freeBuffersLength;

        /// <summary>
        /// Total number of buffers
        /// </summary>
        public int PoolSize { get; }
        /// <summary>
        /// Number of elements in each buffer
        /// </summary>
        public int BufferSize { get; }
        /// <summary>
        /// Number of allocated buffers
        /// </summary>
        public int AllocatedBufferCount { get; private set; }
        /// <summary>
        /// Set to <see langword="false"/> when the <see cref="FixedBufferPool{T}"/> is disposed
        /// </summary>
        public bool Allocated => buffers != null;

        /// <summary>
        /// Constructs a new <see cref="FixedBufferPool{T}"/>
        /// </summary>
        /// <param name="poolSize">How many buffers the pool contains</param>
        /// <param name="bufferSize">The size of the individual buffers, in number of elements</param>
        public FixedBufferPool (int poolSize, int bufferSize)
        {
            PoolSize = poolSize;
            BufferSize = bufferSize;

            buffers = Unmanaged.AllocMemory<T> (PoolSize * BufferSize);
            freeBuffersLength = (int)Math.Ceiling (PoolSize / 64d);
            freeBuffers = Unmanaged.AllocMemory<ulong> (freeBuffersLength, true);
        }

        private void CheckIfAllocated ()
        {
            if (!Allocated)
                throw new ObjectDisposedException (nameof (FixedBufferPool<T>));
        }

        /// <summary>
        /// Allocates a new buffer from the pool.
        /// </summary>
        /// <param name="fillWithDefault">If true, fills all the buffers with <c>default (T)</c></param>
        /// <returns>A pointer to the buffer. The pointer is <c>null</c> if the buffer is full</returns>
        [CLSCompliant (false)]
        public T* Allocate (bool fillWithDefault)
        {
            CheckIfAllocated ();

            if (AllocatedBufferCount == PoolSize)
                return null;

            //Find a free buffer. Each ulong in freeBuffers represents the state of 64 buffers, via the 64 bits of the ulong. A HIGH value is an occupied buffer, a LOW value is a free buffer.
            for (int i = 0; i < freeBuffersLength; i++)
            {
                if (freeBuffers[i] != ulong.MaxValue) //Quickly check if all buffers in this part is occupied, as a fully occupied part will have a bits HIGH, meaning ulong.MaxValue
                {
                    byte* b = (byte*)(freeBuffers + i);

                    for (int j = 0; j < 8; j++)
                    {
                        if (b[j] != 0xFF)//Quickly check if all buffers in this byte is occupied, as a fully occupied byte will have a bits HIGH, meaning 0xFF
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                if (((b[j] >> k) & 1) == 0) //Found a free buffer
                                {
                                    b[j] |= (byte)(1 << k); //Mark the buffer as occupied by setting the corresponding bit HIGH

                                    int bufferIndex = (i * 64) + (j * 8) + k;

                                    T* buffer = buffers + (bufferIndex * BufferSize);
                                    AllocatedBufferCount++;

                                    if (fillWithDefault)    
                                    {
                                        for (int e = 0; e < BufferSize; e++)
                                        {
                                            buffer[e] = default;
                                        }
                                    }

                                    return buffer;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Frees an already allocated buffer from the pool. Does nothing if the buffer is already freed.
        /// </summary>
        /// <param name="buffer">Pointer to the buffer to free. Is set to <c>null</c> afterwards</param>
        [CLSCompliant (false)]
        public void Free (ref T* buffer)
        {
            CheckIfAllocated ();

            int bufferIndex = (int)(buffer - buffers) / BufferSize;
            int freeBufferPartIndex = Math.DivRem (bufferIndex, 64, out int localPartIndex); //The index of the ulong that the state of the buffer is stored in

            //Find which byte of the state the buffer is stored in + the index of the bit.
            int byteIndex = Math.DivRem (localPartIndex, 8, out int bitIndex); 
            byte* b = (byte*)(freeBuffers + freeBufferPartIndex) + byteIndex;

            //Sets the bit at bitIndex to LOW. (1 << bitIndex) sets a bit at bitIndex to HIGH, and the XOR with 0xFF will invert all the bits. The &= will only change the bit at bitIndex because it is the bit that is LOW in the right-hand side.
            (*b) &= (byte)(0xFF ^ (1 << bitIndex)); 

            AllocatedBufferCount--;

            buffer = null;
        }

        ~FixedBufferPool () => Dispose (false);

        /// <summary>
        /// Free the unmanaged memory. The pool will be unusable afterwards
        /// </summary>
        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (!Allocated)
                return;

            Unmanaged.FreeMemory (ref buffers, PoolSize * BufferSize);
        }
    }
}
