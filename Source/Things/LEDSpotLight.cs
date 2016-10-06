using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;
using ppumkin.LEDTechnology.GlowFlooders;
using ppumkin.LEDTechnology.Managers;

namespace ppumkin.LEDTechnology
{
    public class LEDSpotLight : Building
    {

        AngledGlowFlooder thisFlooder;

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            registerFlooder();
        }

        protected override void ReceiveCompSignal(string signal)
        {
            //Log.Message("AngleLight Signal:" + signal);
            if (signal == "PowerTurnedOn") {
                if (!CustomGlowFloodManager.IsGlowerRegistered(thisFlooder))
                    registerFlooder();

                thisFlooder.CalculateGlowFlood();
            }

            if (signal == "PowerTurnedOff")
                CustomGlowFloodManager.DeRegisterGlower(thisFlooder);
            
            //we need to force a lighting refresh manually here
            CustomGlowFloodManager.RefreshGlowFlooders();
        }


        private void registerFlooder()
        {
            thisFlooder = new AngledGlowFlooder(this.Position, this.Rotation, this.PowerComp, base.GetComp<CompPowerTrader>());
            CustomGlowFloodManager.RegisterFlooder(thisFlooder);
        }

    }
}
