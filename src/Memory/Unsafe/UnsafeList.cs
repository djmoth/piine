//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace piine.Memory.Unsafe
//{
//    public unsafe ref struct UnsafeList<T> where T : unmanaged
//    {
//        private const int DEFAULT_CAPACITY = 4; //Default capacity to initialize new instances with
//        private const float TRIM_EXCESS_IGNORE_THRESHOLD = 0.9f; //TrimExcess will not do anything if the List if filled over this percentage.

//        private T* array; //Internal pointer to unmanaged memory
//        private int capacity; //Size of unmanaged memory (in sizeof (T))
//        private bool stackbased;

//        /// <summary>
//        /// Number of elements in the list
//        /// </summary>
//        public int Count { get; private set; }
//        /// <summary>
//        /// Size of the internal array. Setting this value will resize the internal array. Cannot be less than Count.
//        /// </summary>
//        public int Capacity { get => capacity; set => SetCapacity (value); }
//        /// <summary>
//        /// Set to <see langword="false"/> when the <see cref="UnsafeList{T}"/> is disposed
//        /// </summary>
//        public bool Allocated => array != null;
//        /// <summary>
//        /// Return the underlying pointer for the list. Be mindful when working with this.
//        /// </summary>
//        [CLSCompliant (false)]
//        public T* InternalPointer => array;

//        public int Size { get; }

//        public T this[int index]
//        {
//            get => array[index];
//            set => array[index] = value;
//        }

//        public void Alloc ()
//        {
//            array = Unmanaged.AllocMemory<T> (DEFAULT_CAPACITY);
//            capacity = DEFAULT_CAPACITY;
//        }

//        [CLSCompliant (false)]
//        public void Alloc (T* source, int sourceLength, int startCount)
//        {
//            array = source;
//            capacity = sourceLength;
//            Count = startCount;
//            stackbased = true;
//        }

//        public void Free ()
//        {
//            if (!stackbased)
//                Unmanaged.FreeMemory (ref array, capacity);
//            else
//                array = null;
//        }   

//        private void SetCapacity (int newCapacity)
//        {
//            if (newCapacity < Count)
//                throw new ArgumentOutOfRangeException (nameof (newCapacity), "Capacity cannot be less than Count");

//            Unmanaged.ReAllocMemory (ref array, capacity, newCapacity);

//            capacity = newCapacity;
//        }

//        //Use MemoryCopy to quickly copy elements
//        private void MoveElements (int from, int count, int to) => Buffer.MemoryCopy (array + from, array + to, capacity * sizeof (T), count * sizeof (T));

//        /// <summary>
//        /// Add a new item to the end of the list. Will double the Capacity if there is not enough space.
//        /// </summary>
//        /// <param name="item"></param>
//        public void Add (T item)
//        {
//            if (Count == capacity)
//                SetCapacity (capacity > 0 ? capacity * 2 : DEFAULT_CAPACITY);

//            array[Count] = item;
//            Count++;
//        }

//        /// <summary>
//        /// Insert a new item at a specific index. Moves the items at and above 'index' 1 index up.
//        /// </summary>
//        /// <param name="index">Index to insert the item at.</param>
//        /// <param name="item">Item to insert.</param>
//        public void Insert (int index, T item)
//        {
//            if (Count == capacity)
//                SetCapacity (capacity > 0 ? capacity * 2 : DEFAULT_CAPACITY);

//            if (index < Count) //Moving the elements can be skipped if 'index' is equal to Count
//                MoveElements (index, Count - index, index + 1); //Move the elements at & after 'index' one index up in the array

//            array[index] = item;
//            Count++;
//        }

//        /// <summary>
//        /// Removes an item from the list. Preserves Capacity.
//        /// </summary>
//        /// <param name="item">Item to remove.</param>
//        /// <returns>True if item was found and removed, false if item was not found.</returns>
//        public bool Remove (T item)
//        {
//            int index = IndexOf (item);

//            if (index == -1) //Did not find item
//                return false;

//            Count--;

//            if (index == Count) //If the last element is the one to be removed, the Count can just be decremented without moving any elements
//                return true;

//            MoveElements (index + 1, Count - index, index);

//            return true;
//        }

//        /// <summary>
//        /// Remove an element at a specific index. Preserves Capacity.
//        /// </summary>
//        /// <param name="index">Index to remove at</param>
//        public void RemoveAt (int index)
//        {
//            Count--;

//            if (index == Count) //If the last element is the one to be removed, the Count can just be decremented without moving any elements
//                return;

//            MoveElements (index + 1, Count - index, index);

//            return;
//        }

//        /// <summary>
//        /// Tries to trim unused memeory by reallocating the internal array to match Count. Will not do anything if the Count is more than 90% of Capacity.
//        /// </summary>
//        public void TrimExcess ()
//        {
//            float filledPercentage = (float)Count / capacity;

//            if (filledPercentage <= TRIM_EXCESS_IGNORE_THRESHOLD)
//                SetCapacity (Count);
//        }

//        /// <summary>
//        /// Clears the list by setting Count to 0. Preserves Capacity.
//        /// </summary>
//        public void Clear ()
//        {
//            Count = 0;
//        }

//        public bool Contains (T item)
//        {
//            for (int i = 0; i < Count; i++)
//            {
//                if (Compare (item, array[i]))
//                    return true;
//            }

//            return false;
//        }

//        /// <summary>
//        /// Tries to find the index of item.
//        /// </summary>
//        /// <param name="item">Item to find.</param>
//        /// <returns>Index of item if it was found, otherwise -1.</returns>
//        public int IndexOf (T item)
//        {
//            CheckIfAllocated ();

//            for (int i = 0; i < Count; i++)
//            {
//                if (Compare (item, array[i]))
//                    return i;
//            }

//            return -1;
//        }

//        public void CopyTo (T[] destination, int destinationIndex)
//        {
//            CheckIfAllocated ();

//            if (destination == null)
//                throw new ArgumentNullException (nameof (destination));

//            if (destinationIndex < 0 || destinationIndex >= destination.Length)
//                throw new ArgumentOutOfRangeException (nameof (destinationIndex));

//            if (destinationIndex + Count > destination.Length)
//                throw new ArgumentException ("Not enough space in destination array");

//            for (int i = 0; i < Count; i++)
//            {
//                destination[destinationIndex + i] = array[i];
//            }
//        }

//        public Enumerator GetEnumerator ()
//        {
//            CheckIfAllocated ();

//            return new Enumerator (this);
//        }

//        IEnumerator<T> IEnumerable<T>.GetEnumerator () => GetEnumerator ();

//        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

//        private bool Compare (T x, T y)
//        {
//            return EqualityComparer<T>.Default.Equals (x, y);
//        }

//        public struct Enumerator : IEnumerator<T>
//        {
//            private readonly UnmanagedList<T> list;
//            public T Current => list.array[position];
//            object IEnumerator.Current => Current;

//            private int position;

//            public Enumerator (UnmanagedList<T> list)
//            {
//                this.list = list;
//                position = -1;
//            }

//            public bool MoveNext ()
//            {
//                position++;

//                return position < list.Count;
//            }

//            public void Reset ()
//            {
//                position = -1;
//            }

//            public void Dispose () { }
//        }
//    }
//}
