﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace stretch_ceilings_app
{
    public partial class FactoriesForm : Form
    {
        public FactoriesForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new FactoryForm().ShowDialog();
        }
    }
}