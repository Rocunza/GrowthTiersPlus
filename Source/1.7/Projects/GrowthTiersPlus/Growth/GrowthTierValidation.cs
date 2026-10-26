namespace Tier_expand
{
    // Pure, gameplay-owned validation rules for Growth Tier 9/10 configuration.
    // These methods intentionally retain the exact conditions previously tested
    // by GrowthTiersPlusSelfTests. No RimWorld types or Harmony hooks are needed.
    internal static class GrowthTierValidation
    {
        public static bool IsValidPointConfiguration(
            bool tier9Enabled,
            int tier9Points,
            bool tier10Enabled,
            int tier10Points)
        {
            if (!tier9Enabled)
            {
                return !tier10Enabled;
            }

            if (tier9Points < 163 || tier9Points > 180)
            {
                return false;
            }

            if (!tier10Enabled)
            {
                return true;
            }

            if (tier10Points < 164 || tier10Points > 180)
            {
                return false;
            }

            return tier10Points > tier9Points;
        }


        public static bool IsValidPassionConfiguration(
            int minimum,
            int maximum,
            int choices)
        {
            if (minimum < 1 || minimum > 12)
            {
                return false;
            }

            if (maximum < minimum || maximum > 12)
            {
                return false;
            }

            if (choices < maximum || choices > 12)
            {
                return false;
            }

            return true;
        }


        public static bool IsValidTierDependency(
            bool tier9Enabled,
            bool tier10Enabled)
        {
            return tier9Enabled || !tier10Enabled;
        }


        public static bool IsValidTraitChoices(int choices)
        {
            return choices >= 1 && choices <= 12;
        }
    }
}
