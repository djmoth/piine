using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace piine.Memory
{
    public unsafe class UnmanagedStack<T> : IEnumerable<T>, IDisposable where T : unmanaged
    {
        private const int DEFAULT_CAPACITY = 4; //Default capacity to initialize new instances with
        private const float TRIM_EXCESS_IGNORE_THRESHOLD = 0.9f; //TrimExcess will not do anything if the List if filled over this percentage.

        private T* array; //Internal pointer to unmanaged memory
        private int capacity; //Size of array (in sizeof (T))

#if TRACK_ALLOC
        private StackTrace allocationPoint;
#endif

        /// <summary>
        /// Number of elements in the stack
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Set to <see langword="false"/> when the <see cref="UnmanagedStack{T}"/> is disposed
        /// </summary>
        public bool Allocated => array != null;

        public UnmanagedStack () => Construct (DEFAULT_CAPACITY);

        public UnmanagedStack (int capacity) => Construct (capacity);

        private void Construct (int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException (nameof (capacity));

            this.capacity = capacity;

            array = Unmanaged.AllocMemory<T> (capacity);

#if TRACK_ALLOC
            allocationPoint = new StackTrace ();
#endif
        }

        private void CheckIfAllocated ()
        {
            if (!Allocated)
                throw new ObjectDisposedException (nameof (UnmanagedList<T>));
        }

        private void SetCapacity (int newCapacity)
        {
            CheckIfAllocated ();

            if (newCapacity < Count)
                throw new ArgumentOutOfRangeException (nameof (newCapacity), "Capacity cannot be less than Count");

            Unmanaged.ReAllocMemory (ref array, capacity, newCapacity);

            capacity = newCapacity;
        }

        /// <summary>
        /// Get the underlying pointer. Be mindful when working with this.
        /// </summary>
        [CLSCompliant (false)]
        public T* GetInternalPointer ()
        {
            CheckIfAllocated ();

            return array;
        }

        /// <summary>
        /// Pushes a new element onto the stack. Will double the Capacity if there is not enough space.
        /// </summary>
        /// <param name="item"></param>
        public void Push (T item)
        {
            CheckIfAllocated ();

            if (Count == capacity)
                SetCapacity (capacity > 0 ? capacity * 2 : DEFAULT_CAPACITY);

            array[Count] = item;
            Count++;
        }

        /// <summary>
        /// Pops an element off the stack. Preserves Capacity.
        /// </summary>
        public T Pop ()
        {
            CheckIfAllocated ();

            if (Count == 0)
                throw new InvalidOperationException ("Stack is empty");

            Count--;

            return array[Count];
        }

        /// <summary>
        /// Gets a pointer to the element on the top of the stack.
        /// </summary>
        /// <returns>Pointer to the element on top of the stack. Null if stack is empty.</returns>
        [CLSCompliant (false)]
        public T* Peek ()
        {
            CheckIfAllocated ();

            if (Count == 0)
                throw new InvalidOperationException ("Stack is empty");

            return array + (Count - 1);
        }

        /// <summary>
        /// Tries to trim unused memory by reallocating the internal array to match Count. Will not do anything if the Count is more than 90% of Capacity.
        /// </summary>
        public void TrimExcess ()
        {
            CheckIfAllocated ();

            float filledPercentage = (float)Count / capacity;

            if (filledPercentage <= TRIM_EXCESS_IGNORE_THRESHOLD)
                SetCapacity (Count);
        }

        /// <summary>
        /// Clears the list by setting Count to 0. Preserves Capacity.
        /// </summary>
        public void Clear ()
        {
            CheckIfAllocated ();

            Count = 0;
        }

        public void CopyTo (Span<T> destination, int destinationIndex)
        {
            CheckIfAllocated ();

            if (destination == null)
                throw new ArgumentNullException (nameof (destination));

            if (destinationIndex < 0 || destinationIndex >= destination.Length)
                throw new ArgumentOutOfRangeException (nameof (destinationIndex));

            if (destinationIndex + Count > destination.Length)
                throw new ArgumentException ("Not enough space in destination array");

            for (int i = 0; i < Count; i++)
            {
                destination[destinationIndex + i] = array[i];
            }
        }

        public Enumerator GetEnumerator ()
        {
            CheckIfAllocated ();

            return new Enumerator (this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator () => GetEnumerator ();

        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

        private bool Compare (T x, T y)
        {
            return EqualityComparer<T>.Default.Equals (x, y);
        }

        ~UnmanagedStack () => Dispose (false);

        /// <summary>
        /// Free the unmanaged memory. The list will not be usable afterwards
        /// </summary>
        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool fromDispose)
        {
            if (!Allocated)
                return;

            Unmanaged.FreeMemory (ref array, capacity);

            if (!fromDispose)
            {
#if TRACK_ALLOC
                Trace.WriteLine (nameof (UnmanagedArray<T>) + " was disposed from Finalizer. You may have forgotten to call Dispose. The object was allocated here: " + allocationPoint.ToString ());
#else
                Trace.WriteLine (nameof (UnmanagedArray<T>) + " was disposed from Finalizer. You may have forgotten to call Dispose. Compile with symbol TRACK_ALLOC to keep track of where in your code unmanaged memory is allocated.");
#endif
            }
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly UnmanagedStack<T> stack;
            public T Current => stack.array[position];
            object IEnumerator.Current => Current;

            private int position;

            public Enumerator (UnmanagedStack<T> stack)
            {
                this.stack = stack;
                position = stack.Count;
            }

            public bool MoveNext ()
            {
                position--;

                return position >= 0;
            }

            public void Reset ()
            {
                position = stack.Count;
            }

            public void Dispose () { }
        }
    }
}
