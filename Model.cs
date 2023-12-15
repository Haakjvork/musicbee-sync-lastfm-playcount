using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime;
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
        public string PlayCount { get; private set; }

        public MBSong(string sourceFileUrl)
        {
            File = sourceFileUrl;
            Name = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.TrackTitle);
            SortTitle = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.SortTitle);
            AlbumName = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Album);
            AlbumArtist = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.AlbumArtist);
            Artist = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artists);
            PlayCount = mbApiInterface.Library_GetFileTag(sourceFileUrl, (MetaDataType)FilePropertyType.PlayCount);
        }
    }
}
