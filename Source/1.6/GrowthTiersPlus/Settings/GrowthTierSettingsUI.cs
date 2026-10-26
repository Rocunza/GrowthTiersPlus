using UnityEngine;
using Verse;

namespace Tier_expand
{
    // Stage 4S-1A: extracted unchanged from TierExpandMod settings window.
    // Still uses the legacy combined settings type; no save-key migration yet.
    internal static class GrowthTierSettingsUI
    {
        internal static void Draw(Listing_Standard listing, TierExpandSettings Settings)
        {
            // =========================================================
            // Growth Tier 9
            // =========================================================

            Text.Font = GameFont.Medium;
            listing.Label("GTP_Tier9".Translate());

            Text.Font = GameFont.Small;

            SettingsTypography.FeatureToggle(
                listing,
                "GTP_EnableTier9".Translate(),
                ref Settings.tier9Enabled
            );

            if (!Settings.tier9Enabled)
            {
                Settings.tier10Enabled = false;
            }

            if (Settings.tier9Enabled)
            {
                int tier9MaxPoints =
                    Settings.tier10Enabled ? 179 : 180;

                Settings.tier9Points = IntSlider(
                    listing,
                    "GTP_GrowthPointsRequired".Translate(),
                    Settings.tier9Points,
                    163,
                    tier9MaxPoints
                );

                Settings.tier9PassionGainsMin = IntSlider(
                    listing,
                    "GTP_PassionGainsMin".Translate(),
                    Settings.tier9PassionGainsMin,
                    1,
                    12
                );

                Settings.tier9PassionGainsMax = IntSlider(
                    listing,
                    "GTP_PassionGainsMax".Translate(),
                    Settings.tier9PassionGainsMax,
                    Settings.tier9PassionGainsMin,
                    12
                );

                Settings.tier9PassionChoices = IntSlider(
                    listing,
                    "GTP_PassionChoices".Translate(),
                    Settings.tier9PassionChoices,
                    Settings.tier9PassionGainsMax,
                    12
                );

                Settings.tier9TraitChoices = IntSlider(
                    listing,
                    "GTP_TraitChoices".Translate(),
                    Settings.tier9TraitChoices,
                    1,
                    12
                );
            }

            listing.GapLine();


            // =========================================================
            // Growth Tier 10
            // =========================================================

            Text.Font = GameFont.Medium;
            listing.Label("GTP_Tier10".Translate());

            Text.Font = GameFont.Small;

            if (Settings.tier9Enabled)
            {
                SettingsTypography.FeatureToggle(
                    listing,
                    "GTP_EnableTier10".Translate(),
                    ref Settings.tier10Enabled
                );
            }
            else
            {
                Settings.tier10Enabled = false;

                listing.Label(
                    "GTP_EnableTier10Requires9".Translate()
                );
            }

            if (Settings.tier10Enabled)
            {
                if (Settings.tier9Points >= 180)
                {
                    Settings.tier9Points = 179;
                }

                if (Settings.tier10Points <= Settings.tier9Points)
                {
                    Settings.tier10Points =
                        Settings.tier9Points + 1;
                }

                Settings.tier10Points = IntSlider(
                    listing,
                    "GTP_GrowthPointsRequired".Translate(),
                    Settings.tier10Points,
                    Settings.tier9Points + 1,
                    180
                );

                Settings.tier10PassionGainsMin = IntSlider(
                    listing,
                    "GTP_PassionGainsMin".Translate(),
                    Settings.tier10PassionGainsMin,
                    1,
                    12
                );

                Settings.tier10PassionGainsMax = IntSlider(
                    listing,
                    "GTP_PassionGainsMax".Translate(),
                    Settings.tier10PassionGainsMax,
                    Settings.tier10PassionGainsMin,
                    12
                );

                Settings.tier10PassionChoices = IntSlider(
                    listing,
                    "GTP_PassionChoices".Translate(),
                    Settings.tier10PassionChoices,
                    Settings.tier10PassionGainsMax,
                    12
                );

                Settings.tier10TraitChoices = IntSlider(
                    listing,
                    "GTP_TraitChoices".Translate(),
                    Settings.tier10TraitChoices,
                    1,
                    12
                );
            }

            listing.GapLine();


        }

        private static int IntSlider(
            Listing_Standard listing,
            string label,
            int value,
            int min,
            int max)
        {
            value = Mathf.Clamp(value, min, max);

            listing.Label($"{label}: {value}");

            value = Mathf.RoundToInt(
                listing.Slider(
                    value,
                    min,
                    max
                )
            );

            return Mathf.Clamp(value, min, max);
        }
    }
}
