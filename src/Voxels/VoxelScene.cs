using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public class VoxelScene
    {
        private HashSet<VoxelObject> voxelObjects;

        public void AddObject (VoxelObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException (nameof (obj));
            if (voxelObjects.Contains (obj))
                throw new InvalidOperationException ("The VoxelScene already contains this object");

            voxelObjects.Add (obj);
        }

        public void RemoveObject (VoxelObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException (nameof (obj));
            if (!voxelObjects.Contains (obj))
                throw new InvalidOperationException ("The VoxelScene does not contain this object");

            voxelObjects.Remove (obj);
        }
    }
}
