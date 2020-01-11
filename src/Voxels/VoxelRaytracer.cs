using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Voxels
{
    public static class VoxelRaycaster
    {/*
        public static Raycast Step (Ray ray, float length, IVoxelVolume<IVoxel> voxels)
        {
            Float3 scaledDirection = ray.Direction * length;
            Float3 end = ray.Origin + scaledDirection;          

            Int3 iStart = (Int3)ray.Origin;
            Int3 iEnd = (Int3)end;

            scaledDirection = Float3.Absolute (scaledDirection);

            Int3 currentPosition = iStart;

            float tDirX = 1f / scaledDirection.x;
            float tDirY = 1f / scaledDirection.y;
            float tDirZ = 1f / scaledDirection.z;

            int n = 1;

            int xIncrement;
            int yIncrement;
            int zIncrement;

            float tY;
            float tX;
            float tZ;

            if (scaledDirection.x == 0)
            {
                xIncrement = 0;
                tX = tDirX;
            } else if (end.x > ray.Origin.x)
            {
                xIncrement = 1;
                n += iEnd.x - currentPosition.x;
                tX = (iStart.x + 0.5f - ray.Origin.x) * tDirX;
            } else
            {
                xIncrement = -1;
                n += currentPosition.x - iEnd.x;
                tX = (ray.Origin.x + 0.5f - iStart.x) * tDirX;
            }

            if (scaledDirection.y == 0)
            {
                yIncrement = 0;
                tY = tDirY;
            } else if (end.y > ray.Origin.y)
            {
                yIncrement = 1;
                n += iEnd.y - currentPosition.y;
                tY = (iStart.y + 0.5f - ray.Origin.y) * tDirY;
            } else
            {
                yIncrement = -1;
                n += currentPosition.y - iEnd.y;
                tY = (ray.Origin.y + 0.5f - iStart.y) * tDirY;
            }

            if (scaledDirection.z == 0)
            {
                zIncrement = 0;
                tZ = tDirZ;
            } else if (end.z > ray.Origin.z)
            {
                zIncrement = 1;
                n += iEnd.z - currentPosition.z;
                tZ = (iStart.z + 0.5f - ray.Origin.z) * tDirZ;
            } else
            {
                zIncrement = -1;
                n += currentPosition.z - iEnd.z;
                tZ = (ray.Origin.z + 0.5f - iStart.z) * tDirZ;
            }

            Int3 previousPosition = currentPosition;

            while (n > 0)
            {
                if (currentPosition.y < 0)
                {
                    hit.hitBottom = true;

                    if (world.Bottom != 0 && World.world.IsVoxelInsideWorld (new Int3 (currentPosition.x, 0, currentPosition.z)))
                    {
                        hit.hitPosition = currentPosition;
                        hit.normal = (Int3)(previousPosition - currentPosition);
                        hit.voxel = world.Bottom;

                        return true;
                    }
                    return false;
                }

                Int3 chunkPos = World.VoxelToChunkPos (currentPosition);

                if ((currentChunk == null || chunkPos != currentChunk.Position))
                {
                    if (world.IsChunkInsideWorld (chunkPos))
                    {
                        currentChunk = world.GetChunk (World.VoxelToChunkPos (currentPosition));
                        chunkInt3 = currentChunk.Int3ition;
                    } else
                        currentChunk = null;
                }

                if (currentChunk != null)
                {
                    byte voxel = currentChunk.GetVoxel (currentPosition - chunkInt3);

                    if (voxel != 0)
                    {
                        hit.hitPosition = currentPosition;
                        hit.normal = (Int38)(previousPosition - currentPosition);
                        hit.voxel = voxel;

                        return true;
                    }
                }

                previousPosition = currentPosition;

                if (tX < tY && tX < tZ)
                {
                    currentPosition.x += xIncrement;
                    tX += tDirX;
                } else if (tY < tX && tY < tZ)
                {
                    currentPosition.y += yIncrement;
                    tY += tDirY;
                } else if (tZ < tY && tZ < tX)
                {
                    currentPosition.z += zIncrement;
                    tZ += tDirZ;
                }

                n--;
            }

            return false;
        }
        */
       
    }
}
