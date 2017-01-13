using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;

namespace ppumkin.LEDTechnology.GlowFlooders
{
    public class HydroponicsFlooder : IGlowFlooder
    {
        //The start position. Basically whatever rimworld assigns to the Things.Position
        public IntVec3 Position { get; private set; }

        public Rot4 Orientation { get; private set; }
        public Color32 Color { get; private set; }
        CompPower CP { get;  set; }
        CompPowerTrader CPT { get;  set; }

        public List<GlowGridCache> ColorCellIndexCache { get; set; }


        public HydroponicsFlooder(IntVec3 position, Rot4 orientation, CompPower compPower, CompPowerTrader compPowerTrader)
        {
            Position = position;
            Orientation = orientation;
            CP = compPower;
            CPT = compPowerTrader;

            ColorCellIndexCache = new List<GlowGridCache>();

            Color = new Color32(191, 63, 191, 1);
        }


        /// <summary>
        /// Clears the grid from any colors and dumps the cache.
        /// </summary>
        public void Clear()
        {
            Color32 noColor = new Color32(0,0,0,0);
            foreach (var i in ColorCellIndexCache)
            {
                Find.VisibleMap.glowGrid.glowGrid[i.CellGridIndex] = noColor;
                //Find.MapDrawer.MapMeshDirty(thingPosition, MapMeshFlag.GroundGlow);
            }
            ColorCellIndexCache = new List<GlowGridCache>();
        }


        public void CalculateGlowFlood()
        {

            if (this.CP == null || this.CP.PowerNet == null)
                return;

            if (!CPT.PowerOn)
                return;

            //if (!LEDTools.IsGridGlowing(thisPosition))
            //    return;

            if (ColorCellIndexCache.Count > 0)
                updateGlowGrid();

            switch ((int)Orientation.AsAngle)
            {
                case 0:
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, -1).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 1).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 2).AsCellIndex(), ColorAtCellIndex = Color });
                    break;

                case 90:
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, -1, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 1, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 2, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    break;

                case 180:
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, -2).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, -1).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 1).AsCellIndex(), ColorAtCellIndex = Color });
                    break;

                case 270:
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, -2, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, -1, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    ColorCellIndexCache.Add(new GlowGridCache() { CellGridIndex = LEDTools.OffsetPosition(Position, 1, 0).AsCellIndex(), ColorAtCellIndex = Color });
                    break;

            }
            updateGlowGrid();
        }


        private void updateGlowGrid()
        {
            foreach (var i in ColorCellIndexCache)
            {
                Find.VisibleMap.glowGrid.glowGrid[i.CellGridIndex] = i.ColorAtCellIndex;
            }
        }

        public override string ToString()
        {
            return "x:" + Position.x.ToString() + " z:" + Position.z.ToString();
        }
    }
}
