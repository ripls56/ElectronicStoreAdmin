using System;
using System.Collections.Generic;
using ElectronicStoreAdmin.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;

namespace ElectronicStoreAdmin.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewData.xaml
    /// </summary>
    public partial class ViewData
    {
        public ViewData()
        {
            DataContext = this;

            InitializeComponent();

        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            var items = cb.Items;
            var name = (items[cb.SelectedIndex] as ComboBoxItem).Name;
            switch (name)
            {
                case "Brand":
                    DataGrid.ItemsSource = ApiClient.getInstance().GetBrandsAsync().GetAwaiter().GetResult();
                    break;
                case "Client":
                    DataGrid.ItemsSource = ApiClient.getInstance().GetClientsAsync().GetAwaiter().GetResult();
                    break;
                default:
                    Snackbar.Show();
                    break;
            }
        }

        private void DataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            var dg = (sender as DataGrid);
            var item = dg.CurrentCell.Item;
            var a = item.GetType().GetProperties()[0];
            var id = (int)a.GetValue(item);
            var header = dg.CurrentCell.Column.Header;
            if (item != null)
            {
                foreach (var propertyInfo in item.GetType().GetProperties())
                {
                    if (propertyInfo.Name.Equals(header))
                    {
                        propertyInfo.SetValue(item, (e.EditingElement as TextBox).Text);
                    }
                }
            }
            else
            {
                Snackbar.Show();
            }
        }
    }
}
