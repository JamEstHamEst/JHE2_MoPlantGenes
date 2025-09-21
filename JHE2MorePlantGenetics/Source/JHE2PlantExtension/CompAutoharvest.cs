using RimWorld;
using Verse;
using Verse.Sound;

namespace JHE2PlantExtension
{
    public class CompAutoharvest : ThingComp
    {
        private CompProperties_Autoharvest Props => (CompProperties_Autoharvest)props;

        public void SelfHarvest()
        {
            if (parent == null || parent.Destroyed)
            {
                Log.Message("Parent is null or destoryed, but is harvestable?");
                return;
            }
            ThingDef harvestedDef = parent.def.plant.harvestedThingDef;
            if (harvestedDef != null)
            {
                Thing thing = ThingMaker.MakeThing(harvestedDef);
                if ((int)(parent.def.plant.harvestYield * Props.harvestmultiplier) >= 0)
                {
                    thing.stackCount = (int)(parent.def.plant.harvestYield * Props.harvestmultiplier);
                }
                else
                {
                    thing.stackCount = 1;
                }
                GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
                parent.def.plant.soundHarvestFinish.PlayOneShot(parent);
                if (parent.def.plant.HarvestDestroys)
                {
                  
                    parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, ((Plant)parent).HitPoints - 1));
                    ((Plant)parent).Age = parent.def.plant.LifespanTicks;
                    ((Plant)parent).Growth = 0.01f;
                }
                else
                { 
                    ((Plant)parent).Growth = parent.def.plant.harvestAfterGrowth;
                }
            }

        }
        public override void CompTickLong()
        {
            base.CompTickLong();


            if (!parent.Spawned || parent.Destroyed || parent == null)
            {
                Log.Message("Parent is not spawned, is destroyed or null");
                return;
            }

            if (((Plant)parent).Growth >= 0.99f)
            {
                SelfHarvest();

            }


        }


    }
}
