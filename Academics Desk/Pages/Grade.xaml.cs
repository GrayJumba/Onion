using System;
using System.Collections.Generic;
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
    /// Interaction logic for Grade.xaml
    /// </summary>
    public partial class Grade : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public Grade()
        {
            InitializeComponent();
          
            Overall_DG.DataContext = MySQLHandler.OverallGrade.Default.Dt;
            SubGrade_DG.DataContext = MySQLHandler.SubjectGrade.Default.Dt;
            form_picker.SelectedFormChanged += form_picker_SelectedFormChanged;
            subject_picker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            this.IsVisibleChanged += Grade_IsVisibleChanged;
        }

        void Grade_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommandBinding_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed += SaveCommandBinding_Executed;
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommandBinding_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= SaveCommandBinding_Executed;
            }
        }

        void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.OverallGrade.Default.saveChanges() && MySQLHandler.SubjectGrade.Default.saveChanges())
                MessageBox.Show("Saved");
            MessageBox.Show("Not Saved");
        }

        void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MySQLHandler.SubjectGrade.Default.Dt.GetChanges() == null && MySQLHandler.OverallGrade.Default.Dt.GetChanges()==null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySQLHandler.SubjectGrade.Default.refreshDt(form_picker.SelectedForm, subject_picker.selected_subject_code);
        }

        void form_picker_SelectedFormChanged(SmartDesk.Controls.FormPicker s, EventArgs e)
        {
            MySQLHandler.SubjectGrade.Default.refreshDt(form_picker.SelectedForm, subject_picker.selected_subject_code);
            MySQLHandler.OverallGrade.Default.refreshDt(form_picker.SelectedForm);
        }
        private void Overall_DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() != "from") return;
            if (e.Row.GetIndex() == 11)
            {
                    MySQLHandler.OverallGrade.Default.Dt.Rows[11]["lower_bound"] = 0;
                    return;
            }
            int new_value =Convert.ToInt32((e.EditingElement as MahApps.Metro.Controls.NumericUpDown).Value);
            var obj=MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex()]["upper_bound"];
            if(obj==DBNull.Value) obj=0;
            int upper_value=Convert.ToInt32(obj);
            string error = "";
            if (new_value <0 || new_value >100)
            {
                error = "Value can not be negative nor above 100.";
            }
            if (new_value > upper_value)
            {
                error+=" Value can not be greater than upper boundary.";
            }
            if (!String.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error);
                if (upper_value == 0) MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = DBNull.Value;
                else MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = upper_value;

                var l_obj = MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex() + 1]["upper_bound"];
                if (l_obj == DBNull.Value)
                {
                    MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = DBNull.Value;
                }
                else MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = Convert.ToInt32(l_obj) + 1;
            }
            else
            {
               MySQLHandler.OverallGrade.Default.Dt.Rows[e.Row.GetIndex() + 1]["upper_bound"] = new_value - 1;
            }
        }
        private void SubGrade_DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.Column.Header.ToString().ToLower() != "from") return;
            if (e.Row.GetIndex() == 11)
            {
                MySQLHandler.SubjectGrade.Default.Dt.Rows[11]["lower_bound"] = 0;
                return;
            }
            int new_value = Convert.ToInt32((e.EditingElement as MahApps.Metro.Controls.NumericUpDown).Value);
            var obj = MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex()]["upper_bound"];
            if (obj == DBNull.Value) obj = 0;
            int upper_value = Convert.ToInt32(obj);
            string error = "";
            if (new_value < 0 || new_value > 100)
            {
                error = "Value can not be negative nor above 100.";
            }
            if (new_value > upper_value)
            {
                error += " Value can not be greater than upper boundary.";
            }
            if (!String.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error);
                if (upper_value == 0) MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = DBNull.Value;
                else MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = upper_value;

                var l_obj = MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex() + 1]["upper_bound"];
                if (l_obj == DBNull.Value)
                {
                    MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = DBNull.Value;
                }
                else MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex()]["lower_bound"] = Convert.ToInt32(l_obj) + 1;
            }
            else
            {
                MySQLHandler.SubjectGrade.Default.Dt.Rows[e.Row.GetIndex() + 1]["upper_bound"] = new_value - 1;
            }
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

        private void SubGrade_DG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SubGrade_DG_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }
    }
}
