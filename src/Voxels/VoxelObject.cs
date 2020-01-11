using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public class VoxelObject
    {
        public Float3 Position { get; set; }
        public Float3 Scale { get; set; }
        public IVoxelVolume<IVoxel> Voxels { get; set; }
    }
}
