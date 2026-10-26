using Verse;

namespace Tier_expand
{
    // Stage 4S-1B: mechanical extraction of existing Scribe_Values calls.
    // Field names, defaults, order and normalization are unchanged.
    internal static class GrowthTierSettingsData
    {
        internal static void Expose(TierExpandSettings settings)
        {
            // Tier 9
            Scribe_Values.Look(
                ref settings.tier9Enabled,
                "tier9Enabled",
                true
            );

            Scribe_Values.Look(
                ref settings.tier9Points,
                "tier9Points",
                168
            );

            Scribe_Values.Look(
                ref settings.tier9PassionGainsMin,
                "tier9PassionGainsMin",
                3
            );

            Scribe_Values.Look(
                ref settings.tier9PassionGainsMax,
                "tier9PassionGainsMax",
                4
            );

            Scribe_Values.Look(
                ref settings.tier9PassionChoices,
                "tier9PassionChoices",
                7
            );

            Scribe_Values.Look(
                ref settings.tier9TraitChoices,
                "tier9TraitChoices",
                7
            );


            // Tier 10
            Scribe_Values.Look(
                ref settings.tier10Enabled,
                "tier10Enabled",
                true
            );

            Scribe_Values.Look(
                ref settings.tier10Points,
                "tier10Points",
                172
            );

            Scribe_Values.Look(
                ref settings.tier10PassionGainsMin,
                "tier10PassionGainsMin",
                3
            );

            Scribe_Values.Look(
                ref settings.tier10PassionGainsMax,
                "tier10PassionGainsMax",
                4
            );

            Scribe_Values.Look(
                ref settings.tier10PassionChoices,
                "tier10PassionChoices",
                8
            );

            Scribe_Values.Look(
                ref settings.tier10TraitChoices,
                "tier10TraitChoices",
                8
            );
        }
    }
}
