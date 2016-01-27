using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SDLibrary.Reports;

namespace AcademicsDesk.Pages
{
    /// <summary>
    /// Interaction logic for ReportCards.xaml
    /// </summary>
    public partial class ReportCards : UserControl
    {
        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.ReportCardDetails.Default.Dt);
        public Onion.Controls.DtTraversor Dt_Traversor
        {
            get { return dt_traversor; }
        }
        SDLibrary.Reports.Converter term_points =new Converter(MySQLHandler.StudentTermTrend.ReportCard.Dt, "term", "average");
        SDLibrary.Reports.Converter subject_points=new Converter(MySQLHandler.ReportCardMarks.Default.Dt, "subject", "average");
        public ReportCards()
        {
            InitializeComponent();
            term_chart.ItemsSource = term_points.Points;
            subject_chart.ItemsSource = subject_points.Points;
            student_picker.SelectedStudentsChanged += student_picker_SelectedStudentsChanged;
            term_picker.comboBox.SelectionChanged += comboBox_SelectionChanged;
            marksDg1.DataContext = MySQLHandler.ReportCardMarks.Default.Dt;
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            MySQLHandler.ReportCardDetails.Default.refreshDt(student_picker.SelectedStudentAutoID,term_picker.SelectedTermAutoId);
            MySQLHandler.ReportCardMarks.Default.refreshDt(student_picker.SelectedStudentAutoID, term_picker.SelectedTermAutoId);
            MySQLHandler.StudentTermTrend.ReportCard.refreshDt(student_picker.SelectedStudentAutoID);
            term_points.refresh();
            subject_points.refresh();
            marksDg1.AutoGenerateColumns = false;
            marksDg1.AutoGenerateColumns = true;
        }
        void student_picker_SelectedStudentsChanged(SmartDesk.Controls.ExtendedStudentPicker s, EventArgs e)
        {

                refresh();

        }

     
        private void ReportCard_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                MainWindow.Default.PrintCommandBinding.CanExecute += PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed += PrintCommand_Executed;
                MainWindow.Default.PrintManyCommandBinding.CanExecute += PrintManyCommand_CanExecute;
                MainWindow.Default.PrintManyCommandBinding.Executed += PrintManyCommand_Executed;
            }
            else
            {
                MainWindow.Default.PrintCommandBinding.CanExecute -= PrintCommand_CanExecute;
                MainWindow.Default.PrintCommandBinding.Executed -= PrintCommand_Executed;
                MainWindow.Default.PrintManyCommandBinding.CanExecute -= PrintManyCommand_CanExecute;
                MainWindow.Default.PrintManyCommandBinding.Executed -= PrintManyCommand_Executed;
            }
        }

        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
           
        }
        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
               SmartDesk.Printing.Printing.PrintVisualWithoutDialog(ReportCard);
           
        }
        private void PrintManyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

        }
        private void PrintManyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int count = student_picker.getNumberOfStudents();
            for (int i = 0; i < 5; i++)
            {
                SmartDesk.Printing.Printing.PrintVisualWithoutDialog(ReportCard);
                Dt_Traversor.Next();
            }

        }

        private void marksDg1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();
            if (headername == "subject_code")
            {
                e.Cancel = true;
            }
            //if (headername == "name")
            //{
            //    e.Column.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            //}
        }

       
    }

   

  
}
