using Verse;

namespace ppumkin.LEDTechnology
{
    public class LEDCornerStripCorner : Building
    {
        public override string GetInspectString()
        {
            var text = base.GetInspectString();
            return text + "..";
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