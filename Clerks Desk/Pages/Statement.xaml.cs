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
    /// Interaction logic for Statement.xaml
    /// </summary>
    public partial class Statement : UserControl
    {
        public Statement()
        {
            InitializeComponent();
            StatementDg.DataContext = MySQLHandler.Statement.Default.Dt;
            studentPicker.SelectedStudentChanged += studentPicker_SelectedStudentChanged; 
            datePicker.SelectedDateChanged += datePicker_SelectedDateChanged;
        }

        void studentPicker_SelectedStudentChanged(SmartDesk.Controls.StudentPicker s, EventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.Statement.Default.refreshDt(Convert.ToUInt64(studentPicker.SelectedStudentAutoId),datePicker.SelectedDate.Value);
            double total = 0;
            Double.TryParse(MySQLHandler.Statement.Default.Dt.Compute("SUM(amount)", null).ToString(), out total);
            total_label.Content = "Total Ksh " + Math.Round(total, 2);
        }
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }

        private void StructurePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue; 
            if (value)
            {
               MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;
               MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
       
            }
            else
            {
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommandBinding_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommandBinding_Executed;
    
            }
        }

        private void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintGrid(StatementDg, "");
  
        }

        private void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        
    }
}
