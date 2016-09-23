using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using rawcode.ILTools;

namespace ppumkin.LEDTechnology.Injectomatic
{
    [StaticConstructorOnStartup]
    public static class InjectomaticGameStart
    {
        private const BindingFlags UniversalBindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        static InjectomaticGameStart()
        {
            //Log.Message("LED Technology - InjectomaticGameStart - OK");
            MethodInfo RimWorld_GlowGrid_RecalculateAllGlow = typeof(GlowGrid).GetMethod("RecalculateAllGlow", UniversalBindingFlags);
            MethodInfo ppumkin_GlowGrid_RecalculateAllGlow = typeof(ppumkin.LEDTechnology.Injectomatic.Facades.CustomGlowGrid).GetMethod("_RecalculateAllGlow", UniversalBindingFlags);
            Rawcode.TryInjectoFromTo(RimWorld_GlowGrid_RecalculateAllGlow, ppumkin_GlowGrid_RecalculateAllGlow);
        }
    }
}
