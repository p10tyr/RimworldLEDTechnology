using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;
using ppumkin.LEDTechnology.Managers;
using ppumkin.LEDTechnology.GlowFlooders;

//using RimWorld.SquadAI;  // RimWorld specific functions for squad brains 
namespace ppumkin.LEDTechnology
{
    public class LEDHydroponicsBasin : Building_PlantGrower
    {

        //private CompPowerTrader compPower;

        public override void SpawnSetup(Map map)
        {
            base.SpawnSetup(map);

            registerFlooder();
        }

        public override void TickRare()
        {
            base.TickRare();
        }


        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            text = text + "..!!!";
            return text;
        }

        private void registerFlooder()
        {
            HydroponicsFlooder hf = new HydroponicsFlooder(this.Position, this.Rotation, this.PowerComp, base.GetComp<CompPowerTrader>());
            CustomGlowFloodManager.RegisterFlooder(hf);
        }

    }
}
