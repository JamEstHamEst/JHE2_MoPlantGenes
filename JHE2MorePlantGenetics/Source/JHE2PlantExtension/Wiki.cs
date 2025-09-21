using InGameWiki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace JHE2PlantExtension
{
    [StaticConstructorOnStartup]
    internal static class Wiki
    {
        static Wiki()
        {
            // Get a reference to your mod instance.
            Mod myMod = AllThePlantGenes.Instance;

            // Create and register a new wiki.
            var wiki = ModWiki.Create(myMod);

            // Check if wiki creation was successful. If not, exit from the method.
            if (wiki == null)
                return;

            // Change some wiki properties.
            wiki.WikiTitle = "All The Plant Genes";
        }
    }
}

