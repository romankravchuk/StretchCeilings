﻿using System;
using System.Windows.Forms;
using stretch_ceilings_app.Data.Models;

namespace stretch_ceilings_app.Forms
{
    public partial class AdditionalServiceFormEdit : Form
    {
        private readonly AdditionalService _currentService;
        public AdditionalServiceFormEdit(AdditionalService service)
        {
            _currentService = service;
            InitializeComponent();
        }

        private void SaveChanges()
        {
            _currentService.Name = tbName.Text;
            _currentService.Price = nudPrice.Value;
            _currentService.Update();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
            this.Close();
        }
    }
}