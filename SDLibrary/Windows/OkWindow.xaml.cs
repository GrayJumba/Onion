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
    public partial class OkWindow : Window
    {
        public OkWindow()
        {
            InitializeComponent();
            refresh();
            if (SmartDesk.Controls.SDWindow.current_user.checkRights("administrator")) changeInfoButton.IsEnabled = true;
        }

        private void refresh()
        {
            if (Onion.Floats.Default.Activated)
            {
                messagelabel.Content = "Activated.";
            }
            else
            {
                messagelabel.Content = "Smart desk not activated.";
            }
            sch_name_label.Content = MySQLHandler.Settings.Default["school_name"];
            sch_address_label.Content = MySQLHandler.Settings.Default["school_address"];
            sch_phone_label.Content = MySQLHandler.Settings.Default["school_phone"];
            sch_email_label.Content = MySQLHandler.Settings.Default["school_email"];   
            key_text1.Text = "";
            key_text2.Text = "";
            key_text3.Text = "";
            key_text4.Text = "";
            key_text5.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void licence_ok_Click(object sender, RoutedEventArgs e)
        {
            string code = key_text1.Text+key_text2.Text+key_text3.Text+key_text4.Text+key_text5.Text;
            if (Onion.Floats.Default.updateCode(code))
            {
                ok_panel.Visibility = Visibility.Hidden;
            }
            refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ok_panel.Visibility = Visibility.Visible;
        }

        private void key_text_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textbox = sender as TextBox;
            string text = textbox.Text;

            if (text.Length == 5)
            {
                if (textbox == key_text1)
                {
                    key_text2.Focus();
                }
                else if (textbox == key_text2)
                {
                    key_text3.Focus();
                }
                else if (textbox == key_text3)
                {
                    key_text4.Focus();
                }
                else if (textbox == key_text4)
                {
                    key_text5.Focus();
                }
            }
            
        }

        private void changeInfoButton_Click(object sender, RoutedEventArgs e)
        {
            UserInfoWindow user_info_window = new UserInfoWindow();
            user_info_window.ShowDialog();
            refresh();
        }
    }

    public class CodeLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString().Length == 5) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
