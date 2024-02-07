using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> SyncPlaycountFromLastFm(string[] files)
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
                    mbApiInterface.MB_SetBackgroundTaskMessage(String.Concat(i + 1, " of ", songs.Count, ": ", song.Artist, " - ", song.Name));
                    bool different = await SyncSongPlaycount(song);
                    if (different)
                    {
                        updated++;
                    }

                }
                var msg = String.Concat("Updated ", updated, " of  ", songs.Count, " songs");
                config.Log(msg);
                mbApiInterface.MB_SetBackgroundTaskMessage(msg);
            }
            catch (Exception e)
            {
                var error = String.Concat("ERROR: ", e.Message);
                config.Log(error);
                config.Log(e.StackTrace);
                mbApiInterface.MB_SetBackgroundTaskMessage(error);
            }
            mbApiInterface.MB_RefreshPanels();

            return true;

        }

        private async Task<bool> SyncSongPlaycount(MBSong song)
        {
            Tuple<int, bool> res = await QuerySongPlaycount(song);
            bool changes = false;
            if (res.Item1 > 1 && song.PlayCount != res.Item1)
            {
                config.Log(String.Concat("Updating PlayCount from ", song.PlayCount, " to ", res.Item1));
                mbApiInterface.Library_SetFileTag(song.File, (MetaDataType)FilePropertyType.PlayCount, res.Item1.ToString());
                mbApiInterface.Library_CommitTagsToFile(song.File);
                changes = true;
            }
            if ( config.settings.SyncLovedTracks && res.Item1 > 1 && res.Item2 != song.IsLoved)
            {
                var value = (res.Item2 ? "L" : "");
                config.Log(String.Concat("Updating RatingLove from ", song.IsLovedRaw, " to ", value ));
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
            if (neu <= 1)
            {
                warn = " IGNORE";
            }
            config.Log(String.Concat(old, " -> ", neu, " ", warn));
            Tuple<int, bool> res = new Tuple<int, bool>(neu, isLoved);
            return res;
        }

        private async Task<LastTrack> QueryTackInfo(string name, string artist)
        {
            config.Log(String.Concat("TrackInfo ", artist, " - ", name));
            var res = await lastfmClient.Track.GetInfoAsync(name, artist, config.settings.Username);
            if (res.Status == IF.Lastfm.Core.Api.Enums.LastResponseStatus.Successful)
            {
                var pc = (int)res.Content.UserPlayCount;
                config.Log(String.Concat("TrackInfo ", artist, " - ", name, " | OK = ", pc));
                return res.Content;
            }
            else
            {
                config.Log(String.Concat("TrackInfo ", artist, " - ", name, " | ERROR: ", res.Status));
                return null;
            }

        }

    }


}
