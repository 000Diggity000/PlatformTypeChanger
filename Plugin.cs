using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using PlatformTypeChanger;
using System.IO;

namespace PlatformTypeChanger
{
    [BepInPlugin("com.000diggity000.PlatformTypeChanger", "Platform Type Changer", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ConfigEntry<float> PlatformTypeConfig;
        internal static ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "PlatformTypeConfig.cfg"), true);
        private void Awake()
        {
            Logger.LogInfo("PlatformChanger Loaded");
            Harmony harmony = new Harmony("com.000diggity000.PlatformTypeChanger");
            MethodInfo original = AccessTools.Method(typeof(StickyRoundedRectangle), "Awake");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "Awake_Platform_Plug");
            harmony.Patch(original, new HarmonyMethod(patch));
            PlatformTypeConfig = config.Bind("Platform Type Changer", "Platform Type", 0f, "0 for Default, 1 for Grass, 2 for Ice, 3 for Snow, 4 for Space, 5 for Slime. Disclaimer: Some types may not do anything, but that is not my fault, because Zapray did not implmenent features for the platformtypes.");
            Plugin.config.Save();
        }
    }
    public class Patches
    {
        public static bool Awake_Platform_Plug(StickyRoundedRectangle __instance)
        {
            if(Plugin.PlatformTypeConfig.Value == 0)
            {
                return true;
            }
            if(Plugin.PlatformTypeConfig.Value == 1)
            {
                __instance.platformType = PlatformType.grass;
            }
            else if (Plugin.PlatformTypeConfig.Value == 2)
            {
                __instance.platformType = PlatformType.ice;
            }
            else if (Plugin.PlatformTypeConfig.Value == 3)
            {
                __instance.platformType = PlatformType.snow;
            }
            else if (Plugin.PlatformTypeConfig.Value == 4)
            {
                __instance.platformType = PlatformType.space;
            }
            else if (Plugin.PlatformTypeConfig.Value == 5)
            {
                __instance.platformType = PlatformType.slime;
            }
            else
            {
                BepInEx.Logging.Logger.CreateLogSource("Platform_Type_Changer").LogError("Invalid Type");
                
            }
            return true;
        }

    }
}
