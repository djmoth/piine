﻿using System;
using NUnit.Framework;
using piine;
using piine.Voxels;

namespace tests.Voxels
{    
    public class VoxelVolumeTests
    {
        [Test]
        public void TestIndexPositions ()
        {
            IntVector3 dimensions = new IntVector3 (4, 7, 9);
            int volume = dimensions.CalculateVolume ();

            for (int i = 0; i < volume; i++)
            {
                IntVector3 positionFromIndex = VoxelGrid<byte>.CalculatePosition (dimensions, i);
                int indexFromPosition = VoxelGrid<byte>.CalculateIndex (dimensions, positionFromIndex);

                Assert.IsTrue (indexFromPosition == i);
            }
        }

        [Test]
        public void ForeachTest ()
        {
            VoxelVol
        }
    }
}