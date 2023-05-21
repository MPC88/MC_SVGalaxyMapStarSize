
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace MC_SVGalaxyMapStarSize
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.galaxymapstarsize";
        public const string pluginName = "SV Galaxy Map Star Size";
        public const string pluginVersion = "1.0.0";

        public static ConfigEntry<float> baseScale;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Main));

            baseScale = Config.Bind<float>("Settings",
                "VerySmallSize",
                0.65f,
                "Size of very small sector.");
        }

        [HarmonyPatch(typeof(GalaxyMap), nameof(GalaxyMap.GenerateMap))]
        [HarmonyPostfix]
        private static void GalaxyMap_GenerateMapPost(GalaxyMap __instance)
        {
            ParticleSystem.Particle[] particles = AccessTools.FieldRefAccess<ParticleSystem.Particle[]>(typeof(GalaxyMap), "starParticles")(__instance);
            for (int i = 0; i < particles.Length; i++)
                particles[i].startSize = GameData.data.sectors[i].size * baseScale.Value;

            AccessTools.FieldRefAccess<ParticleSystem>(typeof(GalaxyMap), "starParticleSystem")(__instance).SetParticles(particles, particles.Length);
        }
    }
}
