using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    /*
     * The FixedCapacityList is used to wrap around an existing array or buffer using Span<T>, to use as a list. If you want a temporary list, and you know that the count will never
     * exceed a specific number, then you can for example wrap the FixedCapacityList around memory allocated with stackalloc.
     */
    public ref struct FixedCapacityList<T>
    {
        private Span<T> array;

        /// <summary>
        /// Number of elements in the list
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Size of the internal array.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Access an element in the FixedCapacityList. Performs bounds checking.
        /// </summary>
        /// <param name="index">Zero-based index of element.</param>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException (nameof (index));

                return array[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException (nameof (index));

                array[index] = value;
            }
        }

        public FixedCapacityList (Span<T> source, int startCount = 0)
        {
            if (startCount < 0 || startCount > source.Length)
                throw new ArgumentOutOfRangeException (nameof (startCount));

            array = source;
            Count = startCount;
            Capacity = source.Length;
        }

        public Span<T> GetSpan () => array;

        /// <summary>
        /// Add a new item to the end of the list. Fails if at full capacity.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="InvalidOperationException">Thrown if there is no more space in the backing storage</exception>
        public void Add (T item)
        {
            if (Count == Capacity)
                throw new InvalidOperationException ("Capacity is full");

            array[Count] = item;
            Count++;
        }

        /// <summary>
        /// Insert a new item at a specific index. Moves the items at and above 'index' 1 index up. Fails if at full capacity.
        /// </summary>
        /// <param name="index">Index to insert the item at.</param>
        /// <param name="item">Item to insert.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is no more space in the backing storage</exception>
        public void Insert (int index, T item)
        {
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException (nameof (index));

            if (Count == Capacity)
                throw new InvalidOperationException ("Capacity is full");

            if (index < Count) //Moving the elements can be skipped if 'index' is equal to Count
                MoveElements (index, Count - index, index + 1); //Move the elements at & after 'index' one index up in the array

            array[index] = item;
            Count++;
        }

        /// <summary>
        /// Removes an item from the list. Preserves Capacity.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if item was found and removed, false if item was not found.</returns>
        public bool Remove (T item)
        {
            int index = IndexOf (item);

            if (index == -1) //Did not find item
                return false;

            Count--;

            if (index == Count) //If the last element is the one to be removed, the Count can just be decremented without moving any elements
                return true;

            MoveElements (index + 1, Count - index, index);

            return true;
        }

        /// <summary>
        /// Remove an element at a specific index. Preserves Capacity.
        /// </summary>
        /// <param name="index">Index to remove at</param>
        public void RemoveAt (int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException (nameof (index));

            Count--;

            if (index == Count) //If the last element is the one to be removed, the Count can just be decremented without moving any elements
                return;

            MoveElements (index + 1, Count - index, index);

            return;
        }

        /// <summary>
        /// Clears the list by setting Count to 0. Preserves Capacity.
        /// </summary>
        public void Clear ()
        {
            Count = 0;
        }

        public bool Contains (T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Compare (item, array[i]))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to find the index of item.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>Index of item if it was found, otherwise -1.</returns>
        public int IndexOf (T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Compare (item, array[i]))
                    return i;
            }

            return -1;
        }

        public void CopyTo (T[] destination, int destinationIndex)
        {
            if (destination == null)
                throw new ArgumentNullException (nameof (destination));

            array.CopyTo (destination);
        }

        private unsafe void MoveElements (int from, int count, int to)
        {
            if (from < to)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    array[to + i] = array[from + i];
                }
            } else
            {
                for (int i = 0; i < count; i++)
                {
                    array[to + i] = array[from + i];
                }
            }
        }     

        private bool Compare (T x, T y)
        {
            return EqualityComparer<T>.Default.Equals (x, y);
        }

        public Enumerator GetEnumerator () => new Enumerator (this);

        public ref struct Enumerator
        {
            private readonly FixedCapacityList<T> list;
            public T Current => list.array[position];

            private int position;

            public Enumerator (FixedCapacityList<T> list)
            {
                this.list = list;
                position = -1;
            }

            public bool MoveNext ()
            {
                position++;

                return position < list.Count;
            }

            public void Reset ()
            {
                position = -1;
            }
        }
    }
}
