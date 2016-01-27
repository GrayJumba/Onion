using System;
using System.Collections.Generic;
using System.Data;
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

namespace FeesDesk
{
    /// <summary>
    /// Interaction logic for Qoatations.xaml
    /// </summary>
    public partial class Invoice : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public Invoice()
        {
            InitializeComponent();
            InvoiceDg.DataContext = MySQLHandler.Invoice.Default.Dt;
        }
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
             e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Focusable = false;
            if (MySQLHandler.Invoice.Default.saveChanges())
                MessageBox.Show("Save Successful");
            else
                MessageBox.Show("Not Saved");
            MySQLHandler.Invoice.Default.refreshDt();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += this.SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed += this.SaveCommand_Executed;
                MainWindow.Default.PrintCommandBinding.CanExecute += this.PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed += this.PrintCommand_Executed;

            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= this.SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= this.SaveCommand_Executed;
                MainWindow.Default.PrintCommandBinding.CanExecute -= this.PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= this.PrintCommand_Executed;

            }
        }

        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintGrid(InvoiceDg, "");
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void InvoiceDg_CurrentCellChanged(object sender, EventArgs e)
        {

            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void InvoiceDg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }
    }
}
