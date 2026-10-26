using Verse;

namespace Tier_expand
{
    // Stage 4S-1C: Growth-owned initialization, exactly the old calls.
    // Order relative to Harmony.PatchAll and Reading installation is retained
    // in TierExpandMod until the separate DLL transition.
    internal static class GrowthTierStartup
    {
        internal static void RunSelfTestsIfRequested()
        {
            if (Prefs.DevMode && GrowthTiersDebug.Enabled)
            {
                GrowthTiersPlusSelfTests.Run();
            }
        }

        internal static void Inject()
        {
            GrowthTierInjector.Inject();
        }
    }
}
