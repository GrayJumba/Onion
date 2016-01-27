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
using Onion.ErrorHandler;

namespace LibraryDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :SmartDesk.Controls.SDWindow
    {
        #region RoutedUICommands
        #endregion
        public static MainWindow main_window;
        public MainWindow()
            : base(SplashShowing)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Errors.displayError("An error occured starting the application", ErrorCode.MainWindowConstructor, ErrorAction.Exit, ex);
            }
        }
        private static void SplashShowing(SmartDesk.SplashWindow w, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Onion.MySQLHandler.MySQLHelper.BackUp(@"C:\Inventory\Database Backup", 10);
            }
            catch (Exception ex)
            {

                Errors.displayError("An error occured closing the application", ErrorCode.MainWindowClosing, ErrorAction.Exit, ex);
            }
        }
    }
}
