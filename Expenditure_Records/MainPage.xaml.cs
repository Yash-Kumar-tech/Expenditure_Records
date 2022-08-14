using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Expenditure_Records
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string date, time, purpose, amount, mode, pr, description;

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ListViewPage));
        }

        public ObservableCollection<MainPage> records = new ObservableCollection<MainPage>();

        private void AddTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(BlankPage1));
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ListViewPage));
        }
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
