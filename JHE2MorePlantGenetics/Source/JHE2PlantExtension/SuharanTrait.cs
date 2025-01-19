using HarmonyLib;
using PlantGenetics.Gens;
using RimWorld;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace JHE2PlantExtension
{
    public static class SuharanTrait
    {
        public static bool HasSaharanTrait(this Plant plant)
        {
            var traitExt = plant.def.GetModExtension<TraitExtension>();
            return traitExt != null && traitExt.SpecialTrait != null && traitExt.SpecialTrait.Equals(JHE2_PlantDefOf.JHE2_Saharan);
        }

        [HarmonyPatch(typeof(Plant), nameof(Plant.GrowthRateFactor_Temperature), MethodType.Getter)]
        public class GrowthRateFactor_Temperature
        {
            [HarmonyPrefix]
            public static bool Prefix(ref float __result, Plant __instance)
            {
                if (__instance.HasSaharanTrait())
                {
                    if (!GenTemperature.TryGetTemperatureForCell(__instance.Position, __instance.Map, out var cellTemp))
                    {
                        __result = 1f;
                    }
                    else
                    {
                        __result = 1f;
                        if (cellTemp < 6f)
                        {
                            __result = Mathf.InverseLerp(0f, 6f, cellTemp);
                        }
                    }
                    return false;
                }
                return true;
            }


        }
    }
}
