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
    /// Interaction logic for ManageDatabase.xaml
    /// </summary>
    public partial class ManageDatabaseWindow : Window
    {
        public ManageDatabaseWindow()
        {
            InitializeComponent();
            refreshSettings();
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Sql files(*.sql)|*.sql|All Files (*.*)|*.*";
            dialog.Title = "Enter name of file to back up data";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file_name = dialog.FileName;
                if (Onion.MySQLHandler.MySQLHelper.BackUp(file_name))
                    MessageBox.Show("Database exported successfully");
            }
        }

        private void ImportButton_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Sql files(*.sql)|*.sql|All Files (*.*)|*.*";
            dialog.Title = "Select a data back up file";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file_name = dialog.FileName;
                if (Onion.MySQLHandler.MySQLHelper.RestoreDb(file_name))
                    MessageBox.Show("Database imported successfully");
            }
        }

        private void ok_Button_Click(object sender, RoutedEventArgs e)
        {
            saveSettings();
            MessageBox.Show("Seetings applied successfully");
            refreshSettings();
        }
        private void refreshSettings()
        {
            //database settings
            dbhost_text.Text = SDLibrary.Properties.Settings.Default.db_host;
            local_check.IsChecked = SDLibrary.Properties.Settings.Default.back_up_local_automatic;
            online_check.IsChecked = SDLibrary.Properties.Settings.Default.back_up_online_automatic;

        }
        private void saveSettings()
        {
            //database settings
            SDLibrary.Properties.Settings.Default.db_host = dbhost_text.Text;
            SDLibrary.Properties.Settings.Default.back_up_local_automatic = Convert.ToBoolean(local_check.IsChecked);
            SDLibrary.Properties.Settings.Default.back_up_online_automatic = Convert.ToBoolean(online_check.IsChecked);
            SDLibrary.Properties.Settings.Default.Save();
        }

        private void OnlineBackupButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Backing up online");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Onion.MySQLHandler.MySQLHelper.CreateRemoteUser())
                MessageBox.Show("Remote user has successfully been created");
            else
                MessageBox.Show("Remote user could not be created");
        }
    }
}
