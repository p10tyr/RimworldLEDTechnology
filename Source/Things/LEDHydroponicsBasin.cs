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
        HydroponicsFlooder hydroponicsFlooder;

        public override void SpawnSetup(Map map, bool respawn)
        {
            base.SpawnSetup(map, respawn);

            registerFlooder();
        }

        public override void TickRare()
        {
            base.TickRare();
        }

        protected override void ReceiveCompSignal(string signal)
        {
            Log.Safe("LEDHydroponicsBasin Signal:" + signal);

            if (signal == "PowerTurnedOn")
                hydroponicsFlooder.CalculateGlowFlood();

            if (signal == "PowerTurnedOff")
                hydroponicsFlooder.Clear();

            //we need to force a lighting refresh manually here
            CustomGlowFloodManager.RefreshGlowFlooders();
        }

        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            text = text + "..!!!";
            return text;
        }

        private void registerFlooder()
        {
            hydroponicsFlooder = new HydroponicsFlooder(this.Position, this.Rotation, this.PowerComp, base.GetComp<CompPowerTrader>(), this.Map);
            CustomGlowFloodManager.RegisterFlooder(hydroponicsFlooder);
        }

    }
}
