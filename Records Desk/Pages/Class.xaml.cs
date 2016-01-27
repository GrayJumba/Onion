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
    /// Interaction logic for Class.xaml
    /// </summary>
    public partial class Class : UserControl
    {
        private DataRowView rowBeingEdited = null;
        public Class()
        {
            InitializeComponent();
            StreamDG.DataContext = MySQLHandler.Stream.Default.Dt;
            this.IsVisibleChanged += Class_IsVisibleChanged;
        }

        void Class_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
            {
                RecordsDesk.MainWindow.Default.NewCommandBinding.Executed += NewCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.NewCommandBinding.CanExecute += CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommand_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.Executed += SaveCommand_Executed;
            }
            else
            {
                RecordsDesk.MainWindow.Default.NewCommandBinding.Executed -= NewCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.NewCommandBinding.CanExecute -= CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommand_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.Executed -= SaveCommand_Executed;
            }
        }
        
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.Stream.Default.saveChanges())
                MessageBox.Show("Saved");
            else
                MessageBox.Show("Not Saved");

            MySQLHandler.Stream.Default.refreshDt();
        }
        void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MySQLHandler.Stream.Default.Dt.Rows.Add(MySQLHandler.Stream.Default.Dt.NewRow());
        }

        private void StreamDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;

            if (e.Column is DataGridTemplateColumn)
            {
                BindingOperations.ClearAllBindings(e.EditingElement as ContentPresenter);
            }
        }

        private void StreamDG_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            updateSource(Keyboard.FocusedElement as FrameworkElement);
            DataTable changes = MySQLHandler.Stream.Default.Dt.GetChanges();
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
