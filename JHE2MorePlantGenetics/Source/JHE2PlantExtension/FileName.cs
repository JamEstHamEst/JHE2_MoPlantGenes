using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using InGameWiki;

namespace JHE2PlantExtension
{
    public class AllThePlantGenes : Mod
    {
        public static AllThePlantGenes Instance { get; private set; }

        public AllThePlantGenes(ModContentPack content) : base(content)
        {
            Instance = this;
        }
    }
}
