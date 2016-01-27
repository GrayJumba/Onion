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
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {

        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.Student.Default.Dt);
        private DataRowView rowBeingEdited = null;
        public  Onion.Controls.DtTraversor Dt_Traversor
        {
            get {  return dt_traversor;  }
        }
        public static Student Default;
        public Student()
        {
            Default=this;
            Console.WriteLine("About to initialize component");
            InitializeComponent();
            Console.WriteLine("Initialized");
            this.IsVisibleChanged += Student_IsVisibleChanged;
            StudentsDG.DataContext = MySQLHandler.Student.Default.Dt;
            if (MySQLHandler.Student.Default.Dt.Rows.Count == 0)
            {
                MySQLHandler.Student.Default.Dt.Rows.Add(MySQLHandler.Student.Default.Dt.NewRow());
            }
            dt_traversor.registerButtons(next_button, prev_button);
            dt_traversor.JumpToStart();
            ex_student_picker.SelectedStudentsChanged += ex_student_picker_SelectedStudentsChanged;
        }

        void ex_student_picker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {
            refresh();
        }

        void Student_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if(value)
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
                RecordsDesk.MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;

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
                RecordsDesk.MainWindow.Default.PrintCommandBinding.Executed -= PrintCommandBinding_Executed;
                RecordsDesk.MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommandBinding_CanExecute;
      }
        }

        private void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintGrid(StudentsDG,"");
        }

        private void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RefreshCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            refresh();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            updateSource(Keyboard.FocusedElement as FrameworkElement);
            DataTable changes = MySQLHandler.Student.Default.Dt.GetChanges();
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

        private void StudentPage_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            updateSource(e.OldFocus as FrameworkElement);
        }
        private void refresh()
        {
            MySQLHandler.Student.Default.refreshDt(ex_student_picker.SelectedStudentAutoID,ex_student_picker.SelectedStreamAutoID,ex_student_picker.SelectedClassOf);
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*Save Changes*/
            if (MySQLHandler.Student.Default.saveChanges())
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
            MySQLHandler.Student.Default.Dt.Rows.Add(MySQLHandler.Student.Default.Dt.NewRow());
            dt_traversor.JumbToEnd();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string file_path=Auxiliary.BrowseImage();
            student_image.Source = new ImageSourceConverter().ConvertFromString(file_path) as ImageSource;
        }

        private void StudentsDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column is DataGridTemplateColumn)
            {
                BindingOperations.ClearAllBindings(e.EditingElement as ContentPresenter);
            }
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }
        private void StudentsDG_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }
        private void StudentsDG_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            Console.WriteLine("Beginning edit");
        }
    }
    public class Auxiliary
    {
        public  static string BrowseImage()
        {
            string filepath;
            System.Windows.Forms.FileDialog explorer = new System.Windows.Forms.OpenFileDialog();
            explorer.Filter = "image Files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            explorer.FilterIndex = 2;
            explorer.RestoreDirectory = true;
            if (explorer.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filepath = explorer.FileName;
                    return filepath;
                }
                catch (Exception )
                {
                    return "";
                }
            }
            return "";
        }
    }
}
