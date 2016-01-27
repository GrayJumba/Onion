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
    public partial class ClassResults : UserControl
    {
        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.ClassResultsAggr.Default.Dt);
        public Onion.Controls.DtTraversor Dt_Traversor
        {
            get { return dt_traversor; }
        }

        SDLibrary.Reports.Converter mark_conv = new SDLibrary.Reports.Converter(MySQLHandler.ClassResults.Default.Dt, "subject", "average");
       SDLibrary.Reports.Converter exam_stream_conv = new SDLibrary.Reports.Converter(MySQLHandler.StreamExamTrend.Results.Dt, "exam", "average");
       SDLibrary.Reports.Converter term_stream_conv = new SDLibrary.Reports.Converter(MySQLHandler.StreamTermTrend.Results.Dt, "term", "average");
       SDLibrary.Reports.Converter exam_class_conv = new SDLibrary.Reports.Converter(MySQLHandler.ClassExamTrend.Results.Dt, "exam", "average");
       SDLibrary.Reports.Converter term_class_conv = new SDLibrary.Reports.Converter(MySQLHandler.ClassTermTrend.Results.Dt, "term", "average");
        public ClassResults()
        {
            InitializeComponent();
            gradesDg.DataContext = MySQLHandler.GradeAnalysis.Default.Dt;
            marksDG.DataContext = MySQLHandler.ClassResults.Default.Dt;
            marks_chart.ItemsSource = mark_conv.Points;
            stream_picker.SelectedClassesChanged += stream_picker_SelectedClassesChanged;
            examPicker.SelectedExamsChanged += examPicker_SelectedExamsChanged;

        }

        void examPicker_SelectedExamsChanged(SmartDesk.Controls.ExtendedExamPicker s, EventArgs e)
        {
            refresh();
        }
        public void refresh()
        {
            
            MySQLHandler.ClassResults.Default.refreshDt(stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            mark_conv.refresh();
            MySQLHandler.ClassResultsAggr.Default.refreshDt(stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            MySQLHandler.GradeAnalysis.Default.refreshDt(stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            gradesDg.DataContext = MySQLHandler.GradeAnalysis.Default.Dt;

            if(examPicker.SelectedExamAutoID !=0 && stream_picker.SelectedStreamAutoID !=0)
            {
                MySQLHandler.StreamExamTrend.Results.refreshDt(stream_picker.SelectedStreamAutoID);
                exam_stream_conv.refresh();
                trend_chart.ItemsSource = exam_stream_conv.Points;
            }
            else if (examPicker.SelectedExamAutoID != 0 && stream_picker.SelectedClassOf !=0)
            {
                MySQLHandler.ClassExamTrend.Results.refreshDt(stream_picker.SelectedClassOf);
                exam_class_conv.refresh();
                trend_chart.ItemsSource = exam_class_conv.Points;
            }
            else if (examPicker.SelectedTermAuoID != 0 && stream_picker.SelectedStreamAutoID != 0)
            {
                MySQLHandler.StreamTermTrend.Results.refreshDt(stream_picker.SelectedStreamAutoID);
                term_stream_conv.refresh();
                trend_chart.ItemsSource = term_stream_conv.Points;
            }
            else if (examPicker.SelectedTermAuoID != 0 && stream_picker.SelectedClassOf != 0)
            {
                MySQLHandler.ClassTermTrend.Results.refreshDt(stream_picker.SelectedClassOf);
                term_class_conv.refresh();
                trend_chart.ItemsSource = term_class_conv.Points;

            }
            else
            {
                trend_chart.ItemsSource = null;
            }

        }
        void stream_picker_SelectedClassesChanged(SmartDesk.Controls.ExtendedStreamPicker s, EventArgs e)
        {
            refresh();
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
            SmartDesk.Printing.Printing.PrintVisual(ClassResultsGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
