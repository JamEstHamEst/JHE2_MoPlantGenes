using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace JHE2PlantExtension
{
    public class CompPolluteOverTimePlant : CompPolluteOverTime
    {
        private CompProperties_PolluteOverTime Props => (CompProperties_PolluteOverTime)props;


        private int TicksToPolluteCell => 60000 / Props.cellsToPollutePerDay ;

        private int ticksSinceLastPollute;
        public override void CompTickLong()
        {
            ticksSinceLastPollute += 200;
            Log.Message($"cellsToPollutePerDay:{Props.cellsToPollutePerDay}");
            Log.Message($"TicksToPolluteCell:{TicksToPolluteCell}");
            Log.Message($"IsHashIntervalTick:{parent.IsHashIntervalTick(TicksToPolluteCell)}");
            if (ticksSinceLastPollute >= TicksToPolluteCell)
            {
                Log.Message("[CompPolluteOverTimePlant] Pollute triggered!");
                Pollute();
                ticksSinceLastPollute = 0; 
            }
            else
            {
                Log.Message("Pollute not triggered.");
            }
        }

        private void Pollute()
        {
            if (!ModsConfig.BiotechActive)
            {
                return;
            }
            int num = GenRadial.NumCellsInRadius(GenRadial.MaxRadialPatternRadius - 1f);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = parent.Position + GenRadial.RadialPattern[i];
                if (!intVec.IsPolluted(parent.Map) && intVec.CanPollute(parent.Map))
                {
                    intVec.Pollute(parent.Map);
                    parent.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, parent.Map, Vector3.zero), intVec, 45);
                    break;
                }
            }

        }
    }
}
