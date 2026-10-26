using System;

namespace Tier_expand
{
    public sealed class GrowthTierConfig
    {
        public readonly bool Tier9Enabled;
        public readonly int Tier9Points;
        public readonly int Tier9PassionGainsMin;
        public readonly int Tier9PassionGainsMax;
        public readonly int Tier9PassionChoices;
        public readonly int Tier9TraitChoices;

        public readonly bool Tier10Enabled;
        public readonly int Tier10Points;
        public readonly int Tier10PassionGainsMin;
        public readonly int Tier10PassionGainsMax;
        public readonly int Tier10PassionChoices;
        public readonly int Tier10TraitChoices;


        private GrowthTierConfig(
            bool tier9Enabled,
            int tier9Points,
            int tier9PassionGainsMin,
            int tier9PassionGainsMax,
            int tier9PassionChoices,
            int tier9TraitChoices,

            bool tier10Enabled,
            int tier10Points,
            int tier10PassionGainsMin,
            int tier10PassionGainsMax,
            int tier10PassionChoices,
            int tier10TraitChoices)
        {
            Tier9Enabled = tier9Enabled;
            Tier9Points = tier9Points;
            Tier9PassionGainsMin = tier9PassionGainsMin;
            Tier9PassionGainsMax = tier9PassionGainsMax;
            Tier9PassionChoices = tier9PassionChoices;
            Tier9TraitChoices = tier9TraitChoices;

            Tier10Enabled = tier10Enabled;
            Tier10Points = tier10Points;
            Tier10PassionGainsMin = tier10PassionGainsMin;
            Tier10PassionGainsMax = tier10PassionGainsMax;
            Tier10PassionChoices = tier10PassionChoices;
            Tier10TraitChoices = tier10TraitChoices;
        }


        public static bool TryCreate(
            TierExpandSettings settings,
            out GrowthTierConfig config,
            out string error)
        {
            config = null;
            error = null;

            if (settings == null)
            {
                error = "Settings object is null.";
                return false;
            }


            // ---------------------------------------------------------
            // Tier dependency
            // ---------------------------------------------------------

            if (!GrowthTierValidation.IsValidTierDependency(
                    settings.tier9Enabled,
                    settings.tier10Enabled))
            {
                error =
                    "Tier 10 cannot be enabled while Tier 9 is disabled.";

                return false;
            }


            // ---------------------------------------------------------
            // Growth-point requirements
            // ---------------------------------------------------------

            if (!GrowthTierValidation.IsValidPointConfiguration(
                    settings.tier9Enabled,
                    settings.tier9Points,
                    settings.tier10Enabled,
                    settings.tier10Points))
            {
                error =
                    "Invalid growth-point configuration. " +
                    "Tier 9 must be 163-180. " +
                    "If Tier 10 is enabled, it must be greater than Tier 9 " +
                    "and no higher than 180.";

                return false;
            }


            // ---------------------------------------------------------
            // Tier 9 reward configuration
            // ---------------------------------------------------------

            if (settings.tier9Enabled)
            {
                if (!GrowthTierValidation.IsValidPassionConfiguration(
                        settings.tier9PassionGainsMin,
                        settings.tier9PassionGainsMax,
                        settings.tier9PassionChoices))
                {
                    error =
                        "Invalid Tier 9 passion configuration.";

                    return false;
                }

                if (!GrowthTierValidation.IsValidTraitChoices(
                        settings.tier9TraitChoices))
                {
                    error =
                        "Tier 9 trait choices must be between 1 and 12.";

                    return false;
                }
            }


            // ---------------------------------------------------------
            // Tier 10 reward configuration
            // ---------------------------------------------------------

            if (settings.tier10Enabled)
            {
                if (!GrowthTierValidation.IsValidPassionConfiguration(
                        settings.tier10PassionGainsMin,
                        settings.tier10PassionGainsMax,
                        settings.tier10PassionChoices))
                {
                    error =
                        "Invalid Tier 10 passion configuration.";

                    return false;
                }

                if (!GrowthTierValidation.IsValidTraitChoices(
                        settings.tier10TraitChoices))
                {
                    error =
                        "Tier 10 trait choices must be between 1 and 12.";

                    return false;
                }
            }


            config = new GrowthTierConfig(
                settings.tier9Enabled,
                settings.tier9Points,
                settings.tier9PassionGainsMin,
                settings.tier9PassionGainsMax,
                settings.tier9PassionChoices,
                settings.tier9TraitChoices,

                settings.tier10Enabled,
                settings.tier10Points,
                settings.tier10PassionGainsMin,
                settings.tier10PassionGainsMax,
                settings.tier10PassionChoices,
                settings.tier10TraitChoices
            );

            return true;
        }


        // Backward-compatible public entry point; the shared rule lives in
        // GrowthTierValidation so gameplay never depends on the test class.
        public static bool IsValidTraitChoices(int choices)
        {
            return GrowthTierValidation.IsValidTraitChoices(choices);
        }


        public int ExpectedTierArrayLength
        {
            get
            {
                if (!Tier9Enabled)
                {
                    return 9;
                }

                if (!Tier10Enabled)
                {
                    return 10;
                }

                return 11;
            }
        }
    }
}