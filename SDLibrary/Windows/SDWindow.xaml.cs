using SDLibrary.Windows;
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
using System.Windows.Shapes;

namespace SmartDesk.Controls
{
    /// <summary>
    /// Interaction logic for SDWindow.xaml
    /// </summary>
    public  class SDWindow :MahApps.Metro.Controls.MetroWindow
    {
       
        #region RoutedUICommands
        public static readonly RoutedUICommand NewCommand = new RoutedUICommand("New", "New", typeof(SDWindow), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });
        public static readonly RoutedUICommand SaveCommand = new RoutedUICommand( "Save", "Save", typeof(SDWindow),new InputGestureCollection(){ new KeyGesture(Key.S, ModifierKeys.Control)});
        public static readonly RoutedUICommand PrintCommand = new RoutedUICommand("Print", "Print", typeof(SDWindow), new InputGestureCollection() { new KeyGesture(Key.P, ModifierKeys.Control) });
        public static readonly RoutedUICommand OptionsCommand = new RoutedUICommand("Options", "Options", typeof(SDWindow), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Control) });
        public static readonly RoutedUICommand ExitCommand = new RoutedUICommand("Exit", "Exit", typeof(SDWindow), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });
        public static readonly RoutedUICommand RefreshCommand = new RoutedUICommand("Refresh", "Refresh", typeof(SDWindow), new InputGestureCollection() { new KeyGesture(Key.F5)});
        public static readonly RoutedUICommand ManageUserCommand = new RoutedUICommand("Manage User", "ManageUserCmd", typeof(SDWindow));
        public static readonly RoutedUICommand ManageDatabaseCommand = new RoutedUICommand("ManageDatabase", "ManageDatabase", typeof(SDWindow));
        public static readonly RoutedUICommand ManualCommand = new RoutedUICommand("Manual", "Manual", typeof(SDWindow));
        public static readonly RoutedUICommand QuestionCommand = new RoutedUICommand("Ask a question", "Question", typeof(SDWindow));
        public static readonly RoutedUICommand ForumsCommand = new RoutedUICommand("SmartDesk Forums", "Forums", typeof(SDWindow));
        public static readonly RoutedUICommand SendSmileCommand = new RoutedUICommand("Send a smile", "SendSmile", typeof(SDWindow));
        public static readonly RoutedUICommand SendFrownCommand = new RoutedUICommand("Send a frown", "SendFrown", typeof(SDWindow));
        public static readonly RoutedUICommand ReportBugCommand = new RoutedUICommand("Report a bug", "ReportBug", typeof(SDWindow));
        public static readonly RoutedUICommand LicenseCommand = new RoutedUICommand("License", "License", typeof(SDWindow));
        public static readonly RoutedUICommand AboutCommand = new RoutedUICommand("About SmartDesk", "About", typeof(SDWindow));
        public static readonly RoutedUICommand PrintManyCommand = new RoutedUICommand("Print Many", "Print Many", typeof(SDWindow));

        #endregion
        #region WindowCommandBindings
        public CommandBinding NewCommandBinding;
        public CommandBinding SaveCommandBinding;
        public CommandBinding PrintCommandBinding;
        public CommandBinding OptionsCommandBinding;
        public CommandBinding ExitCommandBinding;
        public CommandBinding RefreshCommandBinding;
        public CommandBinding ManageUserCommandBinding;
        public CommandBinding ManageDatabaseCommandBinding;
        public CommandBinding ManualCommandBinding;
        public CommandBinding QuestionCommandBinding;
        public CommandBinding ForumsCommandBinding;
        public CommandBinding SendSmileCommandBinding;
        public CommandBinding SendFrownCommandBinding;
        public CommandBinding ReportBugCommandBinding;
        public CommandBinding LicenseCommandBinding;
        public CommandBinding AboutCommandBinding;
        public CommandBinding PrintManyCommandBinding;

        #endregion

        public static SmartDesk.MySQLHandler.User current_user = new SmartDesk.MySQLHandler.User();
        public SDWindow(SplashWindow.SplashShowing SplashShowing)
        {
            GlowBrush = Application.Current.TryFindResource("MainBrush") as SolidColorBrush;
            Style st=new Style(){ TargetType=typeof(MahApps.Metro.Controls.MetroTabItem)};
            st.Setters.Add(new Setter(MahApps.Metro.Controls.ControlsHelper.HeaderFontSizeProperty, (double)15));
            this.Resources.Add(typeof(MahApps.Metro.Controls.MetroTabItem), st);

            #region starting the application

            Onion.SystemR.SystemRequirements.confirmDotNet45Installed();
            SmartDesk.SplashWindow splash = new SmartDesk.SplashWindow();
            splash.splashShowing += SplashShowing;
            splash.Run();
            splash.ShowDialog();
            string[] cmd_args = Environment.GetCommandLineArgs();
            string username = cmd_args.FirstOrDefault(x => x.StartsWith("username="));
            string password = cmd_args.FirstOrDefault(x => x.StartsWith("password="));
            if (username != null) username = username.Substring(9);
            if (password != null) password = password.Substring(9);
            if (!current_user.logIn(username, password))
            {
                SmartDesk.AuthenticationWindow authe_window = new SmartDesk.AuthenticationWindow();
                authe_window.ShowDialog();
                current_user = authe_window.user;
            }
            #endregion

            #region initialize command bindings
            NewCommandBinding = new CommandBinding(NewCommand, NewCommand_Executed, NewCommand_CanExecute);
            this.CommandBindings.Add(NewCommandBinding);
            RefreshCommandBinding = new CommandBinding(RefreshCommand, RefreshCommand_Executed, RefreshCommand_CanExecute);
            this.CommandBindings.Add(RefreshCommandBinding);
            SaveCommandBinding = new CommandBinding(SaveCommand, SaveCommand_Executed, SaveCommand_CanExecute);
            this.CommandBindings.Add(SaveCommandBinding);
            PrintCommandBinding = new CommandBinding(PrintCommand, PrintCommand_Executed,PrintCommand_CanExecute);
            this.CommandBindings.Add(PrintCommandBinding);
            OptionsCommandBinding = new CommandBinding(OptionsCommand, OptionsCommand_Executed);
            this.CommandBindings.Add(OptionsCommandBinding);
            ManageUserCommandBinding = new CommandBinding(ManageUserCommand, ManageUserCommand_Executed);
            this.CommandBindings.Add(ManageUserCommandBinding);
            ManageDatabaseCommandBinding = new CommandBinding(ManageDatabaseCommand, ManageDatabaseCommand_Executed);
            this.CommandBindings.Add(ManageDatabaseCommandBinding);
            ManualCommandBinding = new CommandBinding(ManualCommand, ManualCommand_Executed);
            this.CommandBindings.Add(ManualCommandBinding);
            QuestionCommandBinding = new CommandBinding(QuestionCommand, QuestionCommand_Executed);
            this.CommandBindings.Add(QuestionCommandBinding);
            ForumsCommandBinding = new CommandBinding(ForumsCommand, ForumsCommand_Executed);
            this.CommandBindings.Add(ForumsCommandBinding);
            SendSmileCommandBinding = new CommandBinding(SendSmileCommand, SendSmileCommand_Executed);
            this.CommandBindings.Add(SendSmileCommandBinding);
            SendFrownCommandBinding = new CommandBinding(SendFrownCommand, SendFrownCommand_Executed);
            this.CommandBindings.Add(SendFrownCommandBinding);
            ReportBugCommandBinding = new CommandBinding(ReportBugCommand, ReportBugCommand_Executed);
            this.CommandBindings.Add(ReportBugCommandBinding);
            LicenseCommandBinding = new CommandBinding(LicenseCommand, LicenseCommand_Executed);
            this.CommandBindings.Add(LicenseCommandBinding);
            AboutCommandBinding = new CommandBinding(AboutCommand, AboutCommand_Executed);
            this.CommandBindings.Add(AboutCommandBinding);
            ExitCommandBinding = new CommandBinding(ExitCommand, ExitCommand_Executed);
            this.CommandBindings.Add(ExitCommandBinding);
            PrintManyCommandBinding = new CommandBinding(PrintManyCommand);
            this.CommandBindings.Add(PrintManyCommandBinding);
            #endregion
        }

        private void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Refreshing");
        }
       
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Now Saving");
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PrintManyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("New Item");
        }
        private void PrintCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Now Printing");
        }

        private void OptionsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //OptionsWindow optionsWindow = new OptionsWindow();
            //optionsWindow.ShowDialog();
        }
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void ManageUserCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.ManageUserWindow manageUserWindow = new SmartDesk.ManageUserWindow(current_user);
            manageUserWindow.ShowDialog();
        }

        private void ManageDatabaseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.ManageDatabaseWindow manageDataBaseWindow = new SmartDesk.ManageDatabaseWindow();
            manageDataBaseWindow.ShowDialog();
        }

        private void ManualCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://smartdesk.onion.co.ke/manual");
        }

        private void QuestionCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.MessageWindow messageWindow = new SmartDesk.MessageWindow("Question", "Ask your question here.");
            messageWindow.ShowDialog();
        }

        private void ForumsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://smartdesk.onion.co.ke/forums");
        }
        private void SendSmileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.MessageWindow messageWindow = new SmartDesk.MessageWindow("Smile", "Tell us what you liked.");
            messageWindow.ShowDialog();
        }
        private void SendFrownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.MessageWindow messageWindow = new SmartDesk.MessageWindow("Frown", "Tell us what you did not like.");
            messageWindow.ShowDialog();
        }
        private void ReportBugCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.MessageWindow messageWindow = new SmartDesk.MessageWindow("Bug", "Describe the bug here.");
            messageWindow.ShowDialog();
        }
        private void LicenseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.OkWindow licenseWindow = new SmartDesk.OkWindow();
            licenseWindow.ShowDialog();
        }
        private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SmartDesk.AboutWindow aboutWindow = new SmartDesk.AboutWindow();
            aboutWindow.ShowDialog();
        }

    }
}
