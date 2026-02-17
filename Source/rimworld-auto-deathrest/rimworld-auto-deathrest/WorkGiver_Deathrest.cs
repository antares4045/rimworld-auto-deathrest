using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;
using Verse.AI;

namespace SlurmpysAutoDeathrest
{
    public class WorkGiver_AutoDeathrest : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            // Only colonists will use this job giver
            return !pawn.IsColonist;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            // Make sure the given thing is a Pawn
            if (!(t is Pawn))
                return null;

            // Ensure the Biotech mod is active
            if (!ModsConfig.BiotechActive)
                return null;

            // Check if the pawn has the Deathrest need
            Need_Deathrest deathRestNeed = pawn.needs.TryGetNeed<Need_Deathrest>();
            if (deathRestNeed == null)
            {
                return null;
            }

            if (!pawn.CanDeathrest())
            {
                return null;
            }

            // If the need exceeds the threshold and the pawn isn't already deathresting, give the job
            if (deathRestNeed.CurLevelPercentage <= Settings.DeathRestThreshold)
            {
                if (pawn.CurJobDef == JobDefOf.LayDown || pawn.CurJobDef == JobDefOf.Deathrest)
                {
                    // If they are already laying down or in the process of deathresting, return null
                    return null;
                }

                // Check if the pawn is already assigned to go to the bed
                if (pawn.jobs.curDriver is JobDriver_Goto && pawn.CurJob.targetA.Thing is Building_Bed)
                {
                    return null;  // Already assigned to go to a bed
                }

                // Try to find a bed for the pawn
                Building_Bed bed = FindDeathrestCasketFor(pawn);
                if (bed != null && pawn.CanReach(bed, PathEndMode.Touch, Danger.Deadly))
                {
                    notifiedPawns.Remove(pawn);
                    /*Log.Message(pawn + " is using this bed: " + bed);
                    CompDeathrestBindable comp = bed.TryGetComp<CompDeathrestBindable>();
                    if (comp != null)
                    {
                        Log.Message(bed + " is deathrest bindable.");
                    }

                    pawn.ownership.ClaimDeathrestCasket(bed);  // Assign the current pawn as the sole owner*/

                    return JobMaker.MakeJob(JobDefOf.Deathrest, bed);
                }

                else if (!notifiedPawns.Contains(pawn)) // If a suitable bed wasn't found and the pawn hasn't been notified
                {
                    Messages.Message("No available Deathrest Casket for " + pawn.Name, MessageTypeDefOf.NeutralEvent, true);
                    notifiedPawns.Add(pawn);
                }
            }

            return null;
        }

        private Building_Bed FindDeathrestCasketFor(Pawn pawn)
        {
            foreach (Thing thing in pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
            {
                Building_Bed bed = thing as Building_Bed;

                if (bed == null)
                {
                    continue;
                }

                if (Settings.CasketsList.Contains(bed.def.ToString()) && !bed.Position.InSunlight(bed.Map))
                {
                    if (bed.OwnersForReading.Count > 0 && !bed.OwnersForReading.Contains(pawn))
                    {
                        continue;
                    }

                    // Check if the bed isn't reserved or can be reserved by the pawn
                    //if (pawn.CanReserve(bed, bed.SleepingSlotsCount))
                    if (pawn.CanReserve(bed, 1))
                    {
                        return bed;
                    }
                }
            }
            return null;
        }

        private HashSet<Pawn> notifiedPawns = new HashSet<Pawn>();
    }
}