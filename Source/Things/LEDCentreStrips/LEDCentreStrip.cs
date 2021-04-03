using Verse;

namespace ppumkin.LEDTechnology
{
    public class LEDCentreStrip : Building
    {
        //This is here for compatibility only.. All new games will use CentreStrip
        public override string GetInspectString()
        {
            var text = base.GetInspectString();
            return text + "..!!!";
            //if (GenPlant.GrowthSeasonNow(base.Position))
            //{
            //    text = text + "\n" + "GrowSeasonHereNow_TEST".Translate();
            //}
            //else
            //{
            //    text = text + "\n" + "CannotGrowTooCold_TEST".Translate();
            //}
            //return text;
        }
    }
}