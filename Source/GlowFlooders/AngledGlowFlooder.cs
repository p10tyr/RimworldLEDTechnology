using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace ppumkin.LEDTechnology.GlowFlooders
{
    public class AngledGlowFlooder : IGlowFlooder
    {
        public IntVec3 Position { get; private set; }

        public Rot4 Orientation { get; private set; }
        public Color32 Color { get; private set; }
        CompPower CP { get; set; }
        CompPowerTrader CPT { get; set; }

        public List<GlowGridCache> ColorCellIndexCache { get; set; }

        public int MapUniqueId { get; set; }
        //public List<FloodBlocker> FloodBlockers { get; set; }

        Thing[] innerArray;

        int targetDistance;
        int angleModulus;


        public AngledGlowFlooder(IntVec3 position, Rot4 orientation, CompPower compPower, CompPowerTrader compPowerTrader)
        {
            Position = position;
            Orientation = orientation;
            CP = compPower;
            CPT = compPowerTrader;

            ColorCellIndexCache = new List<GlowGridCache>();
            //FloodBlockers = new List<FloodBlocker>();

            //Color = new Color32(191, 63, 191, 1);
            Color = new Color32(254, 255, 179, 0);

            innerArray = Find.VisibleMap.edificeGrid.InnerArray;
            MapUniqueId = Find.VisibleMap.uniqueID;

            targetDistance = 16;
            angleModulus = 2;  //0 is 90 and the higher you go the more narrow the angle. //angle 45 is every two tiles? - actually its 90 because left side is 0->45 and then right is 45<-0
        }

        public void CalculateGlowFlood()
        {
            calculateGrid();
        }

        //public void CalculateGlowFlood(bool forceGridRefresh)
        //{
        //    CalculateGlowFlood();
        //    Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.GroundGlow);
        //}


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


        private void calculateGrid()
        {
            //Log.Message("Anlge: Calc init");

            if (this.CP == null || this.CP.PowerNet == null)
                return;
            //Log.Message("Anlge: powernet OK");

            if (!CPT.PowerOn)
                return;

            //Log.Message("Anlge: Power ON");

            //if (!LEDTools.IsGridGlowing(thisPosition))
            //    return;

            //Log.Message("Anlge: checking cache");
            if (ColorCellIndexCache.Count > 0)
                updateGlowGrid();

            //Log.Message("Anlge: Not cached - doing calc");
            //start tile
            addCellIndex(this.Position, false);
            //next tiles


            ///implement collision detector
            ///check is in bounds
            ///find a way to fix very far lengths

            bool completeBlocker = false;

            int distance = 1;
            int andgleDistanceDelta = 0;
            do
            {

                if (andgleDistanceDelta == 0) //TODO CHECK BLOCKERS HERE, DUHHH!
                    addCellIndex(Position.ToOffsetPositionDirection(distance, 0, this.Orientation), false);

                for (int angleDelta = andgleDistanceDelta * (-1); angleDelta <= andgleDistanceDelta; angleDelta++)
                {
                    //Log.Message("distance: " + distance + " xD:" + x + " zD:" + z + " mod:" + distance % 3);
                    var _pos = Position.ToOffsetPositionDirection(distance, angleDelta, this.Orientation);
                    if (_pos.InBounds(Find.VisibleMap))
                    {
                        ////Log.Message("Block?: X:" + _pos.x + " Z: " + _pos.z);
                        if (isBlocked(_pos, angleDelta))
                        {
                            completeBlocker = true;
                            //addCellIndex(_pos, true); //it gets added in isBlocked method
                            continue; //end of road buddy, neeeeext!
                        }
                        else
                        {
                            completeBlocker = false;
                            //Log.Message("Not blocking: X:" + _pos.x + " Z: " + _pos.z);
                            addCellIndex(_pos, false);
                        }

                    }
                    else
                        break; //Ge'me oughta here!
                }

                //if (completeBlocker)
                //{
                //    //Log.Message("Blocker: Complete blocker detected at: " )
                //    break; //dont bother calculating any more, thats all folks.
                //}

                if (distance % angleModulus == 0) //bigger mod casues tighter angles
                    andgleDistanceDelta++;

                distance++;

            } while (distance < targetDistance);


            updateGlowGrid();
        }

        private bool isBlocked(IntVec3 position, int angleDelta)
        {
            var ci = Find.VisibleMap.cellIndices;
            var thingBlockers = innerArray[ci.CellToIndex(position)];
            if (thingBlockers != null)
            {
                if (thingBlockers.def.blockLight)
                {
                    //block this tile
                    addCellIndex(position, true);
                    //block next forward tile to prevent further light going this way
                    addCellIndex(position.TranslateDirection(this.Orientation), true);

                    //block next left and right tiles to prevent further light going this way and help with "angle" detection
                    addCellIndex(position.TranslateDirection(this.Orientation, 1), true);
                    addCellIndex(position.TranslateDirection(this.Orientation, -1), true);
                }
                thingBlockers = null;
                //Log.Message("Blocking by Def: X: " + position.x + " Z: " + position.z);
                return true;
            }

            /////----------If def.blocked not found scan for previous blocks-------------//////


            //I pre emtifly block the cell at the building collision level so now I can find it here and carry on blocking
            //This part feels a bit iffy to me because I have to carry on calculation blockers. Not sure how flag any further
            //blockers in that particular direction to be forgotten about. possibly a shortfall in my calculation logic. Oh well..it works I suppose
            var thisCellBlocked = ColorCellIndexCache.FirstOrDefault(deltaZ => position == deltaZ.Position & deltaZ.IsBlocked);

            if (thisCellBlocked != null)
            {
                //block the next cell ahead and repeat el'kapitan!
                addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation), true);
                if (angleDelta < 0)
                {
                    //pffff.. a bit ugly i know.. but i need to compensate for wide angles over distance
                    addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation, -1), true);
                    //addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation, -2), true);
                    //addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation, -3), true);
                }
                if (angleDelta > 0)
                {
                    addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation, 1), true);
                    //addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation, 2), true);
                    //addCellIndex(thisCellBlocked.Position.TranslateDirection(this.Orientation, 3), true);
                }
            }



            return false;
        }

        public class FloodBlocker
        {
            public int Z { get; set; }
            public int X { get; set; }
            public Rot4 Direction { get; set; }

            public FloodBlocker(IntVec3 position)
            {
                this.X = position.x;
                this.Z = position.z;
            }

        }

        private void addCellIndex(IntVec3 position, bool isBlocked)
        {
            int _idx = position.AsCellIndex();

            //protect from multipoe entires of the same index. I know we can use an array but in this case we want an IEnumarable list
            if (!ColorCellIndexCache.Any(x => x.CellGridIndex == _idx))
            {
                ColorCellIndexCache.Add(new GlowGridCache()
                {
                    Position = position,
                    CellGridIndex = _idx,
                    ColorAtCellIndex = isBlocked ? new Color32(0, 0, 0, 0) : this.Color,
                    IsBlocked = isBlocked
                });
            }


            //Log.Message("ANGLE POS: " + position.ToLog());
        }

        private void updateGlowGrid()
        {
            //Log.Message("Anlge: start update glow grid using cache");

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

            //this is why I wanted a list to and not an array, saves some valuable CPU overhead
            foreach (var cell in ColorCellIndexCache.Where(x => !x.IsBlocked))
            {
                try
                {
                    visibleMapGlowGrid[cell.CellGridIndex] = cell.ColorAtCellIndex;
                }
                catch (Exception ex)
                {
                    Log.Safe("AngledFlooder.updateGlowGrid exception - Probably cache is stale so will reset it : " + ex.Message);
                }
            }

            //In this case we need to mark several positions as dirty as the internal updated works with regions only
            Find.VisibleMap.mapDrawer.MapMeshDirty(Position, MapMeshFlag.GroundGlow);
        }

        public override string ToString()
        {
            return "AngledGlower - x:" + Position.x.ToString() + " z:" + Position.z.ToString();
        }

    }
}



////if (andgleDistanceDelta == 0)
//                //    addCellIndex(LEDTools.OffsetPosition(Position, 0, distance));

//                if (andgleDistanceDelta == 0)
//                    addCellIndex(LEDTools.OffsetPositionDirection(Position, distance, 0, this.Orientation));

//                for (int angleDelta = andgleDistanceDelta * (-1); angleDelta <= andgleDistanceDelta; angleDelta++)
//                {
//                    //Log.Message("distance: " + distance + " xD:" + x + " zD:" + z + " mod:" + distance % 3);
//                    var _pos = LEDTools.OffsetPosition(Position, angleDelta, distance); //TODO: rotation should be trnsformed here


//                    if (_pos.InBounds())
//                    {
//                        //Log.Message("Block?: X:" + _pos.x + " Z: " + _pos.z);
//                        if (isBlocked(_pos, angleDelta, andgleDistanceDelta))
//                        {
//                            completeBlocker = true;
//                            continue;
//                        }

//                        completeBlocker = false;
//                        //Log.Message("Not blocking: X:" + _pos.x + " Z: " + _pos.z);
//                        addCellIndex(_pos);
//                    }
//                }

//                if (completeBlocker)
//                {
//                    //Log.Message("Blocker: Complete blocker detected at: " )
//                    break; //dont bother calculating any more, thats all folks.
//                }

//                if (distance % angleModulus == 0) //bigger mod casues tighter angles
//                    andgleDistanceDelta++;

//                distance++;

//            } while (distance < targetDistance);