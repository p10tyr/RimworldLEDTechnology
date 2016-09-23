using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;    

namespace ppumkin.LEDTechnology
{
    public class LEDCornerStrip : Building
    {

        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            text = text + "..";
            return text;
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
