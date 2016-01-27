using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Forms;
using System.Threading;
using Onion.ErrorHandler;
namespace SmartDesk
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
 
    public partial class SplashWindow
    { 
        static SplashWindow()
        {
            Onion.SystemR.SystemRequirements.confirmDotNet45Installed();
        }
        BackgroundWorker bgw;
        public SplashWindow()
        {
            try
            {
                InitializeComponent();
                bgw = new BackgroundWorker();
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                bgw.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                 Errors.displayError("An error occured starting the application", ErrorCode.SplashWindow, ErrorAction.Exit, ex);
            }
        }
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Thread.Sleep(4000);
            }
            catch (Exception ex)
            {
                 Errors.displayError("An error occured starting the application",  ErrorCode.BgWorker,  ErrorAction.Exit, ex);
            }
        }
       
        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke((MethodInvoker)delegate
                {
                    GeneralAuthenticationWindow authentication = new GeneralAuthenticationWindow();
                    this.Close();
                });
            }
            catch(Exception ex)
            {
                 Errors.displayError("An error occured starting the application",  ErrorCode.SplashClosing, ErrorAction.Exit, ex);
            }
        }

      
        private void splash_Closing(object sender, CancelEventArgs e)
        {
        }
    }
}
