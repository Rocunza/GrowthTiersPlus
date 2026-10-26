using Verse;

namespace Tier_expand
{
    public class TierExpandSettings : ModSettings
    {
        // -------------------------
        // Tier 9
        // -------------------------

        public bool tier9Enabled = true;

        public int tier9Points = 168;

        public int tier9PassionGainsMin = 3;
        public int tier9PassionGainsMax = 4;

        public int tier9PassionChoices = 7;
        public int tier9TraitChoices = 7;


        // -------------------------
        // Tier 10
        // -------------------------

        public bool tier10Enabled = true;

        public int tier10Points = 172;

        public int tier10PassionGainsMin = 3;
        public int tier10PassionGainsMax = 4;

        public int tier10PassionChoices = 8;
        public int tier10TraitChoices = 8;


        // Growth-only logging. Legacy persisted setting preserved.
        public bool enableDebugLogging = false;

        public override void ExposeData()
        {
            base.ExposeData();
            GrowthTierSettingsData.Expose(this);
            Scribe_Values.Look(ref enableDebugLogging, "enableDebugLogging", false);
        }
    }
}
