using System;
using System.Collections.Generic;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{

    public class MBSong
    {
        public string File { get; private set; }
        public string Name { get; private set; }
        public string SortTitle { get; private set; }
        public string AlbumName { get; private set; }
        public string AlbumArtist { get; private set; }
        public string Artist { get; private set; }
        public int PlayCount { get; private set; }
        public bool IsLoved { get; private set; }
        public string IsLovedRaw { get; private set; }

        public MBSong(string sourceFileUrl)
        {
            File = sourceFileUrl;
            Name = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.TrackTitle);
            SortTitle = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.SortTitle);
            AlbumName = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Album);
            AlbumArtist = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.AlbumArtist);
            Artist = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artists);
            var pc = mbApiInterface.Library_GetFileTag(sourceFileUrl, (MetaDataType)FilePropertyType.PlayCount);
            PlayCount = Int32.Parse(pc);
            IsLovedRaw = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.RatingLove);
            IsLoved = (IsLovedRaw != null && (IsLovedRaw.ToLower().Equals("true") || IsLovedRaw == "1" || IsLovedRaw == "L"));
        }
    }

    public class QueriedLastTracks
    {
        public List<string> Files{ get; private set; } 
        public DateTimeOffset? MinTimePlayed { get; set; }
        public DateTimeOffset? MaxTimePlayed { get; set; }
        public int ValidScrobbles { get; set; }

        public QueriedLastTracks()
        {
            Files = new List<string>();
            ValidScrobbles = 0;
        }
    }
}
