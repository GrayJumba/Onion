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
    public partial class GeneralAuthenticationWindow : Window
    {
        public static SmartDesk.MySQLHandler.User user = new SmartDesk.MySQLHandler.User();
        public GeneralAuthenticationWindow()
        {
            try
            {

                string[] cmd_args = Environment.GetCommandLineArgs();
                string username = cmd_args.FirstOrDefault(x => x.StartsWith("username="));
                string password = cmd_args.FirstOrDefault(x => x.StartsWith("password="));
                if (username != null) username = username.Substring(9);
                if (password != null) password = password.Substring(9);
                if (user.logIn(username, password))
                {
                    this.Close();
                    moveOn();
                    return;
                }
                InitializeComponent();
                login_as_combo.Text = SmartDesk.Properties.Settings.Default.last_opened_module;
                Console.WriteLine(SmartDesk.Properties.Settings.Default.last_opened_module);
                login_as_combo.SelectionChanged += login_as_combo_SelectionChanged;
                login_name.Focus();
                this.Show();
            }
            catch (Exception ex)
            {
                 Errors.displayError("An error occured starting the application",  ErrorCode.AuthenticationConstructor, ErrorAction.Exit, ex);
            }
        }

        void login_as_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SmartDesk.Properties.Settings.Default.last_opened_module = (login_as_combo.SelectedItem as ComboBoxItem).Content.ToString();
            SmartDesk.Properties.Settings.Default.Save();
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
                 Errors.displayError("An error occured starting the application",  ErrorCode.LogingIn, ErrorAction.Continue, ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void login_name_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
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


        private void moveOn()
        {
            if (user.isLoggedIn)
            {
                string cmd_args = "username=" + login_name.Text + " password=" + login_pass.Password;
                string user_type=login_as_combo.Text;
                string app = "";
                if (user_type == "Automatic")
                {
                    switch (user.type)
                    {
                        case "teacher":
                            user_type = "Teacher";
                            break;
                        case "administrator":
                            user_type = "Administrator";
                            break;
                        case "academics":
                            user_type = "Academics Head";
                            break;
                        case "fees":
                             user_type =  "Fees Manager" ;
                            break;
                        case "records":
                             user_type = "Records Manager"  ;
                            break;
                        case "library":
                             user_type =  "Library Manager" ;
                            break;
                        default:
                            break;
                    }
                }

                switch(user_type){
                   
                    case "Academics Head":
                        app = "AcademicsDesk.exe";
                        break;
                    case "Fees Manager":
                        app="FeesDesk.exe";
                        break;
                    case "Teacher":
                        app = "TeachersDesk.exe";
                        break;
                    case "Library Manager":
                        app = "LibraryDesk.exe";
                        break;
                    case "Records Manager":
                        app = "RecordsDesk.exe";
                        break;
                    case "Administrator":
                        app = "AdminsDesk.exe";
                        break;
                    default:
                        MessageBox.Show("The type of user is not recognized");
                        break;
                }
                
                System.Diagnostics.Process.Start(app, cmd_args);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                moveOn();
            }
            catch (Exception ex)
            {
                 Errors.displayError("An error occured starting the application",  ErrorCode.AuthenticationClosing, ErrorAction.Exit, ex);
            }
        }
    }
}
