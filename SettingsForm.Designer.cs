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
            this.buttonOpenSettingsFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.queryMultipleArtistsCB = new System.Windows.Forms.CheckBox();
            this.querySortTitleCB = new System.Windows.Forms.CheckBox();
            this.queryAlbumArtistCB = new System.Windows.Forms.CheckBox();
            this.usernameTB = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelVersionInfo = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(101, 288);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.labelVersionInfo);
            this.panel1.Controls.Add(this.buttonOpenSettingsFolder);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.queryMultipleArtistsCB);
            this.panel1.Controls.Add(this.querySortTitleCB);
            this.panel1.Controls.Add(this.queryAlbumArtistCB);
            this.panel1.Controls.Add(this.usernameTB);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 237);
            this.panel1.TabIndex = 1;
            // 
            // buttonOpenSettingsFolder
            // 
            this.buttonOpenSettingsFolder.Location = new System.Drawing.Point(15, 199);
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
            // queryMultipleArtistsCB
            // 
            this.queryMultipleArtistsCB.AutoSize = true;
            this.queryMultipleArtistsCB.Location = new System.Drawing.Point(15, 124);
            this.queryMultipleArtistsCB.Name = "queryMultipleArtistsCB";
            this.queryMultipleArtistsCB.Size = new System.Drawing.Size(303, 17);
            this.queryMultipleArtistsCB.TabIndex = 3;
            this.queryMultipleArtistsCB.Text = "Query every artist when multiple values of Artist are present";
            this.queryMultipleArtistsCB.UseVisualStyleBackColor = true;
            // 
            // querySortTitleCB
            // 
            this.querySortTitleCB.AutoSize = true;
            this.querySortTitleCB.Location = new System.Drawing.Point(15, 101);
            this.querySortTitleCB.Name = "querySortTitleCB";
            this.querySortTitleCB.Size = new System.Drawing.Size(272, 17);
            this.querySortTitleCB.TabIndex = 2;
            this.querySortTitleCB.Text = "Query Sort Title too when different from Track Name";
            this.querySortTitleCB.UseVisualStyleBackColor = true;
            // 
            // queryAlbumArtistCB
            // 
            this.queryAlbumArtistCB.AutoSize = true;
            this.queryAlbumArtistCB.Location = new System.Drawing.Point(15, 78);
            this.queryAlbumArtistCB.Name = "queryAlbumArtistCB";
            this.queryAlbumArtistCB.Size = new System.Drawing.Size(280, 17);
            this.queryAlbumArtistCB.TabIndex = 1;
            this.queryAlbumArtistCB.Text = "Query Album Artist too when different from Track Artist";
            this.queryAlbumArtistCB.UseVisualStyleBackColor = true;
            // 
            // usernameTB
            // 
            this.usernameTB.Location = new System.Drawing.Point(15, 33);
            this.usernameTB.Name = "usernameTB";
            this.usernameTB.Size = new System.Drawing.Size(191, 20);
            this.usernameTB.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(370, 288);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelVersionInfo
            // 
            this.labelVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersionInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelVersionInfo.Location = new System.Drawing.Point(450, 17);
            this.labelVersionInfo.Name = "labelVersionInfo";
            this.labelVersionInfo.Size = new System.Drawing.Size(82, 13);
            this.labelVersionInfo.TabIndex = 6;
            this.labelVersionInfo.Text = "labelVersionInfo";
            this.labelVersionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(422, 36);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(110, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "MusicBee Forum Post";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(559, 345);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Name = "SettingsForm";
            this.Text = "Sync LastFm Playcount Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox queryMultipleArtistsCB;
        private System.Windows.Forms.CheckBox querySortTitleCB;
        private System.Windows.Forms.CheckBox queryAlbumArtistCB;
        private System.Windows.Forms.TextBox usernameTB;
        private System.Windows.Forms.Button buttonOpenSettingsFolder;
        private System.Windows.Forms.Label labelVersionInfo;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}