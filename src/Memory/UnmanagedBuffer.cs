using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    /// <summary>
    /// A buffer stored in unmanaged memory. Be mindful when working with this type, as memory leaks can occur easily. For a safe alternative, use <see cref="UnmanagedArray{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe struct UnmanagedBuffer<T> where T : unmanaged
    {
        private T* pointer;

        [CLSCompliant (false)]
        public T* Pointer => pointer;
        public long Length { get; }

        public T this[long index]
        {
            get
            {
                if (pointer == null)
                    throw new InvalidOperationException ("The UnmanagedBuffer is not allocated");
                if (index < 0 && index >= Length)
                    throw new ArgumentOutOfRangeException (nameof (index));

                return pointer[index];
            }
            set
            {
                if (pointer == null)
                    throw new InvalidOperationException ("The UnmanagedBuffer is not allocated");
                if (index < 0 && index >= Length)
                    throw new ArgumentOutOfRangeException (nameof (index));

                pointer[index] = value;
            }
        }

        public UnmanagedBuffer (long length, bool fillWithDefault = false)
        {
            pointer = Unmanaged.AllocMemory<T> (length); //AllocMemory checks if length is more than 0
            Length = length;
        }

        [CLSCompliant (false)]
        public UnmanagedBuffer (T* pointer, long length)
        {
            this.pointer = pointer;
            Length = length;
        }

        public void Free () => Unmanaged.FreeMemory (ref pointer, Length);
    }
}
