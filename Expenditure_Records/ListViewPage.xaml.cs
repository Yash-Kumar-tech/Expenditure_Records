using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Expenditure_Records
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class ListViewPage : Page
    {
        public string date, time, purpose, amount, mode, pr, description;
        

        public ObservableCollection<MainPage> records = new ObservableCollection<MainPage>();
        public ListViewPage()
        {
            this.InitializeComponent();
        }

        private async void HomeListView_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile;
            ContentDialog cd = new ContentDialog();
            cd.CloseButtonText = "Close";
            try
            {
                storageFile = await storageFolder.GetFileAsync("Data.csv");
            }
            catch (FileNotFoundException)
            {
                storageFile = await storageFolder.CreateFileAsync("Data.csv");
            }
            catch (Exception ex)
            {
                cd.Content = ex.ToString();
                await cd.ShowAsync();
                return;
            }
            string[] lines = (await Windows.Storage.FileIO.ReadTextAsync(storageFile)).Split('\n');
            string[] data_in_line;
            foreach (string line in lines)
            {
                if (line == "") break;
                data_in_line = line.Split(',');
                date = data_in_line[0];
                time = data_in_line[1];
                purpose = data_in_line[2];
                amount = data_in_line[4];
                mode = data_in_line[5];
                description = data_in_line[6];
                pr = data_in_line[3];
                if (description.Trim() == "") description = "No description.";
                records.Add(new MainPage()
                {
                    date = date,
                    time = time,
                    purpose = purpose,
                    amount = amount,
                    mode = mode,
                    pr = pr,
                    description = "Description: " + description,
                });
            }
            HomeListView.ItemsSource = records;
        }
    }
}
