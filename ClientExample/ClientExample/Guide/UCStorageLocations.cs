﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ClientExample.Guide
{
    public partial class UCStorageLocations : UserControl
    {
        public UCStorageLocations()
        {
            InitializeComponent();
        }

        private void UCStorageLocations_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(typeof(Program), @"Images.flowlib-main.jpg");
        }
    }
}
