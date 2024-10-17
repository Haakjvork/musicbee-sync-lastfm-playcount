using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    internal class LastFMService
    {

        private readonly Config config;
        private readonly LastfmClient lastfmClient;

        public LastFMService(Config config, LastfmClient lastfmClient)
        {
            this.config = config;
            this.lastfmClient = lastfmClient;   
        }

        private void LogAndMessage(string msg)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage(msg);
            config.Log(msg);
        }

        public async Task<bool> SyncFilesPlaycount(string[] files)
        {
            if (files == null) return false;
            try
            {
                config.ControlLogFile();
                int updated = 0;
                List<MBSong> songs = files.Select(f => new MBSong(f)).ToList();
                config.Log(String.Concat("Querying ", songs.Count, " songs"));
                for (int i = 0; i < songs.Count; i++)
                {
                    MBSong song = songs[i];
                    LogAndMessage(String.Concat(i + 1, " of ", songs.Count, ": ", song.Artist, " - ", song.Name));
                    bool different = await SyncSongPlaycount(song);
                    if (different)
                    {
                        updated++;
                    }

                }
                var msg = String.Concat("Updated ", updated, " of  ", songs.Count, " songs");
                LogAndMessage(msg);
            }
            catch (Exception e)
            {
                var error = String.Concat("ERROR: ", e.Message);
                LogAndMessage(error);
                config.Log(e.StackTrace);
            }
            mbApiInterface.MB_RefreshPanels();
            return true;
        }

        public async Task SyncByRecentScrobbles(bool ignoreLastTimePlayed)
        {
            DateTimeOffset? newTimePlayed = null;
            DateTime now = DateTime.Now;
            DateTime limit = now.AddYears(-1);
            if ( ignoreLastTimePlayed)
            {
                limit = now.AddMonths(-1);
            }
            List<string> files = new List<string>();
            int page = 1;
            while (true)
            {
                QueriedLastTracks res = await FindFilesFromRecentScrobblesByPage(page++, ignoreLastTimePlayed);
                if (res == null )
                {
                    break;
                }
                config.Log(String.Concat("Res: ", res.ValidScrobbles, " - ", res.MinTimePlayed, " - ", res.MaxTimePlayed ));
                if (newTimePlayed == null || res.MaxTimePlayed > newTimePlayed)
                {
                    newTimePlayed = res.MaxTimePlayed;
                }
                files.AddRange(res.Files);
                //End loop?
                if ( res.ValidScrobbles == 0)
                {
                    config.Log(String.Concat("No more valid scrobbles."));
                    break;
                }   
                if ( !ignoreLastTimePlayed && res.MinTimePlayed < config.userData.LastTimePlayed )
                {
                    config.Log(String.Concat("Reached Last Time Played: ", res.MinTimePlayed, " < ", config.userData.LastTimePlayed ,". Stopping"));
                    break;
                }
                if (res.MinTimePlayed < limit)
                {
                    config.Log(String.Concat("Reached Time Limit: ", res.MinTimePlayed, " < ", limit, ". Stopping"));
                    break;
                }
                if (page >= 50)
                {
                    config.Log("Reached page 50. Stopping");
                    break;
                }
            }
            string[] array = files.Distinct().ToList().ToArray();
            config.Log(String.Concat("From ", files.Count, " scrobbles to ", array.Length, " files"));
            await SyncFilesPlaycount(array);
            config.userData.LastTimePlayed = newTimePlayed;
            config.SaveUserData();
        }

        private async Task<bool> SyncSongPlaycount(MBSong song)
        {
            Tuple<int, bool> res = await QuerySongPlaycount(song);
            bool changes = false;
            bool updateCount = ( res.Item1 > 0 &&  song.PlayCount != res.Item1);
            if (res.Item1 < config.settings.IgnoreWhenLower)
            {
                updateCount = false;
            }
            //Do not update when LastFm value is lower than ours
            if ( config.settings.UpdateMode == 1 && res.Item1 < song.PlayCount)
            {
                updateCount = false;
            }
            if (updateCount)
            {
                config.Log(String.Concat("  Updating PlayCount from ", song.PlayCount, " to ", res.Item1));
                mbApiInterface.Library_SetFileTag(song.File, (MetaDataType)FilePropertyType.PlayCount, res.Item1.ToString());
                mbApiInterface.Library_CommitTagsToFile(song.File);
                changes = true;
            }
            if ( config.settings.SyncLovedTracks && res.Item1 > 0 && res.Item2 != song.IsLoved)
            {
                var value = (res.Item2 ? "L" : "");
                config.Log(String.Concat("  Updating RatingLove from ", song.IsLovedRaw, " to ", value ));
                mbApiInterface.Library_SetFileTag(song.File, MetaDataType.RatingLove, value );
                mbApiInterface.Library_CommitTagsToFile(song.File);
                changes = true;
            }
            return changes;
        }

        private List<string> getSongTrackNames(MBSong song)
        {
            List<string> names = new List<string>();
            names.Add(song.Name);
            var normalized = song.Name.Normalize();
            if (!String.Equals(song.Name, normalized))
            {
                names.Add(normalized);
            }
            if (config.settings.QuerySortTitle && !String.IsNullOrEmpty(song.SortTitle) && !String.Equals(song.Name.ToLower(), song.SortTitle.ToLower()))
            {
                names.Add(song.SortTitle);
                var normalizedSortTitle = song.SortTitle.Normalize();
                if (!String.Equals(song.SortTitle, normalizedSortTitle))
                {
                    names.Add(normalizedSortTitle);
                }
            }
            return names;
        }
        private List<string> getSongTrackArtists(MBSong song)
        {
            List<string> artists = new List<string>();
            artists.Add(song.Artist);
            if (config.settings.QueryMultipleArtists && song.Artist.Contains(";"))
            {
                string[] splitted = song.Artist.Split(';');
                foreach (string s in splitted)
                {
                    var trimmed = s.Trim();
                    if (!String.IsNullOrEmpty(trimmed))
                    {
                        artists.Add(trimmed);
                    }

                }
            }
            if (config.settings.QueryAlbumArtist && !String.IsNullOrEmpty(song.AlbumArtist))
            {
                bool alreadyContained = artists.Select((a) => a.ToLower()).Contains(song.AlbumArtist.ToLower());
                if (!alreadyContained)
                {
                    artists.Add(song.AlbumArtist);
                }
            }
            return artists;
        }

        private async Task<Tuple<int, bool>> QuerySongPlaycount(MBSong song)
        {
            if (String.IsNullOrEmpty(song.Name) || String.IsNullOrEmpty(song.Artist))
            {
                return null;
            }
            List<string> names = getSongTrackNames(song);
            List<string> artists = getSongTrackArtists(song);
            //Queries
            int old = song.PlayCount;
            var neu = 0;
            bool isLoved = false;
            foreach (string name in names)
            {
                foreach (string artist in artists)
                {
                    LastTrack info = await QueryTackInfo(name, artist);
                    if ( info != null )
                    {
                        neu += (int) info.UserPlayCount;
                        if ( (bool) info.IsLoved )
                        {
                            isLoved = true;
                        }
                    }
                }
            }
            //Results
            int dif = neu - old;
            var warn = "==";
            if (dif > 0)
            {
                warn = String.Concat("+", dif.ToString());
            }
            if (dif < 0)
            {
                warn = dif.ToString();
            }
            if (dif >= 10 || dif <= -10)
            {
                warn = String.Concat(warn, " HUGE");
            }
            if (neu < config.settings.IgnoreWhenLower )
            {
                warn = " IGNORE";
            }
            config.Log(String.Concat("  ", old, " -> ", neu, " ", warn));
            Tuple<int, bool> res = new Tuple<int, bool>(neu, isLoved);
            return res;
        }

        private async Task<LastTrack> QueryTackInfo(string name, string artist)
        {
            config.Log(String.Concat("  TrackInfo ", artist, " - ", name));
            var res = await lastfmClient.Track.GetInfoAsync(name, artist, config.settings.Username);
            if (res.Status == IF.Lastfm.Core.Api.Enums.LastResponseStatus.Successful)
            {
                var pc = (int)res.Content.UserPlayCount;
                config.Log(String.Concat("  TrackInfo ", artist, " - ", name, " | OK = ", pc));
                return res.Content;
            }
            else
            {
                config.Log(String.Concat("  TrackInfo ", artist, " - ", name, " | ERROR: ", res.Status));
                return null;
            }

        }

        private void QueryFiles(List<string> candidates, string query)
        {
            var files = mbApiInterface.Library_QueryFiles(query);
            if (files)
            {
                string file = null;
                do
                {
                    file = mbApiInterface.Library_QueryGetNextFile();
                    if ( file != null && !candidates.Contains(file))
                    {
                        candidates.Add(file);
                    }
                } while (file != null);
            }
        }

        public List<string> FindFiles(LastTrack track)
        {
            return FindFiles(track.ArtistName, track.Name);
        }

        public List<string> FindFiles(string aritst, string track)
        {
            List<string> res = new List<string>();
            QueryFiles(res, "artist=" + aritst + '\0' + "title=" + track);
            QueryFiles(res, "artist=" + aritst + '\0' + "sortTitle=" + track);
            QueryFiles(res, "albumArtist=" + aritst + '\0' + "title=" + track);
            QueryFiles(res, "albumArtist=" + aritst + '\0' + "sortTitle=" + track);
            return res;
        }

        private async Task<QueriedLastTracks> FindFilesFromRecentScrobblesByPage(int page, bool ignoreLastTimePlayed)
        {
            LogAndMessage(String.Concat("Querying recent scrobbles, page ", page));
            var res = await lastfmClient.User.GetRecentScrobbles(config.settings.Username, null, null, true, page, 100);
            if (res.Status == IF.Lastfm.Core.Api.Enums.LastResponseStatus.Successful)
            {
                return FindFilesFromTracks(res.Content, ignoreLastTimePlayed);
            }
            else
            {
                config.Log(String.Concat("GetRecentScrobbles | ERROR: ", res.Status));
                return null;
            }
        }

        private QueriedLastTracks FindFilesFromTracks(IEnumerable<LastTrack> tracks, bool ignoreLastTimePlayed)
        {
            QueriedLastTracks res = new QueriedLastTracks();
            foreach (var track in tracks)
            {
                config.Log(String.Concat(track.ArtistName, " - ", track.Name, " at ", track.TimePlayed, " plays= ", track.UserPlayCount, " loved? ", track.IsLoved));
                if (track.TimePlayed != null)
                {
                    //Register the times
                    if (res.MinTimePlayed == null || track.TimePlayed < res.MinTimePlayed)
                    {
                        res.MinTimePlayed = track.TimePlayed;
                    }
                    if (res.MaxTimePlayed == null || track.TimePlayed > res.MaxTimePlayed)
                    {
                        res.MaxTimePlayed = track.TimePlayed;
                    }
                    if (config.userData.LastTimePlayed == null || track.TimePlayed > config.userData.LastTimePlayed || ignoreLastTimePlayed)
                    {
                        //Count the valid scrobbles
                        res.ValidScrobbles++;
                        List<string> candidates = FindFiles(track);
                        if (candidates.Count == 0)
                        {
                            config.Log(String.Concat("  No file found"));
                        }
                        else if (candidates.Count > 1)
                        {
                            config.Log(String.Concat("  Multiple files found:"));
                            foreach (var str in candidates)
                            {
                                config.Log(String.Concat("    ", str));
                            }
                        }
                        else
                        {
                            string file = candidates[0];
                            res.Files.Add(file);
                            config.Log(String.Concat("  File found: ", file));
                        }
                    }
                    else
                    {
                        config.Log("  Ignored");
                    }
                }
                else
                {
                    config.Log("  Is being played");
                }
            }
            return res;
        }

    }


}
