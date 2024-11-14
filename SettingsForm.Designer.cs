namespace MusicBeePlugin
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUpdateMode = new System.Windows.Forms.ComboBox();
            this.nudIgnoreWhenLower = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSyncRecentTracks = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.labelVersionInfo = new System.Windows.Forms.Label();
            this.buttonOpenSettingsFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSyncLovedTracks = new System.Windows.Forms.CheckBox();
            this.cbQueryMultipleArtists = new System.Windows.Forms.CheckBox();
            this.cbQuerySortTitle = new System.Windows.Forms.CheckBox();
            this.cbQueryAlbumArtist = new System.Windows.Forms.CheckBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.cbQueryRecentOnStartup = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIgnoreWhenLower)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(105, 351);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbQueryRecentOnStartup);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbUpdateMode);
            this.panel1.Controls.Add(this.nudIgnoreWhenLower);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.buttonSyncRecentTracks);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.labelVersionInfo);
            this.panel1.Controls.Add(this.buttonOpenSettingsFolder);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbSyncLovedTracks);
            this.panel1.Controls.Add(this.cbQueryMultipleArtists);
            this.panel1.Controls.Add(this.cbQuerySortTitle);
            this.panel1.Controls.Add(this.cbQueryAlbumArtist);
            this.panel1.Controls.Add(this.tbUsername);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 333);
            this.panel1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(297, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "MusicBee\'s playcount will be updated when other than 0 and:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Update when";
            // 
            // cbUpdateMode
            // 
            this.cbUpdateMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUpdateMode.FormattingEnabled = true;
            this.cbUpdateMode.Items.AddRange(new object[] {
            "playcounts differ",
            "LastFm playcount is higher that MusicBee\'s"});
            this.cbUpdateMode.Location = new System.Drawing.Point(102, 219);
            this.cbUpdateMode.Name = "cbUpdateMode";
            this.cbUpdateMode.Size = new System.Drawing.Size(278, 21);
            this.cbUpdateMode.TabIndex = 13;
            // 
            // nudIgnoreWhenLower
            // 
            this.nudIgnoreWhenLower.Location = new System.Drawing.Point(226, 243);
            this.nudIgnoreWhenLower.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudIgnoreWhenLower.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudIgnoreWhenLower.Name = "nudIgnoreWhenLower";
            this.nudIgnoreWhenLower.Size = new System.Drawing.Size(60, 20);
            this.nudIgnoreWhenLower.TabIndex = 12;
            this.nudIgnoreWhenLower.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Ignore when LastFm count is lower than";
            // 
            // buttonSyncRecentTracks
            // 
            this.buttonSyncRecentTracks.AccessibleDescription = "Update the playcount of the most recent LastFm scrobbles";
            this.buttonSyncRecentTracks.Location = new System.Drawing.Point(359, 296);
            this.buttonSyncRecentTracks.Name = "buttonSyncRecentTracks";
            this.buttonSyncRecentTracks.Size = new System.Drawing.Size(164, 23);
            this.buttonSyncRecentTracks.TabIndex = 8;
            this.buttonSyncRecentTracks.Text = "Sync Last Month Scrobbles";
            this.buttonSyncRecentTracks.UseVisualStyleBackColor = true;
            this.buttonSyncRecentTracks.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(407, 36);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(116, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "MusicBee Forum Topic";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labelVersionInfo
            // 
            this.labelVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersionInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelVersionInfo.Location = new System.Drawing.Point(441, 17);
            this.labelVersionInfo.Name = "labelVersionInfo";
            this.labelVersionInfo.Size = new System.Drawing.Size(82, 13);
            this.labelVersionInfo.TabIndex = 6;
            this.labelVersionInfo.Text = "labelVersionInfo";
            this.labelVersionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonOpenSettingsFolder
            // 
            this.buttonOpenSettingsFolder.Location = new System.Drawing.Point(15, 296);
            this.buttonOpenSettingsFolder.Name = "buttonOpenSettingsFolder";
            this.buttonOpenSettingsFolder.Size = new System.Drawing.Size(130, 23);
            this.buttonOpenSettingsFolder.TabIndex = 5;
            this.buttonOpenSettingsFolder.Text = "Open Settings Folder";
            this.buttonOpenSettingsFolder.UseVisualStyleBackColor = true;
            this.buttonOpenSettingsFolder.Click += new System.EventHandler(this.buttonOpenSettingsFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Last.fm username";
            // 
            // cbSyncLovedTracks
            // 
            this.cbSyncLovedTracks.AutoSize = true;
            this.cbSyncLovedTracks.Location = new System.Drawing.Point(15, 147);
            this.cbSyncLovedTracks.Name = "cbSyncLovedTracks";
            this.cbSyncLovedTracks.Size = new System.Drawing.Size(191, 17);
            this.cbSyncLovedTracks.TabIndex = 3;
            this.cbSyncLovedTracks.Text = "Sync \"loved track\" ♥ from Last.fm";
            this.cbSyncLovedTracks.UseVisualStyleBackColor = true;
            // 
            // cbQueryMultipleArtists
            // 
            this.cbQueryMultipleArtists.AutoSize = true;
            this.cbQueryMultipleArtists.Location = new System.Drawing.Point(15, 124);
            this.cbQueryMultipleArtists.Name = "cbQueryMultipleArtists";
            this.cbQueryMultipleArtists.Size = new System.Drawing.Size(303, 17);
            this.cbQueryMultipleArtists.TabIndex = 3;
            this.cbQueryMultipleArtists.Text = "Query every artist when multiple values of Artist are present";
            this.cbQueryMultipleArtists.UseVisualStyleBackColor = true;
            // 
            // cbQuerySortTitle
            // 
            this.cbQuerySortTitle.AutoSize = true;
            this.cbQuerySortTitle.Location = new System.Drawing.Point(15, 101);
            this.cbQuerySortTitle.Name = "cbQuerySortTitle";
            this.cbQuerySortTitle.Size = new System.Drawing.Size(272, 17);
            this.cbQuerySortTitle.TabIndex = 2;
            this.cbQuerySortTitle.Text = "Query Sort Title too when different from Track Name";
            this.cbQuerySortTitle.UseVisualStyleBackColor = true;
            // 
            // cbQueryAlbumArtist
            // 
            this.cbQueryAlbumArtist.AutoSize = true;
            this.cbQueryAlbumArtist.Location = new System.Drawing.Point(15, 78);
            this.cbQueryAlbumArtist.Name = "cbQueryAlbumArtist";
            this.cbQueryAlbumArtist.Size = new System.Drawing.Size(280, 17);
            this.cbQueryAlbumArtist.TabIndex = 1;
            this.cbQueryAlbumArtist.Text = "Query Album Artist too when different from Track Artist";
            this.cbQueryAlbumArtist.UseVisualStyleBackColor = true;
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(15, 33);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(191, 20);
            this.tbUsername.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(369, 351);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // cbQueryRecentOnStartup
            // 
            this.cbQueryRecentOnStartup.AutoSize = true;
            this.cbQueryRecentOnStartup.Location = new System.Drawing.Point(15, 170);
            this.cbQueryRecentOnStartup.Name = "cbQueryRecentOnStartup";
            this.cbQueryRecentOnStartup.Size = new System.Drawing.Size(169, 17);
            this.cbQueryRecentOnStartup.TabIndex = 16;
            this.cbQueryRecentOnStartup.Text = "Query recent tracks on startup";
            this.cbQueryRecentOnStartup.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(559, 386);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Name = "SettingsForm";
            this.Text = "Sync playcount from Last.fm - Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudIgnoreWhenLower)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbQueryMultipleArtists;
        private System.Windows.Forms.CheckBox cbQuerySortTitle;
        private System.Windows.Forms.CheckBox cbQueryAlbumArtist;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Button buttonOpenSettingsFolder;
        private System.Windows.Forms.Label labelVersionInfo;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox cbSyncLovedTracks;
        private System.Windows.Forms.Button buttonSyncRecentTracks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudIgnoreWhenLower;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbUpdateMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbQueryRecentOnStartup;
    }
}