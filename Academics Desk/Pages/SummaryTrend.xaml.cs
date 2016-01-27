using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AcademicsDesk.Pages
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class SummaryTrend : UserControl
    {
        SDLibrary.Reports.Converter term_conv1 = new SDLibrary.Reports.Converter(MySQLHandler.FormTrend.Form1.Dt, "term", "average");
        SDLibrary.Reports.Converter term_conv2 = new SDLibrary.Reports.Converter(MySQLHandler.FormTrend.Form2.Dt, "term", "average");
        SDLibrary.Reports.Converter term_conv3 = new SDLibrary.Reports.Converter(MySQLHandler.FormTrend.Form3.Dt, "term", "average");
        SDLibrary.Reports.Converter term_conv4 = new SDLibrary.Reports.Converter(MySQLHandler.FormTrend.Form4.Dt, "term", "average");
        SDLibrary.Reports.Converter term_conv5 = new SDLibrary.Reports.Converter(MySQLHandler.SchoolTermTrend.Default.Dt, "term", "average");

        public SummaryTrend()
        {
            InitializeComponent();
            term_chart1.ItemsSource = term_conv1.Points;
            term_chart2.ItemsSource = term_conv2.Points;
            term_chart3.ItemsSource = term_conv3.Points;
            term_chart4.ItemsSource = term_conv4.Points;
            term_chart5.ItemsSource = term_conv5.Points;
            year_picker.SelectedYearChanged += year_picker_SelectedYearChanged;
        }

        void year_picker_SelectedYearChanged(SmartDesk.Controls.YearPicker w, EventArgs e)
        {
            MySQLHandler.FormTrend.Form1.refreshDt(year_picker.SelectedYear);
            MySQLHandler.FormTrend.Form2.refreshDt(year_picker.SelectedYear);
            MySQLHandler.FormTrend.Form3.refreshDt(year_picker.SelectedYear);
            MySQLHandler.FormTrend.Form4.refreshDt(year_picker.SelectedYear);

            term_conv1.refresh();
            term_conv2.refresh(); 
            term_conv3.refresh();
            term_conv4.refresh();

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed += PrintCommand_Executed;
            }
            else
            {
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommand_Executed;
            }
        }


        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintVisual(SummaryTrendGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

    }
}
