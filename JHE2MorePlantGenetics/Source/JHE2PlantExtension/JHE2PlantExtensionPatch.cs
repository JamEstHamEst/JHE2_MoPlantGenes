using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace JHE2PlantExtension
{
    [StaticConstructorOnStartup]
    public static class JHE2PlantExtensionPatch
    {
        static JHE2PlantExtensionPatch() //our constructor
        {
            new Harmony("JHE2PlantExtensionPatch").PatchAll();
        }
    }
}
