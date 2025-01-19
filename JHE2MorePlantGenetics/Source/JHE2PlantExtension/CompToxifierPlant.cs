using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using static HarmonyLib.Code;
using Verse.Sound;

namespace JHE2PlantExtension
{
    public class CompToxifierPlant : ThingComp

    {
        private CompProperties_Toxifier Props => (CompProperties_Toxifier)props;

        private IntVec3 nextPollutionCell = IntVec3.Invalid;
        private int pollutingProgressTicks;
        public bool CanPolluteNow
        {
            get
            {
                if (!parent.Spawned)
                {
                    return false;
                }
                if (!nextPollutionCell.CanPollute(parent.Map))
                {
                    UpdateNextPolluteCell();
                }
                return nextPollutionCell.CanPollute(parent.Map);
            }
        }
        private void UpdateNextPolluteCell()
        {
            if (nextPollutionCell.CanPollute(parent.Map))
            {
                return;
            }
            nextPollutionCell = IntVec3.Invalid;
            int num = GenRadial.NumCellsInRadius(Props.radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 c2 = parent.Position + GenRadial.RadialPattern[i];
                if (NextPolluteCellValidator(c2))
                {
                    nextPollutionCell = c2;
                    break;
                }
            }
            bool NextPolluteCellValidator(IntVec3 c)
            {
                if (!c.InBounds(parent.Map))
                {
                    return false;
                }
                if (!c.CanPollute(parent.Map))
                {
                    return false;
                }
                return true;
            }
        }
        public override void CompTickLong()
        {
            base.CompTickLong();
            if (CanPolluteNow)
            {
                pollutingProgressTicks++;
                if (pollutingProgressTicks >= Props.pollutionIntervalTicks)
                {
                    pollutingProgressTicks = 0;
                    PolluteNextCell();
                }
            }
        }
        private void PolluteNextCell(bool silent = false)
        {
            if (!CanPolluteNow)
            {
                return;
            }
            int num = GenRadial.NumCellsInRadius(Props.radius);
            int num2 = 0;
            for (int i = 0; i < num; i++)
            {
                IntVec3 c = parent.Position + GenRadial.RadialPattern[i];
                if (c.InBounds(parent.Map) && c.CanPollute(parent.Map))
                {
                    c.Pollute(parent.Map, silent);
                    num2++;
                    if (num2 >= Props.cellsToPollute)
                    {
                        break;
                    }
                }
            }
            if (!silent)
            {
                DoEffects();
            }
        }
        private void DoEffects()
        {
            FleckMaker.Static(parent.TrueCenter(), parent.Map, FleckDefOf.Fleck_ToxifierPollutionSource);
            SoundDefOf.Toxifier_Pollute.PlayOneShot(parent);
        }
        public override string CompInspectStringExtra()
        {
            TaggedString taggedString = base.CompInspectStringExtra();
            if (CanPolluteNow)
            {
                if (!taggedString.NullOrEmpty())
                {
                    taggedString += "\n";
                }
                taggedString += (string)("PollutingTerrainProgress".Translate() + ": " + ((Props.pollutionIntervalTicks- pollutingProgressTicks) * 2000).ToStringTicksToPeriod() + " (") + Props.cellsToPollute + " " + "TilesLower".Translate() + ")";
            }
            return taggedString.Resolve();
        }

    }
}
