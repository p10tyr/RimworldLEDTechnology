﻿using ppumkin.LEDTechnology.GlowFlooders;
using ppumkin.LEDTechnology.Managers;
using RimWorld;
using Verse;

namespace ppumkin.LEDTechnology
{
    public class LEDSpotLight : Building
    {
        private AngledGlowFlooder thisFlooder;

        public override void SpawnSetup(Map map, bool respawn)
        {
            base.SpawnSetup(map, respawn);
            registerFlooder();
        }

        protected override void ReceiveCompSignal(string signal)
        {
            //Log.Message("AngleLight Signal:" + signal);
            if (signal == "PowerTurnedOn")
            {
                if (!CustomGlowFloodManager.IsGlowerRegistered(thisFlooder))
                {
                    registerFlooder();
                }

                thisFlooder.CalculateGlowFlood();
            }

            if (signal == "PowerTurnedOff")
            {
                CustomGlowFloodManager.DeRegisterGlower(thisFlooder);
            }

            //we need to force a lighting refresh manually here
            CustomGlowFloodManager.RefreshGlowFlooders();
        }


        private void registerFlooder()
        {
            thisFlooder = new AngledGlowFlooder(Position, Rotation, PowerComp, GetComp<CompPowerTrader>(), Map);
            CustomGlowFloodManager.RegisterFlooder(thisFlooder);
        }
    }
}