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
    public partial class SubjectTeachers : UserControl
    {
        public SubjectTeachers()
        {
            InitializeComponent();
            SubjectTeacher_DG.DataContext = MySQLHandler.SubjectTeacher.Default.Dt;
            stream_picker.SelectedStreamChanged += stream_picker_SelectedStreamChanged;
            this.IsVisibleChanged += UserControl_IsVisibleChanged;
        }

        void stream_picker_SelectedStreamChanged(SmartDesk.Controls.StreamPicker s, EventArgs e)
        {
            MySQLHandler.SubjectTeacher.Default.refreshDt(stream_picker.SelectedStreamAutoID);
        }
        void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MySQLHandler.SubjectTeacher.Default.saveChanges())
                MessageBox.Show("Saved Successfully");
            else MessageBox.Show("Not saved");
        }

        void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MySQLHandler.SubjectTeacher.Default.Dt.GetChanges() == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.SaveCommandBinding.CanExecute += SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed += SaveCommand_Executed;
            }
            else
            {
                MainWindow.Default.SaveCommandBinding.CanExecute -= SaveCommand_CanExecute;
                MainWindow.Default.SaveCommandBinding.Executed -= SaveCommand_Executed;
            }
        }

        private void SubjectTeacher_DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column is DataGridTemplateColumn)
            {
                BindingOperations.ClearAllBindings(e.EditingElement as ContentPresenter);
            }
        }
       
        
    }
}
