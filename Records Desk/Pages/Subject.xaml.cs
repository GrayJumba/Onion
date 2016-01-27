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

namespace RecordsDesk.Pages
{
    /// <summary>
    /// Interaction logic for Subject.xaml
    /// </summary>
    public partial class Subject : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public Subject()
        {
            InitializeComponent();
            SubjectDG.DataContext = MySQLHandler.Subject.Default.Dt;
            done_subjects_check.Checked += done_subjects_check_Checked;
            done_subjects_check.Unchecked += done_subjects_check_Unchecked;
            this.IsVisibleChanged += Subject_IsVisibleChanged;
        }
        void done_subjects_check_Unchecked(object sender, RoutedEventArgs e)
        {
            MySQLHandler.Subject.Default.refreshDt(false);
            SubjectDG.Columns[4].Visibility = Visibility.Visible;
        }

        void done_subjects_check_Checked(object sender, RoutedEventArgs e)
        {
            MySQLHandler.Subject.Default.refreshDt(true);
            SubjectDG.Columns[4].Visibility = Visibility.Collapsed;
        }
        void Subject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if(value)
            {
                RecordsDesk.MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommand_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.Executed += SaveCommand_Executed;
                RecordsDesk.MainWindow.Default.RefreshCommandBinding.Executed += RefreshCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.RefreshCommandBinding.CanExecute += RefreshCommandBinding_CanExecute;

            }
            else
            {
                RecordsDesk.MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommand_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.Executed -= SaveCommand_Executed;
                RecordsDesk.MainWindow.Default.RefreshCommandBinding.Executed -= RefreshCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.RefreshCommandBinding.CanExecute -= RefreshCommandBinding_CanExecute;
            }
        }

        private void RefreshCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MySQLHandler.Subject.Default.refreshDt();
        }
      
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.Subject.Default.saveChanges())
            {         
                MessageBox.Show("Save Successful");
            }
            else
            {
                MessageBox.Show("Save unsuccessful");
            }
            MySQLHandler.Subject.Default.refreshDt();
        }

        private void SubjectDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

        private void SubjectDG_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }


        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            updateSource(Keyboard.FocusedElement as FrameworkElement);
            DataTable changes = MySQLHandler.Subject.Default.Dt.GetChanges();
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
            if (fw_element is CheckBox)
            {
                var expression = fw_element.GetBindingExpression(CheckBox.IsCheckedProperty);
                if (expression != null) expression.UpdateSource();
            }
        }

        private void UserControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            updateSource(e.OldFocus as FrameworkElement);
        }


    }
}
