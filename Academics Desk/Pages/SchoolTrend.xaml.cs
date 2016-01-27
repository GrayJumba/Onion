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
    public partial class SchoolTrend : UserControl
    {
        SDLibrary.Reports.Converter term_conv = new SDLibrary.Reports.Converter(MySQLHandler.SchoolTermTrend.Default.Dt, "term", "average");
        SDLibrary.Reports.Converter exam_conv = new SDLibrary.Reports.Converter(MySQLHandler.SchoolExamTrend.Default.Dt, "exam", "average");
        public SchoolTrend()
        {
            InitializeComponent();
            term_chart.ItemsSource = term_conv.Points;
            exam_chart.ItemsSource = exam_conv.Points;
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
            SmartDesk.Printing.Printing.PrintVisual(SchoolTrendGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
