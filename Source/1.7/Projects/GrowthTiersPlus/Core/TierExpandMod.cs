using HarmonyLib;
using UnityEngine;
using Verse;

namespace Tier_expand
{
    // Same CLR Mod type and packageId as the combined mod for Growth setting continuity.
    public sealed class TierExpandMod : Mod
    {
        public static TierExpandSettings Settings { get; private set; }
        private Vector2 scrollPosition = Vector2.zero;

        public TierExpandMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<TierExpandSettings>();
            StageBuildIdentity.LogStartup();
            GrowthTierStartup.RunSelfTestsIfRequested();
            Log.Message("[Growth Tiers Plus] Settings loaded. Tier 9 = " +
                Settings.tier9Enabled + ", " + Settings.tier9Points +
                " points; Tier 10 = " + Settings.tier10Enabled +
                ", " + Settings.tier10Points + " points.");
            new Harmony("zcn.growthtiersplus").PatchAll();
            Log.Message("[Growth Tiers Plus] Growth-only Harmony patches applied!");
            GrowthTierStartup.Inject();
        }

        public override string SettingsCategory() { return "Growth Tiers Plus"; }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect view = new Rect(0f, 0f, inRect.width - 20f, 1100f);
            Widgets.BeginScrollView(inRect, ref scrollPosition, view);
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(view);
            listing.Label("GTP_RestartRequired".Translate());
            listing.GapLine();
            GrowthTierSettingsUI.Draw(listing, Settings);
            Text.Font = GameFont.Medium;
            listing.Label("GTP_Diagnostics".Translate());
            Text.Font = GameFont.Small;
            SettingsTypography.FeatureToggle(listing,
                "GTP_DebugLogging".Translate(),
                ref Settings.enableDebugLogging);
            SettingsTypography.Description(listing,
                "GTP_DebugRestart".Translate());
            listing.End();
            Widgets.EndScrollView();
            Text.Font = GameFont.Small;
            base.DoSettingsWindowContents(inRect);
        }
    }
}
