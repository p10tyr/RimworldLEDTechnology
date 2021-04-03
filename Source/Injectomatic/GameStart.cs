using System.Reflection;
using ppumkin.LEDTechnology.Injectomatic.Facades;
using rawcode.ILTools;
using Verse;

namespace ppumkin.LEDTechnology.Injectomatic
{
    [StaticConstructorOnStartup]
    public static class InjectomaticGameStart
    {
        private const BindingFlags UniversalBindingFlags =
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        static InjectomaticGameStart()
        {
            //Log.Message("LED Technology - InjectomaticGameStart - OK");
            var RimWorld_GlowGrid_RecalculateAllGlow =
                typeof(GlowGrid).GetMethod("RecalculateAllGlow", UniversalBindingFlags);
            var ppumkin_GlowGrid_RecalculateAllGlow =
                typeof(CustomGlowGrid).GetMethod("_RecalculateAllGlow", UniversalBindingFlags);
            Rawcode.TryInjectoFromTo(RimWorld_GlowGrid_RecalculateAllGlow, ppumkin_GlowGrid_RecalculateAllGlow);
        }
    }
}