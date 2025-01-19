using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace JHE2PlantExtension
{
    public class CompPollutionPumpPlant : ThingComp
    {
        public CompProperties_PollutionPump Props => (CompProperties_PollutionPump)props;

        private int ticksUntilPump = 1800;

        private int currentIntervalPumps;

        
        private bool Active
        {
            get
            {
                if (!parent.Spawned)
                {
                    return false;
                }
                
                if (!GetCellToUnpollute().IsValid)
                {
                    return false;
                }
                return true;
            }
        }

        private IntVec3 GetCellToUnpollute()
        {
            int num = GenRadial.NumCellsInRadius(Props.radius);
            Map map = parent.Map;
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = parent.Position + GenRadial.RadialPattern[i];
                if (intVec.InBounds(map) && intVec.CanUnpollute(map))
                {
                    return intVec;
                }
            }
            return IntVec3.Invalid;
        }
        public override void CompTickLong()
        {
            base.CompTickLong();
          
            if (Active)
            {
                ticksUntilPump -= 60;
                if (ticksUntilPump <= 0)
                {
                    ticksUntilPump = Props.intervalTicks;
                    Pump();
                }
            }
        }
        private void Pump()
        {
            IntVec3 cellToUnpollute = GetCellToUnpollute();
            if (!cellToUnpollute.IsValid)
            {
                return;
            }
            Map map = parent.Map;
            map.pollutionGrid.SetPolluted(cellToUnpollute, isPolluted: false);
            currentIntervalPumps++;
            Props.pumpEffecterDef?.Spawn(parent.Position, map).Cleanup();
            if (Props.pumpsPerWastepack > 0 && currentIntervalPumps % Props.pumpsPerWastepack == 0)
            {
                GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Wastepack), parent.Position, parent.Map, ThingPlaceMode.Near, null, (IntVec3 p) => p.GetFirstBuilding(map)?.IsClearableFreeBuilding ?? true);
                currentIntervalPumps = 0;
            }
            MoteMaker.MakeAttachedOverlay(parent, ThingDefOf.Mote_PollutionPump, Vector3.zero);
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }
            if (Active)
            {
                stringBuilder.Append("AbsorbingPollutionNext".Translate((ticksUntilPump*30).ToStringTicksToPeriod()));
            }     
            else
            {
                stringBuilder.Append("CanAbsorbPollution".Translate());
            }
            return stringBuilder.ToString();
        }

    }
}
