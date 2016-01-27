using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class ClassTrend : UserControl
    {



        SDLibrary.Reports.Converter term_conv = new SDLibrary.Reports.Converter(MySQLHandler.ClassTermTrend.Default.Dt, "term", "average");
        SDLibrary.Reports.Converter exam_conv = new SDLibrary.Reports.Converter(MySQLHandler.ClassExamTrend.Default.Dt, "exam", "average");
        public ClassTrend()
        {
            InitializeComponent();
            term_chart.ItemsSource = term_conv.Points;
            exam_chart.ItemsSource = exam_conv.Points;
            class_picker.comboBox.SelectionChanged += comboBox_SelectionChanged;
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySQLHandler.ClassTermTrend.Default.refreshDt(class_picker.SelectedClassOf);
            MySQLHandler.ClassExamTrend.Default.refreshDt(class_picker.SelectedClassOf);
            term_conv.refresh();
            exam_conv.refresh();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if ((bool)e.NewValue)
            {
                MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed += PrintCommand_Executed;
            }
            else
            {
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommand_Executed;
            }
        }



        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.Printing.Printing.PrintVisual(ClassTrendGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

    }
}
