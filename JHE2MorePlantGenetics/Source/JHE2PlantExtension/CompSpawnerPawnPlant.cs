using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;


namespace JHE2PlantExtension
{
    public class CompSpawnerPawnPlant : CompSpawnerPawn
    {
        private PawnKindDef chosenKind;

        private CompProperties_SpawnerPawn Props => (CompProperties_SpawnerPawn)props;

        private PawnKindDef RandomPawnKindDef()
        {
            float curPoints = SpawnedPawnsPoints;
            IEnumerable<PawnKindDef> source = Props.spawnablePawnKinds;
            if (Props.maxSpawnedPawnsPoints > -1f)
            {
                source = source.Where((PawnKindDef x) => curPoints + x.combatPower <= Props.maxSpawnedPawnsPoints);
            }
            if (source.TryRandomElement(out var result))
            {
                return result;
            }
            return null;
        }
        private float SpawnedPawnsPoints
        {
            get
            {
                FilterOutUnspawnedPawns();
                float num = 0f;
                for (int i = 0; i < spawnedPawns.Count; i++)
                {
                    num += spawnedPawns[i].kindDef.combatPower;
                }
                return num;
            }
        }
        public override void CompTickLong()
        {
            if (!parent.Spawned)
            {

                return;
            }
            FilterOutUnspawnedPawns();
            if (Active && Find.TickManager.TicksGame >= nextPawnSpawnTick)
            {

                if ((Props.maxSpawnedPawnsPoints < 0f || SpawnedPawnsPoints < Props.maxSpawnedPawnsPoints) && TrySpawnPawn(out var pawn) && pawn.caller != null)
                {
                    pawn.caller.DoCall();
                }
                nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(Props.pawnSpawnIntervalDays.RandomInRange * 60000f);
                Log.Message(nextPawnSpawnTick);
                Log.Message(Props.pawnSpawnIntervalDays);
              //  CalculateNextPawnSpawnTick(Props.pawnSpawnIntervalDays.RandomInRange * 30f);
            }
        }
        private void FilterOutUnspawnedPawns()
        {
            for (int num = spawnedPawns.Count - 1; num >= 0; num--)
            {
                if (!spawnedPawns[num].Spawned)
                {
                    spawnedPawns.RemoveAt(num);
                }
            }
        }
      //  private void CalculateNextPawnSpawnTick()
   //     {
      //      CalculateNextPawnSpawnTick(Props.pawnSpawnIntervalDays.RandomInRange * 30f);
     //   }

        ///  public void NewCalculateNextPawnSpawnTick(float delayTicks)
        //   {
        //       nextPawnSpawnTick = Find.TickManager.TicksGame + (int)delayTicks;
        //   }
        private bool TrySpawnPawn(out Pawn pawn)
        {
            if (!canSpawnPawns)
            {
                pawn = null;
                return false;

            }
            if (!Props.chooseSingleTypeToSpawn)
            {
                chosenKind = RandomPawnKindDef();

            }
            if (chosenKind == null)
            {
                pawn = null;

                return false;
            }
            PawnGenerationRequest request = new PawnGenerationRequest(chosenKind, Faction.OfPlayer);
            if (chosenKind.RaceProps.IsMechanoid)
            {
                request.AllowedDevelopmentalStages = DevelopmentalStage.Newborn;
            }
            else
            {
                int index = chosenKind.lifeStages.Count - 1;
                request.FixedBiologicalAge = chosenKind.race.race.lifeStageAges[index].minAge;
            }
            pawn = PawnGenerator.GeneratePawn(request);
            spawnedPawns.Add(pawn);
            GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(parent.Position, parent.Map, Props.pawnSpawnRadius), parent.Map);
            pawn.connections?.ConnectTo(parent);
            (Lord ?? CreateNewLord(parent, aggressive, Props.defendRadius, Props.lordJob)).AddPawn(pawn);
            Props.spawnSound?.PlayOneShot(parent);
            if (pawnsLeftToSpawn > 0)
            {
                pawnsLeftToSpawn--;
            }
            SendMessage();
            return true;
        }
        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.CompInspectStringExtra());
            int maxpoints = (int)Math.Floor(Props.maxSpawnedPawnsPoints / 40);
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }
            if (Math.Floor(SpawnedPawnsPoints / 40) < Math.Floor(Props.maxSpawnedPawnsPoints / 40))
            {
                stringBuilder.Append("JHE2_AutoSpawningNext".Translate((nextPawnSpawnTick - Find.TickManager.TicksGame).ToStringTicksToDays() + " " + (SpawnedPawnsPoints) / 40 + "/" + Math.Floor((Props.maxSpawnedPawnsPoints) / 40)));
            }
            else
            {
                stringBuilder.Append("JHE2_AutoSpawningCan".Translate() + " " + maxpoints + "/" + maxpoints);
            }

            return stringBuilder.ToString();
        }
    }
}
