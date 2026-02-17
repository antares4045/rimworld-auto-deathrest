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
        public static List<string> CasketsList = new List<string>(["DeathrestCasket", "DeathrestBasicCoffin", "DeathrestCoffin"]);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref DeathRestThreshold, "DeathRestThreshold", 0.2f);

            Scribe_Collections.Look(ref CasketsList, "CasketsList", LookMode.Value);

            
            if (CasketsList == null) CasketsList = new List<string>();
        
        }

        public void DoWindowContents(Rect canvas)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(canvas);

            list.Label("Death Rest Threshold: " + DeathRestThreshold.ToStringPercent());
            DeathRestThreshold = list.Slider(DeathRestThreshold, 0f, 1f);

            list.Gap(10f);
            list.Label("Use Death Rest Caskets: ");
            if (list.ButtonText("Add Casket"))
            {
                CasketsList.Add("");
            }

            for (int i = 0; i < CasketsList.Count; i++)
            {
                Rect rowRect = list.GetRect(Text.LineHeight);
                Rect fieldRect = new Rect(rowRect.x, rowRect.y, rowRect.width - 30f, rowRect.height);
                Rect buttonRect = new Rect(rowRect.xMax - 25f, rowRect.y, 25f, rowRect.height);

                CasketsList[i] = Widgets.TextField(fieldRect, CasketsList[i]);

                if (Widgets.ButtonText(buttonRect, "X"))
                {
                    CasketsList.RemoveAt(i);
                    break;
                }
                list.Gap(4f);
            }

            list.Gap(20f);
            if (list.ButtonText("Reset to defaults"))
            {
                Reset();
            }

            list.End();
        }

        
        public static void Reset()
        {
            DeathRestThreshold = 0.2f;
            CasketsList = new List<string>(["DeathrestCasket", "DeathrestBasicCoffin", "DeathrestCoffin"]);
        }
    }
}
