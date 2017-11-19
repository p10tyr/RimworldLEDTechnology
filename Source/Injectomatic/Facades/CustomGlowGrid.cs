using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace ppumkin.LEDTechnology.Injectomatic.Facades
{

    public class _GlowerGridPropertyHelper
    {

        //private FieldInfo _initialGlowerLocs;
        //private static FieldInfo _litGlowers;

        //not really sure how to cache this

        public static HashSet<CompGlower> LitGlowers()
        {
            //if (_litGlowers == null)
            //{
            //    _litGlowers = typeof(GlowGrid).GetField("litGlowers", UniversalBindingFlags);
            //    if (_litGlowers == null)
            //    {
            //        Log.Error("Could not get property for litGlowers");
            //    }
            //}

            var _initialGlowerLocs = typeof(GlowGrid).GetField("litGlowers", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Log.Message("_initialGlowerLocs was NOT NULL and returning value");
            //return (List<IntVec3>)_initialGlowerLocs.GetValue(_initialGlowerLocs);

            var data = Find.VisibleMap.glowGrid;
            //FieldInfo[] fields = data.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //foreach (FieldInfo f in fields)
            //{
            //    Log.Message(f.Name + " = " + f.GetValue(data));
            //}


            return (HashSet<CompGlower>)_initialGlowerLocs.GetValue(data);
        }

        public static List<IntVec3> InitialGlowerLocs()
        {
            //Log.Message("Getting InitialGlowerLocs");
            //if (_initialGlowerLocs == null)
            //{
            //    Log.Message("_initialGlowerLocs is NULL");
            //    _initialGlowerLocs = typeof(GlowGrid).GetField("initialGlowerLocs", UniversalBindingFlags);

            //    Log.Message(_initialGlowerLocs.GetType().ToString());

            //    if (_initialGlowerLocs == null)
            //    {
            //        Log.Error("Could not get property for initialGlowerLocs");
            //    }
            //}

            var _initialGlowerLocs = typeof(GlowGrid).GetField("initialGlowerLocs", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Log.Message("_initialGlowerLocs was NOT NULL and returning value");
            //return (List<IntVec3>)_initialGlowerLocs.GetValue(_initialGlowerLocs);

            var data = Find.VisibleMap.glowGrid;
            //FieldInfo[] fields = data.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //foreach (FieldInfo f in fields)
            //{
            //    Log.Message(f.Name + " = " + f.GetValue(data));
            //}

            return (List<IntVec3>)_initialGlowerLocs.GetValue(data);

        }
    }


    public static class CustomGlowGrid
    {

        /// <summary>
        /// Injected method jump use only
        /// </summary>
        public static void _RecalculateAllGlow()
        {
            //Log.Message("CustomGlowGrid: RecalculateAllGlow()");

            var glowFlooder = Find.VisibleMap.glowFlooder;
            var initialGlowerLocs = _GlowerGridPropertyHelper.InitialGlowerLocs();
            //Log.Message("InitialGlowerLocs count: " + initialGlowerLocs.Count );
            var litGlowers = _GlowerGridPropertyHelper.LitGlowers();
            //Log.Message("InitialGlowerLocs count: " + litGlowers.Count);


            //Log.Message("CustomGlowGrid: Got private fields()");

            if (Current.ProgramState != ProgramState.Playing)
            {
                return;
            }
            if (initialGlowerLocs != null)
            {
                foreach (IntVec3 current in initialGlowerLocs)
                {
                    Find.VisibleMap.glowGrid.MarkGlowGridDirty(current);
                }
                initialGlowerLocs = null;
            }
            //Log.Message("CustomGlowGrid: MarkGlowGridDirty(IntVec3)");

            var ci = Find.VisibleMap.cellIndices;
            for (int i = 0; i < ci.NumGridCells; i++)
            {
                Find.VisibleMap.glowGrid.glowGrid[i] = new Color32(0, 0, 0, 0); //luckily this was public.. phew :)
            }
            //Log.Message("CustomGlowGrid: Cleared grid cells with RGB(0,0,0,0)");

            foreach (CompGlower current2 in litGlowers)
            {
                glowFlooder.AddFloodGlowFor(current2, Find.VisibleMap.glowGrid.glowGrid);
            }
            //Log.Message("CustomGlowGrid: Recalculated the original flood glower");


            //I just wanted this one freaking line in the original game and had to go to all this trouble to hack it in.
            //It took me about 20hours to figure out how to put this here.
            //It started out with me playing RimWorld and enjoying it so much that I looked into MOD's. Then I realised that they are pretty easy
            //to make using the XML documents and then later that you can use C# to make even nicer one.
            //So from opening VisualStudio and loading in the Rimworld DLL for the first time, first time looking at the code and hardly any documents, 20 man hours later
            //I finally got this in here.
            Managers.CustomGlowFloodManager.RefreshGlowFlooders();
            //Log.Message("CustomGlowGrid: Recalculated universal glowers");
        }
    }

}
