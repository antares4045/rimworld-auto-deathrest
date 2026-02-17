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

            listingStandard.Gap(10f);
            listingStandard.Label("Use Death Rest Caskets: ");
            if (listingStandard.ButtonText("Add Casket"))
            {
                Settings.CasketsList.Add("");
            }

            for (int i = 0; i < Settings.CasketsList.Count; i++)
            {
                Rect rowRect = listingStandard.GetRect(Text.LineHeight);
                Rect fieldRect = new Rect(rowRect.x, rowRect.y, rowRect.width - 30f, rowRect.height);
                Rect buttonRect = new Rect(rowRect.xMax - 25f, rowRect.y, 25f, rowRect.height);

                Settings.CasketsList[i] = Widgets.TextField(fieldRect, Settings.CasketsList[i]);

                if (Widgets.ButtonText(buttonRect, "X"))
                {
                    Settings.CasketsList.RemoveAt(i);
                    break;
                }
                listingStandard.Gap(4f);
            }

            listingStandard.Gap(20f);
            if (listingStandard.ButtonText("Reset to defaults"))
            {
                Settings.Reset();
            }

            listingStandard.End();
        }

        public override string SettingsCategory()
        {
            return "Slurmpy's Auto-Deathrest";
        }
    }
}
