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
using SmartDesk.MySQLHandler;
namespace SmartDesk
{
    /// <summary>
    /// Interaction logic for ManageUser.xaml
    /// </summary>
    public partial class ManageUserWindow : Window
    {
        User user;
        public ManageUserWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            username_label.Content = "User: " + this.user.name;
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            if (new_passwordBox.Password != confirm_passwordBox.Password)
            { 
                MessageBox.Show("Sorry.The confirmation password does not match the new password."); 
                return; 
            }
            if (!this.user.updateUserPassword(old_passwordBox.Password, new_passwordBox.Password))
            {
                MessageBox.Show("You have entered a wrong password.");
                return;
            }
            this.Close();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void adjustOkButton()
        {
            if (string.IsNullOrEmpty(old_passwordBox.Password) || string.IsNullOrEmpty(new_passwordBox.Password) || string.IsNullOrEmpty(confirm_passwordBox.Password))
                  ok_button.IsEnabled = false;
            else ok_button.IsEnabled = true;
        }
        private void old_passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            adjustOkButton();
        }

        private void new_passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            adjustOkButton();
        }

        private void confirm_passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            adjustOkButton();
        }
    }
}
