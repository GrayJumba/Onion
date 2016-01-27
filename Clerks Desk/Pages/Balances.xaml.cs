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
    /// Interaction logic for Balances.xaml
    /// </summary>
    public partial class Balances : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public Balances()
        {
            InitializeComponent();
            BalanceDg.DataContext = MySQLHandler.Balance.Default.Dt;
            extendedStudentPicker.SelectedStudentsChanged += extendedStudentPicker_SelectionChanged;
            datePicker.SelectedDateChanged+=datePicker_SelectedDateChanged;
        }
        private void refresh()
        {
            MySQLHandler.Balance.Default.refreshDt(extendedStudentPicker.SelectedStudentAutoID,
                extendedStudentPicker.SelectedStreamAutoID,
                extendedStudentPicker.SelectedClassOf,
                datePicker.SelectedDate.Value);
        }

        private void extendedStudentPicker_SelectionChanged(SmartDesk.Controls.ExtendedStudentPicker sender, EventArgs e)
        {
            refresh();
        }
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
              refresh();
        }

        private void BalanceDg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void BalanceDg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

        private void BalanceDg_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
            {
                FeesDesk.MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
                FeesDesk.MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;

            }
            else
            {
                FeesDesk.MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
                FeesDesk.MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;

            }      
        }

        private void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintGrid(BalanceDg, "");
        }

       

    }
}
