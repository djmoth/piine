using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    /// <summary>
    /// Points to a buffer stored in unmanaged memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe struct UnmanagedBuffer<T> : IEquatable<UnmanagedBuffer<T>> where T : unmanaged
    {
        private T* pointer;

        public long Length { get; }
        public bool Allocated => pointer != null;

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

        [CLSCompliant (false)]
        public UnmanagedBuffer (T* pointer, long length)
        {
            this.pointer = pointer;
            Length = length;
        }

        public UnmanagedBuffer (IntPtr pointer, long length)
        {
            this.pointer = (T*)pointer;
            Length = length;
        }

        [CLSCompliant (false)]
        public T* GetPointer ()
        {
            if (!Allocated)
                throw new InvalidOperationException ("Buffer is not allocated");

            return pointer;
        }

        public static bool operator == (UnmanagedBuffer<T> left, UnmanagedBuffer<T> right) => left.pointer == right.pointer && left.Length == right.Length;

        public static bool operator != (UnmanagedBuffer<T> left, UnmanagedBuffer<T> right) => left.pointer != right.pointer || left.Length != right.Length;

        public override int GetHashCode () => (int)pointer;

        public override bool Equals (object obj)
        {
            UnmanagedBuffer<T> ? v = obj as UnmanagedBuffer<T>?;

            if (v != null)
                return v == this;
            else
                return false;
        }

        public bool Equals (UnmanagedBuffer<T> other) => this == other;
    }
}
