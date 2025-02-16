﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin {
    public class Config {
        public static string SUBFOLDER = "SyncLastFmPlaycount"; // Plugin subfolder.

        public Settings settings { get; private set; }
        public UserData userData { get; private set; }

        public Config() {
            settings = LoadSettings();
            userData = LoadUserData();
        }

        public string getSubfolderPath() {
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            string path = String.Concat(dataPath, SUBFOLDER, Path.DirectorySeparatorChar);
            Directory.CreateDirectory(path);
            return path;
        }

        public void Log(string text) {
            // Log the timestamp, the failed scrobble and the error message in the error file.
            string errorTimestamp = DateTime.Now.ToString();
            // Create the folder where the error log will be stored.
            string path = getSubfolderPath();
            File.AppendAllText(String.Concat(path, "log.log"), errorTimestamp + " "
                                                                                        + text + Environment.NewLine);
        }

        public void SaveSettings() {
            var json = JsonConvert.SerializeObject(settings);
            string path = getSubfolderPath();
            File.WriteAllText(String.Concat(path, "settings.json"), json);
            //Update userData if user changed
            userData = LoadUserData();
            userData.Username = settings.Username;
            SaveUserData();
        }
        private Settings LoadSettings() {
            string path = getSubfolderPath();
            string file = String.Concat(path, "settings.json");
            if (File.Exists(file)) {
                string json = File.ReadAllText(file);
                if (String.IsNullOrEmpty(json)) {
                    return new Settings();
                }
                return JsonConvert.DeserializeObject<Settings>(json);
            } else {
                return new Settings();
            }
        }
        private string GetUserFolder() {
            if (String.IsNullOrEmpty(settings.Username)) {
                return null;
            }
            string path = getSubfolderPath();
            string folderName = settings.Username;
            foreach (char c in Path.GetInvalidPathChars())
                folderName = folderName.Replace(Char.ToString(c), "");
            string folder = String.Concat(path, folderName, Path.DirectorySeparatorChar);
            Directory.CreateDirectory(folder);
            return folder;
        }
        public void SaveUserData() {
            string userFolder = GetUserFolder();
            if (userFolder != null) {
                var json = JsonConvert.SerializeObject(userData);
                File.WriteAllText(String.Concat(userFolder, "data.json"), json);
            }

        }
        private UserData LoadUserData() {
            string userFolder = GetUserFolder();
            if (userFolder != null) {
                string file = String.Concat(userFolder, "data.json");
                if (File.Exists(file)) {
                    string json = File.ReadAllText(file);
                    if (!String.IsNullOrEmpty(json)) {
                        return JsonConvert.DeserializeObject<UserData>(json);
                    }
                }
            }
            return new UserData();
        }
        public void ControlLogFile() {
            //If the log file is too big, remove the first half of its lines
            string path = getSubfolderPath();
            string logFile = String.Concat(path, "log.log");
            //If file bigger than 5 MB
            if (File.Exists(logFile)) {
                {
                    var fi = new FileInfo(logFile);
                    if (fi.Length > 5 * 1024 * 1024) {
                        var tempFile = Path.GetTempFileName();
                        var allLines = File.ReadLines(logFile).ToList();
                        var line = 0;
                        var linesToKeep = File.ReadLines(logFile).ToList().Where(l => {
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
    public class Settings {
        public string Username { get; set; }
        public bool QueryAlbumArtist { get; set; } = true;
        public bool QuerySortTitle { get; set; } = true;
        public bool QueryMultipleArtists { get; set; } = true;
        public bool SyncLovedTracks { get; set; } = false;
        public bool QueryRecentOnStartup { get; set; } = true;
        public int IgnoreWhenLower { get; set; } = 2;
        public int UpdateMode { get; set; } = 0;

    }
    public class UserData {
        public string Username { get; set; }
        public DateTimeOffset? LastTimePlayed { get; set; }

    }

}
