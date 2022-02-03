﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StretchCeilings.Helpers;
using StretchCeilings.Helpers.Controls;
using StretchCeilings.Helpers.Enums;
using StretchCeilings.Helpers.Extensions.Controls;
using StretchCeilings.Models;
using StretchCeilings.Repositories;

namespace StretchCeilings.Views
{
    public partial class AdditionalServicesForm : Form
    {
        private List<AdditionalService> _additionalServices;

        private AdditionalService _firstFilter;
        private AdditionalService _secondsFilter;

        private int _rowsCount = 0;
        private int _currentPage = 1;
        private int _maxPage = 1;
        private int _count;
        
        private readonly bool _forSearching;

        public AdditionalServicesForm(bool forSearching = false)
        {
            _forSearching = forSearching;
            InitializeComponent();
        }

        private void SetUpDataGrid()
        {
            _additionalServices = AdditionalServiceRepository.GetAll(out _rowsCount);

            dgvAdditionalServices.AddDataGridViewTextBoxColumn("№", DataGridViewAutoSizeColumnMode.DisplayedCells);
            dgvAdditionalServices.AddDataGridViewTextBoxColumn("Название", DataGridViewAutoSizeColumnMode.Fill);
            dgvAdditionalServices.AddDataGridViewTextBoxColumn("Цена", DataGridViewAutoSizeColumnMode.Fill);
            dgvAdditionalServices.AddDataGridViewButtonColumn(DraculaColor.Red);

            dgvAdditionalServices.Font = GoogleFont.OpenSans;
            dgvAdditionalServices.DefaultCellStyle.SelectionBackColor = DraculaColor.Selection;
            dgvAdditionalServices.DefaultCellStyle.SelectionForeColor = DraculaColor.Foreground;
            dgvAdditionalServices.CellClick += dgvAdditionalServices_CellClick;

            FillDataGrid();
        }

        private void SetUpControls()
        {
            if (UserSession.Can(PermissionCode.AddAdditionalService) || UserSession.IsAdmin)
            {
                var btnAddService = new FlatButton("btnAddAdditionalService", "Добавить", btnAddAdditionalService_Click);
                panelUserButtons.Controls.Add(btnAddService);
            }

            nudId.Maximum = decimal.MaxValue;
            nudTotalFrom.Maximum = decimal.MaxValue;
            nudTotalTo.Maximum = decimal.MaxValue;

            foreach (var item in Constants.RowCountItems)
                cbRows.Items.Add(item);
            cbRows.SelectedItem = cbRows.Items[0];

            UpdatePagesTextBox();

            btnResetFilters.FlatAppearance.MouseOverBackColor = DraculaColor.Red;
        }

        private void OpenServiceForm(DataGridViewCellEventArgs e)
        {
            if (dgvAdditionalServices.SelectedRows.Count <= 0 || e.RowIndex < 0) return;

            var service = AdditionalServiceRepository.GetById((int)dgvAdditionalServices.SelectedRows[0].Cells[0].Value);
            new AdditionalServiceForm(service).ShowDialog();
            FilterDataGrid();
        }

        private void FilterDataGrid()
        {
            if (nudTotalFrom.Value > nudTotalTo.Value)
            {
                CustomMessageBox.Show("Неверно указан ценовой диапозон", CustomMessageBoxCaption.Error);
                return;
            }
            _additionalServices = AdditionalServiceRepository.GetAll(
                _firstFilter,
                _secondsFilter,
                _count,
                _currentPage,
                out _rowsCount);

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            dgvAdditionalServices.Rows.Clear();

            for (var i = 0; i < _additionalServices?.Count; i++)
            {
                dgvAdditionalServices.Rows.Add(new DataGridViewRow());

                dgvAdditionalServices.Rows[i].Cells[0].Value = _additionalServices[i].Id;
                dgvAdditionalServices.Rows[i].Cells[1].Value = _additionalServices[i].Name;
                dgvAdditionalServices.Rows[i].Cells[2].Value = _additionalServices[i].Price;
            }

            _maxPage = (int)Math.Ceiling((double)_rowsCount / _count);
            UpdatePagesTextBox();
        }

        private void ResetFilters()
        {
            _currentPage = 1;
            _maxPage = 1;

            _firstFilter = new AdditionalService();
            _secondsFilter = new AdditionalService();

            nudTotalFrom.Value = Constants.DefaultNumericUpDownValue;
            nudTotalTo.Value = Constants.DefaultNumericUpDownValue;
            nudId.Value = Constants.DefaultNumericUpDownValue;
            tbName.Text = Constants.DefaultTextBoxText;
            
            _additionalServices = AdditionalServiceRepository.GetAll(out _rowsCount);

            FillDataGrid();
        }

        private void AddAdditionalService()
        {
            var service = new AdditionalServiceEditForm(new AdditionalService(), true);
            if (service.ShowDialog() == DialogResult.OK) FilterDataGrid();
        }

        private void UpdatePagesTextBox()
        {
            if (_maxPage == 0)
                _currentPage = 0;
            tbPage.UpdatePagesValue(_currentPage, _maxPage);
        }

        private void UpdateComboBoxRow()
        {
            _currentPage = 1;
            _count = int.Parse(cbRows.Items?[cbRows.SelectedIndex].ToString());
            FilterDataGrid();
        }

        private void ShowNextPage()
        {
            if (_currentPage >= _maxPage) return;
            
            _currentPage++;
            UpdatePagesTextBox();
            FilterDataGrid();
        }

        private void ShowPreviousPage()
        {
            if (_currentPage <= 1) return;

            _currentPage--;
            UpdatePagesTextBox();
            FilterDataGrid();
        }

        private void AdditionalServicesForm_Load(object sender, EventArgs e)
        {
            _firstFilter = new AdditionalService();
            _secondsFilter = new AdditionalService();

            SetUpDataGrid();
            SetUpControls();
        }

        private void btnAddAdditionalService_Click(object sender, EventArgs e)
        {
            AddAdditionalService();
        }

        private void btnUseFilters_Click(object sender, EventArgs e)
        {
            FilterDataGrid();
        }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }

        private void cbRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxRow();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            ShowNextPage();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            ShowPreviousPage();
        }

        private void dgvAdditionalServices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvAdditionalServices.Columns[" "]?.Index) return;

            var service = AdditionalServiceRepository.GetById((int)dgvAdditionalServices.SelectedRows[0].Cells["№"].Value);

            service.Delete();

            FilterDataGrid();
        }

        private void dgvAdditionalServices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenServiceForm(e);
        }

        private void nudIdValue_ValueChanged(object sender, EventArgs e)
        {
            _firstFilter.Id = (int)nudId.Value;
        }

        private void tbNameValue_TextChanged(object sender, EventArgs e)
        {
            _firstFilter.Name = tbName.Text;
        }

        private void nudTotalFrom_ValueChanged(object sender, EventArgs e)
        {
            _firstFilter.Price = nudTotalFrom.Value;
        }

        private void nudTotalTo_ValueChanged(object sender, EventArgs e)
        {
            _secondsFilter.Price = nudTotalTo.Value;
        }
    }
}