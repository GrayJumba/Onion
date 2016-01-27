using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeachersDesk
{
    /// <summary>
    /// Interaction logic for GeneralOptions.xaml
    /// </summary>
    public partial class GeneralOptions : UserControl
    {
        public GeneralOptions()
        {
            InitializeComponent();
            refreshSettings();
        }

      
        private void reset_Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to reset the settings to default factory settings", "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                System.Windows.Forms.MessageBox.Show("Settings reset Successful");
                refreshSettings();
            }
        }

        private void refreshSettings()
        {
            //printing settings
            //pr_font_size_text.Text = Properties.Settings.Default.printing_font_size.ToString();
        }
        private void saveSettings()
        {
            //printing settings
            //int size = Properties.Settings.Default.printing_font_size;
            //if (int.TryParse(pr_font_size_text.Text, out size) && size > 0 && size < 41)
            //{
            //    Properties.Settings.Default.printing_font_size = size;
            //}
            //else
            //{
            //    MessageBox.Show("Printing Font Size can only be a number between 1 and 40.The value wont be saved.");
            //}
            ////then save
            //Properties.Settings.Default.Save();
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            saveSettings();
            MessageBox.Show("Save Successful");
            refreshSettings();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            refreshSettings();
        }
    }
}
