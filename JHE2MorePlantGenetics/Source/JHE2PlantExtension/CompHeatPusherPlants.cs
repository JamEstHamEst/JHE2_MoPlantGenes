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

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }
          if (Props.heatPerSecond> 0)
            {
                stringBuilder.Append("JHE2_Tempeture_Goal".Translate()+ " " + Props.heatPushMaxTemperature.ToStringTemperature("F0"));
            //    stringBuilder.AppendLine(Props.heatPushMaxTemperature.ToStringTemperature("F0"));
            }
            else
            {
                stringBuilder.Append("JHE2_Tempeture_Goal".Translate() +" "+ Props.heatPushMinTemperature.ToStringTemperature("F0"));
            }
            return stringBuilder.ToString();
        }
    }
}
