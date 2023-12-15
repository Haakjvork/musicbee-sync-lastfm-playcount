using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class Config
    {
        public static string SUBFOLDER = "SyncLastFmPlaycount\\"; // Plugin subfolder.

        public Settings settings { get; private set; }

        public Config()
        {
            settings = LoadSettings();
        }

        public void Log(string text)
        {
            // Log the timestamp, the failed scrobble and the error message in the error file.
            string errorTimestamp = DateTime.Now.ToString();
            // Create the folder where the error log will be stored.
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            Directory.CreateDirectory(String.Concat(dataPath, SUBFOLDER));
            File.AppendAllText(String.Concat(dataPath, SUBFOLDER, "log.log"), errorTimestamp + " "
                                                                                        + text + Environment.NewLine);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(settings);
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            Directory.CreateDirectory(String.Concat(dataPath, SUBFOLDER));
            File.WriteAllText(String.Concat(dataPath, SUBFOLDER, "settings.json"), json);
        }
        private Settings LoadSettings()
        {
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            Directory.CreateDirectory(String.Concat(dataPath, SUBFOLDER));
            string file = String.Concat(dataPath, SUBFOLDER, "settings.json");
            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);
                if (String.IsNullOrEmpty(json))
                {
                    return new Settings();
                }
                return JsonConvert.DeserializeObject<Settings>(json);
            }
            else
            {
                return new Settings();
            }
        }

    }
    public class Settings
    {
        public string Username { get; set; }
        public bool QueryAlbumArtist { get; set; } = true;
        public bool QuerySortTitle { get; set; } = true;

    }

}
