﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ClientExample.Containers;

namespace ClientExample.Guide
{
    public partial class GuideWindow : Form
    {
        protected bool allowTabChange = false;
        enum Tabs
        {
            Welcome = 0,
            InstallationMode = 1,
            ImportSettings = 2,
            ConnectionDetection = 3,
            Finished = 4
        }

        public GuideWindow()
        {
            InitializeComponent();
            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);
        }

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == (int)Tabs.Finished)
            {
                btnNext.Text = "Start Xmpl";
            }
            e.Cancel = !allowTabChange;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex + 1 < tabControl1.TabCount)
            {
                // TODO : Do current index allow change?
                switch ((Tabs)tabControl1.SelectedIndex)
                {
                    case Tabs.Welcome:
                        allowTabChange = true;
                        break;
                    case Tabs.InstallationMode:
                        #region Installation Mode
                        switch (ucInstallationMode1.Mode)
                        {
                            case 0:     // Simple mode
                                allowTabChange = true;
                                break;
                            case 1:     // Advanced mode (Close Guide)
                                allowTabChange = true;
                                tabControl1.SelectedIndex = (int)Tabs.Finished;
                                return;
                        }
                        break;
                        #endregion
                    case Tabs.ImportSettings:
                        #region Import Settings
                        List<string> list = new List<string>();
                        if (!allowTabChange)
                        {
                            switch (ucImport1.Mode)
                            {
                                case 0:     // Search for installation path
                                    Import.Search search = new Import.Search();
                                    search.ShowDialog();
                                    if (search.List.Count > 0)
                                    {
                                        list.AddRange(search.List);
                                    }
                                    break;
                                case 1:     // Select installation path
                                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                                    {
                                        Import.Search search2 = new Import.Search();
                                        search2.StartDirectory = folderBrowserDialog1.SelectedPath;
                                        search2.ShowDialog();
                                        if (search2.List.Count > 0)
                                        {
                                            list.AddRange(search2.List);
                                        }
                                        //importPath = folderBrowserDialog1.SelectedPath;
                                    }
                                    break;
                                case 2:     // Do not import
                                    allowTabChange = true;
                                    break;
                                default:
                                    break;
                            }

                            if (list.Count > 0)
                            {
                                Import.SelectClient select = new Import.SelectClient();
                                select.Files = list;
                                if (select.ShowDialog() == DialogResult.OK)
                                {
                                    Program.Settings.SavedHubs = select.Settings;
                                    Save();
                                    allowTabChange = true;
                                }
                            }
                        }
                        break;
                        #endregion
                    case Tabs.ConnectionDetection:
                        switch (ucConnection1.Mode)
                        {
                            case 0:
                                Connection.Detect detectConnection = new Connection.Detect();
                                detectConnection.ShowDialog();
                                if (detectConnection.Mode != 0)
                                {
                                    allowTabChange = true;
                                }
                                Program.Settings.ConnectionMode = ucConnection1.Mode;
                                Save();
                                break;
                            case 1:
                                // TODO : Add settings here
                                allowTabChange = true;
                                break;
                            case 2:
                                Program.Settings.ConnectionMode = 1;
                                Save();
                                allowTabChange = true;
                                break;
                        }
                        break;
                    //case Tabs.Finished:
                        //Program.Settings.Installed = true;
                        //Save();
                        //this.Close();
                        //return;
                }
                if (allowTabChange)
                    tabControl1.SelectedIndex += 1;
                allowTabChange = false;
            }
            else
            {
                Program.Settings.Installed = true;
                Save();
                this.Close();
            }
        }

        public void Save()
        {
            try
            {
                FlowLib.Utils.FileOperations<AppSetting>.SaveObject(AppSetting.GetSettingsFile(), Program.Settings);
            }
            catch { }
        }
    }
}
