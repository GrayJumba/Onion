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
using Onion.ErrorHandler;

namespace SmartDesk
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public SmartDesk.MySQLHandler.User user = new SmartDesk.MySQLHandler.User();
        public string AppTitle{ set  { title_label.Content = value;  } }

        public AuthenticationWindow()
        {
            try
            {
                InitializeComponent();
                login_name.Focus();
            }
            catch (Exception ex)
            {
                Errors.displayError("An error occured starting the application", ErrorCode.AuthenticationConstructor, ErrorAction.Exit, ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (user.logIn(login_name.Text, login_pass.Password))
                {
                    this.Close();
                }
                else
                    MessageBox.Show("Wrong username or password");
            }
            catch (Exception ex)
            {
                Errors.displayError("An error occured starting the application", ErrorCode.LogingIn, ErrorAction.Continue, ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }catch( Exception)
            {

            }
        }

        private void login_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                login_pass.Focus();
        }

        private void login_pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (user.logIn(login_name.Text, login_pass.Password))
                {
                    this.Close();
                }
                else
                    MessageBox.Show("Wrong username or password");
            }
        }
    }
}

