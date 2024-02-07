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

namespace MusicBeePlugin
{
    public partial class SettingsForm : Form
    {

        private readonly Config config;

        public SettingsForm(Config config)
        {
            this.config = config;
            InitializeComponent();
            usernameTB.Text = config.settings.Username;
            queryAlbumArtistCB.Checked = config.settings.QueryAlbumArtist;
            querySortTitleCB.Checked = config.settings.QuerySortTitle;
            queryMultipleArtistsCB.Checked = config.settings.QueryMultipleArtists;
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
    }
}
