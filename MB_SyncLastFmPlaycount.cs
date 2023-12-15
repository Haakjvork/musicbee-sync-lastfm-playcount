using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public static MusicBeeApiInterface mbApiInterface;

        private PluginInfo about = new PluginInfo();
        private LastfmClient lastFmClient;
        private Config config;

        private TextBox usernameTB;
        private CheckBox queryAlbumArtistCB;
        private CheckBox querySortTitleCB;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "MB_SyncLastFmPlaycount";
            about.Description = "Synchronizes MusicBee playcount with LastFm data";
            about.Author = "Yago Fernández-Valladares";
            about.TargetApplication = "";   //  the name of a Plugin Storage device or panel header for a dockable panel
            about.Type = PluginType.General;
            about.VersionMajor = 1;  // your plugin version
            about.VersionMinor = 0;
            about.Revision = 2;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.StartupOnly);
            about.ConfigurationPanelHeight = 66;  // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            lastFmClient = new LastfmClient(Private.LAST_FM_API_KEY, Private.LAST_FM_API_SECRET);
            // Read the user token from settings.
            config = new Config();
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
                List<Control> controls = new List<Control>();
                usernameTB = new TextBox()
                {
                    Text = config.settings.Username
                };
                PositionLabelControl(controls, "Last.fm username", usernameTB, 0);
                queryAlbumArtistCB = new CheckBox()
                {
                    Checked = config.settings.QueryAlbumArtist
                };
                PositionLabelControl(controls, "Query Album Artist too when different from Track Artist", queryAlbumArtistCB, 1);
                querySortTitleCB = new CheckBox()
                {
                    Checked = config.settings.QuerySortTitle
                };
                PositionLabelControl(controls, "Query Sort Title too when different from Track Name", querySortTitleCB, 2);
                configPanel.Controls.AddRange(controls.ToArray());
            }
            return false;
        }

        private void PositionLabelControl(List<Control> controls, string label, Control control, int row)
        {
            Label prompt = new Label();
            prompt.Text = label;
            int y = row * 20 + row*2; //Padding: 2
            prompt.AutoSize = true;
            prompt.Location = new Point(0, y+2 );
            int offset = (20 - control.Height)/2;
            control.Bounds = new Rectangle(350, y+offset, 100, control.Height);
            controls.Add(prompt);
            controls.Add(control);
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            try
            {
                config.Log("SaveSettings");
                config.settings.Username = usernameTB.Text;
                config.settings.QueryAlbumArtist = queryAlbumArtistCB.Checked;
                config.settings.QuerySortTitle = querySortTitleCB.Checked;

                config.Save();
            }
            catch (Exception e)
            {
                var error = String.Concat("ERROR: ", e.Message);
                config.Log(error);
                config.Log(e.StackTrace);
            }
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            config.Log("-- Close --");
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            
        }

        // receive event notifications from MusicBee
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.PluginStartup:
                    config.Log("-- Startup --");
                    mbApiInterface.MB_AddMenuItem($"context.Main/Sync Playcount from LastFm", "Sync Playcount from LastFm", SyncPlaycountFromLastFm);
                    break;
            }
        }

        public async void SyncPlaycountFromLastFm(object sender, EventArgs args)
        {
            if ( String.IsNullOrEmpty(config.settings.Username) )
            {
                mbApiInterface.MB_SetBackgroundTaskMessage(String.Concat("Can't sync with LastFm: Empty username"));
                return;
            }
            
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            if (files == null) return;

            try
            {
                List<MBSong> songs = files.Select(f => new MBSong(f)).ToList();
                for (int i = 0; i < songs.Count; i++)
                {
                    MBSong song = songs[i];
                    mbApiInterface.MB_SetBackgroundTaskMessage(String.Concat(i + 1, " of ", songs.Count, ": ", song.Artist, " - ", song.Name));
                    int playcount = await QueryTrackPlaycount(song);
                    if (playcount > 1)
                    {
                        mbApiInterface.Library_SetFileTag(song.File, (MetaDataType)FilePropertyType.PlayCount, playcount.ToString());
                        mbApiInterface.Library_CommitTagsToFile(song.File);
                    }

                }
                mbApiInterface.MB_SetBackgroundTaskMessage(String.Concat("Finished syncing ", songs.Count, " songs"));
            }
            catch (Exception e)
            {
                var error = String.Concat("ERROR: ", e.Message);
                config.Log(error);
                config.Log(e.StackTrace);
                mbApiInterface.MB_SetBackgroundTaskMessage(error);
            }
            mbApiInterface.MB_RefreshPanels();

        }


        private async Task<int> QueryTackInfo(string name,string artist)
        {
            config.Log(String.Concat("TrackInfo ", artist, " - ", name));
            var res = await lastFmClient.Track.GetInfoAsync(name, artist, config.settings.Username);
            if ( res.Status == IF.Lastfm.Core.Api.Enums.LastResponseStatus.Successful )
            {
                var pc = (int)res.Content.UserPlayCount;
                config.Log(String.Concat("TrackInfo ", artist, " - ", name, " = ", pc));
                return pc;
            }
            else
            {
                config.Log(String.Concat("TrackInfo ERROR ", artist, " - ", name, " = ", res.Status ));
                return 0;
            }
            
        }

        private async Task<int> QueryTrackPlaycount(MBSong song)
        {
            List<string> names = new List<string>();
            List<string> artists = new List<string>();
            names.Add(song.Name);
            artists.Add(song.Artist);
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
            if (config.settings.QueryAlbumArtist && !String.IsNullOrEmpty(song.AlbumArtist) && !String.Equals(song.Artist.ToLower(), song.AlbumArtist.ToLower()))
            {
                artists.Add(song.AlbumArtist);
            }
            //Queries
            int old = Int32.Parse(song.PlayCount);
            var neu = 0;
            foreach (string name in names)
            {
                foreach (string artist in artists)
                {
                    neu += await QueryTackInfo(name, artist);
                }
            }
            //Results
            int dif = neu - old;
            var warn = "==";
            if (dif > 0)
            {
                warn = String.Concat("+", dif.ToString());
            }
            if ( dif < 0)
            {
                warn = dif.ToString();
            }
            if ( dif >= 10 || dif <= -10 )
            {
                warn = String.Concat(warn," HUGE");
            }
            if ( neu <= 1)
            {
                warn = " IGNORE";
            }
            config.Log(String.Concat(old, " -> ", neu, " ", warn));
            return neu;
        }

    }
}