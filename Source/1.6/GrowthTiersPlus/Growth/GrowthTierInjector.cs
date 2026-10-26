using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Tier_expand
{
    public static class GrowthTierInjector
    {
        public static void Inject()
        {
            try
            {
                // -----------------------------------------------------
                // 1. Validate user settings first.
                // -----------------------------------------------------

                GrowthTierConfig config;
                string configError;

                if (!GrowthTierConfig.TryCreate(
                        TierExpandMod.Settings,
                        out config,
                        out configError))
                {
                    Log.Error(
                        "[Growth Tiers Plus] Invalid settings. " +
                        "Growth tier injection aborted.\n" +
                        configError
                    );

                    return;
                }


                // -----------------------------------------------------
                // 2. Read vanilla GrowthTiers.
                // -----------------------------------------------------

                GrowthUtility.GrowthTier[] oldTiers =
                    GrowthUtility.GrowthTiers;

                if (oldTiers == null)
                {
                    Log.Error(
                        "[Growth Tiers Plus] GrowthTiers was null. " +
                        "Injection aborted."
                    );

                    return;
                }

                Log.Message(
                    "[Growth Tiers Plus] Original GrowthTiers length = " +
                    oldTiers.Length
                );


                // -----------------------------------------------------
                // 3. Safety guard.
                //
                // We currently support injection only when the array is
                // still in its vanilla 9-entry state.
                // -----------------------------------------------------

                if (oldTiers.Length != 9)
                {
                    Log.Warning(
                        "[Growth Tiers Plus] Expected vanilla " +
                        "GrowthTiers length 9, but found " +
                        oldTiers.Length +
                        ". Injection aborted to avoid overwriting " +
                        "another mod's growth-tier changes."
                    );

                    return;
                }


                // -----------------------------------------------------
                // 4. Both custom tiers disabled:
                // leave vanilla completely untouched.
                // -----------------------------------------------------

                if (!config.Tier9Enabled)
                {
                    Log.Message(
                        "[Growth Tiers Plus] Tier 9 and Tier 10 are " +
                        "disabled. Vanilla GrowthTiers left unchanged. " +
                        "Length = 9."
                    );

                    return;
                }


                // -----------------------------------------------------
                // 5. Build the new array.
                // -----------------------------------------------------

                GrowthUtility.GrowthTier[] newTiers =
                    new GrowthUtility.GrowthTier[
                        config.ExpectedTierArrayLength
                    ];

                Array.Copy(
                    oldTiers,
                    newTiers,
                    oldTiers.Length
                );


                // -----------------------------------------------------
                // Tier 9
                //
                // traitGains remains fixed at 1.
                // -----------------------------------------------------

                newTiers[9] =
                    new GrowthUtility.GrowthTier(
                        9,
                        config.Tier9Points,
                        new ByteRange(
                            (byte)config.Tier9PassionGainsMin,
                            (byte)config.Tier9PassionGainsMax
                        ),
                        (byte)config.Tier9PassionChoices,
                        1,
                        (byte)config.Tier9TraitChoices
                    );


                // -----------------------------------------------------
                // Tier 10
                // -----------------------------------------------------

                if (config.Tier10Enabled)
                {
                    newTiers[10] =
                        new GrowthUtility.GrowthTier(
                            10,
                            config.Tier10Points,
                            new ByteRange(
                                (byte)config.Tier10PassionGainsMin,
                                (byte)config.Tier10PassionGainsMax
                            ),
                            (byte)config.Tier10PassionChoices,
                            1,
                            (byte)config.Tier10TraitChoices
                        );
                }


                // -----------------------------------------------------
                // 6. Replace the static readonly array through Harmony.
                // -----------------------------------------------------

                ref GrowthUtility.GrowthTier[] tiersRef =
                    ref AccessTools.StaticFieldRefAccess<
                        GrowthUtility.GrowthTier[]
                    >(
                        typeof(GrowthUtility),
                        "GrowthTiers"
                    );

                tiersRef = newTiers;


                // -----------------------------------------------------
                // 7. Runtime audit.
                //
                // If something unexpected happened after assignment,
                // restore the original vanilla array immediately.
                // -----------------------------------------------------

                string auditError;

                if (!AuditInjectedTiers(
                        tiersRef,
                        oldTiers,
                        config,
                        out auditError))
                {
                    tiersRef = oldTiers;

                    Log.Error(
                        "[Growth Tiers Plus] Runtime injection audit " +
                        "FAILED. Original vanilla GrowthTiers restored.\n" +
                        auditError
                    );

                    return;
                }


                // -----------------------------------------------------
                // 8. Success log.
                // -----------------------------------------------------

                if (config.Tier10Enabled)
                {
                    Log.Message(
                        "[Growth Tiers Plus] Growth tiers injected " +
                        "successfully! " +
                        "Length = " + tiersRef.Length +
                        ", Tier 9 = " +
                        tiersRef[9].pointsRequirement +
                        ", Tier 10 = " +
                        tiersRef[10].pointsRequirement +
                        "."
                    );
                }
                else
                {
                    Log.Message(
                        "[Growth Tiers Plus] Growth tiers injected " +
                        "successfully! " +
                        "Length = " + tiersRef.Length +
                        ", Tier 9 = " +
                        tiersRef[9].pointsRequirement +
                        ", Tier 10 disabled."
                    );
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    "[Growth Tiers Plus] Failed to inject growth tiers:\n" +
                    ex
                );
            }
        }


        // =============================================================
        // Runtime audit
        // =============================================================

        private static bool AuditInjectedTiers(
    GrowthUtility.GrowthTier[] tiers,
    GrowthUtility.GrowthTier[] originalVanillaTiers,
    GrowthTierConfig config,
    out string error)
        {
            error = null;

            if (tiers == null)
            {
                error = "Injected GrowthTiers array is null.";
                return false;
            }

            if (originalVanillaTiers == null ||
                originalVanillaTiers.Length != 9)
            {
                error =
                    "Original vanilla GrowthTiers snapshot is invalid.";

                return false;
            }

            if (tiers.Length != config.ExpectedTierArrayLength)
            {
                error =
                    "Expected array length " +
                    config.ExpectedTierArrayLength +
                    ", but found " +
                    tiers.Length +
                    ".";

                return false;
            }


            // ---------------------------------------------------------
            // Verify that vanilla Tier 0-8 were preserved exactly.
            // ---------------------------------------------------------

            for (int i = 0; i < 9; i++)
            {
                if (!SameGrowthTierValues(
                        tiers[i],
                        originalVanillaTiers[i]))
                {
                    error =
                        "Vanilla growth tier " +
                        i +
                        " was unexpectedly modified.";

                    return false;
                }
            }


            // ---------------------------------------------------------
            // Tier 9
            // ---------------------------------------------------------

            if (config.Tier9Enabled)
            {
                if (tiers.Length <= 9)
                {
                    error =
                        "Tier 9 is enabled, but index 9 does not exist.";

                    return false;
                }

                if (!AuditCustomTier(
                        tiers[9],
                        9,
                        config.Tier9Points,
                        config.Tier9PassionGainsMin,
                        config.Tier9PassionGainsMax,
                        config.Tier9PassionChoices,
                        config.Tier9TraitChoices,
                        out error))
                {
                    return false;
                }
            }


            // ---------------------------------------------------------
            // Tier 10
            // ---------------------------------------------------------

            if (config.Tier10Enabled)
            {
                if (tiers.Length <= 10)
                {
                    error =
                        "Tier 10 is enabled, but index 10 does not exist.";

                    return false;
                }

                if (!AuditCustomTier(
                        tiers[10],
                        10,
                        config.Tier10Points,
                        config.Tier10PassionGainsMin,
                        config.Tier10PassionGainsMax,
                        config.Tier10PassionChoices,
                        config.Tier10TraitChoices,
                        out error))
                {
                    return false;
                }
            }


            return true;
        }


        private static bool AuditCustomTier(
            GrowthUtility.GrowthTier tier,
            int expectedTierNumber,
            int expectedPoints,
            int expectedPassionMin,
            int expectedPassionMax,
            int expectedPassionChoices,
            int expectedTraitChoices,
            out string error)
        {
            error = null;


            if (tier.tier != expectedTierNumber)
            {
                error =
                    "Tier number mismatch. Expected " +
                    expectedTierNumber +
                    ", found " +
                    tier.tier +
                    ".";

                return false;
            }


            if (Math.Abs(
                    tier.pointsRequirement -
                    expectedPoints) > 0.001f)
            {
                error =
                    "Tier " +
                    expectedTierNumber +
                    " points requirement mismatch. Expected " +
                    expectedPoints +
                    ", found " +
                    tier.pointsRequirement +
                    ".";

                return false;
            }


            ByteRange expectedPassionRange =
                new ByteRange(
                    (byte)expectedPassionMin,
                    (byte)expectedPassionMax
                );

            if (!tier.passionGainsRange.Equals(
                    expectedPassionRange))
            {
                error =
                    "Tier " +
                    expectedTierNumber +
                    " passion gains range mismatch. Expected " +
                    expectedPassionMin +
                    "-" +
                    expectedPassionMax +
                    ".";

                return false;
            }


            if (tier.passionChoices != expectedPassionChoices)
            {
                error =
                    "Tier " +
                    expectedTierNumber +
                    " passion choices mismatch. Expected " +
                    expectedPassionChoices +
                    ", found " +
                    tier.passionChoices +
                    ".";

                return false;
            }


            // Multi-trait gain is intentionally unsupported.
            if (tier.traitGains != 1)
            {
                error =
                    "Tier " +
                    expectedTierNumber +
                    " trait gains must remain exactly 1, but found " +
                    tier.traitGains +
                    ".";

                return false;
            }


            if (tier.traitChoices != expectedTraitChoices)
            {
                error =
                    "Tier " +
                    expectedTierNumber +
                    " trait choices mismatch. Expected " +
                    expectedTraitChoices +
                    ", found " +
                    tier.traitChoices +
                    ".";

                return false;
            }


            return true;
        }


        private static bool SameGrowthTierValues(
            GrowthUtility.GrowthTier a,
            GrowthUtility.GrowthTier b)
        {
            return
                a.tier == b.tier &&
                Math.Abs(
                    a.pointsRequirement -
                    b.pointsRequirement) <= 0.001f &&
                a.passionGainsRange.Equals(
                    b.passionGainsRange) &&
                a.passionChoices == b.passionChoices &&
                a.traitGains == b.traitGains &&
                a.traitChoices == b.traitChoices;
        }
    }
}