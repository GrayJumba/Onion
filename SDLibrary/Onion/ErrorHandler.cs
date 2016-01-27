using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Onion.ErrorHandler
{
    public enum ErrorAction
    {
        Exit,
        Restart,
        Continue
    }
    public class ErrorCode
    {

        public static ErrorCode
            //User interface error codes
        SplashWindow = new ErrorCode("#UI1"),
        BgWorker = new ErrorCode("#UI2"),
        SplashClosing = new ErrorCode("#UI3"),
        CreateTemplateFolder = new ErrorCode("#UI4"),
        MainWindowConstructor = new ErrorCode("#UI5"),
        AuthenticationConstructor = new ErrorCode("#UI6"),
        AuthenticationClosing = new ErrorCode("#UI7"),
        LogingIn = new ErrorCode("#UI8"),
        MainWindowClosing = new ErrorCode("#UI9"),
        InitializeDay = new ErrorCode("#UI10"),
        InitializeEmployee = new ErrorCode("#UI11"),
        InitializeStore = new ErrorCode("#UI12"),
        MainWindowCustomizing = new ErrorCode("UI13"),
            //Database error codes
        StoppingMysqlProcess = new ErrorCode("#DB2"),
        StartingMysqlProcess = new ErrorCode("#DB1"),
        ConfirmMySqlProcessIsRunning = new ErrorCode("#DB3"),
        ConnectingToDB = new ErrorCode("#DB4"),
        ReadingFromDB = new ErrorCode("#DB5"),
        WritingToDB = new ErrorCode("#DB6"),
        ConfirmingDBCredentials = new ErrorCode("#DB7"),
        RestoreDb = new ErrorCode("#DB8"),
        BackUpDB = new ErrorCode("#DB9"),
        CreateDB = new ErrorCode("#DB10"),
        ExecuteExists = new ErrorCode("#DB11"),
        ConfirmingDBExists = new ErrorCode("#DB12"),
        DbFileMissing = new ErrorCode("#DB13"),
        DropDB = new ErrorCode("#DB14"),

        //System errors
        ApplicationStart = new ErrorCode("#SYS1"),
        DotNet45Missing = new ErrorCode("#SYS2"),

       //Printing errors
       PrintingDay = new ErrorCode("PR1"),
       PrintingSalaries = new ErrorCode("PR2"),
       PrintingStock = new ErrorCode("PR3");



        public string error_code = "";
        
        public ErrorCode(string error_code)
        {
            this.error_code = error_code;
        }



        
    }
    public class Errors
    {
        public static void displayError(string message, ErrorCode code, ErrorAction type, Exception ex)
        {
           
            string dir = @"C:\Inventory\Error log";
            int max_logs = 10;
            string file = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString().Replace(':', '.');
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                FileInfo[] log_files = directory.GetFiles("*.log").OrderByDescending(f => f.LastWriteTime).ToArray();
                if (log_files.Length >= max_logs)
                {
                    log_files.Last().Delete();
                }
            }
            string path= (dir + @"\" + file + ".log");
            string contents = "Error Code: " + code.error_code;
            if (ex != null)
            {
                contents += "\n\nMain Exception\n" + ex.Message + "\nStackTrace\n" + ex.StackTrace;
                if (ex.InnerException != null)
                {
                   contents+="\n\nInner Exception\n" + ex.InnerException.Message + "\nStackTrace\n" + ex.InnerException.StackTrace;
                }
                if(ex.GetBaseException()!=null)
                {
                    contents += "\n\nBase Exception\n" + ex.GetBaseException().Message + "\nStackTrace\n" + ex.GetBaseException().StackTrace;
                }
                contents+= "\n\nData\n" + ex.Data + "\n\nHresult: " + ex.HResult + "\n\nMethod\n" + ex.TargetSite;
            }


                
            File.WriteAllText(path,contents);

            if (type == ErrorAction.Exit)
            {
                MessageBox.Show(message + "\nError Code: " + code.error_code + "\n" + "Click ok to exit the application.\n" + ex.Message, "An Error Occured");
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Console app
                    System.Environment.Exit(1);
                }
            }
            else if (type == ErrorAction.Restart)
            {
                MessageBox.Show(message + "\nError Code: " + code.error_code + "\n" + "Click ok to restart the application.\n" + ex.Message, "An Error Occured");
                Application.Exit();
                System.Diagnostics.Process.Start(Application.ExecutablePath);
            }
            else
            {
                MessageBox.Show(message + "\nError Code: " + code.error_code + "\n" + ex.Message, "An Error Occured");

            }



        }
        
    }
}
