using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime;
using static MusicBeePlugin.Plugin;
using System.Linq;

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

        public string getSubfolderPath()
        {
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            return String.Concat(dataPath, SUBFOLDER);
        }

        public void Log(string text)
        {
            // Log the timestamp, the failed scrobble and the error message in the error file.
            string errorTimestamp = DateTime.Now.ToString();
            // Create the folder where the error log will be stored.
            string path = getSubfolderPath();
            Directory.CreateDirectory(path);
            File.AppendAllText(String.Concat(path, "log.log"), errorTimestamp + " "
                                                                                        + text + Environment.NewLine);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(settings);
            string path = getSubfolderPath();
            Directory.CreateDirectory(path);
            File.WriteAllText(String.Concat(path, "settings.json"), json);
        }
        private Settings LoadSettings()
        {
            string path = getSubfolderPath();
            Directory.CreateDirectory(path);
            string file = String.Concat(path, "settings.json");
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
        public void ControlLogFile()
        {
            //If the log file is too big, remove the first half of its lines
            string path = getSubfolderPath();
            string logFile = String.Concat(path, "log.log");
            //If file bigger than 5 MB
            if (File.Exists(logFile) ) {
                {
                    var fi = new FileInfo(logFile);
                    if (fi.Length > 5 * 1024 * 1024)
                    {
                        var tempFile = Path.GetTempFileName();
                        var allLines = File.ReadLines(logFile).ToList();
                        var line = 0;
                        var linesToKeep = File.ReadLines(logFile).ToList().Where(l =>
                        {
                            line++;
                            return line > allLines.Count / 2;
                        }).ToArray();

                        File.WriteAllLines(tempFile, linesToKeep);

                        File.Delete(logFile);
                        File.Move(tempFile, logFile);

                    }
                }
            }
        }

    }
    public class Settings
    {
        public string Username { get; set; }
        public bool QueryAlbumArtist { get; set; } = true;
        public bool QuerySortTitle { get; set; } = true;
        public bool QueryMultipleArtists { get; set; } = true;
        public bool SyncLovedTracks { get; set; } = false;

    }

}
