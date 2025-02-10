using System;
using System.Diagnostics;
using System.Windows.Forms;
using IF.Lastfm.Core.Api;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin {
    public partial class SettingsForm : Form {

        private static readonly string FORUM_URL = "https://getmusicbee.com/forum/index.php?topic=40383";

        private readonly Config config;
        private readonly PluginInfo about;
        private readonly LastfmClient lastfmClient;

        public SettingsForm(PluginInfo about, Config config, LastfmClient lastfmClient) {
            this.about = about;
            this.config = config;
            this.lastfmClient = lastfmClient;
            InitializeComponent();
            tbUsername.Text = config.settings.Username;
            cbQueryAlbumArtist.Checked = config.settings.QueryAlbumArtist;
            cbQuerySortTitle.Checked = config.settings.QuerySortTitle;
            cbQueryMultipleArtists.Checked = config.settings.QueryMultipleArtists;
            cbSyncLovedTracks.Checked = config.settings.SyncLovedTracks;
            cbQueryRecentOnStartup.Checked = config.settings.QueryRecentOnStartup;
            nudIgnoreWhenLower.Value = config.settings.IgnoreWhenLower;
            cbUpdateMode.SelectedIndex = Math.Min(config.settings.UpdateMode, cbUpdateMode.Items.Count - 1);
            this.labelVersionInfo.Text = "v" + about.VersionMajor + "." + about.VersionMinor + "." + about.Revision;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings() {
            try {
                config.Log("SaveSettings");
                config.settings.Username = tbUsername.Text;
                config.settings.QueryAlbumArtist = cbQueryAlbumArtist.Checked;
                config.settings.QuerySortTitle = cbQuerySortTitle.Checked;
                config.settings.QueryMultipleArtists = cbQueryMultipleArtists.Checked;
                config.settings.SyncLovedTracks = cbSyncLovedTracks.Checked;
                config.settings.QueryRecentOnStartup = cbQueryRecentOnStartup.Checked;
                config.settings.IgnoreWhenLower = (int)nudIgnoreWhenLower.Value;
                config.settings.UpdateMode = cbUpdateMode.SelectedIndex;

                config.SaveSettings();
            } catch (Exception e) {
                var error = String.Concat("ERROR: ", e.Message);
                config.Log(error);
                config.Log(e.StackTrace);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e) {
            this.SaveSettings();
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void buttonOpenSettingsFolder_Click(object sender, EventArgs e) {
            Process.Start(@"" + config.getSubfolderPath());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(@"" + FORUM_URL);
        }

        private async void button1_Click(object sender, EventArgs e) {
            LastFMService service = new LastFMService(config, lastfmClient);
            await service.SyncByRecentScrobbles(true);
        }

    }

}
