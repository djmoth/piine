using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;

namespace piine.Memory
{
    /// <summary>
    /// An Array stored in unmanaged memory. May help relieve pressure from the Garbage Collector
    /// </summary>
    /// <typeparam name="T">The unmanaged type that the array will contain</typeparam>
    public unsafe class UnmanagedArray<T> : IList<T>, IDisposable where T : unmanaged
    {
        /// <summary>
        /// Get the underlying pointer. Be mindful when working with this.
        /// </summary>
        [CLSCompliant (false)]
        public T* ArrayPointer { get; }
        /// <summary>
        /// Size of the array. The same as Count
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// Size of the array. The same as Length
        /// </summary>
        public int Count => Length;
        /// <summary>
        /// Set to false when the object is disposed
        /// </summary>
        public bool Allocated { get; private set; } = true;
        /// <summary>
        /// Always false
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Access an element in the UnmanagedArray. Performs bounds checking.
        /// </summary>
        /// <param name="index">Zero-based index of element.</param>
        public T this[int index]
        {
            get
            {
                CheckIfAllocated ();

                if (index < 0 || index >= Length)
                    throw new ArgumentOutOfRangeException (nameof (index));

                return ArrayPointer[index];
            }
            set
            {
                CheckIfAllocated ();

                if (index < 0 || index >= Length)
                    throw new ArgumentOutOfRangeException (nameof (index));

                ArrayPointer[index] = value;
            }
        }

        /// <summary>
        /// Allocate a new UnmanagedArray.
        /// </summary>
        /// <param name="size">Size of the new array</param>
        public UnmanagedArray (int size)
        {
            ArrayPointer = Unmanaged.AllocMemory<T> (size);
            Length = size;

            for (int i = 0; i < Length; i++)
            {
                ArrayPointer[i] = default;
            }
        }

        /// <summary>
        /// Allocate a new UnmanagedArray.
        /// </summary>
        /// <param name="size">Size of the new array</param>
        /// <param name="fillWithDefaultValues">"Zero-out" the array with the default value of T. The array can otherwise contain garbage data.</param>
        public UnmanagedArray (int size, bool fillWithDefaultValues)
        {
            ArrayPointer = Unmanaged.AllocMemory<T> (size);
            Length = size;

            if (fillWithDefaultValues)
            {
                for (int i = 0; i < Length; i++)
                {
                    ArrayPointer[i] = default;
                }
            }
        }

        /// <summary>
        /// Allocate a new UnmanagedArray with the size of span. The contents of span are copied to the new UnmanagedArray.
        /// </summary>
        /// <param name="span">Span to copy from</param>
        public UnmanagedArray (Span<T> span)
        {
            Length = span.Length;
            ArrayPointer = Unmanaged.AllocMemory<T> (Length);
            //Copy to new array
            span.CopyTo (GetSpan ());
        }

        private void CheckIfAllocated ()
        {
            if (!Allocated)
                throw new ObjectDisposedException (nameof (UnmanagedArray<T>));
        }

        /// <summary>
        /// Tries to find the index of item.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>Index of item if it was found, otherwise -1.</returns>
        public int IndexOf (T item)
        {
            CheckIfAllocated ();

            for (int i = 0; i < Length; i++)
            {
                if (Compare (item, ArrayPointer[i]))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Fills the array with the default value of T.
        /// </summary>
        public void Clear ()
        {
            CheckIfAllocated ();

            for (int i = 0; i < Length; i++)
            {
                ArrayPointer[i] = default;
            }
        }

        public bool Contains (T item)
        {
            CheckIfAllocated ();

            for (int i = 0; i < Length; i++)
            {
                if (Compare (item, ArrayPointer[i]))
                    return true;
            }

            return false;
        }

        public void CopyTo (T[] destination, int destinationIndex)
        {
            CheckIfAllocated ();

            if (destination == null)
                throw new ArgumentNullException (nameof (destination));

            if (destinationIndex < 0 || destinationIndex >= destination.Length)
                throw new ArgumentOutOfRangeException (nameof (destinationIndex));

            if (destinationIndex + Length > destination.Length)
                throw new ArgumentException ("Not enough space in destination array");

            for (int i = 0; i < Length; i++)
            {
                destination[destinationIndex + i] = ArrayPointer[i];
            }
        }

        public void CopyTo (UnmanagedArray<T> destination, int destinationIndex)
        {
            CheckIfAllocated ();

            if (destination == null)
                throw new ArgumentNullException (nameof (destination));

            if (destinationIndex < 0 || destinationIndex >= destination.Length)
                throw new ArgumentOutOfRangeException (nameof (destinationIndex));

            if (destinationIndex + Length > destination.Length)
                throw new ArgumentException ("Not enough space in destination array");

            int lengthInBytes = Length * sizeof (T);
            Buffer.MemoryCopy (ArrayPointer, destination.ArrayPointer, destination.Length, lengthInBytes);
        }

        public void CopyTo (Span<T> destination)
        {
            CheckIfAllocated ();

            GetSpan ().CopyTo (destination);
        }

        /// <summary>
        /// Get a Span that points to this array
        /// </summary>
        public Span<T> GetSpan () => new Span<T> (ArrayPointer, Length);

        public IEnumerator<T> GetEnumerator ()
        {
            CheckIfAllocated ();

            return new Enumerator (this);
        }

        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        private bool Compare (T x, T y)
        {
            return EqualityComparer<T>.Default.Equals (x, y);
        }

        void IList<T>.Insert (int index, T item) => throw new NotSupportedException ("Insert cannot have a meaningful implementation on a fixed-size array");

        void IList<T>.RemoveAt (int index) => throw new NotSupportedException ("RemoveAt cannot have a meaningful implementation on a fixed-size array");

        void ICollection<T>.Add (T item) => throw new NotSupportedException ("Add cannot have a meaningful implementation on a fixed-size array");

        bool ICollection<T>.Remove (T item) => throw new NotSupportedException ("Remove cannot have a meaningful implementation on a fixed-size array");

        ~UnmanagedArray () => Dispose (false);

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        /// <summary>
        /// Free the unmanaged memory. The array will be unusable afterwards
        /// </summary>
        protected virtual void Dispose (bool disposing)
        {
            if (!Allocated)
                return;

            Unmanaged.FreeMemory (ArrayPointer, Length);
                
            Allocated = false;
        }

        private struct Enumerator : IEnumerator<T>
        {
            private readonly UnmanagedArray<T> array;
            public T Current => array.ArrayPointer[position];
            object IEnumerator.Current => Current;

            private int position;
            
            public Enumerator (UnmanagedArray<T> array)
            {
                this.array = array;
                position = -1;
            }

            public bool MoveNext ()
            {
                position++;

                return position < array.Length;
            }

            public void Reset ()
            {
                position = -1;
            }

            public void Dispose () { }
        }
    }
}
