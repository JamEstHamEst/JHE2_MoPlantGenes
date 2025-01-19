using HarmonyLib;
using PlantGenetics;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;


namespace JHE2PlantExtension
{
    [HarmonyPatch]
    public static class PlantGenetics_ApplyTrait_Patch
    {

        static MethodBase TargetMethod()
        {

            return AccessTools.Method(typeof(PlantGenetics.BreedHelper), "ApplyTrait");
        }
        static void Postfix(CloneData cloneData, ThingDef clone)
        {
            if (cloneData.Trait.associatedPlantProperty != null)
            {
                switch (cloneData.Trait.associatedPlantProperty)
                {
                    case "fertilitySensitivity":
                        {
                            PlantProperties plant = clone.plant;
                            plant.fertilitySensitivity *= cloneData.Trait.statmultiplier;
                            break;
                        }
                    case "lifespanDaysPerGrowDays":
                        {
                            PlantProperties plant = clone.plant;
                            plant.lifespanDaysPerGrowDays *= cloneData.Trait.statmultiplier;
                            break;
                        }
                    case "harvestFailable":
                        {
                            PlantProperties plant = clone.plant;
                            clone.plant.harvestFailable = false;
                            break;
                        }
                    case "harvestAfterGrowth":
                        {
                            PlantProperties plant = clone.plant;
                            if (plant.harvestAfterGrowth < 0.01f)
                            {
                                plant.harvestAfterGrowth += 0.05f;
                            }
                            else
                            {
                                plant.harvestAfterGrowth *= cloneData.Trait.statmultiplier;
                            }
                            break;
                        }

                    case "fertilityMinUn":
                        {
                            PlantProperties plant = clone.plant;
                            plant.fertilityMin *= cloneData.Trait.statmultiplier;
                            break;
                        }
                    case "ravitsera":
                        {
                            PlantProperties plant = clone.plant;
                            clone.plant.harvestedThingDef = JHE2_PlantDefOf.JHE2_bigfruit;
                            plant.harvestYield *= cloneData.Trait.statmultiplier;
                            break;
                        }
                    case "Ambrosia":
                        {
                            PlantProperties plant = clone.plant;
                            clone.plant.harvestedThingDef = JHE2_PlantDefOf.Ambrosia;
                            plant.harvestYield *= cloneData.Trait.statmultiplier;
                            break;
                        }
                    case "golden":
                        {
                            PlantProperties plant = clone.plant;
                            clone.plant.harvestedThingDef = ThingDefOf.Gold;
                            plant.harvestYield *= cloneData.Trait.statmultiplier;
                            break;
                        }
                    case "growOptimalGlowCave":
                        {
                            PlantProperties plant = clone.plant;
                            clone.plant.growOptimalGlow *= cloneData.Trait.statmultiplier;
                            clone.plant.growMinGlow *= cloneData.Trait.statmultiplier;
                            clone.plant.cavePlant = true;
                            break;
                        }
                    case "straight":
                        {
                            PlantProperties plant = clone.plant;
                            if (clone.plant.sowMinSkill >= 1)
                            {
                                clone.plant.sowMinSkill /= (int)cloneData.Trait.statmultiplier;
                            }
                            break;
                        }
                    case "growOptimalGlowSun":
                        {
                            PlantProperties plant = clone.plant;
                            clone.plant.growOptimalGlow = 100f;
                            clone.plant.growMinGlow = 51f;
                            clone.plant.cavePlant = false;
                            break;
                        }
                    case "pollution":
                        {
                            PlantProperties plant = clone.plant;
                            if (clone.plant.pollution == (Pollution.PollutedOnly))
                            {
                                clone.plant.pollution = Pollution.Any;
                            }
                            else
                            {
                                clone.plant.pollution = Pollution.PollutedOnly;
                            }
                            break;
                        }

                    case "glower":
                        {
                            PlantProperties plant = clone.plant;
                            CompProperties_Glower oldcomp = clone.GetCompProperties<CompProperties_Glower>();

                            if (oldcomp == null)
                            {
                                CompProperties_Glower comp = new CompProperties_Glower();
                                comp.glowRadius = 4.9f;
                                comp.glowColor = new ColorInt(204, 255, 153, 0);
                                clone.comps.Add(comp);
                            }
                            else
                            {

                                oldcomp.glowRadius *= cloneData.Trait.statmultiplier;

                            }
                            break;
                        }
                    case "heater":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_HeatPusher oldcomp = clone.GetCompProperties<CompProperties_HeatPusher>();
                            CompProperties_HeatPusher comp = new CompProperties_HeatPusher();

                            if (oldcomp == null)
                            {
                                comp.heatPerSecond = 0.05f;
                                comp.heatPushMaxTemperature = 25f;
                                comp.compClass = typeof(CompHeatPusherPlants);
                                clone.comps.Add(comp);
                            }
                            else
                            {

                                oldcomp.heatPerSecond *= cloneData.Trait.statmultiplier; ;
                                oldcomp.heatPushMaxTemperature *= cloneData.Trait.statmultiplier;
                            }
                            break;
                        }
                    case "cooling":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_HeatPusher comp = new CompProperties_HeatPusher();
                            CompProperties_HeatPusher oldcomp = clone.GetCompProperties<CompProperties_HeatPusher>();

                            if (oldcomp == null)
                            {
                                comp.heatPerSecond = 0.05f;
                                comp.heatPushMaxTemperature = 10f;
                                comp.compClass = typeof(CompHeatPusherPlants);
                                clone.comps.Add(comp);
                            }
                            else
                            {

                                oldcomp.heatPerSecond *= cloneData.Trait.statmultiplier;
                                oldcomp.heatPushMaxTemperature *= 0.5f;
                            }
                            break;
                        }
                    case "CompProperties_SelfhealHitpoints":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_SelfhealHitpoints comp = new CompProperties_SelfhealHitpoints();
                            CompProperties_SelfhealHitpoints oldcomp = clone.GetCompProperties<CompProperties_SelfhealHitpoints>();

                            if (oldcomp == null)
                            {
                                comp.ticksPerHeal = 200;
                                clone.comps.Add(comp);
                            }
                            else
                            {

                                oldcomp.ticksPerHeal *= (int)cloneData.Trait.statmultiplier;
                            }
                            break;
                        }
                    case "CompProperties_PollutionPump":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_PollutionPump comp = new CompProperties_PollutionPump();
                            CompProperties_PollutionPump oldcomp = clone.GetCompProperties<CompProperties_PollutionPump>();

                            if (oldcomp == null)
                            {
                                comp.radius = 1.9f;
                                comp.pumpsPerWastepack = 2;
                                comp.intervalTicks = 1800;
                                comp.pumpEffecterDef = EffecterDefOf.CellPollution_Performant;
                                comp.compClass = typeof(CompPollutionPumpPlant);
                                clone.comps.Add(comp);
                            }
                            else
                            {

                                oldcomp.radius *= cloneData.Trait.statmultiplier;
                                oldcomp.pumpsPerWastepack *= (int)cloneData.Trait.statmultiplier;
                                oldcomp.intervalTicks /= (int)cloneData.Trait.statmultiplier;
                            }
                            break;
                        }
                    case "CompToxifierPlant":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_Toxifier comp = new CompProperties_Toxifier();
                            CompProperties_Toxifier oldcomp = clone.GetCompProperties<CompProperties_Toxifier>();

                            if (oldcomp == null)
                            {
                                comp.radius = 1.9f;
                                comp.pollutionIntervalTicks = 60;
                                comp.cellsToPollute = 1;
                                comp.compClass = typeof(CompToxifierPlant);
                                clone.comps.Add(comp);
                            }
                            else
                            {
                                oldcomp.radius *= cloneData.Trait.statmultiplier;
                                oldcomp.pollutionIntervalTicks /= (int)cloneData.Trait.statmultiplier;   
                            }
                            break;
                        }
                    case "Gaurenlen":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_SpawnerPawn comp = new CompProperties_SpawnerPawn();
                            CompProperties_SpawnerPawn oldcomp = clone.GetCompProperties<CompProperties_SpawnerPawn>();
                            if (oldcomp == null)
                            {
                                comp.spawnablePawnKinds = new List<PawnKindDef>
                                {JHE2_PlantDefOf.JHE2_Minidryad};
                                comp.pawnSpawnIntervalDays = new FloatRange(4f, 7f);
                                comp.lordJob = typeof(LordJob_MechanoidsDefend);
                                comp.spawnSound = JHE2_PlantDefOf.Hive_Spawn;
                                comp.maxSpawnedPawnsPoints = 45;
                                comp.shouldJoinParentLord = true;
                                comp.compClass = typeof(CompSpawnerPawnPlant);
                                clone.comps.Add(comp);
                            }
                            else
                            {
                                float oldrange = oldcomp.maxSpawnedPawnsPoints *= cloneData.Trait.statmultiplier;
                                oldcomp.maxSpawnedPawnsPoints *= cloneData.Trait.statmultiplier;
                                oldcomp.pawnSpawnIntervalDays = new FloatRange((180 / oldrange), (315 / oldrange)); ;
                            }
                            break;
                        }
                    case "CompProperties_MeditationFocus":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_MeditationFocus comp = new CompProperties_MeditationFocus();
                            CompProperties_MeditationFocus oldcomp = clone.GetCompProperties<CompProperties_MeditationFocus>();
                            if (oldcomp == null)
                            {
                                if (comp.focusTypes == null)
                                {
                                    comp.focusTypes = new List<MeditationFocusDef>();
                                }
                                comp.focusTypes.Add(MeditationFocusDefOf.Natural);
                                comp.statDef = StatDefOf.MeditationFocusStrength;
                                clone.comps.Add(comp);
                                clone.statBases.Add(new StatModifier
                                {
                                    stat = StatDefOf.MeditationFocusStrength,
                                    value = 0.001f
                                });
                            }
                            else
                            {
                                clone.SetStatBaseValue(StatDefOf.MeditationFocusStrength, clone.GetStatValueAbstract(StatDefOf.MeditationFocusStrength) * cloneData.Trait.statmultiplier);
                            }
                            break;
                        }
                    case "CompProperties_Autoharvest":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_Autoharvest comp = new CompProperties_Autoharvest();
                            CompProperties_Autoharvest oldcomp = clone.GetCompProperties<CompProperties_Autoharvest>();
                            if (oldcomp == null)
                            {
                                plant.autoHarvestable = false;
                                comp.harvestmultiplier = 0.65f;
                                clone.comps.Add(comp);

                            }
                            else
                            {
                                if (oldcomp.harvestmultiplier * cloneData.Trait.statmultiplier <= 1)
                                {
                                    oldcomp.harvestmultiplier *= cloneData.Trait.statmultiplier;
                                }
                                else
                                {
                                    oldcomp.harvestmultiplier = 1f;
                                }
                            }
                            break;
                        }
                    case "CompAutoSow":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_SpawnSubplant comp = new CompProperties_SpawnSubplant();
                            CompProperties_SpawnSubplant oldcomp = clone.GetCompProperties<CompProperties_SpawnSubplant>();
                            if (oldcomp == null)
                            {
                                comp.compClass = typeof(CompAutoSow);
                                comp.subplant = ThingDefOf.Plant_Potato;
                                comp.maxRadius = 1.9f;
                                comp.subplantSpawnDays = plant.growDays / 4;
                                comp.minGrowthForSpawn = 0.4f;
                                comp.initialGrowthRange = new FloatRange(0.1f, 0.1f);
                                comp.canSpawnOverPlayerSownPlants = false;
                                comp.plantsToNotOverwrite = new List<ThingDef>();
                                comp.plantsToNotOverwrite.Add(clone);
                                if (ModsConfig.IdeologyActive)
                                {
                                    comp.plantsToNotOverwrite.Add(ThingDefOf.Plant_PodGauranlen);
                                }
                                if (ModsConfig.RoyaltyActive)
                                {
                                    comp.plantsToNotOverwrite.Add(ThingDefOf.Plant_GrassAnima);
                                    comp.plantsToNotOverwrite.Add(ThingDefOf.Plant_TreeAnima);
                                }
                                clone.comps.Add(comp);

                            }
                            else
                            {
                                if (oldcomp.subplantSpawnDays * cloneData.Trait.statmultiplier <= plant.growDays / 4)
                                {
                                    oldcomp.subplantSpawnDays *= cloneData.Trait.statmultiplier;
                                }
                                else
                                {
                                    oldcomp.subplantSpawnDays = plant.growDays / 4; ;
                                }
                            }
                            break;
                        }
                    case "Gaseous":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_ReleaseGas comp = new CompProperties_ReleaseGas();
                            CompProperties_ReleaseGas oldcomp = clone.GetCompProperties<CompProperties_ReleaseGas>();
                            GasType gastype = GasType.BlindSmoke;
                            switch (cloneData.Trait.statmultiplier)
                            {
                                case 1:
                                    {
                                        gastype = GasType.RotStink;
                                        break;
                                    }
                                case 2:
                                    {
                                        gastype = GasType.ToxGas;
                                        break;
                                    }
                                case 3:
                                    {
                                        gastype = GasType.DeadlifeDust;
                                        break;
                                    }
                            }
                            if (oldcomp == null)
                            {
                                comp.gasType = gastype;
                                comp.cellsToFill = 3f;
                                comp.durationSeconds = 10f;
                                comp.compClass = typeof(CompReleaseGasJamesPlant);
                                clone.comps.Add(comp);
                            }
                            else
                            {
                                oldcomp.gasType = gastype;
                                oldcomp.cellsToFill *= 2f;

                            }
                            break;
                        }
                    case "CompProperties_SpawnerFilth":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_SpawnerFilth comp = new CompProperties_SpawnerFilth();
                            CompProperties_SpawnerFilth oldcomp = clone.GetCompProperties<CompProperties_SpawnerFilth>();
                            if (oldcomp == null)
                            {
                                comp.filthDef = JHE2_PlantDefOf.JHE2_Filth_plantslime;
                                comp.spawnCountOnSpawn = 4;
                                comp.spawnRadius = 1f;
                                comp.spawnMtbHours = 8f;
                                comp.compClass = typeof(CompSpawnerFilthJamesPlant);

                                clone.comps.Add(comp);
                                                           }
                            else
                            {
                                comp.spawnCountOnSpawn *= (int)cloneData.Trait.statmultiplier;
                                comp.spawnRadius *= cloneData.Trait.statmultiplier;
                                comp.spawnMtbHours *= cloneData.Trait.statmultiplier;
                               
                            }
                            break;
                        }
                    case "CompProperties_CauseHediff_AoE":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_CauseHediff_AoE comp = new CompProperties_CauseHediff_AoE();
                            CompProperties_CauseHediff_AoE oldcomp = clone.GetCompProperties<CompProperties_CauseHediff_AoE>();
                            if (oldcomp == null)
                            {
                                comp.hediff = JHE2_PlantDefOf.JHE2_blisssmell;
                                comp.range = 1.9f;
                                comp.ignoreMechs = true;
                                comp.compClass = typeof(CompCauseHediff_AoEJamesPlant);
                                clone.comps.Add(comp);
                            }
                            else
                            {
                                comp.range *= cloneData.Trait.statmultiplier;
                               

                            }
                            break;
                        }
                    case "CompProperties_GiveHediffSeverity":
                        {
                            PlantProperties plant = clone.plant;

                            CompProperties_GiveHediffSeverity comp = new CompProperties_GiveHediffSeverity();
                            CompProperties_GiveHediffSeverity oldcomp = clone.GetCompProperties<CompProperties_GiveHediffSeverity>();
                            if (oldcomp == null)
                            {
                                comp.hediff = JHE2_PlantDefOf.JHE2_sootherplant;
                                comp.range = 1.9f;
                                comp.allowMechs = false;
                                comp.severityPerSecond = 0.0015f;
                                comp.compClass = typeof(CompGiveHediffSeverityJamesPlant);
                                clone.comps.Add(comp);
                            }
                            else
                            {
                                comp.range *= cloneData.Trait.statmultiplier;
                                comp.severityPerSecond *= cloneData.Trait.statmultiplier;
                            }
                            break;
                        }
                    case "pottable":
                        {
                            PlantProperties plant = clone.plant;

                            if (!clone.plant.sowTags.Contains("Decorative"))
                            {
                                clone.plant.sowTags.Add("Decorative");
                                clone.altitudeLayer = AltitudeLayer.Item;
                            }
                            break;
                        }
                    case "Hydroponic":
                        {
                            PlantProperties plant = clone.plant;

                            if (!clone.plant.sowTags.Contains("Hydroponic"))
                            {
                                clone.plant.sowTags.Add("Hydroponic");
                            }
                            break;
                        }
                    case "neurotoxin":
                        {
                            if (clone.ingestible.outcomeDoers == null)
                            {
                                clone.ingestible.outcomeDoers = new List<IngestionOutcomeDoer>();
                            }
                            PlantProperties plant = clone.plant;

                            IngestionOutcomeDoer_GiveHediff comp = new IngestionOutcomeDoer_GiveHediff();
                            comp.hediffDef = JHE2_PlantDefOf.JHE2_Neurotoxic;
                            comp.severity = 0.01f;
                            clone.ingestible.outcomeDoers.Add(comp);


                            break;
                        }
                    case "toxic":
                        {
                            if (clone.ingestible.outcomeDoers == null)
                            {
                                clone.ingestible.outcomeDoers = new List<IngestionOutcomeDoer>();
                            }
                            PlantProperties plant = clone.plant;
                            IngestionOutcomeDoer_GiveHediff comp = new IngestionOutcomeDoer_GiveHediff();
                            comp.hediffDef = HediffDefOf.ToxicBuildup;
                            comp.severity = 0.01f;
                            clone.ingestible.outcomeDoers.Add(comp);
                            break;
                        }
                    case "pathCostEasy":
                        {
                            PlantProperties plant = clone.plant;
                            clone.pathCost /= 2;
                            break;
                        }
                    case "pathCostHard":
                        {
                            PlantProperties plant = clone.plant;
                            clone.pathCost *= 2;
                            break;
                        }
                    case "minifiedDef":
                        {
                            PlantProperties plant = clone.plant;

                            if (clone.minifiedDef != ThingDefOf.MinifiedTree)
                            {
                                (clone).minifiedDef = ThingDefOf.MinifiedTree;
                                plant.forceIsTree = true;

                            }
                            break;
                        }


                    default:
                        {
                            Log.Warning($"Unrecognized plant property: {cloneData.Trait.associatedPlantProperty}");
                            break;
                        }
                }
            }
        }
    }
}
