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
using Onion.ErrorHandler;

namespace AcademicsDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region RoutedUICommands
        #endregion
        public static MainWindow Default;
        public MainWindow()
            : base(SplashShowing)
        {
            Default = this;
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Errors.displayError("An error occured starting the application", ErrorCode.MainWindowConstructor, ErrorAction.Exit, ex);
            }
        }
        private static void SplashShowing(SmartDesk.SplashWindow w, EventArgs e)
        {
            MySQLHandler.ClassExamTrend.Default.initialize();
            MySQLHandler.ClassList.Default.initialize();
            MySQLHandler.ClassResults.Default.initialize();
            MySQLHandler.ClassTermTrend.Default.initialize();
            MySQLHandler.Exam.Default.initialize();
            MySQLHandler.FormTrend.Form1.initialize();
            MySQLHandler.FormTrend.Form2.initialize();
            MySQLHandler.FormTrend.Form3.initialize();
            MySQLHandler.FormTrend.Form4.initialize();
            MySQLHandler.GradeAnalysis.Default.initialize();
            MySQLHandler.JuniorSubjectSelection.Default.initialize();
            MySQLHandler.MarksEntry.Default.initialize();
            MySQLHandler.MarkSheet.Default.initialize();
            MySQLHandler.MostImproved.Default.initialize();
            MySQLHandler.OverallGrade.Default.initialize();
            MySQLHandler.ReportCardDetails.Default.initialize();
            MySQLHandler.ReportCardMarks.Default.initialize();
            MySQLHandler.ResultSlip.Default.initialize();
            MySQLHandler.ResultSlipAggr.Default.initialize();
            MySQLHandler.SchoolExamTrend.Default.initialize();
            MySQLHandler.SchoolTermTrend.Default.initialize();
            MySQLHandler.SeniorSubjectSelection.Default.initialize();
            MySQLHandler.StreamExamTrend.Default.initialize();
            MySQLHandler.StreamTermTrend.Default.initialize();
            MySQLHandler.StudentExamTrend.Default.initialize();
            MySQLHandler.StudentMarksEntry.Default.initialize();
            MySQLHandler.StudentTermTrend.Default.initialize();
            MySQLHandler.SubjectGrade.Default.initialize();
            MySQLHandler.SubjectResults.Default.initialize();
            MySQLHandler.StreamResults.Default.initialize();
            MySQLHandler.TopStudents.Default.initialize();
            MySQLHandler.TopStudentsPerSubject.Default.initialize();
            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Onion.MySQLHandler.MySQLHelper.BackUp(@"C:\Inventory\Database Backup", 10);
            }
            catch (Exception ex)
            {

                Errors.displayError("An error occured closing the application", ErrorCode.MainWindowClosing, ErrorAction.Exit, ex);
            }
        }
    }
}
