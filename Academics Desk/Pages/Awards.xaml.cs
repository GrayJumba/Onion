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
    public partial class Awards : UserControl
    {
       
        public Awards()
        {
            InitializeComponent();
            topStudentPerSubjectDg.DataContext = MySQLHandler.TopStudentsPerSubject.Default.Dt;
            topStudentsDg.DataContext = MySQLHandler.TopStudents.Default.Dt;
            mostImprovedDg.DataContext = MySQLHandler.MostImproved.Default.Dt;
            stream_picker.SelectedClassesChanged += stream_picker_SelectedClassesChanged;
            examPicker.SelectedExamsChanged += examPicker_SelectedExamsChanged;
        }

        void examPicker_SelectedExamsChanged(SmartDesk.Controls.ExtendedExamPicker s, EventArgs e)
        {
            refresh();
        }

        void stream_picker_SelectedClassesChanged(SmartDesk.Controls.ExtendedStreamPicker s, EventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.TopStudents.Default.refreshDt(stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            MySQLHandler.TopStudentsPerSubject.Default.refreshDt(stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
            MySQLHandler.MostImproved.Default.refreshDt(stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf, examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID);
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
            SmartDesk.Printing.Printing.PrintVisual(AwardsGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
