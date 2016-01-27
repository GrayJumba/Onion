using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for MarksEntry.xaml
    /// </summary>
    public partial class StudentMarksEntry : UserControl
    {
        private DataRowView rowBeingEdited = null;
     
        public StudentMarksEntry()
        {
            InitializeComponent();
            studentMarksEntryDg.DataContext = MySQLHandler.StudentMarksEntry.Default.Dt;
            studentPicker.SelectedStudentsChanged += studentPicker_SelectedStudentsChanged;
            examPicker.comboBox.SelectionChanged+=exam_comboBox_SelectionChanged;
            this.IsVisibleChanged += MarksEntry_IsVisibleChanged;
            
        }
      
        void MarksEntry_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed+=SaveCommand_Executed;
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= SaveCommand_Executed;
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {          
           e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.StudentMarksEntry.Default.saveChanges())
                MessageBox.Show("Saved");
            else
                MessageBox.Show("Not Saved");
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }
        void exam_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            if (examPicker.selected_exam_auto_id == 0 ) return;
            MySQLHandler.StudentMarksEntry.Default.refreshDt(studentPicker.SelectedStudentAutoID,  examPicker.selected_exam_auto_id);
           
        }
        void studentPicker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {
            refresh();
        }

        private void studentMarksEntryDg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void studentMarksEntryDg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }
    }
}
