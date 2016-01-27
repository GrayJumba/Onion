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

namespace RecordsDesk.Pages
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class Teacher : UserControl
    {
        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.Teacher.Default.Dt);
        private DataRowView rowBeingEdited = null;
        public Onion.Controls.DtTraversor Dt_Traversor
        {
            get { return dt_traversor; }
        }
        public Teacher()
        {
            InitializeComponent();
            TecherDG.DataContext = MySQLHandler.Teacher.Default.Dt;
            if (MySQLHandler.Teacher.Default.Dt.Rows.Count == 0)
            {
                MySQLHandler.Teacher.Default.Dt.Rows.Add(MySQLHandler.Teacher.Default.Dt.NewRow());
            }
            dt_traversor.registerButtons(next_button, prev_button);
            dt_traversor.JumpToStart();
            this.IsVisibleChanged+=Teacher_IsVisibleChanged;
        }
        void Teacher_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
            {
                RecordsDesk.MainWindow.Default.NewCommandBinding.Executed += NewCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.SingularCommandBinding.Executed += SingularCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.TabularCommandBinding.Executed += TabularCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.SingularCommandBinding.CanExecute += CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.TabularCommandBinding.CanExecute += CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.NewCommandBinding.CanExecute += CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommand_CanExecute;
                RecordsDesk.MainWindow.Default.SaveCommandBinding.Executed += SaveCommand_Executed;
                RecordsDesk.MainWindow.Default.RefreshCommandBinding.Executed += RefreshCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.RefreshCommandBinding.CanExecute += RefreshCommandBinding_CanExecute;

            }
            else
            {
                RecordsDesk.MainWindow.Default.NewCommandBinding.Executed -= NewCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.SingularCommandBinding.Executed -= SingularCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.TabularCommandBinding.Executed -= TabularCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.SingularCommandBinding.CanExecute -= CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.TabularCommandBinding.CanExecute -= CommandBinding_CanExecute;
                RecordsDesk.MainWindow.Default.NewCommandBinding.CanExecute -= CommandBinding_CanExecute;
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
            MySQLHandler.Teacher.Default.refreshDt();
        }

      
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.Teacher.Default.saveChanges())
                MessageBox.Show("Saved");
            else
                MessageBox.Show("Not saved");  
        }

        void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void TabularCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TableView.IsSelected = true;
        }

        void SingularCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SingleView.IsSelected = true;
        }

        void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MySQLHandler.Teacher.Default.Dt.Rows.Add(MySQLHandler.Teacher.Default.Dt.NewRow());
            dt_traversor.JumbToEnd();
        }

        void save_DoWork(object sender, DoWorkEventArgs e)
        {
            MySQLHandler.Teacher.Default.saveChanges();
            MySQLHandler.Teacher.Default.refreshDt();
        }

        private void TecherDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

        private void TecherDG_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string file_path = Auxiliary.BrowseImage();          
            teacher_image.Source = new ImageSourceConverter().ConvertFromString(file_path) as ImageSource;
        }

        private void TeacherPage_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            updateSource(e.OldFocus as FrameworkElement);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            updateSource(Keyboard.FocusedElement as FrameworkElement);
            DataTable changes = MySQLHandler.Teacher.Default.Dt.GetChanges();
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

    }
}
