using System;
using piine.Graphics;

namespace piine.Voxels
{
    public interface IVoxel
    {
        Color32 Color { get; }
        float Density { get; }
        float Specular { get; }
        Float3 Normal { get; }
        float Emission { get; }
    }
}
