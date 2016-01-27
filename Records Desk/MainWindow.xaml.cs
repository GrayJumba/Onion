using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace RecordsDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /*Commands*/

        #region RoutedUICommands
        public static readonly RoutedUICommand SingularCommand = new RoutedUICommand("Singular", "Singular", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control| ModifierKeys.Alt) });
        public static readonly RoutedUICommand TabularCommand = new RoutedUICommand("Tabular", "Tabular", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Alt) });
        public static readonly RoutedUICommand ViewStudentCommand = new RoutedUICommand("View Student", "viewstudent", typeof(MainWindow));
        #endregion

        #region WindowCommandBindings
        public CommandBinding SingularCommandBinding;
        public CommandBinding TabularCommandBinding;
        public CommandBinding ViewStudentCommandBinding;
     
        #endregion

        public static MainWindow Default;
        public MainWindow()
            : base(SplashShowing)
        {
            Default = this;
            SingularCommandBinding = new CommandBinding(SingularCommand, null, SingularCommandBinding_CanExecute);
            this.CommandBindings.Add(SingularCommandBinding);
            TabularCommandBinding = new CommandBinding(TabularCommand, null, TabularCommandBinding_CanExecute);
            this.CommandBindings.Add(TabularCommandBinding);
            ViewStudentCommandBinding = new CommandBinding(ViewStudentCommand, ViewStudentCommandBinding_Executed, ViewStudentCommandBinding_CanExecute);
            this.CommandBindings.Add(ViewStudentCommandBinding);
        }
        void ViewStudentCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int index=Convert.ToInt32(e.Parameter);
            if (e.Parameter!=null && index>=0 && index<MySQLHandler.Student.Default.Dt.Rows.Count)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        void ViewStudentCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StudentsTab.IsSelected = true;
            Pages.Student.Default.SingleView.IsSelected = true;
            int index =Convert.ToInt32(e.Parameter);
            Pages.Student.Default.Dt_Traversor.CurrentRowIndex = index;
        }

     
        private void TabularCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void SingularCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private static void SplashShowing(SmartDesk.SplashWindow w, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
        }
      



    }
}
