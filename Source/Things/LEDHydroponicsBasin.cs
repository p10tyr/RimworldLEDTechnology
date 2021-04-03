using ppumkin.LEDTechnology.GlowFlooders;
using ppumkin.LEDTechnology.Managers;
using RimWorld;
using Verse;

//using RimWorld.SquadAI;  // RimWorld specific functions for squad brains 
namespace ppumkin.LEDTechnology
{
    public class LEDHydroponicsBasin : Building_PlantGrower
    {
        //private CompPowerTrader compPower;
        private HydroponicsFlooder hydroponicsFlooder;

        public override void SpawnSetup(Map map, bool respawn)
        {
            base.SpawnSetup(map, respawn);

            registerFlooder();
        }

        protected override void ReceiveCompSignal(string signal)
        {
            Log.Safe("LEDHydroponicsBasin Signal:" + signal);

            if (signal == "PowerTurnedOn")
            {
                hydroponicsFlooder.CalculateGlowFlood();
            }

            if (signal == "PowerTurnedOff")
            {
                hydroponicsFlooder.Clear();
            }

            //we need to force a lighting refresh manually here
            CustomGlowFloodManager.RefreshGlowFlooders();
        }

        public override string GetInspectString()
        {
            var text = base.GetInspectString();
            text = text + "..!!!";
            return text;
        }

        private void registerFlooder()
        {
            hydroponicsFlooder = new HydroponicsFlooder(Position, Rotation, PowerComp, GetComp<CompPowerTrader>(), Map);
            CustomGlowFloodManager.RegisterFlooder(hydroponicsFlooder);
        }
    }
}