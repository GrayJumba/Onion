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
    public partial class SubjectResults : UserControl
    {
        SDLibrary.Reports.Converter points = new SDLibrary.Reports.Converter(MySQLHandler.SchoolSubjectTrend.Default.Dt, "name", "average");
        public SubjectResults()
        {
            InitializeComponent();
            marksDG.DataContext = MySQLHandler.SubjectResults.Default.Dt;
            examPicker.SelectedExamsChanged += examPicker_SelectedExamsChanged;
            subject_picker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            trend_chart.ItemsSource = points.Points;
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }

        void examPicker_SelectedExamsChanged(SmartDesk.Controls.ExtendedExamPicker s, EventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.SubjectResults.Default.refreshDt(subject_picker.selected_subject_code, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            MySQLHandler.SchoolSubjectTrend.Default.refreshDt(examPicker.SelectedExamAutoID != 0, subject_picker.selected_subject_code);
            points.refresh();
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
            SmartDesk.Printing.Printing.PrintVisual(SubjectResultsGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
