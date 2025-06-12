using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace SlurmpysAutoDeathrest
{
    internal class Settings : ModSettings
    {
        public static float DeathRestThreshold = 0.2f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref DeathRestThreshold, "DeathRestThreshold", 0.2f);
        }

        public void DoWindowContents(Rect canvas)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(canvas);

            list.Label("Death Rest Threshold: " + DeathRestThreshold.ToStringPercent());
            DeathRestThreshold = list.Slider(DeathRestThreshold, 0f, 1f);

            list.End();
        }
    }
}
