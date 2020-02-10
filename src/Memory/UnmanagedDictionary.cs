using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    public unsafe class UnmanagedDictionary<TKey, TValue> : IDisposable where TKey : unmanaged, IEquatable<TKey> where TValue : unmanaged
    {
        private const int DEFAULT_BUCKET_SIZE = 32;
        private const float RESIZE_THRESHOLD = 0.7f;

        private BucketSlot* bucket;
        private int bucketSize;
        
        public int Count { get; private set; }
        public bool Allocated { get; private set; } = true;

        public TValue this[TKey key]
        {
            get
            {
                CheckIfAllocated ();

                if (TryGetValue (key, out TValue value))
                    return value;
                else
                    throw new KeyNotFoundException ();
            }
            set
            {
                CheckIfAllocated ();

                BucketSlot* slot = GetBucketSlot (key);
                HashNode* node = slot->GetNode (key);

                if (node != null)
                    node->Value = value;
                else
                    slot->Add (key, value);
            }
        }

        public UnmanagedDictionary ()
        {
            bucket = Unmanaged.AllocMemory<BucketSlot> (DEFAULT_BUCKET_SIZE, true);
            bucketSize = DEFAULT_BUCKET_SIZE;

            for (int i = 0; i < bucketSize; i++)
            {
                bucket[i].Allocate ();
            }
        }

        private void CheckIfAllocated ()
        {
            if (!Allocated)
                throw new ObjectDisposedException (nameof (UnmanagedDictionary<TKey, TValue>));
        }

        public void Add (TKey key, TValue value)
        {
            CheckIfAllocated ();

            if (ContainsKey (key))
                throw new ArgumentException ("An element with the same key already exists", nameof (key));

            if (Count > bucketSize * RESIZE_THRESHOLD)
                ResizeBucket (bucketSize * 2);

            BucketSlot* slot = GetBucketSlot (key);

            slot->Add (key, value);
        }

        public bool TryAdd (TKey key, TValue value)
        {
            CheckIfAllocated ();

            if (ContainsKey (key))
                return false;

            if (Count > bucketSize * RESIZE_THRESHOLD)
                ResizeBucket (bucketSize * 2);

            BucketSlot* slot = GetBucketSlot (key);

            slot->Add (key, value);

            return true;
        }

        private void AddUnsafe (TKey key, TValue value) => GetBucketSlot (key)->Add (key, value); //Adds a new key without checking for duplicates or space

        public bool Remove (TKey key)
        {
            CheckIfAllocated ();

            BucketSlot* slot = GetBucketSlot (key);

            return slot->Remove (key); //BucketSlot.Remove checks if key exists
        }

        public bool ContainsKey (TKey key)
        {
            CheckIfAllocated ();

            return GetBucketSlot (key)->GetNode (key) != null;
        }

        public bool TryGetValue (TKey key, out TValue value)
        {
            CheckIfAllocated ();

            if (!ContainsKey (key))
            {
                value = default;
                return false; //Could not find key
            }

            BucketSlot* slot = GetBucketSlot (key);

            value = slot->GetValue (key);
            return true;
        }

        private BucketSlot* GetBucketSlot (TKey key)
        {
            uint hash = (uint)key.GetHashCode ();
            return bucket + (hash & (bucketSize - 1)); //Efficient modulo
        }

        private void ResizeBucket (int newSize)
        {
            BucketSlot* oldBucket = bucket;
            int oldSize = bucketSize;

            bucket = Unmanaged.AllocMemory<BucketSlot> (newSize);
            bucketSize = newSize;

            //Allocate the internal linked list for each new bucket slot
            for (int i = oldSize; i < bucketSize; i++)
            {
                bucket[i] = new BucketSlot (); //The new memory is not filled with default values
                bucket[i].Allocate ();
            }

            //Relocate the old entries to their optimal locations
            for (int i = 0; i < oldSize; i++)
            {
                BucketSlot* slot = oldBucket + i;

                if (slot->Count == 0)
                    continue;

                AddUnsafe (slot->FirstNode->Key, slot->FirstNode->Value); //We don't have to check for duplicates or if there is enough space here

                HashNode* nextNode = slot->FirstNode->Next;

                while (nextNode != null)
                {
                    AddUnsafe (nextNode->Key, nextNode->Value);

                    nextNode = nextNode->Next;
                }
            }        

            Unmanaged.FreeMemory (ref oldBucket, oldSize);
        }

        ~UnmanagedDictionary () => Dispose (false);

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        /// <summary>
        /// Free the unmanaged memory. The dictionary will not be usable afterwards
        /// </summary>
        protected virtual void Dispose (bool disposing)
        {
            if (!Allocated)
                return;

            for (int i = 0; i < bucketSize; i++)
            {
                bucket[i].Deallocate ();
            }

            Unmanaged.FreeMemory (ref bucket, bucketSize);

            Allocated = false;
        }

        private struct HashNode
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public HashNode* Previous { get; set; }
            public HashNode* Next { get; set; }

            public HashNode (TKey key, TValue value, HashNode* previous)
            {
                Key = key;
                Value = value;
                Previous = previous;
                Next = null;
            }
        }

        private struct BucketSlot
        {
            private const int DEFAULT_SLOT_SIZE = 2;

            private HashNode* nodes;           

            public int SlotSize { get; private set; }
            public HashNode* FirstNode { get; private set; }
            public int Count { get; private set; }

            public void Allocate ()
            {
                SlotSize = DEFAULT_SLOT_SIZE;

                nodes = Unmanaged.AllocMemory<HashNode> (SlotSize);
            }

            public void Deallocate ()
            {
                if (nodes != null)
                    Unmanaged.FreeMemory (ref nodes, SlotSize);
            }

            public void Add (TKey key, TValue value)
            {
                if (Count == 0)
                {
                    nodes[0] = new HashNode (key, value, null);
                    FirstNode = nodes;
                } else if (Count < SlotSize)
                {
                    HashNode* lastNode = GetLastNode ();

                    //Find empty space in nodes for new HashNode
                    for (int i = 0; i < SlotSize; i++)
                    {
                        if (nodes[i].Previous == null && (nodes + i) != FirstNode) //An empty node space is indicated by the Previous property being null & and it not being the first node. (FirstNode.Previous is always null)
                        {
                            nodes[i] = new HashNode (key, value, lastNode);
                            lastNode->Next = nodes + i;
                        }
                    }
                } else
                {
                    int oldSize = SlotSize;
                    SlotSize *= 2;

                    HashNode* oldNodesPtr = nodes;
                    Unmanaged.ReAllocMemory (ref nodes, oldSize, SlotSize);

                    if (oldNodesPtr != nodes) //ReAllocMemory might move the memory to another spot. In that case FirstNode and each HashNode.Next & Previous pointers needs to be updated
                    {
                        long memoryMoveDelta = nodes - oldNodesPtr; //The difference between the new and old node addresses

                        FirstNode += memoryMoveDelta; //Set the pointer to the new address of FirstNode

                        HashNode* node = FirstNode;

                        while (node->Next != null)
                        {
                            node->Next += memoryMoveDelta;

                            if (node->Previous != null)
                                node->Previous += memoryMoveDelta;

                            node = node->Next;
                        }
                    }

                    HashNode* lastNode = GetLastNode ();
                    nodes[oldSize] = new HashNode (key, value, lastNode); //Allocate the new node right after the old node buffer
                    lastNode->Next = nodes + oldSize;
                }

                Count++;
            }

            public bool Remove (TKey key)
            {
                if (Count == 0)
                    return false;

                HashNode* node = GetNode (key);

                if (node == null)
                    return false;

                if (node->Previous == null)//This is the first node
                {
                    if (node->Next != null)
                    {
                        node->Next->Previous = null;
                        FirstNode = node->Next;
                    }
                } else
                {
                    if (node->Next != null)
                        node->Previous->Next = node->Next; //"Skip" this node, so the previous node now points to the next node after this one
                    else
                        node->Previous->Next = null;//Remove reference from previous to this node

                    node->Previous = null; //Mark this node as empty
                }

                Count--;
                return true;          
            }

            public HashNode* GetNode (TKey key)
            {
                HashNode* node = FirstNode;

                while (true)
                {
                    if (node->Key.Equals (key)) //Found the correct key
                        return node;

                    if (node->Next != null)
                        node = node->Next;
                    else
                        return null;
                }
            }

            public TValue GetValue (TKey key) => GetNode (key)->Value;

            public HashNode* GetLastNode ()
            {
                HashNode* node = FirstNode;

                while (node->Next != null)
                    node = node->Next;

                return node;
            }         
        }
    }
}
