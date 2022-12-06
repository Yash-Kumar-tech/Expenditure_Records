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
    public sealed partial class AddPage : Page
    {
        private string date, time;

        public AddPage()
        {
            this.InitializeComponent();
        }

        private void clearError()
        {
            //This function restores the controls to their original appearances when called
            //This is used after the controls were turned red to notify errors
            DateInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            TimeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            PurposeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            ModeInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            AmountInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            PRInput.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        private string ModifyDate(string dt)
        {
            //This function takes in date from the date picker and returns the date in "dd-mm-yyyy" format
            string day, month, year;
            month = dt.Substring(0, 2);
            if (month[1] == '/')
            {
                month = "0" + month[0].ToString();
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
            //This function returns time in "hh:mm" format
            return tm.Substring(0,2) + ":" + tm.Substring(3,2);
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //This function checks if all controls have valid values and then saves the result in the 
            //data.csv file

            //This part is the check which controls (if any) have invalid values
            //The controls with invalid alues are turned red
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
                //If invalid values are found in controls, a teachingtip is opened
                ErrorMessage.Text = "The fields marked in red have invalid values.";
                ErrorTeachingTip.IsOpen = true;
                //The controls are restored to their original appearances after 5s
                await Task.Delay(5000);
                clearError();
                return;
            }
            //To save the data when all cntrols have valid values
            date = ModifyDate(DateInput.SelectedDate.ToString());
            time = ModifyTime(TimeInput.SelectedTime.ToString());
            string to_save = "";
            string pr = "";
            if (PRInput.SelectedIndex == 0) pr = "Paid";
            else pr = "Received";
            to_save = $"{date},{time},{PurposeInput.Text},{pr},{AmountInput.Text},{ModeInput.Text},{Description.Text}\n";
            StorageFolder storage = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file_to_write;
            try
            {
                file_to_write = await storage.GetFileAsync("Data.csv");
            } catch
            {
                //Thsi block handles exceptions such as file not found by creating a new data.csv file
                file_to_write = await storage.CreateFileAsync("Data.csv");
            }
            await Windows.Storage.FileIO.AppendTextAsync(file_to_write, to_save);

            //ContentDialog signalling successful saving operation
            ContentDialog cd = new ContentDialog()
            {
                Title = "Save successful",
                Content = "Following details have been saved successfully:\nDate : " + date
                + "\nTime : " + time + "\nPurpose : " + PurposeInput.Text + "\nMode of payment : " +
                ModeInput.Text + "\nDescription : " + Description.Text,
                CloseButtonText = "Close",

            };
            await cd.ShowAsync();
            //The result from this contentdialog is not required so, it is not stored
        }
    }
}
