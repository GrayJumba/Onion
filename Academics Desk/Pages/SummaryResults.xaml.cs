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
    public partial class SummaryResults : UserControl
    {
        SDLibrary.Reports.Converter exam_points = new SDLibrary.Reports.Converter(MySQLHandler.SchoolExamTrend.Default.Dt, "exam", "average");
        SDLibrary.Reports.Converter term_points = new SDLibrary.Reports.Converter(MySQLHandler.SchoolTermTrend.Default.Dt, "term", "average");

        public SummaryResults()
        {
            InitializeComponent();
            marksDG.DataContext = MySQLHandler.StreamResults.Default.Dt;
            examPicker.SelectedExamsChanged += examPicker_SelectedExamsChanged;
        }

      
        void examPicker_SelectedExamsChanged(SmartDesk.Controls.ExtendedExamPicker s, EventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.StreamResults.Default.refreshDt(examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            if(examPicker.SelectedExamAutoID !=0)
            {
                trend_chart.ItemsSource = exam_points.Points;
            }
            else if(examPicker.SelectedTermAuoID!=0)
            {
                trend_chart.ItemsSource = term_points.Points;
            }
            else
            {
                trend_chart.ItemsSource = null;
            }
        }
        private void resultSlip_usercontrol_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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
            SmartDesk.Printing.Printing.PrintVisual(SummaryResultGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
