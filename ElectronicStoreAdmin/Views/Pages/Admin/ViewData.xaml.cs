using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Controls.NumberBoxControl;
using controls = Wpf.Ui.Controls;
using DataGrid = System.Windows.Controls.DataGrid;
using TextBox = System.Windows.Controls.TextBox;

namespace ElectronicStoreAdmin.Views.Pages.Admin
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
            //dg.Columns.Insert(0,new DataGridCheckBoxColumn());
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
                            var text = (e.EditingElement as TextBox).Text;
                            if (text == null) continue;
                            if (propertyInfo.PropertyType == typeof(string))
                                propertyInfo.SetValue(lastObject, text);
                            if (propertyInfo.PropertyType == typeof(decimal?))
                                propertyInfo.SetValue(lastObject, Convert.ToDecimal(text));
                            if (propertyInfo.PropertyType == typeof(DateTime?))
                                propertyInfo.SetValue(lastObject, DateTime.Parse(text));
                            if (propertyInfo.PropertyType == typeof(bool))
                                propertyInfo.SetValue(lastObject, Convert.ToBoolean(text));
                        }
                    }

                    var a = endpointName() + $"/{lastObject.GetType().GetProperties().First().GetValue(lastObject)}";
                    ApiClient.getInstance().PutAsyncModel(lastObject, endpointName() + $"/{lastObject.GetType().GetProperties().First().GetValue(lastObject)}");
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

                ApiClient.getInstance().PostAsync(inst, endpointName());
                Refresh(ComboBox);
            }
            catch (Exception exception)
            {
                Snackbar.Show(exception.Message);
            }
        }

        HashSet<string> nums = new HashSet<string>();

        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                var items = (sender as DataGrid).SelectedItems;
                //new controls.Snackbar
                //{
                //    Title = "Добавлено для удаления"
                //}.Show();
                foreach (var item in items)
                {
                    var property = item.GetType().GetProperties()[0];
                    if (property.PropertyType == typeof(int?))
                    {
                        nums.Add(property.GetValue(item)?.ToString() + '&');
                    }
                }
            }
            catch
            {

            }

        }

        private void DelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //string endpoint = endpointName() + '?';
                //foreach (var item in nums)
                //{
                //    endpoint += "idList=" + item + '&';
                //}

                //Snackbar.Show(endpoint);

                //nums = new HashSet<string>();
                var endpoint = type.Name;
                if (type.Name.Last() == 'y')
                    endpoint = $"{endpoint.Remove(endpoint.Length - 1)}ies";
                else endpoint += "s";
                endpoint += "/";
                endpoint += nums.Last();
                endpoint = endpoint.Replace("&&", "&").TrimEnd('&');

                Snackbar.Show(endpoint);
                //Orderrs?idList=4&idList=3
                ApiClient.getInstance().DeleteAsync(endpoint);
                nums = new HashSet<string>();
            }
            catch (Exception ex)
            {
                Snackbar.Show(ex.Message);
            }

        }

        string endpointName()
        {
            var endpoint = "";
            if (type.Name.Last() == 'y')
            {
                endpoint = type.Name.Substring(type.Name.Length - 1) + "ies";
            }
            else
            {
                endpoint = type.Name + "s";
            }
            return endpoint;
        }

        private void ResButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //https://localhost:7008/api/Orderrs/recovery?id=1'
                var endpoint = type.Name;
                if (type.Name.Last() == 'y')
                    endpoint = $"{endpoint.Remove(endpoint.Length - 1)}ies";
                else endpoint += "s";
                endpoint += "/restore?id=";
                endpoint = endpoint.Replace("&&", "&");
                foreach (var item in nums)
                {
                    ApiClient.getInstance().PostAsync(item, endpoint + item);
                }
                nums = new HashSet<string>();
            }
            catch (Exception ex)
            {
                Snackbar.Show(ex.Message);
            }
        }

        private void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string Filepath = @$"{path}\doc.docx";
                using var wordprocessingDocument = WordprocessingDocument.Create(Filepath, WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = wordprocessingDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                ParagraphProperties paragraphProperties1 = new ParagraphProperties();
                var clients = ApiClient.getInstance().GetClientsAsync().GetAwaiter().GetResult();
                foreach (var client in clients)
                {
                    run.AppendChild(new Text($"Логин: {client.LoginClient} \"\"\n"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Text($"Email: {client.EmailClient} \"\"\n"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Text($"Номер телефона: {client.PhoneClient} \"\"\n"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Text(" ================================================== \"\"\n"));
                    run.AppendChild(new Break());
                }

                wordprocessingDocument.MainDocumentPart.Document.Save();

                Snackbar.Show("Готово", "Экспорт выполнен успешно", new IconSourceElement(), ControlAppearance.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Show(ex.Message);
            }
        }
    }
}
