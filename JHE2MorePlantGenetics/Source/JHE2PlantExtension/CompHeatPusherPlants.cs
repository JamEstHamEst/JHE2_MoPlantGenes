using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace JHE2PlantExtension
{
    public class CompHeatPusherPlants : CompHeatPusher
    {
    
        public override void CompTickLong()
        {
            base.CompTickLong();
            if (ShouldPushHeatNow)
            {
                GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, Props.heatPerSecond * 33.3333f);
            }
        }
    }
}
