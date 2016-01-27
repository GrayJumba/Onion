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

namespace TeachersDesk.Pages
{
    /// <summary>
    /// Interaction logic for MarksEntry.xaml
    /// </summary>
    public partial class SubjectMarksEntry : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public static int out_of;
   
        public SubjectMarksEntry()
        {
            InitializeComponent();
            marksEntryDg.DataContext = MySQLHandler.MarksEntry.Default.Dt;
            studentPicker.SelectedStudentsChanged += studentPicker_SelectedStudentsChanged;
            subjectPicker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            examPicker.comboBox.SelectionChanged+=exam_comboBox_SelectionChanged;
            this.IsVisibleChanged += MarksEntry_IsVisibleChanged;
            
        }
        void save_DoWork(object sender, DoWorkEventArgs e)
        {
            MySQLHandler.MarksEntry.Default.saveChanges();
            MySQLHandler.MarksEntry.Default.refreshDt();
            refreshExamProgress();
        }
        private void refreshExamProgress()
        {
            double exam_progress = MySQLHandler.MarksEntry.Default.getExamProgress(examPicker.selected_exam_auto_id);
            progress_label.Content = "Progress: " + (int)(exam_progress)+ "%";
            exam_progress_bar.Value = exam_progress;
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
           // MainWindow.Default.Status_Bar.startWork(save_DoWork, "Saving marks.");

            if (MySQLHandler.MarksEntry.Default.saveChanges())
            {
                MySQLHandler.MarksEntry.Default.refreshDt();
                refreshExamProgress();
                MessageBox.Show("Saved");
            }
            else
            {
                MessageBox.Show("Error Saving");
            }
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }
        void exam_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshOutof();
            refreshExamProgress();
            refresh();

        }
        void refreshOutof()
        {
            out_of = MySQLHandler.MarksEntry.getMarksOutOf(examPicker.selected_exam_auto_id);
            marksEntryDg.Columns[2].Header = "Marks (X/"+ out_of+")";
        }
        private void refresh()
        {
            if (examPicker.selected_exam_auto_id == 0 || subjectPicker.selected_subject_code==0) return;
            MySQLHandler.MarksEntry.Default.refreshDt(studentPicker.SelectedStudentAutoID, studentPicker.SelectedStreamAutoID,
                studentPicker.SelectedClassOf, examPicker.selected_exam_auto_id, subjectPicker.selected_subject_code
                );
           
        }
        void studentPicker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {
            refresh();
        }

        private void marksEntryDg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
          
        }
        bool isManualEdit = false;
        private void marksEntryDg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
          
            if (isManualEdit == true)
                return;
            isManualEdit = true;
            int current_row = e.Row.GetIndex();
            var x = MySQLHandler.MarksEntry.Default.Dt.Rows[current_row]["mark"];
            if (x == DBNull.Value)
                x = null;
            int mark = Convert.ToInt32(x);

            marksEntryDg.CommitEdit(DataGridEditingUnit.Row, true);

            int mark2 = Convert.ToInt32(MySQLHandler.MarksEntry.Default.Dt.Rows[current_row]["mark"]);
            if (mark2 > out_of || mark2 < 0)
            {
                MessageBox.Show("Mark entered is out of range.Previous mark will be retained");
                MySQLHandler.MarksEntry.Default.Dt.Rows[current_row]["mark"] = mark;
                isManualEdit = false;
                return;
            }
            else
            {
                isManualEdit = false;
            }
            //DataRowView rowView = e.Row.Item as DataRowView;
            //rowBeingEdited = rowView;
        }
    }

    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == DBNull.Value)
                value = null;
            var x= (System.Convert.ToDouble(value)/SubjectMarksEntry.out_of)*100;
       
            return Math.Round(x);
     
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}
