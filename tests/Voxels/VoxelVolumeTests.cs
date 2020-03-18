using System;
using NUnit.Framework;
using piine;
using piine.Voxels;
using piine.Memory;
using System.Runtime.InteropServices;

namespace tests.Voxels
{    
    public class VoxelVolumeTests
    {
        [Test]
        public void TestIndexPositions ()
        {
            Int3 dimensions = new Int3 (4, 7, 9);
            int volume = dimensions.CalculateVolume ();

            for (int i = 0; i < volume; i++)
            {
                Int3 positionFromIndex = VoxelGrid<byte>.CalculatePosition (dimensions, i);
                int indexFromPosition = VoxelGrid<byte>.CalculateIndex (dimensions, positionFromIndex);

                Assert.IsTrue (indexFromPosition == i);
            }
        }

        [Test]
        public void ForeachTest ()
        {
            
        }

        [Test]
        public void HashCodeTest ()
        {

        }

        [Test]
        public unsafe void TestVoxelStructSize ()
        {
            Voxel* voxels = Unmanaged.AllocMemory<Voxel> (4096);

            Assert.IsTrue ((long)(voxels + 1) - (long)voxels == 1); //Check if the difference between the first and second element is 1 byte

            Assert.Pass (((long)(voxels + 4096) - (long)voxels).ToString () + "   " + ((long)voxels).ToString () + "   " + ((long)(voxels + 4095)).ToString ());
        }

        [StructLayout (LayoutKind.Sequential, Size = 1)]
        public struct Voxel
        {
            public byte value;
        }
    }
}
