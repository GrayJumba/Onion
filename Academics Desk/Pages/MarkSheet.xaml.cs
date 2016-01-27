using System;
using System.Collections.Generic;
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
    public partial class MarkSheet : UserControl
    {
        public MarkSheet()
        {
            InitializeComponent();
            markSheetDg.DataContext = MySQLHandler.MarkSheet.Default.Dt;
            examPicker.SelectedExamsChanged += examPicker_SelectedExamsChanged; 
            stream_picker.SelectedClassesChanged += stream_picker_SelectedClassesChanged;
        }

        void examPicker_SelectedExamsChanged(SmartDesk.Controls.ExtendedExamPicker s, EventArgs e)
        {
            refresh();
        }
        public void refresh()
        {
            MySQLHandler.MarkSheet.Default.refreshDt(examPicker.SelectedExamAutoID, examPicker.SelectedTermAuoID,stream_picker.SelectedStreamAutoID, stream_picker.SelectedClassOf);
            markSheetDg.AutoGenerateColumns = false;
            markSheetDg.AutoGenerateColumns = true;
        }
        void stream_picker_SelectedClassesChanged(SmartDesk.Controls.ExtendedStreamPicker s, EventArgs e)
        {
            refresh();
        }

        private void markSheetDg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();
            if (headername == "student_auto_id")
            {
                e.Cancel = true;
            }
            if (headername == "name")
            {
                e.Column.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            }

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
            SmartDesk.Printing.Printing.PrintGrid(markSheetDg,"Marksheet");
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
       
    }
}
