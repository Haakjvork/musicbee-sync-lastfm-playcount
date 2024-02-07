using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class SettingsForm : Form
    {

        private static readonly string FORUM_URL = "https://getmusicbee.com/forum/index.php?topic=40383";

        private readonly Config config;
        private readonly PluginInfo about;

        public SettingsForm(PluginInfo about, Config config)
        {
            this.about = about;
            this.config = config;
            InitializeComponent();
            usernameTB.Text = config.settings.Username;
            queryAlbumArtistCB.Checked = config.settings.QueryAlbumArtist;
            querySortTitleCB.Checked = config.settings.QuerySortTitle;
            queryMultipleArtistsCB.Checked = config.settings.QueryMultipleArtists;
            this.labelVersionInfo.Text = "v" + about.VersionMajor + "." + about.VersionMinor + "." + about.Revision;
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
                config.settings.QueryMultipleArtists = queryMultipleArtistsCB.Checked;

                config.Save();
            }
            catch (Exception e)
            {
                var error = String.Concat("ERROR: ", e.Message);
                config.Log(error);
                config.Log(e.StackTrace);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOpenSettingsFolder_Click(object sender, EventArgs e)
        {
            Process.Start(@""+config.getSubfolderPath());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@""+ FORUM_URL);
        }
    }
}
