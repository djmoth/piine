using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public class ManagedVoxelOctree<T> : VoxelOctree<T>
    {
        private Node root;

        public override int Size { get; }
        public int MaxSubdivisions { get; }
        public bool IsEmpty => root == null;

        public ManagedVoxelOctree (int size)
        {
            if (size < 2 || size % 2 != 0)
                throw new ArgumentException ("Size must be more than 0 and must be an even number", nameof (size));

            Size = size;

            MaxSubdivisions = (int)Math.Log (Size, 2);
        }

        public override void SetVoxel (Int3 position, T data)
        {
            if (IsEmpty)
                root = new Node ();

            Node parent = root;

            while (true)
            {
                int halfSize = Size / 2;

                Int3 nodePosition = new Int3 (position.x / halfSize, position.x / halfSize, position.x / halfSize);

                if (!parent.HasChildren)
                    parent.CreateChildren ();

                parent = parent.GetChild (nodePosition);
            }
        }

        public override T GetVoxel (Int3 position)
        {
                return default;

            
        }

        private class Node
        {
            public Node[] children;
            public int depth;

            public T Data { get; private set; }
            public bool HasChildren => children != null;

            public void Fill (T fillData)
            {
                children = null;
                Data = fillData;
            }

            public void CreateChildren () => children = new Node[8];

            public Node GetChild (int index) => children?[index];

            public Node GetChild (Int3 position) => children?[CalculateNodeIndex (position)];
        }
    }
}
