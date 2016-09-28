using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;    

namespace ppumkin.LEDTechnology
{
    [Obsolete()]
    public class LEDSmallStrip : Building
    {

        //This is here for compatibility only.. All new save games will use LEDCentreStrip
        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            text = text + "..!!!";
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
