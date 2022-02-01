﻿using StretchCeilings.Helpers;
using StretchCeilings.Helpers.Controls;
using StretchCeilings.Helpers.Enums;
using StretchCeilings.Helpers.Extensions;
using StretchCeilings.Helpers.Extensions.Controls;
using StretchCeilings.Models;
using StretchCeilings.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms;

namespace StretchCeilings.Views
{
    public partial class ManufacturersForm : Form
    {
        private List<Manufacturer> _manufacturers;

        private Manufacturer _manufacturer;
        private Manufacturer _filter;

        private readonly bool _forSearching;
        private int _rows;
        private int _count;
        private int _currentPage = 1;
        private int _maxPage = 1;

        public ManufacturersForm(bool forSearching = false)
        {
            _forSearching = forSearching;
            InitializeComponent();
        }

        public Manufacturer GetManufacturer() => _manufacturer;

        private void SetUpDataGrid()
        {
            _manufacturers = ManufacturerModelsRepository.GetAll(out _rows);

            dgvManufacturers.AddDataGridViewTextBoxColumn("№", DataGridViewAutoSizeColumnMode.DisplayedCells);
            dgvManufacturers.AddDataGridViewTextBoxColumn("Производитель", DataGridViewAutoSizeColumnMode.Fill);
            dgvManufacturers.AddDataGridViewTextBoxColumn("Адрес", DataGridViewAutoSizeColumnMode.Fill);
            dgvManufacturers.AddDataGridViewTextBoxColumn("Страна", DataGridViewAutoSizeColumnMode.DisplayedCells);
            dgvManufacturers.AddDataGridViewButtonColumn(DraculaColor.Red);
            dgvManufacturers.Font = GoogleFont.OpenSans;
            dgvManufacturers.DefaultCellStyle.SelectionBackColor = DraculaColor.Selection;
            dgvManufacturers.DefaultCellStyle.SelectionForeColor = DraculaColor.Foreground;
            dgvManufacturers.CellClick += RemoveGridRow;
            dgvManufacturers.CellDoubleClick += OpenManufacturerForm;

            FillDataGrid();
        }

        private static bool HasUserPermissions()
            => UserSession.IsAdmin || UserSession.Can(PermissionCode.AddCustomer);

        private void DrawAddCustomerButton()
        {
            var btnAddManufacturer = new FlatButton("btnAddOrder", "Добавить", btnAddManufacturer_Click);
            panelUserButtons.Controls.Add(btnAddManufacturer);
        }

        private void FillCountryComboBox()
        {
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                if (country == Country.Unknown) continue;
                var item = new ComboBoxItem()
                {
                    Content = country.ParseString(),
                    Tag = country
                };
                cbCountry.Items.Add(item);
            }
            cbCountry.DisplayMember = "Content";
            cbCountry.SelectedItem = null;
        }

        private void FillRowsComboBox()
        {
            foreach (var rowCountItem in Constants.RowCountItems)
                cbRows.Items.Add(rowCountItem);
            cbRows.SelectedItem = cbRows.Items[0];
        }

        private void SetUpControls()
        {
            btnResetFilters.FlatAppearance.MouseOverBackColor = DraculaColor.Red;
            btnNext.Click += ShowNextPage;
            btnPrevious.Click += ShowPreviousPage;

            if (HasUserPermissions())
                DrawAddCustomerButton();

            FillCountryComboBox();
            FillRowsComboBox();
            UpdatePageTextBox();
        }


        private void FillDataGrid()
        {
            dgvManufacturers.Rows.Clear();

            for (var i = 0; i < _manufacturers?.Count; i++)
            {
                dgvManufacturers.Rows.Add(new DataGridViewRow());

                dgvManufacturers.Rows[i].Cells[0].Value = _manufacturers[i].Id;
                dgvManufacturers.Rows[i].Cells[1].Value = _manufacturers[i].Name;
                dgvManufacturers.Rows[i].Cells[2].Value = _manufacturers[i].Address;
                dgvManufacturers.Rows[i].Cells[3].Value = _manufacturers[i].Country?.ParseString();
            }

            _maxPage = (int)Math.Ceiling((double)_rows / _count);
            UpdatePageTextBox();
        }

        private void FilterDataGrid()
        {
            _manufacturers = ManufacturerModelsRepository.GetAll(
                _filter,
                _count,
                _currentPage,
                out _rows);

            FillDataGrid();
        }

        private void ResetFilters()
        {
            _filter = new Manufacturer() { Country = Country.Unknown };
            _currentPage = 1;
            _maxPage = 1;
            tbAddress.Text = Constants.DefaultTextBoxText;
            tbName.Text = Constants.DefaultTextBoxText;

            cbCountry.SelectedItem = null;

            FilterDataGrid();
        }

        private void RemoveGridRow(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvManufacturers.Columns[" "]?.Index) return;

            var order = OrderModelsRepository.GetById((int)dgvManufacturers.SelectedRows[0].Cells["№"].Value);

            order.Delete();

            FilterDataGrid();
        }

        private void OpenManufacturerForm(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvManufacturers.SelectedRows.Count <= 0 || e.RowIndex < 0 || e.RowIndex < 0) return;

            var manufacturer = ManufacturerModelsRepository.GetById((int)dgvManufacturers.SelectedRows[0].Cells[0].Value);
            new ManufacturerForm(manufacturer).ShowDialog();
            FilterDataGrid();
        }

        private void UpdatePageTextBox()
        {
            if (_maxPage == 0)
                _currentPage = 0;
            tbPages.UpdatePagesValue(_currentPage, _maxPage);
        }

        private void ShowNextPage(object sender, EventArgs e)
        {
            if (_currentPage >= _maxPage) return;

            _currentPage++;
            UpdatePageTextBox();
            FilterDataGrid();
        }

        private void ShowPreviousPage(object sender, EventArgs e)
        {
            if (_currentPage <= 1) return;

            _currentPage--;
            UpdatePageTextBox();
            FilterDataGrid();
        }

        private void btnAddManufacturer_Click(object sender, EventArgs e)
        {
            var form = new ManufacturerEditForm(new Manufacturer());
            if (form.ShowDialog() == DialogResult.OK)
                FilterDataGrid();
        }

        private void btnUserFilters_Click(object sender, EventArgs e)
        {
            FilterDataGrid();
        }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }

        private void ManufacturersForm_Load(object sender, EventArgs e)
        {
            _filter = new Manufacturer() { Country = Country.Unknown };
            SetUpDataGrid();
            SetUpControls();
        }

        private void cbRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            _count = int.Parse(cbRows.Items[cbRows.SelectedIndex].ToString());
            FilterDataGrid();
        }

        private void cbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ComboBoxItem item in cbCountry.Items)
            {
                if (item == cbCountry.SelectedItem) _filter.Country = (Country)item.Tag;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (tbName.Text.Length > 0) _filter.Name = tbName.Text;
        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {
            if (tbAddress.Text.Length > 0) _filter.Address = tbAddress.Text;
        }
    }
}
