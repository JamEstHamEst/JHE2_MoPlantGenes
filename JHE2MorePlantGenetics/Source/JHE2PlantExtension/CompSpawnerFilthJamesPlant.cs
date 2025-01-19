using Mono.Unix.Native;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace JHE2PlantExtension
{
    public class CompSpawnerFilthJamesPlant : ThingComp
    {
        private int nextSpawnTimestamp = -1;

        private CompProperties_SpawnerFilth Props => (CompProperties_SpawnerFilth)props;

        private bool CanSpawnFilth
        {
            get
            {
                if (Props.requiredRotStage.HasValue && parent.GetRotStage() != Props.requiredRotStage)
                {
                    return false;
                }
                return true;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref nextSpawnTimestamp, "nextSpawnTimestamp", -1);
        }   

        public override void CompTickLong()
        {
            base.CompTickRare();
            TickInterval(2000);
        }

        private void TickInterval(int interval)
        {
            if (!CanSpawnFilth)
            {
                return;
            }
            if (Props.spawnMtbHours > 0f && Rand.MTBEventOccurs(Props.spawnMtbHours, 2500f, interval))
            {
                TrySpawnFilth();
            }
            if (Props.spawnEveryDays >= 0f && Find.TickManager.TicksGame >= nextSpawnTimestamp)
            {
                if (nextSpawnTimestamp != -1)
                {
                    TrySpawnFilth();
                }
                nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(Props.spawnEveryDays * 60000f);
            }
        }

        public void TrySpawnFilth()
        {
            if (parent.Map != null && CellFinder.TryFindRandomReachableNearbyCell(parent.Position, parent.Map, Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors), (IntVec3 x) => x.Standable(parent.Map), (Region x) => true, out var result))
            {
                FilthMaker.TryMakeFilth(result, parent.Map, Props.filthDef);
            }
        }
    }
}
