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
    public class CompProperties_Autoharvest : CompProperties
    {
        public float harvestmultiplier = 0.65f;
        public CompProperties_Autoharvest()
        {
            compClass = typeof(CompAutoharvest);
        }
    }
}
