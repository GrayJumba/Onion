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
    public partial class StudentTrend : UserControl
    {
        SDLibrary.Reports.Converter term_conv = new SDLibrary.Reports.Converter(MySQLHandler.StudentTermTrend.Default.Dt, "term", "average");
        SDLibrary.Reports.Converter exam_conv = new SDLibrary.Reports.Converter(MySQLHandler.StudentExamTrend.Default.Dt, "exam", "average");
        public StudentTrend()
        {
            InitializeComponent();
            term_chart.ItemsSource = term_conv.Points;
            exam_chart.ItemsSource = exam_conv.Points;
            student_picker.SelectedStudentsChanged += student_picker_SelectedStudentsChanged;
        }

        void student_picker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {
            MySQLHandler.StudentTermTrend.Default.refreshDt(student_picker.SelectedStudentAutoID);
            MySQLHandler.StudentExamTrend.Default.refreshDt(student_picker.SelectedStudentAutoID);

            term_conv.refresh();
            exam_conv.refresh();
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
            SmartDesk.Printing.Printing.PrintVisual(StudentTrendGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
      
    }
}
