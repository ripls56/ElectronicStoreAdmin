using ElectronicStoreAdmin.ViewModels;
using ElectronicStoreAdmin.Views.Pages.Auth;
using System;
using System.Windows;

namespace ElectronicStoreAdmin.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();
            var apiClient = ApiClient.getInstance();
            Frame.Content = new Authorization(this, new AuthViewModel(apiClient));
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            string? temp = Utils.RegGet("Left");
            if (temp != null) Left = Convert.ToDouble(temp);
            temp = Utils.RegGet("Top");
            if (temp != null) Top = Convert.ToDouble(temp);
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            Utils.RegSet("Left", Left.ToString());
            Utils.RegSet("Top", Top.ToString());
        }
    }
}
