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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public static MusicBeeApiInterface mbApiInterface;

        private PluginInfo about = new PluginInfo();
        private LastfmClient lastFmClient;
        private Config config;
        private LastFMService lastFMService;

        private Form settingsForm;

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
            about.VersionMinor = 2;
            about.Revision = 0;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.StartupOnly);
            about.ConfigurationPanelHeight = 0;  // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            lastFmClient = new LastfmClient(Private.LAST_FM_API_KEY, Private.LAST_FM_API_SECRET);
            // Read the user token from settings.
            config = new Config();
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
            }
            //Click on default MusicBee plugin button
            if (this.settingsForm == null || !this.settingsForm.Visible )
            {
                this.settingsForm = new SettingsForm(about,config,lastFmClient);
            }
            settingsForm.ShowDialog();
            //True to avoid About box
            return true;
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
                    ToolStripMenuItem menu = (ToolStripMenuItem) mbApiInterface.MB_AddMenuItem($"context.Main/Sync From LastFm", null, null);
                    menu.DropDown.Items.Add($"Update playcount of selected files",null,SyncPlaycountFromSelectedFiles);
                    menu.DropDown.Items.Add($"Update playcount of recent scrobbles",null,SyncPlaycountFromRecentScrobbles);
                    break;
            }
        }

        public async void SyncPlaycountFromSelectedFiles(object sender, EventArgs args)
        {
            if ( String.IsNullOrEmpty(config.settings.Username) )
            {
                mbApiInterface.MB_SetBackgroundTaskMessage(String.Concat("Can't sync with LastFm: Empty username"));
                return;
            }
            if (lastFMService == null)
            {
                lastFMService = new LastFMService(config,lastFmClient);
            }
            mbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files);
            await lastFMService.SyncFilesPlaycount(files);

        }

        public async void SyncPlaycountFromRecentScrobbles(object sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(config.settings.Username))
            {
                mbApiInterface.MB_SetBackgroundTaskMessage(String.Concat("Can't sync with LastFm: Empty username"));
                return;
            }
            if (lastFMService == null)
            {
                lastFMService = new LastFMService(config, lastFmClient);
            }
            await lastFMService.SyncByRecentScrobbles(false);

        }

    }
}