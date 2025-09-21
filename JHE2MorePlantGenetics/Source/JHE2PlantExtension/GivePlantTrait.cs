using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudeonTK;
using PlantGenetics;
using RimWorld;
using Verse;


namespace JHE2PlantExtension
{
    public static class DevTraitGiver 
    {
        [DebugAction("JHE2tools", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static List<DebugActionNode> GivePlantTrait()
        {
            List<DebugActionNode> list = new List<DebugActionNode>();
            foreach (PlantGenetics.TraitDef item in from kd in DefDatabase<PlantGenetics.TraitDef>.AllDefs
                                         where !kd.special
                                         orderby kd.defName
                                         select kd)
            {
                PlantGenetics.TraitDef localKindDef = item;
                list.Add(new DebugActionNode(localKindDef.defName, DebugActionType.ToolMap)
                {
                    action = delegate
                    {
                        foreach (Thing plantdef in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
                        {
                            CompPlantGenetics compRottable = plantdef.TryGetComp<CompPlantGenetics>();
                            if (compRottable != null)
                            {
                                compRottable.Trait = item;
                            }
                        }
                        // IntVec3 intVec = UI.MouseCell();
                        // Plant plant = intVec.GetPlant(Find.CurrentMap);
                        // ThingDef getplantdef = plant.def;
                        // PlantProperties plantprop = getplantdef.PlantProperties
                        //CompPlantGenetics CompPlantGenetics = getplantdef.<CompPlantGenetics>();

                        //   if (plant != null && plant.def.plant != null)
                        //    {
                        //        getplantdef;
                        //    }
                    }
                }
                );
              
            }
            return list;
        }
    }
}
