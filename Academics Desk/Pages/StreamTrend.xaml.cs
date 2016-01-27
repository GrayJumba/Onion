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
    public partial class StreamTrend : UserControl
    {
        SDLibrary.Reports.Converter term_conv = new SDLibrary.Reports.Converter(MySQLHandler.StreamTermTrend.Default.Dt, "term", "average");
        SDLibrary.Reports.Converter exam_conv = new SDLibrary.Reports.Converter(MySQLHandler.StreamExamTrend.Default.Dt, "exam", "average");
        ObservableCollection<SDLibrary.Reports.Point> term_points;
        ObservableCollection<SDLibrary.Reports.Point> exam_points;
        public StreamTrend()
        {
            InitializeComponent();
            term_chart.ItemsSource = term_conv.Points;
            exam_chart.ItemsSource = exam_conv.Points;
            stream_picker.SelectedClassesChanged += stream_picker_SelectedClassesChanged;
        }

        void stream_picker_SelectedClassesChanged(SmartDesk.Controls.ExtendedStreamPicker s, EventArgs e)
        {
            MySQLHandler.StreamTermTrend.Default.refreshDt(stream_picker.SelectedStreamAutoID);
            MySQLHandler.StreamExamTrend.Default.refreshDt(stream_picker.SelectedStreamAutoID);
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
            SmartDesk.Printing.Printing.PrintVisual(StreamTrendGrid);
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
