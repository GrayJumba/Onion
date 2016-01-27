using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Exam.xaml
    /// </summary>
    public partial class Exam : UserControl
    {
      
        ObservableCollection<SDLibrary.Reports.Point> points;
        private DataRowView rowBeingEdited = null;
        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.Exam.Default.Dt);
        public Onion.Controls.DtTraversor Dt_Traversor
        {
            get { return dt_traversor; }
        }
        public Exam()
        {
            InitializeComponent();
            ExamDG.DataContext = MySQLHandler.Exam.Default.Dt;
            dt_traversor.registerButtons(next_button, prev_button);
            Dt_Traversor.selectionChanged += Dt_Traversor_selectionChanged;
            dt_traversor.JumpToStart();
            examPicker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            termPicker.comboBox.SelectionChanged+=comboBox_SelectionChanged;
            Dt_Traversor.selectionChanged += Dt_Traversor_selectionChanged;
        }

        void Dt_Traversor_selectionChanged(Onion.Controls.DtTraversor d, EventArgs e)
        {
            ShowProgress(d.CurrentRowIndex);
        }
        void ShowProgress(int current_row_index)
        {
            points = new ObservableCollection<SDLibrary.Reports.Point>();
            var x = dt_traversor["auto_id"];
            if (x == DBNull.Value) x = null;
            double progress = MySQLHandler.MarksEntry.Default.getExamProgress(Convert.ToInt32(x));
            if (MySQLHandler.Exam.Default.Dt.Rows.Count == 0)
            {
                MySQLHandler.Exam.Default.Dt.Rows.Add(MySQLHandler.Exam.Default.Dt.NewRow());
            }
            points.Add(new SDLibrary.Reports.Point { X = "", Y = progress });
            Charts.ItemsSource = points;
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySQLHandler.Exam.Default.refreshDt(examPicker.selected_exam_auto_id, termPicker.SelectedTermAutoId);
        }

        void Exam_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed += SaveCommandBinding_Executed;
                MainWindow.Default.NewCommandBinding.CanExecute += NewCommandBinding_CanExecute;
                MainWindow.Default.NewCommandBinding.Executed += NewCommandBinding_Executed;
                MainWindow.Default.RefreshCommandBinding.CanExecute += RefreshCommandBinding_CanExecute;
                MainWindow.Default.RefreshCommandBinding.Executed += RefreshCommandBinding_Executed;
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= SaveCommandBinding_Executed;
                MainWindow.Default.NewCommandBinding.CanExecute -= NewCommandBinding_CanExecute;
                MainWindow.Default.NewCommandBinding.Executed -= NewCommandBinding_Executed;
                MainWindow.Default.RefreshCommandBinding.CanExecute -= RefreshCommandBinding_CanExecute;
                MainWindow.Default.RefreshCommandBinding.Executed -= RefreshCommandBinding_Executed;
            }
        }

        void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MySQLHandler.Exam.Default.refreshDt();
        }

        void RefreshCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           MySQLHandler.Exam.Default.Dt.Rows.Add(MySQLHandler.Exam.Default.Dt.NewRow());
            dt_traversor.JumbToEnd();
        }

        void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(MySQLHandler.Exam.Default.saveChanges())
                MessageBox.Show("Save successful");
            MySQLHandler.Exam.Default.refreshDt();
              
        }
        private void ExamDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

        private void ExamDG_CurrentCellChanged(object sender, EventArgs e)
        {
             if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }

        }


        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            updateSource(Keyboard.FocusedElement as FrameworkElement);
            DataTable changes = MySQLHandler.Exam.Default.Dt.GetChanges();
            if (changes != null && changes.Rows.Count != 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        private void updateSource(FrameworkElement fw_element)
        {
            if (fw_element is TextBox)
            {
                var expression = fw_element.GetBindingExpression(TextBox.TextProperty);
                if (expression != null) expression.UpdateSource();
            }
            if (fw_element is ComboBox)
            {
                var expression = fw_element.GetBindingExpression(ComboBox.TextProperty);
                if (expression != null) expression.UpdateSource();
            }
        }

        private void ExamPage_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            updateSource(e.OldFocus as FrameworkElement);
        }
    }
}
