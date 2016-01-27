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
    public partial class ResultSlip : UserControl
    {
        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.ResultSlipAggr.Default.Dt);
        SDLibrary.Reports.Converter points = new SDLibrary.Reports.Converter(MySQLHandler.ResultSlip.Default.Dt, "subject_abbrev", "percentage");
        SDLibrary.Reports.Converter exam_points= new SDLibrary.Reports.Converter(MySQLHandler.StudentExamTrend.ResultSlip.Dt, "exam", "average");
        public Onion.Controls.DtTraversor Dt_Traversor
        {

            get {  return dt_traversor; }
        }
        public ResultSlip()
        {
            InitializeComponent();
            resultsDg.DataContext = MySQLHandler.ResultSlip.Default.Dt;
            chart.ItemsSource = MySQLHandler.ResultSlip.Default.Dt.DefaultView;
            extendedStudentPicker.SelectedStudentsChanged += extendedStudentPicker_SelectedStudentsChanged;
            examPicker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            dt_traversor.JumpToStart();
            chart.ItemsSource = points.Points;
            exam_chart.ItemsSource = exam_points.Points;
        }
        private void refresh()
        {
            MySQLHandler.ResultSlip.Default.refreshDt(extendedStudentPicker.SelectedStudentAutoID, examPicker.selected_exam_auto_id);
            MySQLHandler.ResultSlipAggr.Default.refreshDt(extendedStudentPicker.SelectedStudentAutoID, examPicker.selected_exam_auto_id);
            MySQLHandler.StudentExamTrend.ResultSlip.refreshDt(extendedStudentPicker.SelectedStudentAutoID);
            points.refresh();
            exam_points.refresh();
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }

        void extendedStudentPicker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
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
            SmartDesk.Printing.Printing.PrintVisual(ResultSlipGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
