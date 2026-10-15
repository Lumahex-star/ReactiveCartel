using HarmonyLib;
using MelonLoader;
using ScheduleOne.Cartel;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using UnityEngine;
using S1API.Leveling;
using FishNet;
using System.Collections.Generic;
using System.Linq;
using GameKit.Utilities.Types;
using UnityEngine.XR;

namespace ReactiveCartel.Patches
{
    [HarmonyPatch(typeof(CartelRegionActivities), "TryStartActivity")]
    public static class TryStartActivity_WeightedPatch
    {
        static bool Prefix(CartelRegionActivities __instance)
        {
            FullRank rank = LevelManager.CurrentRank;
            int playerRank = rank.GetRankIndex();

            __instance.HoursUntilNextActivity = CartelRegionActivities.GetNewCooldown(__instance.Region);

            var activities = __instance.Activities;

            var valid = activities.Where(a => a.IsRegionValidForActivity(__instance.Region)).ToList();
            if (valid.Count == 0) return false;

            var weights = valid.Select(a => GetWeight(a, playerRank)).ToList();

            CartelActivity chosen = WeightedPick(valid, weights);

            int index = activities.IndexOf(chosen);

            AccessTools.Method(typeof(CartelRegionActivities), "StartActivity")
            .Invoke(__instance, new object[] { null, index });

            return false;
        }

        static float GetWeight(CartelActivity activity, int playerRank)
        {
            bool isAggressive = activity is Ambush || activity is RobDealer;
            float progress = Mathf.Clamp01((float)(playerRank + 1) / 40f );

            if (isAggressive)
            {
                return 1 + progress * 5f;
            }
            else
            {
                return 1;
            }
        }

        static CartelActivity WeightedPick(List<CartelActivity> options, List<float> weights)
        {
            float total = 0f;
            foreach (float weight in weights)
            {
                total += weight;
            }
            float roll = UnityEngine.Random.Range(0f, total);

            float cumulative = 0f;
            for (int i = 0; i < options.Count; i++)
            {
                cumulative += weights[i];
                if (roll <= cumulative)
                {
                    return options[i];
                }
            }

            return options[0];
        }
    }
}