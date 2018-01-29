using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ppumkin.LEDTechnology.GlowFlooders
{
    public class HydroponicsFlooder : IGlowFlooder
    {
        //The start position. Basically whatever rimworld assigns to the Things.Position
        public IntVec3 Position { get; private set; }

        public Rot4 Orientation { get; private set; }
        public Color32 Color { get; private set; }
        CompPower CP { get; set; }
        CompPowerTrader CPT { get; set; }
        public int MapUniqueId { get; set; }

        public List<GlowGridCache> ColorCellIndexCache { get; set; }


        public HydroponicsFlooder(IntVec3 position, Rot4 orientation, CompPower compPower, CompPowerTrader compPowerTrader)
        {
            Position = position;
            Orientation = orientation;
            CP = compPower;
            CPT = compPowerTrader;

            ColorCellIndexCache = new List<GlowGridCache>();
            MapUniqueId = Find.VisibleMap.uniqueID;

            Color = new Color32(191, 63, 191, 1);
        }


        /// <summary>
        /// Clears the grid from any colors and dumps the cache.
        /// </summary>
        public void Clear()
        {
            Color32 noColor = new Color32(0, 0, 0, 0);
            foreach (var i in ColorCellIndexCache)
            {
                Find.VisibleMap.glowGrid.glowGrid[i.CellGridIndex] = noColor;
                //Find.MapDrawer.MapMeshDirty(thingPosition, MapMeshFlag.GroundGlow);
            }
            ColorCellIndexCache = new List<GlowGridCache>();

            Find.VisibleMap.mapDrawer.MapMeshDirty(Position, MapMeshFlag.GroundGlow);
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
            var visibleMap = Find.VisibleMap;

            if (visibleMap.uniqueID != this.MapUniqueId)
            {
                Log.Safe($"This Thing was created on map '{this.MapUniqueId}' and you are currently on map {visibleMap.uniqueID} - Skipping rendering");

                if (ColorCellIndexCache.Any())
                {
                    Log.Safe($"Clearing out any glow cells as they do not belong on this map");
                    this.Clear();
                }

                return;
            }

            var visibleMapGlowGrid = visibleMap.glowGrid.glowGrid;

            foreach (var i in ColorCellIndexCache)
            {
                try
                {
                    visibleMapGlowGrid[i.CellGridIndex] = i.ColorAtCellIndex;
                }
                catch (Exception ex)
                {
                    Log.Safe("HydroponicsFlooder.updateGlowGrid - exception - : " + ex.Message);
                }
            }
        }

        public override string ToString()
        {
            return "x:" + Position.x.ToString() + " z:" + Position.z.ToString();
        }
    }
}
