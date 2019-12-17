using System;
using System.Collections.Generic;
using System.Text;
using piine.Graphics;

namespace piine.Voxels
{
    public interface IColorVolume
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }

        Color32 GetColor (Int3 voxel);
    }
}
