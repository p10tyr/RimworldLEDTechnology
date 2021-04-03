using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ppumkin.LEDTechnology.GlowFlooders
{
    public class HydroponicsFlooder : IGlowFlooder
    {
        public HydroponicsFlooder(IntVec3 position, Rot4 orientation, CompPower compPower,
            CompPowerTrader compPowerTrader, Map map)
        {
            Position = position;
            Orientation = orientation;
            CP = compPower;
            CPT = compPowerTrader;
            Map = map;

            ColorCellIndexCache = new List<GlowGridCache>();

            Color = new Color32(191, 63, 191, 1);

            Log.Safe($"HydroponicsFlooder belongs on {Map.uniqueID} and we are on {Find.CurrentMap.uniqueID}  ");
        }

        //The start position. Basically whatever rimworld assigns to the Things.Position
        public IntVec3 Position { get; }

        public Rot4 Orientation { get; }
        public Color32 Color { get; }
        private CompPower CP { get; }
        private CompPowerTrader CPT { get; }
        public Map Map { get; set; }

        public List<GlowGridCache> ColorCellIndexCache { get; set; }

        /// <summary>
        ///     Clears the grid from any colors and dumps the cache.
        /// </summary>
        public void Clear()
        {
            Log.Safe("HydroponicsFlooder Clear");

            var noColor = new Color32(0, 0, 0, 0);

            foreach (var i in ColorCellIndexCache)
            {
                Map.glowGrid.glowGrid[i.CellGridIndex] = noColor;
                //Find.MapDrawer.MapMeshDirty(thingPosition, MapMeshFlag.GroundGlow);
            }

            ColorCellIndexCache = new List<GlowGridCache>();
        }


        public void CalculateGlowFlood()
        {
            Log.Safe("HydroponicsFlooder CalculateGlowFlood start");

            if (CP?.PowerNet == null)
            {
                return;
            }

            if (!CPT.PowerOn)
            {
                return;
            }

            //if (!LEDTools.IsGridGlowing(thisPosition))
            //    return;

            Log.Safe("HydroponicsFlooder CompPower OK PowerNet OK");

            if (ColorCellIndexCache.Count > 0)
            {
                updateGlowGrid();
            }

            switch ((int) Orientation.AsAngle)
            {
                case 0:
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, -1).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 1).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 2).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    break;

                case 90:
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, -1, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 1, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 2, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    break;

                case 180:
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, -2).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, -1).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 1).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    break;

                case 270:
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, -2, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, -1, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 0, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    ColorCellIndexCache.Add(new GlowGridCache
                    {
                        CellGridIndex = LEDTools.OffsetPosition(Position, 1, 0).AsCellIndex(), ColorAtCellIndex = Color
                    });
                    break;
            }

            Log.Safe("HydroponicsFlooder Calculated ColorCellIndexCache");

            updateGlowGrid();
        }


        private void updateGlowGrid()
        {
            Log.Safe("HydroponicsFlooder.updateGlowGrid using ColorCellIndexCache");

            foreach (var i in ColorCellIndexCache)
            {
                try
                {
                    Map.glowGrid.glowGrid[i.CellGridIndex] = i.ColorAtCellIndex;
                }
                catch (Exception ex)
                {
                    Log.Safe("HydroponicsFlooder.updateGlowGrid - exception - : " + ex.Message);
                }
            }

            //Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.GroundGlow);
        }

        public override string ToString()
        {
            return "x:" + Position.x + " z:" + Position.z;
        }
    }
}