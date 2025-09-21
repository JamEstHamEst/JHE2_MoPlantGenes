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
    public class CompReleaseGasJamesPlant : ThingComp
    {

        private int remainingGas;

        private bool started = true;

        [Unsaved(false)]
        private Effecter effecter;

        private const int ReleaseGasInterval = 30;

        private CompProperties_ReleaseGas Props => (CompProperties_ReleaseGas)props;

        private int TotalGas => Mathf.CeilToInt(Props.cellsToFill * 255f);

        private float GasReleasedPerTick => (float)TotalGas / Props.durationSeconds / 60f;

        public override void PostPostMake()
        {
            remainingGas = TotalGas;
        }

        public void StartRelease()
        {
            started = true;
        }


        public override void CompTickLong()
        {
            if (!started)
            {
                Log.Message("nah");
                return;
            }
            
            if (remainingGas > 0)
            {
                int num = Mathf.Min(remainingGas, Mathf.RoundToInt(GasReleasedPerTick * 30f));
                GasUtility.AddGas(parent.PositionHeld, parent.MapHeld, Props.gasType, num);
                Log.Message("released gas");
                if (remainingGas <= 0)
                {
                    Log.Message("no gas");
                }
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref remainingGas, "remainingGas", 0);
            Scribe_Values.Look(ref started, "started", defaultValue: true);
        }
    }
}
