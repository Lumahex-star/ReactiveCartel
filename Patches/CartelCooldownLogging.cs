using HarmonyLib;
using MelonLoader;
using ScheduleOne.Cartel;
using ScheduleOne.Map;

namespace ReactiveCartel.Patches
{
    /*[HarmonyPatch(typeof(CartelRegionActivities), "GetNewCooldown")]
    public static class CooldownLoggingPatch
    {
        [HarmonyPostfix]
        public static void Postfix(int __result)
        {
            MelonLogger.Msg("[ReactiveCartel] COOLDOWN PATCH FIRED");
            MelonLogger.Msg("[ReactiveCartel] Regional cooldown: " + __result);
        }
    }

    [HarmonyPatch(typeof(CartelActivity), "Activate")]
    public static class ActivityLoggingPatch
    {
        public static void Postfix(CartelActivity __instance, EMapRegion region)
        {
            MelonLogger.Msg("[ReactiveCartel] ACTIVITY PATCH FIRED");
            MelonLogger.Msg("[ReactiveCartel] Region + Activity selected: " + __instance + region);
        }
    }*/
}