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

namespace FeesDesk
{
    /// <summary>
    /// Interaction logic for Receipts.xaml
    /// </summary>
    public partial class Receipts : UserControl
    {
        public static Receipts ReceiptsUserControl;
        public Receipts()
        {
            InitializeComponent();
            ReceiptsUserControl = this;
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Focusable = false;
            if (MySQLHandler.Receipt.Default.saveChanges())
                MessageBox.Show("Save Successful");
            else
                MessageBox.Show("Not Saved");
            MySQLHandler.Receipt.Default.refreshDt(); 
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue; 
            if (value)
            {
               MainWindow.Default.SaveCommandBinding.CanExecute += this.SaveCommand_CanExecute;
               MainWindow.Default.SaveCommandBinding.Executed += this.SaveCommand_Executed;
               MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;
               MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
       
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= this.SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= this.SaveCommand_Executed;
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommandBinding_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommandBinding_Executed;
    
            }
        }

        private void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
          //  SmartDesk.Printing.Printing.PrintGrid(ReceiptDg, "");
  
        }

        private void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
