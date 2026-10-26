using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Tier_expand
{
    public static class GrowthMomentSafety
    {
        public static int ClampPassionGainCount(
            int requestedGains,
            int actualChoiceCount)
        {
            if (requestedGains <= 0)
            {
                return 0;
            }

            if (actualChoiceCount <= 0)
            {
                return 0;
            }

            return Math.Min(
                requestedGains,
                actualChoiceCount
            );
        }
    }


    // =============================================================
    // Safety patch:
    //
    // Vanilla may generate fewer actual passion options than the
    // configured passion gain count.
    //
    // Example:
    //   requested gains = 12
    //   actual eligible skills = 5
    //
    // Without this clamp the growth dialog could require selecting
    // more passions than actually exist.
    // =============================================================

    [HarmonyPatch(
        typeof(ChoiceLetter_GrowthMoment),
        "TrySetChoices"
    )]
    public static class Patch_GrowthMoment_PassionSafety
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(
            ChoiceLetter_GrowthMoment __instance)
        {
            if (__instance == null)
            {
                return;
            }

            int actualChoiceCount =
                __instance.passionChoices?.Count ?? 0;

            int originalGainCount =
                __instance.passionGainsCount;

            int safeGainCount =
                GrowthMomentSafety.ClampPassionGainCount(
                    originalGainCount,
                    actualChoiceCount
                );

            if (safeGainCount ==
                originalGainCount)
            {
                return;
            }

            __instance.passionGainsCount =
                safeGainCount;

            if (Prefs.DevMode)
            {
                Log.Warning(
                    "[Growth Tiers Plus] Growth moment passion gains " +
                    "were clamped to available choices. " +
                    "Requested = " +
                    originalGainCount +
                    ", available = " +
                    actualChoiceCount +
                    ", applied = " +
                    safeGainCount +
                    "."
                );
            }
        }
    }
}