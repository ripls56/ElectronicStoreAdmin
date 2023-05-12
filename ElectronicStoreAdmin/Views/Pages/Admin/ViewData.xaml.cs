using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using Wpf.Ui.Controls.NumberBoxControl;
using controls = Wpf.Ui.Controls;

namespace ElectronicStoreAdmin.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewData.xaml
    /// </summary>
    public partial class ViewData
    {
        public delegate void TypeCallBack();

        private Object? lastObject;
        public ViewData()
        {
            DataContext = this;

            InitializeComponent();

        }

        private Type? type;
        List<object> elements = new List<object>();

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh(sender);
        }

        private void AddCheckBoxInLastColumn(DataGrid dg)
        {
            dg.Columns.Insert(0,new DataGridCheckBoxColumn());
        }

        private void Refresh(object sender)
        {
            try
            {
                GeneratePanel.Children.Clear();
                elements.Clear();
                var apiClient = ApiClient.getInstance();
                var cb = sender as ComboBox;
                var items = cb.Items;
                var name = (items[cb.SelectedIndex] as ComboBoxItem)?.Name;
                type = Type.GetType("ElectronicStoreAdmin.Models." + name);
                foreach (var propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        var tb = new controls.TextBox
                        {
                            Name = propertyInfo.Name,
                            Width = 100
                        };
                        GeneratePanel.Children.Add(tb);
                        GeneratePanel.Children.Add(new Rectangle { Width = 10 });
                        elements.Add(tb);
                    }

                    if (propertyInfo.PropertyType == typeof(decimal?) || propertyInfo.PropertyType == typeof(int?))
                    {
                        var nb = new controls.NumberBoxControl.NumberBox
                        {
                            Name = propertyInfo.Name,
                            Text = "0",
                            ClearButtonEnabled = false
                        };
                        GeneratePanel.Children.Add(nb);
                        GeneratePanel.Children.Add(new Rectangle { Width = 10 });
                        elements.Add(nb);
                    }

                }
                switch (name)
                {
                    case "Brand":
                        DataGrid.ItemsSource = apiClient.GetBrandsAsync().GetAwaiter().GetResult();
                        break;
                    case "Client":
                        DataGrid.ItemsSource = apiClient.GetClientsAsync().GetAwaiter().GetResult();
                        break;
                    case "ClientPromocode":
                        DataGrid.ItemsSource = apiClient.GetClientPromocodesAsync.GetAwaiter().GetResult();
                        break;
                    case "DefectInformation":
                        DataGrid.ItemsSource = apiClient.GetDefectInformationsAsync.GetAwaiter().GetResult();
                        break;
                    case "Employee":
                        DataGrid.ItemsSource = apiClient.GetEmployeesAsync().GetAwaiter().GetResult();
                        break;
                    case "Feedback":
                        DataGrid.ItemsSource = apiClient.GetFeedbacksAsync().GetAwaiter().GetResult();
                        break;
                    case "LoyaltyCard":
                        DataGrid.ItemsSource = apiClient.GetLoyaltyCardsAsync().GetAwaiter().GetResult();
                        break;
                    case "Nomenclature":
                        DataGrid.ItemsSource = apiClient.GetNomenclaturesAsync().GetAwaiter().GetResult();
                        break;
                    case "NomenclatureOrder":
                        DataGrid.ItemsSource = apiClient.GetNomenclatureOrdersAsync().GetAwaiter().GetResult();
                        break;
                    case "Order":
                        DataGrid.ItemsSource = apiClient.GetOrdersAsync().GetAwaiter().GetResult();
                        break;
                    case "OrderHistory":
                        DataGrid.ItemsSource = apiClient.GetOrderHistoriesAsync().GetAwaiter().GetResult();
                        break;
                    case "Post":
                        DataGrid.ItemsSource = apiClient.GetPostsAsync().GetAwaiter().GetResult();
                        break;
                    case "ProductCategory":
                        DataGrid.ItemsSource = apiClient.GetProductСategoriesAsync().GetAwaiter().GetResult();
                        break;
                    case "Profile":
                        DataGrid.ItemsSource = apiClient.GetProfilesAsync().GetAwaiter().GetResult();
                        break;
                    case "Promocode":
                        DataGrid.ItemsSource = apiClient.GetPromocodesAsync().GetAwaiter().GetResult();
                        break;
                    case "Stock":
                        DataGrid.ItemsSource = apiClient.GetStocksAsync().GetAwaiter().GetResult();
                        break;
                    case "StoreAddress":
                        DataGrid.ItemsSource = apiClient.GetStoreAddressesAsync().GetAwaiter().GetResult();
                        break;
                    case "Supply":
                        DataGrid.ItemsSource = apiClient.GetSuppliesAsync().GetAwaiter().GetResult();
                        break;
                    case "VendorType":
                        DataGrid.ItemsSource = apiClient.GetVendorTypesAsync().GetAwaiter().GetResult();
                        break;
                    default:
                        Snackbar.Show();
                        break;
                }
                DataGrid.Columns[0].IsReadOnly = true;
                DataGrid.Columns[0].CanUserSort = true;
                AddCheckBoxInLastColumn(DataGrid);
            }
            catch
            {
                DataGrid.Items.Clear();
                DataGrid.Items.Refresh();
                Snackbar.Show();
            }
        }

        private void DataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                var dg = (sender as DataGrid);
                lastObject = dg.CurrentCell.Item;
                //var a = lastObject.GetType().GetProperties()[0];
                //var id = (int)a.GetValue(lastObject);
                var header = dg.CurrentCell.Column.Header;
                if (lastObject != null)
                {
                    foreach (var propertyInfo in lastObject.GetType().GetProperties())
                    {
                        if (propertyInfo.Name.Equals(header))
                        {
                            propertyInfo.SetValue(lastObject, (e.EditingElement as TextBox).Text);
                        }
                    }
                }
                else
                {
                    Snackbar.Show();
                }
            }
            catch
            {
                Snackbar.Show();
            }
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var inst = Activator.CreateInstance(type);
                var i = 0;
                foreach (var propertyInfo in type.GetProperties())
                {
                    var el = elements[i];
                    switch (el)
                    {
                        case NumberBox nm:
                            propertyInfo.SetValue(inst, int.Parse(nm.Text));
                            i++;
                            break;
                        case controls.TextBox tb:
                            propertyInfo.SetValue(inst, tb.Text);
                            i++;
                            break;
                    }
                }
                type.GetProperties()[0].SetValue(inst, null);

                var endpoint = "";
                if (type.Name.Last() == 'y')
                {
                    endpoint = type.Name.Substring(type.Name.Length - 1) + "ies";
                }
                else
                {
                    endpoint = type.Name + "s";
                }
                ApiClient.getInstance().PostAsync(inst, endpoint);
                Refresh(ComboBox);
            }
            catch (Exception exception)
            {
                Snackbar.Show(exception.Message);
            }
        }

        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //var cells = (sender as DataGrid).SelectedCells;
            //Snackbar.Show(cells.Count.ToString());

        }

        private void DataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cells = (sender as DataGrid).SelectedCells;
            Snackbar.Show(cells.Count.ToString());

        }
    }
}
