using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace ppumkin.LEDTechnology
{
    public static class LEDTools
    {

        public static IntVec3 OffsetPosition(IntVec3 thingPosition, int offsetX, int offsetZ)
        {
            thingPosition.x += offsetX;
            thingPosition.z += offsetZ;

            return thingPosition;
        }


        public static IntVec3 ToOffsetPositionDirection(this IntVec3 thingPosition, int ForwardBackward, int LeftRight, Rot4 orientation)
        {
            if (ForwardBackward == 0 & LeftRight == 0)
                return thingPosition;

            if (orientation == Rot4.North)
            {
                thingPosition.x += LeftRight;
                thingPosition.z += ForwardBackward;
            }

            if (orientation == Rot4.East)
            {
                thingPosition.x += ForwardBackward;
                thingPosition.z += LeftRight;
            }

            if (orientation == Rot4.South)
            {
                thingPosition.x += (LeftRight * -1);
                thingPosition.z += (ForwardBackward * -1);
            }

            if (orientation == Rot4.West)
            {
                thingPosition.x += (ForwardBackward * -1);
                thingPosition.z += (LeftRight * -1);
            }

            return thingPosition;
        }

        /// <summary>
        /// Returns the next cell position based on the rotation of the thing, as if going away from the start in the the orientaion direction
        /// </summary>
        public static IntVec3 TranslateDirection(this IntVec3 thingPosition, Rot4 orientation, int LeftRight = 0, bool backwards = false)
        {

            if (orientation == Rot4.North)
            {
                thingPosition.x += LeftRight;
                thingPosition.z += backwards ? -1 : 1;
            }

            if (orientation == Rot4.East)
            {
                thingPosition.x += backwards ? -1 : 1;
                thingPosition.z += LeftRight;
            }

            if (orientation == Rot4.South)
            {
                thingPosition.x += LeftRight * -1; //mirror
                thingPosition.z += backwards ? 1 : -1;
            }

            if (orientation == Rot4.West)
            {
                thingPosition.x += backwards ? 1 : -1;
                thingPosition.z += LeftRight * -1; //mirror
            }

            return thingPosition;

        }

        //public static void SetGridGlow(IntVec3 thingPosition, Color32 color, int offsetX = 0, int offsetZ = 0)
        //{
        //    thingPosition.x += offsetX;
        //    thingPosition.z += offsetZ;
        //    Find.GlowGrid.glowGrid[CellIndices.CellToIndex(thingPosition)] = color;
        //    //Log.Message("Color: " + color.ToString() + " Postion: " + thingPosition);
        //    Find.MapDrawer.MapMeshDirty(thingPosition, MapMeshFlag.GroundGlow);
        //}

        public static bool IsGridCellGlowing(IntVec3 thingPosition)
        {
            var ci = Find.VisibleMap.cellIndices;
            Color32 gridColor = Find.VisibleMap.glowGrid.glowGrid[ci.CellToIndex(thingPosition)];
            //Log.Message("IsGridGlowing Color: " + gridColor.ToString() + " Postion: " + thingPosition);
            return (gridColor.r != 0 & gridColor.r != 0 & gridColor.r != 0);
        }

        public static int AsCellIndex(this IntVec3 position)
        {
            var ci = Find.VisibleMap.cellIndices;
            return ci.CellToIndex(position);
        }

        public static string ToLog(this IntVec3 postition)
        {
            return " X:" + postition.x + " Y:" + postition.y + " Z:" + postition.z;
        }
    }
}


