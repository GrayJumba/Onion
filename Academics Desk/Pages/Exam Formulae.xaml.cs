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
    public partial class Formulae : UserControl
    {
        public Formulae()
        {
            InitializeComponent();
            Formular_DG.DataContext = MySQLHandler.Formulae.Default.Dt;
            form_picker.SelectedFormChanged += form_picker_SelectedFormChanged;
            term_picker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            this.IsVisibleChanged += UserControl_IsVisibleChanged;
        }

        public void save()
        {
            var x = MySQLHandler.Formulae.Default.Dt.Compute("SUM(account_for)", "");
            if (x == DBNull.Value) x = null;
            int sum = Convert.ToInt32(x);
            if (sum == 100)
            {
                if (MySQLHandler.Formulae.Default.saveChanges())
                    MessageBox.Show("Saved Successfully");
                else MessageBox.Show("Not saved");
            }
            else
            {
                MessageBox.Show("Formular incorrect. Make sure the percentages add up to 100.");
            }
           
        }

        void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            save();
        }

        void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MySQLHandler.Formulae.Default.Dt.GetChanges() == null)
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
            MySQLHandler.Formulae.Default.refreshDt(form_picker.SelectedForm, term_picker.SelectedTermAutoId);
        }

        void form_picker_SelectedFormChanged(SmartDesk.Controls.FormPicker s, EventArgs e)
        {
            MySQLHandler.Formulae.Default.refreshDt(form_picker.SelectedForm, term_picker.SelectedTermAutoId);
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
       
        
    }
}
