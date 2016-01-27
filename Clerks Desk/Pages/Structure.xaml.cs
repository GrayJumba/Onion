using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class FeeAmount : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public FeeAmount()
        {
            InitializeComponent();
            year_picker.SelectedYearChanged+=YearPicker_SelectedYearChanged;
            Form1Dg.DataContext = MySQLHandler.FeeAmount.Form1.Dt;
            Form2Dg.DataContext = MySQLHandler.FeeAmount.Form2.Dt;
            Form3Dg.DataContext = MySQLHandler.FeeAmount.Form3.Dt;
            Form4Dg.DataContext = MySQLHandler.FeeAmount.Form4.Dt;
            computeTotals();
        }
        private void YearPicker_SelectedYearChanged(SmartDesk.Controls.YearPicker w, EventArgs e)
        {
            int year = year_picker.SelectedYear;
            MySQLHandler.FeeAmount.Form1.refreshDt(year);
            MySQLHandler.FeeAmount.Form2.refreshDt(year);
            MySQLHandler.FeeAmount.Form3.refreshDt(year);
            MySQLHandler.FeeAmount.Form4.refreshDt(year);
            computeTotals();
        }
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MySQLHandler.FeeAmount.Form1.saveChanges();
            MySQLHandler.FeeAmount.Form2.saveChanges();
            MySQLHandler.FeeAmount.Form3.saveChanges();
            MySQLHandler.FeeAmount.Form4.saveChanges();

            MySQLHandler.FeeAmount.Form1.refreshDt();
            MySQLHandler.FeeAmount.Form2.refreshDt();
            MySQLHandler.FeeAmount.Form3.refreshDt();
            MySQLHandler.FeeAmount.Form4.refreshDt();
            computeTotals();
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
            SmartDesk.Printing.Printing.PrintVisual(StructureGrid);

        }

        private void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void computeTotals()
        {
            if (this.IsInitialized)
            {
                double total_amount1 = 0;
                Double.TryParse(MySQLHandler.FeeAmount.Form1.Dt.Compute("SUM(amount)", "").ToString(), out total_amount1);
                Form1TotalLabel.Content = "Total Ksh " + Math.Round(total_amount1, 2);

                double total_amount2 = 0;
                Double.TryParse(MySQLHandler.FeeAmount.Form2.Dt.Compute("SUM(amount)", "").ToString(), out total_amount2);
                Form2TotalLabel.Content = "Total Ksh " + Math.Round(total_amount2, 2);

                double total_amount3 = 0;
                Double.TryParse(MySQLHandler.FeeAmount.Form3.Dt.Compute("SUM(amount)", "").ToString(), out total_amount3);
                Form3TotalLabel.Content = "Total Ksh " + Math.Round(total_amount3, 2);

                double total_amount4 = 0;
                Double.TryParse(MySQLHandler.FeeAmount.Form4.Dt.Compute("SUM(amount)", "").ToString(), out total_amount4);
                Form4TotalLabel.Content = "Total Ksh " + Math.Round(total_amount4, 2);
            }
        }
        private void Form2Dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            computeTotals();
        }

        private void Form1Dg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void Form1Dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }
    }
}
