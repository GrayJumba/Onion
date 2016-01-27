using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SmartDesk
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        BackgroundWorker bgw = new BackgroundWorker();
        string type;
        string filepath = @"C:\Users\" + Environment.UserName + @"\Documents\screenshot.png";
        Bitmap screenbmp = null;
       
    
        public MessageWindow(string type,string instruction)
        {
            InitializeComponent();
            //setup background worker
            bgw.WorkerReportsProgress = true;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.DoWork += bgw_DoWork;
            //initialize
            this.type = type;
            this.instruction_label.Content += instruction;
            //Set screenshot
            screenbmp = takeScreenshot();
            screenshot_img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                (screenbmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
        private void send_button_Click(object sender, RoutedEventArgs e)
        {
            bgw.RunWorkerAsync();
            this.Close();
        }
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string message = feedback_text.Text;
                List<string> attachments = new List<string>();
                if (screenshot_check.IsChecked == true) attachments.Add(filepath);
                if (email_check.IsChecked == true) sendEmail(email_text.Text, "", message, attachments.ToArray());
                else sendEmail("", "", message, attachments.ToArray());
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          
        }
      

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            screenshot_img.Source = null;
        }

      
        public static void sendEmail(string sender, string recipient, string message, string[] attachments) //send email
        {
            System.Windows.MessageBox.Show("Sending message");
        }
        public static Bitmap takeScreenshot()//taking an image of the running desktop and save it
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            return bitmap;

        }

       

    }
}
