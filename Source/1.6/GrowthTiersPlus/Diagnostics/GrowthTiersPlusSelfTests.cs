using System;
using Verse;

namespace Tier_expand
{
    public static class GrowthTiersPlusSelfTests
    {
        public static void Run()
        {
            try
            {
                TestDefaultSettings();
                TestPointRules();
                TestPassionRules();
                TestTierDependency();
                TestValidatedConfigLayer();
                TestGrowthMomentSafety();

                Log.Message(
                    "[Growth Tiers Plus] DEBUG self-tests passed: " +
                    "default settings; point boundaries; passion invariants; " +
                    "tier dependency; validated runtime config; " +
                    "growth moment safety."
                );
            }
            catch (Exception ex)
            {
                Log.Error(
                    "[Growth Tiers Plus] DEBUG self-tests FAILED:\n" + ex
                );
            }
        }


        // ---------------------------------------------------------
        // Default values must preserve the currently proven version.
        // ---------------------------------------------------------

        private static void TestDefaultSettings()
        {
            TierExpandSettings s = new TierExpandSettings();

            Assert(s.tier9Enabled, "Default Tier 9 should be enabled.");
            Assert(s.tier10Enabled, "Default Tier 10 should be enabled.");

            Assert(s.tier9Points == 168,
                "Default Tier 9 requirement should be 168.");

            Assert(s.tier10Points == 172,
                "Default Tier 10 requirement should be 172.");

            Assert(s.tier9PassionGainsMin == 3,
                "Default T9 passion minimum should be 3.");

            Assert(s.tier9PassionGainsMax == 4,
                "Default T9 passion maximum should be 4.");

            Assert(s.tier9PassionChoices == 7,
                "Default T9 passion choices should be 7.");

            Assert(s.tier9TraitChoices == 7,
                "Default T9 trait choices should be 7.");

            Assert(s.tier10PassionGainsMin == 3,
                "Default T10 passion minimum should be 3.");

            Assert(s.tier10PassionGainsMax == 4,
                "Default T10 passion maximum should be 4.");

            Assert(s.tier10PassionChoices == 8,
                "Default T10 passion choices should be 8.");

            Assert(s.tier10TraitChoices == 8,
                "Default T10 trait choices should be 8.");
        }


        // ---------------------------------------------------------
        // Growth-point ordering / allowed range.
        // ---------------------------------------------------------

        private static void TestPointRules()
        {
            // Valid representative values.
            Assert(
                GrowthTierValidation.IsValidPointConfiguration(true, 163, true, 164),
                "163 / 164 should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidPointConfiguration(true, 168, true, 172),
                "168 / 172 should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidPointConfiguration(true, 179, true, 180),
                "179 / 180 should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidPointConfiguration(true, 180, false, 180),
                "Tier 9 alone at 180 should be valid."
            );

            // Invalid configurations.
            Assert(
                !GrowthTierValidation.IsValidPointConfiguration(true, 162, false, 172),
                "Tier 9 at vanilla Tier 8 requirement must be invalid."
            );

            Assert(
                !GrowthTierValidation.IsValidPointConfiguration(true, 168, true, 168),
                "Tier 9 and Tier 10 cannot have equal requirements."
            );

            Assert(
                !GrowthTierValidation.IsValidPointConfiguration(true, 172, true, 168),
                "Tier 10 cannot be below Tier 9."
            );

            Assert(
                !GrowthTierValidation.IsValidPointConfiguration(true, 180, true, 180),
                "Tier 9 cannot be 180 while Tier 10 is enabled."
            );

            Assert(
                !GrowthTierValidation.IsValidPointConfiguration(false, 168, true, 172),
                "Tier 10 cannot exist without Tier 9."
            );
        }


        // ---------------------------------------------------------
        // Passion gains must never exceed offered choices.
        // ---------------------------------------------------------

        private static void TestPassionRules()
        {
            Assert(
                GrowthTierValidation.IsValidPassionConfiguration(3, 4, 7),
                "3-4 gains from 7 choices should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidPassionConfiguration(12, 12, 12),
                "12 gains from 12 choices should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidPassionConfiguration(1, 1, 1),
                "1 gain from 1 choice should be valid."
            );

            Assert(
                !GrowthTierValidation.IsValidPassionConfiguration(5, 4, 7),
                "Minimum cannot exceed maximum."
            );

            Assert(
                !GrowthTierValidation.IsValidPassionConfiguration(3, 8, 7),
                "Passion gains cannot exceed choices."
            );

            Assert(
                !GrowthTierValidation.IsValidPassionConfiguration(0, 4, 7),
                "Passion minimum below supported range must be invalid."
            );

            Assert(
                !GrowthTierValidation.IsValidPassionConfiguration(3, 13, 13),
                "Values above 12 must be invalid."
            );
        }


        // ---------------------------------------------------------
        // Tier dependency.
        // ---------------------------------------------------------

        private static void TestTierDependency()
        {
            Assert(
                GrowthTierValidation.IsValidTierDependency(true, true),
                "Tier 9 ON / Tier 10 ON should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidTierDependency(true, false),
                "Tier 9 ON / Tier 10 OFF should be valid."
            );

            Assert(
                GrowthTierValidation.IsValidTierDependency(false, false),
                "Tier 9 OFF / Tier 10 OFF should be valid."
            );

            Assert(
                !GrowthTierValidation.IsValidTierDependency(false, true),
                "Tier 10 ON while Tier 9 OFF must be invalid."
            );
        }

        private static void TestValidatedConfigLayer()
        {
            GrowthTierConfig config;
            string error;


            // ---------------------------------------------------------
            // Default configuration
            // ---------------------------------------------------------

            TierExpandSettings defaults = new TierExpandSettings();

            Assert(
                GrowthTierConfig.TryCreate(
                    defaults,
                    out config,
                    out error
                ),
                "Default settings must produce a valid runtime config. " +
                "Error: " + error
            );

            Assert(config != null,
                "Valid default settings must return a config.");

            Assert(config.ExpectedTierArrayLength == 11,
                "Default configuration should produce 11 growth tiers.");

            Assert(config.Tier9Points == 168,
                "Validated Tier 9 default should be 168.");

            Assert(config.Tier10Points == 172,
                "Validated Tier 10 default should be 172.");

            Assert(config.Tier9PassionGainsMin == 3,
                "Validated Tier 9 passion minimum should be 3.");

            Assert(config.Tier9PassionGainsMax == 4,
                "Validated Tier 9 passion maximum should be 4.");

            Assert(config.Tier9PassionChoices == 7,
                "Validated Tier 9 passion choices should be 7.");

            Assert(config.Tier9TraitChoices == 7,
                "Validated Tier 9 trait choices should be 7.");

            Assert(config.Tier10PassionChoices == 8,
                "Validated Tier 10 passion choices should be 8.");

            Assert(config.Tier10TraitChoices == 8,
                "Validated Tier 10 trait choices should be 8.");


            // ---------------------------------------------------------
            // Config must be an immutable snapshot of settings.
            // ---------------------------------------------------------

            defaults.tier9Points = 175;
            defaults.tier10Points = 179;
            defaults.tier9PassionChoices = 12;

            Assert(config.Tier9Points == 168,
                "Runtime config must not change when source settings change.");

            Assert(config.Tier10Points == 172,
                "Runtime config must preserve its validated snapshot.");

            Assert(config.Tier9PassionChoices == 7,
                "Runtime config passion values must be immutable snapshots.");


            // ---------------------------------------------------------
            // Tier 9 only -> vanilla 9 + Tier 9 = 10 entries
            // ---------------------------------------------------------

            TierExpandSettings tier9Only = new TierExpandSettings();

            tier9Only.tier10Enabled = false;
            tier9Only.tier9Points = 180;

            Assert(
                GrowthTierConfig.TryCreate(
                    tier9Only,
                    out config,
                    out error
                ),
                "Tier 9-only configuration should be valid. Error: " + error
            );

            Assert(config.ExpectedTierArrayLength == 10,
                "Tier 9-only configuration should produce 10 growth tiers.");

            Assert(config.Tier9Points == 180,
                "Tier 9 alone should be allowed at 180.");


            // ---------------------------------------------------------
            // Both custom tiers disabled -> vanilla 9 entries
            // ---------------------------------------------------------

            TierExpandSettings vanillaOnly = new TierExpandSettings();

            vanillaOnly.tier9Enabled = false;
            vanillaOnly.tier10Enabled = false;

            Assert(
                GrowthTierConfig.TryCreate(
                    vanillaOnly,
                    out config,
                    out error
                ),
                "Both extra tiers disabled should be a valid configuration."
            );

            Assert(config.ExpectedTierArrayLength == 9,
                "Both extra tiers disabled should preserve vanilla length 9.");


            // ---------------------------------------------------------
            // Maximum supported valid configuration
            // ---------------------------------------------------------

            TierExpandSettings maximum = new TierExpandSettings();

            maximum.tier9Points = 179;
            maximum.tier10Points = 180;

            maximum.tier9PassionGainsMin = 12;
            maximum.tier9PassionGainsMax = 12;
            maximum.tier9PassionChoices = 12;
            maximum.tier9TraitChoices = 12;

            maximum.tier10PassionGainsMin = 12;
            maximum.tier10PassionGainsMax = 12;
            maximum.tier10PassionChoices = 12;
            maximum.tier10TraitChoices = 12;

            Assert(
                GrowthTierConfig.TryCreate(
                    maximum,
                    out config,
                    out error
                ),
                "Maximum supported configuration should validate. Error: " + error
            );

            Assert(config.ExpectedTierArrayLength == 11,
                "Maximum configuration should still produce 11 growth tiers.");


            // ---------------------------------------------------------
            // Invalid: Tier 10 without Tier 9
            // ---------------------------------------------------------

            TierExpandSettings tier10Without9 = new TierExpandSettings();

            tier10Without9.tier9Enabled = false;
            tier10Without9.tier10Enabled = true;

            Assert(
                !GrowthTierConfig.TryCreate(
                    tier10Without9,
                    out config,
                    out error
                ),
                "Tier 10 without Tier 9 must be rejected."
            );

            Assert(config == null,
                "Rejected configuration must not return a runtime config.");

            Assert(!string.IsNullOrEmpty(error),
                "Rejected configuration should provide an error message.");


            // ---------------------------------------------------------
            // Invalid: equal point requirements
            // ---------------------------------------------------------

            TierExpandSettings equalPoints = new TierExpandSettings();

            equalPoints.tier9Points = 170;
            equalPoints.tier10Points = 170;

            Assert(
                !GrowthTierConfig.TryCreate(
                    equalPoints,
                    out config,
                    out error
                ),
                "Equal Tier 9 / Tier 10 requirements must be rejected."
            );


            // ---------------------------------------------------------
            // Invalid: gains exceed offered passion choices
            // ---------------------------------------------------------

            TierExpandSettings badPassions = new TierExpandSettings();

            badPassions.tier9PassionGainsMin = 3;
            badPassions.tier9PassionGainsMax = 8;
            badPassions.tier9PassionChoices = 7;

            Assert(
                !GrowthTierConfig.TryCreate(
                    badPassions,
                    out config,
                    out error
                ),
                "Passion gains exceeding choices must be rejected."
            );


            // ---------------------------------------------------------
            // Invalid: trait choices exceed supported maximum
            // ---------------------------------------------------------

            TierExpandSettings badTraits = new TierExpandSettings();

            badTraits.tier10TraitChoices = 13;

            Assert(
                !GrowthTierConfig.TryCreate(
                    badTraits,
                    out config,
                    out error
                ),
                "Trait choices above 12 must be rejected."
            );
        }

        private static void TestGrowthMomentSafety()
        {
            // ---------------------------------------------------------
            // Normal cases: requested gains fit inside available choices.
            // ---------------------------------------------------------

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    4,
                    7
                ) == 4,
                "4 passion gains from 7 available choices should remain 4."
            );

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    3,
                    3
                ) == 3,
                "3 passion gains from exactly 3 choices should remain 3."
            );


            // ---------------------------------------------------------
            // Dangerous case:
            // requested gains exceed actual available choices.
            // ---------------------------------------------------------

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    12,
                    5
                ) == 5,
                "12 requested passion gains with only 5 actual choices " +
                "must be clamped to 5."
            );

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    8,
                    1
                ) == 1,
                "8 requested passion gains with only 1 actual choice " +
                "must be clamped to 1."
            );


            // ---------------------------------------------------------
            // No choices available.
            // ---------------------------------------------------------

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    4,
                    0
                ) == 0,
                "Passion gains must become 0 when no choices exist."
            );

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    12,
                    -1
                ) == 0,
                "Invalid/non-positive actual choice count must safely return 0."
            );


            // ---------------------------------------------------------
            // Non-positive requested gains.
            // ---------------------------------------------------------

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    0,
                    7
                ) == 0,
                "0 requested passion gains should remain 0."
            );

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    -1,
                    7
                ) == 0,
                "Negative requested passion gains must safely return 0."
            );


            // ---------------------------------------------------------
            // Supported extreme value.
            // ---------------------------------------------------------

            Assert(
                GrowthMomentSafety.ClampPassionGainCount(
                    12,
                    12
                ) == 12,
                "12 gains from 12 available choices should remain valid."
            );
        }

        // =========================================================
        // Compatibility delegates. Keep public methods for consumers of the
        // pre-refactor API; executable validation rules now live in
        // GrowthTierValidation, and tests call that class directly.
        // =========================================================

        public static bool IsValidPointConfiguration(
            bool tier9Enabled,
            int tier9Points,
            bool tier10Enabled,
            int tier10Points)
        {
            return GrowthTierValidation.IsValidPointConfiguration(
                tier9Enabled, tier9Points, tier10Enabled, tier10Points);
        }

        public static bool IsValidPassionConfiguration(
            int minimum,
            int maximum,
            int choices)
        {
            return GrowthTierValidation.IsValidPassionConfiguration(
                minimum, maximum, choices);
        }

        public static bool IsValidTierDependency(
            bool tier9Enabled,
            bool tier10Enabled)
        {
            return GrowthTierValidation.IsValidTierDependency(
                tier9Enabled, tier10Enabled);
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}