using Verse;
namespace Tier_expand
{
    internal static class StageBuildIdentity
    {
        internal const string BuildId = "GTP-4S-2A1-20261023-R1";
        internal static void LogStartup()
        {
            Log.Message("[Growth Tiers Plus] BUILD_ID=" + BuildId +
                "; assembly=GrowthTiersPlus; mode=growth-only; independent-dll=True.");
        }
    }
}
