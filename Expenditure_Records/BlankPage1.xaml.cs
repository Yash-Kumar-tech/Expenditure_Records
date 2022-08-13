using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Expenditure_Records
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        private string date, time;

        public BlankPage1()
        {
            this.InitializeComponent();
        }

        private void clearError()
        {
            DateInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            TimeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            PurposeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            ModeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            AmountInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            PRInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        private string ModifyDate(string dt)
        {
            string day, month, year;
            month = dt.Substring(0, 2);
            if (month[1] == '/')
            {
                month = month[0].ToString();
                day = dt.Substring(2, 2);
                if (day[1] == '/')
                {
                    day = "0" + day[0].ToString();
                    year = dt.Substring(4,4);
                } else
                {
                    year = dt.Substring(5, 4);
                }
            } else
            {
                day = dt.Substring(3, 2);
                if (day[1] == '/')
                {
                    day = "0" + day[0].ToString();
                    year = dt.Substring(6, 4);
                } else
                {
                    year = dt.Substring(7, 4);
                }
            }
            return day+"-"+month+"-"+year;
        }

        private string ModifyTime(string tm)
        {
            return tm.Substring(0,2) + ":" + tm.Substring(3,2);
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int flag = 0;
            if (DateInput.SelectedDate == null)
            {
                DateInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                flag = 1;
            }
            if (TimeInput.SelectedTime == null)
            {
                TimeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                flag = 1;
            }
            if (PurposeInput.Text == "")
            {
                PurposeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                flag = 1;
            }
            if(AmountInput.Text == "")
            {
                AmountInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                flag = 1;
            }
            if (ModeInput.Text == "")
            {
                ModeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                flag = 1;
            }
            if(PRInput.SelectedItem == null)
            {
                PRInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                flag = 1;
            }
            if(flag == 1)
            {
                ErrorMessage.Text = "The fields marked in red have invalid values.";
                ErrorTeachingTip.IsOpen = true;
                await Task.Delay(5000);
                clearError();
                return;
            }
            date = ModifyDate(DateInput.SelectedDate.ToString());
            time = ModifyTime(TimeInput.SelectedTime.ToString());
            string to_save = "";
            string pr = "";
            if (PRInput.SelectedIndex == 0) pr = "Paid";
            else pr = "Received";
            to_save = $"\n{date},{time},{PurposeInput.Text},{pr},{AmountInput.Text},{ModeInput.Text},{Description.Text}";
            StorageFolder storage = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file_to_write;
            try
            {
                file_to_write = await storage.GetFileAsync("Data.csv");
            } catch
            {
                file_to_write = await storage.CreateFileAsync("Data.csv");
            }
            await Windows.Storage.FileIO.AppendTextAsync(file_to_write, to_save);
            ContentDialog cd = new ContentDialog();
            cd.Title = "Save Successful";
            cd.Content = "Following details have been saved successfully:\nDate:" + date
                + "\nTime: " + time + "\nPurpose: " + PurposeInput.Text + "\nMode of payment: " +
                ModeInput.Text + "\nDescription: " + Description.Text;
            cd.CloseButtonText = "Close";
            await cd.ShowAsync();
        }
    }
}
