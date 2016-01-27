using System;
using System.Collections.Generic;
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
using SmartDesk.Printing;

namespace AcademicsDesk.Pages
{
    /// <summary>
    /// Interaction logic for ClassList.xaml
    /// </summary>
    public partial class ClassList : UserControl
    {
        public ClassList()
        {
            InitializeComponent();
            classListDg.DataContext = MySQLHandler.ClassList.Default.Dt;
            if (classListDg.Columns.Count >= 2) classListDg.Columns[1].Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            Purpose_Combo.SelectionChanged+=Purpose_Combo_SelectionChanged;
            streamPicker.SelectedClassesChanged += streamPicker_SelectedClassesChanged;
            this.IsVisibleChanged += ClassList_IsVisibleChanged;
        }

        void ClassList_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommandBinding_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed += PrintCommandBinding_Executed;
                MainWindow.Default.RefreshCommandBinding.CanExecute+=RefreshCommandBinding_CanExecute;
                MainWindow.Default.RefreshCommandBinding.Executed+=RefreshCommandBinding_Executed;
            }
            else
            {
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommandBinding_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommandBinding_Executed;
                MainWindow.Default.RefreshCommandBinding.CanExecute -= RefreshCommandBinding_CanExecute;
                MainWindow.Default.RefreshCommandBinding.Executed -= RefreshCommandBinding_Executed;
            }
        }

        private void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            refresh();
        }

        private void RefreshCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Printing.PrintGrid(classListDg,"");
        }

        void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MySQLHandler.ClassList.Default.Dt.Rows.Count == 0)
                e.CanExecute = false;
            else e.CanExecute = true;
        }

        void streamPicker_SelectedClassesChanged(SmartDesk.Controls.ExtendedStreamPicker s, EventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.ClassList.Default.refreshDt(streamPicker.SelectedStreamAutoID.ToString(), streamPicker.SelectedClassOf.ToString(), (Purpose_Combo.SelectedItem as ComboBoxItem).Content.ToString());
            classListDg.DataContext = new Object();
            classListDg.DataContext = MySQLHandler.ClassList.Default.Dt;
           
            Console.WriteLine("Refreshed");
        }
        private void Purpose_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                refresh();
            }
        }

        private void classListDg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();
            Console.WriteLine("Autogenerting " + headername);
            if (headername == "name")
            {
                e.Column.Width = new DataGridLength(3.0, DataGridLengthUnitType.Star);
            }
            //if (headername == "name")
            //{
            //    e.Column.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            //}

        }
    }
}
