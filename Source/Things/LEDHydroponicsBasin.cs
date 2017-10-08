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

        public override void SpawnSetup(Map map, bool respawn)
        {
            base.SpawnSetup(map, respawn);

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
