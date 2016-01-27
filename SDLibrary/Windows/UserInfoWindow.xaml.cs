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
using System.Windows.Shapes;

namespace SmartDesk
{
    /// <summary>
    /// Interaction logic for Licence.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        public UserInfoWindow()
        {
            InitializeComponent();
            refresh();
        }

        private void refresh()
        {
            sch_name_label_txt.Value = MySQLHandler.Settings.Default["school_name"];
            sch_address_label_txt.Value = MySQLHandler.Settings.Default["school_address"];
            sch_phone_label_txt.Value = MySQLHandler.Settings.Default["school_phone"];
            sch_email_label_txt.Value = MySQLHandler.Settings.Default["school_email"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string filepath = BrowseImage();
            logo_img.Source = new ImageSourceConverter().ConvertFromString(filepath) as ImageSource;
        }
        private string BrowseImage()
        {
            string filepath;
            System.Windows.Forms.FileDialog explorer = new System.Windows.Forms.OpenFileDialog();
            explorer.Filter = "image Files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            explorer.FilterIndex = 2;
            explorer.RestoreDirectory = true;
            if (explorer.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filepath = explorer.FileName;
                    return filepath;

                }
                catch (Exception ex)
                {
                    return "";
                }

            }
            return "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MySQLHandler.Settings.Default["school_name"]=sch_name_label_txt.Value;
            MySQLHandler.Settings.Default["school_address"]=sch_address_label_txt.Value;
            MySQLHandler.Settings.Default["school_phone"]=sch_phone_label_txt.Value;
            MySQLHandler.Settings.Default["school_email"] = sch_email_label_txt.Value;
            if (MySQLHandler.Settings.Default.saveSettings())
            {
                if (Onion.Floats.Default.Activate())
                {
                    MessageBox.Show("Your school information has been changed successfully. And your license key has been confirmed.");
                }
                else
                MessageBox.Show("Your school information has been changed successfully.");
            }
            else MessageBox.Show("Saving failed");
            refresh();
        }
    }

}
