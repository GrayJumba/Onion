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
using Onion.MySQLHandler;
using SmartDesk.MySQLHandler;
using SmartDesk.Controls;

namespace FeesDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        #region RoutedUICommands
        public static readonly RoutedUICommand ReceiptSingularCommand = new RoutedUICommand("Receipt Singular", "ReceiptSingular", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control|ModifierKeys.Alt) });
        public static readonly RoutedUICommand ReceiptTabularCommand = new RoutedUICommand("Receipt Tabular", "ReceiptTabular", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Alt) });
        public static readonly RoutedUICommand FeesRegisterCommand = new RoutedUICommand("Fees Register", "FeesRegister", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control | ModifierKeys.Alt) });
        public static readonly RoutedUICommand FeesBalancesCommand = new RoutedUICommand("Fees Balances", "FeesBalances", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.B, ModifierKeys.Control | ModifierKeys.Alt) });
        #endregion

        #region WindowCommandBindings
        public CommandBinding ReceiptSingularCommandBinding;
        public CommandBinding ReceiptTabularCommandBinding;
        public CommandBinding FeesRegisterCommandBinding;
        public CommandBinding FeesBalancesCommandBinding;
        #endregion

        public static MainWindow Default;
        public MainWindow()
            : base(SplashShowing)
        {
            Default = this;

            #region initialize command bindings
            SDWindow.NewCommand.Text = "New Receipt";
            this.NewCommandBinding.Executed += NewReceiptCommand_Executed;
            this.OptionsCommandBinding.Executed += OptionsCommand_Executed;
            ReceiptSingularCommandBinding = new CommandBinding(ReceiptSingularCommand, ReceiptSingularCommand_Executed);
            this.CommandBindings.Add(ReceiptSingularCommandBinding);
            ReceiptTabularCommandBinding = new CommandBinding(ReceiptTabularCommand, ReceiptTabularCommand_Executed);
            this.CommandBindings.Add(ReceiptTabularCommandBinding);
            FeesRegisterCommandBinding = new CommandBinding(FeesRegisterCommand, FeesRegisterCommand_Executed);
            this.CommandBindings.Add(FeesRegisterCommandBinding);
            FeesBalancesCommandBinding = new CommandBinding(FeesBalancesCommand, FeesBalancesCommand_Executed);
            this.CommandBindings.Add(FeesBalancesCommandBinding);
            #endregion
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
            System.Threading.Thread.Sleep(1000);
            SmartDesk.MySQLHandler.UserHandler.Default.initialize();
            SmartDesk.MySQLHandler.SDObjectIDs.StudentIDHandler.initialize();
            SmartDesk.MySQLHandler.SDObjectIDs.StudentUFIHandler.initialize();
            SmartDesk.MySQLHandler.SDObjectIDs.StreamIDHandler.initialize();
            SmartDesk.MySQLHandler.SDObjectIDs.ClassIDHandler.initialize();

            MySQLHandler.Receipt.Default.initialize();
            MySQLHandler.FeeAmount.Form1.initialize();
            MySQLHandler.FeeAmount.Form2.initialize();
            MySQLHandler.FeeAmount.Form3.initialize();
            MySQLHandler.FeeAmount.Form4.initialize();
            MySQLHandler.Invoice.Default.initialize();
            MySQLHandler.Statement.Default.initialize();
            MySQLHandler.Balance.Default.initialize();
            MySQLHandler.Credit.Default.initialize();
            MySQLHandler.AllObjectIDs.PaymentMethodUFIHandler.initialize();
            MySQLHandler.AllObjectIDs.BankUFIHandler.initialize();
            MySQLHandler.AllObjectIDs.ReceiptUFIHandler.initialize();
           
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
        private void NewReceiptCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainTabControl.SelectedItem = ReceiptsTab;
            FeesDesk.MySQLHandler.Receipt.Default.Dt.Rows.Add(FeesDesk.MySQLHandler.Receipt.Default.Dt.NewRow());
            ReceiptSingular.thisUserControl.Dt_Traversor.JumbToEnd();
        }
        private void OptionsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
        }
        private void ReceiptSingularCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainTabControl.SelectedItem = ReceiptsTab;
            Receipts.ReceiptsUserControl.ReceiptTabControl.SelectedItem = Receipts.ReceiptsUserControl.SingularRecieptTab;
        }

        private void ReceiptTabularCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainTabControl.SelectedItem = ReceiptsTab;
            Receipts.ReceiptsUserControl.ReceiptTabControl.SelectedItem = Receipts.ReceiptsUserControl.TabularReceiptTab;
        }
        private void FeesBalancesCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainTabControl.SelectedItem = BalancesTab;
        }

        private void FeesRegisterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainTabControl.SelectedItem = RegisterTab;
        }
        
    }
}
