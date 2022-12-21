using System;
using System.Collections.Generic;
using System.IO;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Helpers;
using UnityEngine;

namespace AdminTools
{
    public class Plugin
    {
        public const string Author = "Neil";
        public const string Name = "Admin Tools";
        public const string Version = "8.5.4";

        public EventHandlers EventHandlers;

        public static System.Random NumGen = new();
        public static List<Jailed> JailedPlayers = new();
        public static Dictionary<Player, InstantKillComponent> IkHubs = new();
        public static Dictionary<Player, RegenerationComponent> RgnHubs = new();
        public static HashSet<Player> PryGateHubs = new();
        public static Dictionary<Player, List<GameObject>> BchHubs = new();
        public static Dictionary<Player, List<GameObject>> DumHubs = new();
        public static float HealthGain = 5;
        public static float HealthInterval = 1;
        public string OverwatchFilePath;
        public string HiddenTagsFilePath;
        public static bool RestartOnEnd = false;
        public static HashSet<Player> RoundStartMutes = new();

        [PluginConfig] public Config Config;

        [PluginEntryPoint(Name, Version, "Tools to better support staff", Author)]
        public void Start()
        {
            foreach (var translation in PlayerStatsSystem.DeathTranslations.TranslationsById)
                Handlers.UniversalDamageTypeIDs.Add(translation.Value, translation.Key);
            
            string path = Path.Combine(Paths.LocalPlugins.Plugins, "Admin Tools");
            string overwatchFileName = Path.Combine(path, "AdminTools-Overwatch.txt");
            string hiddenTagFileName = Path.Combine(path, "AdminTools-HiddenTags.txt");
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            if (!File.Exists(overwatchFileName))
                File.Create(overwatchFileName).Close();
            
            if (!File.Exists(hiddenTagFileName))
                File.Create(hiddenTagFileName).Close();
            
            OverwatchFilePath = overwatchFileName;
            HiddenTagsFilePath = hiddenTagFileName;
            
            EventHandlers = new EventHandlers(this);
        }
    }
}
