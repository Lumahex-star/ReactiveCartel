using HarmonyLib;
using MelonLoader;
using ScheduleOne.Cartel;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using UnityEngine;
using S1API.Leveling;
using FishNet;

namespace ReactiveCartel.Patches
{
    public static class ReactiveCartel
    {
        public static bool GameLoaded = false;

        public static bool TryToInitialize()
        {
            if (LoadManager.Instance == null)
                return false;

            LoadManager.Instance.onLoadComplete.AddListener(OnLoadComplete);
            MelonLogger.Msg("Hooked LoadManager");
            return true;
        }

        private static void OnLoadComplete()
        {
            GameLoaded = true;
            MelonLogger.Msg("Game fully loaded");

        }
    }

    [HarmonyPatch(typeof(CartelRegionActivities), "GetNewCooldown")]
    public static class RegionalCooldownPatch
    {
        static bool Prefix(EMapRegion region, ref int __result)
        {
            if (!ReactiveCartel.GameLoaded)
                return true;

            if (!LevelManager.Exists)
                return true;

            FullRank rank = LevelManager.CurrentRank;
            int rankIndex = rank.GetRankIndex();

            float maxPossibleIndex = 40f;
            float progress = Mathf.Clamp01((float)rankIndex + 1 / maxPossibleIndex);

            float minCooldown = Mathf.Lerp(24f, 6f, progress);
            float maxCooldown = Mathf.Lerp(48f, 12f, progress);

            __result = Mathf.RoundToInt(UnityEngine.Random.Range(minCooldown, maxCooldown));

            MelonLogger.Msg(
                $"[ReactiveCartel] Region: {region} | RankIndex: {rankIndex} | Progress: {progress:F2} | Cooldown: {__result}h"
            );

            return false;
        }
    }
}