namespace Tier_expand
{
    // Stage 4S-1C: Growth-owned diagnostics gate. Keeping the original
    // type name avoids unintended changes to existing startup checks.
    internal static class GrowthTiersDebug
    {
        internal static bool Enabled
        {
            get
            {
                return TierExpandMod.Settings != null &&
                    TierExpandMod.Settings.enableDebugLogging;
            }
        }
    }
}
