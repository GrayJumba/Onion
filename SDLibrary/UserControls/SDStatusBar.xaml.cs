using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartDesk.Controls
{
    /// <summary>
    /// Interaction logic for SDStatusBar.xaml
    /// </summary>
    /// 
    public partial class SDStatusBar : UserControl
    {
        BackgroundWorker bg_worker = new BackgroundWorker();

        Timer timer = new Timer();
        public string StatusText
        {
            set
            {
                status_label.Content = value;
            }
        }

        public Boolean isProgressBarActive
        {
            set
            {
                if (value)
                    status_progress_bar.Visibility = Visibility.Visible;
                else
                    status_progress_bar.Visibility = Visibility.Collapsed;
            }
        }
        public SDStatusBar()
        {
            InitializeComponent();
            bg_worker.RunWorkerCompleted += bg_worker_RunWorkerCompleted;
        }

        public void ShowTempMessage(string msg, double time)
        {
            StatusText = msg;
            timer.Interval = time;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            this.Dispatcher.Invoke((System.Windows.Forms.MethodInvoker)delegate
            {
                StatusText = "Ready";
            });

        }
        public void startWork(DoWorkEventHandler work, String description)
        {
            if (bg_worker.IsBusy) return;
            isProgressBarActive = true;
            StatusText = description;
            bg_worker.DoWork += work;
            bg_worker.RunWorkerAsync();
            bg_worker.DoWork -= work;
        }
        void bg_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                isProgressBarActive = false;
                if ((bool)e.Result) ShowTempMessage("Success.", 5000);
                else ShowTempMessage("Failed.", 5000);
            }
            catch (Exception)
            {   if(e.Error==null)
                ShowTempMessage("Success.", 5000);
                else ShowTempMessage("Error.", 5000);
            }
        }

  
       
    }
}
