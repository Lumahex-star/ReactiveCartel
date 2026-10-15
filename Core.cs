using MelonLoader;
using HarmonyLib;

[assembly: MelonInfo(typeof(ReactiveCartel.Core), "ReactiveCartel", "1.1.0", "Lumahex", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ReactiveCartel
{
    public class Core : MelonMod
    {
        private bool _initialized = false;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Reactive Cartel Initialized.");

            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("ReactiveCartel");
            harmony.PatchAll();

            MelonLogger.Msg("Harmony Patches Applied");
        }

        public override void OnUpdate()
        {
            if (!_initialized && Patches.ReactiveCartel.TryToInitialize())
            {
                _initialized = true;
            }
        }
    }
}