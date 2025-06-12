using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SlurmpysAutoDeathrest
{
    internal class SlurmpysAutoDeathrestMod : Mod
    {
        private Settings settings;

        public SlurmpysAutoDeathrestMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Death Rest Threshold: " + (Settings.DeathRestThreshold * 100).ToString("0") + "%");
            Settings.DeathRestThreshold = listingStandard.Slider(Settings.DeathRestThreshold, 0.01f, 1f);
            listingStandard.End();
        }

        public override string SettingsCategory()
        {
            return "Slurmpy's Auto-Deathrest";
        }
    }
}
