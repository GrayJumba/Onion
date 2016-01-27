using System;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Reflection;

namespace Onion.MySQLHandler
{

    public class MySQLHelper
    {
        public static string db_name = SDLibrary.Properties.Settings.Default.db_name;
        private static string db_version = SDLibrary.Properties.Settings.Default.db_version;
        private static string db_hostname = SDLibrary.Properties.Settings.Default.db_host;
        private static string db_username = SDLibrary.Properties.Settings.Default.db_username;
        private static string db_password = SDLibrary.Properties.Settings.Default.db_password;
        public static string db_file_resource = "SDLibrary.Database.database.txt";
        private static bool allow_user_variables = true;
        private static MySqlConnection conn = new MySqlConnection();
        public static MySqlCommand cmd = new MySqlCommand();
        public static MySqlDataReader data_reader = null;
        static MySQLHelper()
        {
            ConfirmMySqlProcessIsRunning();
            ConfirmExistingCredentials();
            ConfirmDBExists();
            ConfirmDBVersion();
        }
        public static MySqlConnection getNewConnection()
        {
            MySqlConnection new_conn = new MySqlConnection();
            new_conn.ConnectionString = "SERVER=" + db_hostname + ";" + "DATABASE=" + db_name + ";" + "UID=" + db_username + ";" + "PASSWORD=" + db_password + ";Allow User Variables=" + allow_user_variables + ";";
            return new_conn;
        }
        public static MySqlConnection getDefaultConnection()
        {

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.ConnectionString = "SERVER=" + db_hostname + ";" + "DATABASE=" + db_name + ";" + "UID=" + db_username + ";" + "PASSWORD=" + db_password + ";Allow User Variables=" + allow_user_variables + ";";
            return conn;
        }
        public static bool CreateRemoteUser()
        {
            string sql = "CREATE USER  'remote'@'%' IDENTIFIED BY PASSWORD 'onion'; GRANT ALL ON school.* TO remote@'%' IDENTIFIED BY 'onion';";
            return executeWrite(sql);    
        }

        public static bool stopMysql()
        {
            try
            {
                string folder_path1 = @" C:\Program Files\MySQL\MySQL Server*\bin";
                string folder_path2 = @" C:\Program Files (x86)\MySQL\MySQL Server*\bin";

                FileInfo fi1 = new FileInfo(folder_path1 + "\\mysqladmin.exe");
                FileInfo fi2 = new FileInfo(folder_path2 + "\\mysqladmin.exe");
                if (fi1.Exists)
                {
                    System.Diagnostics.Process.Start(fi1.FullName, "--user=root shutdown");
                    return true;
                }
                else if (fi2.Exists)
                {
                    System.Diagnostics.Process.Start(fi2.FullName, "--user=root shutdown");
                    return true;
                }
                else
                {
                    ErrorHandler.Errors.displayError("We were unabe to stop Mysql.Please stop it manually.", ErrorHandler.ErrorCode.StoppingMysqlProcess, ErrorHandler.ErrorAction.Continue, new Exception());
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("We were unabe to stop Mysql.Please stop it manually.", ErrorHandler.ErrorCode.StoppingMysqlProcess, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
        }

        private static bool ConfirmDBVersion()
        {
            try
            {
            
                string current_db_version="";
                if(!connect()) return false;
                string query = "SELECT * FROM dbversion";
                if(executeRead(query))
                {
                    if (data_reader.Read())
                    {
                        current_db_version = data_reader["major"].ToString() + "." + data_reader["minor"].ToString() + "." + data_reader["patch"].ToString();
                    }
                    else return false;
                }
                if (current_db_version==db_version)
                    return true;
                else
                {
                    BackUp(@"C:\Inventory\Old Database Backup", 10);
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(db_file_resource);
                    if (stream == null)
                    {
                        ErrorHandler.Errors.displayError("We are unable to install database.", ErrorHandler.ErrorCode.DbFileMissing, ErrorHandler.ErrorAction.Exit, new Exception());
                    }
                    StreamReader reader = new StreamReader(stream);
                    string create_db_query = reader.ReadToEnd();
                    if (!connectWithoutDbName()) throw new Exception("Connecting to db failed");
                    MySqlBackup bk = new MySqlBackup(cmd);
                    bk.ImportFromString(create_db_query);
                    return true;
                }
            }

            catch (Exception ex)
            {
                dropDB();
                ErrorHandler.Errors.displayError("We were unable to install database.", ErrorHandler.ErrorCode.ConfirmingDBExists, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }

        }
        private static bool ConfirmDBExists()
        {
            try
            {
                if (DBExists())
                    return true;
                else
                {
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(db_file_resource);
                    if(stream==null)
                    {
                        ErrorHandler.Errors.displayError("We are unable to install database.", ErrorHandler.ErrorCode.DbFileMissing, ErrorHandler.ErrorAction.Exit,new Exception());
                    }
                    StreamReader reader = new StreamReader(stream);
                    string create_db_query = reader.ReadToEnd();
                    if (!connectWithoutDbName()) throw new Exception("Connecting to db failed");
                    MySqlBackup bk = new MySqlBackup(cmd);
                    bk.ImportFromString(create_db_query);
                    return true;
                }
            }

            catch (Exception ex)
            {
                dropDB();
                ErrorHandler.Errors.displayError("We were unable to install database.", ErrorHandler.ErrorCode.ConfirmingDBExists, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }

        }
        /// <summary>
        /// Checks whether the credentials for connecting to database are correct. That is..The database host name, username and password
        /// </summary>
        /// <returns></returns>
        public static bool ConfirmExistingCredentials()
        {
            return ConfirmCredentials(MySQLHandler.MySQLHelper.db_hostname, MySQLHandler.MySQLHelper.db_username, MySQLHandler.MySQLHelper.db_password);
        }
        public static bool ConfirmCredentials(string hostname, string user_name, string password)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.ConnectionString = "SERVER=" + hostname + ";" + "UID=" + user_name + ";" + "PASSWORD=" + password + ";Allow User Variables=" + allow_user_variables + ";";
                conn.Open();

                MySQLHandler.MySQLHelper.db_hostname = hostname;
                MySQLHandler.MySQLHelper.db_username = user_name;
                MySQLHandler.MySQLHelper.db_password = password;

                /// <summary>
                ///  code to save the credentials down here
                /// </summary>
                SDLibrary.Properties.Settings.Default.db_host = hostname;
                SDLibrary.Properties.Settings.Default.db_username = user_name;
                SDLibrary.Properties.Settings.Default.db_password = password;
                SDLibrary.Properties.Settings.Default.Save();
                return true;
            }
            catch (MySqlException ex)
            {
                ErrorHandler.Errors.displayError("The database credentials are incorrect.Click ok to enter them.", ErrorHandler.ErrorCode.ConfirmingDBCredentials, ErrorHandler.ErrorAction.Continue, ex);
                DBCredentialsForm credentials_form = null;  credentials_form = new DBCredentialsForm();
                credentials_form.ShowDialog();
                return false;
            }
           
        }
        private static bool ConfirmMySqlProcessIsRunning()
        {
            try
            {
                System.Diagnostics.Process[] server = System.Diagnostics.Process.GetProcessesByName("mysqld");
                if (server.Length != 0)
                {
                    return true;
                }
                else
                {
                    ErrorHandler.Errors.displayError("We have detected that mysql service is not available.Please start it if you have it installed otherwise download and install it from"
                    + " this link : https://dev.mysql.com/get/Downloads/MySQL-5.6/mysql-5.6.25-win32.zip", ErrorHandler.ErrorCode.ConfirmMySqlProcessIsRunning, ErrorHandler.ErrorAction.Exit, new Exception());
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("An error occured checking mysql process.", ErrorHandler.ErrorCode.ConfirmMySqlProcessIsRunning, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
        }
        public static bool connect()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.ConnectionString = "SERVER=" + db_hostname + ";" + "DATABASE=" + db_name + ";" + "UID=" + db_username + ";" + "PASSWORD=" + db_password + ";Allow User Variables=" + allow_user_variables + ";";
                conn.Open();
                cmd.Connection = conn;
                if (data_reader != null && !data_reader.IsClosed)
                {
                    data_reader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("An error occured connecting to the database.", ErrorHandler.ErrorCode.ConnectingToDB, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
        }
        private static bool connectWithoutDbName()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.ConnectionString = "SERVER=" + db_hostname + ";"+ "UID=" + db_username + ";" + "PASSWORD=" + db_password + ";Allow User Variables=" + allow_user_variables + ";";
                conn.Open();
                cmd.Connection = conn;
                if (data_reader != null && !data_reader.IsClosed)
                {
                    data_reader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("An error occured connecting to the database.", ErrorHandler.ErrorCode.ConnectingToDB, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
        }
        public static bool RestoreDb(string file)
        {
            try
            {
                if (!connect()) return false;
                if (!File.Exists(file)) return false;
                MySqlBackup mb = new MySqlBackup(cmd);
                mb.ImportFromFile(file);
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("We were unable to restore the database.", ErrorHandler.ErrorCode.RestoreDb, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
        }
        public static bool BackUp(string dir, int max_backups)
        {
            try
            {
                if (!connect()) return false;
                string file = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString().Replace(':', '.');
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(dir);
                    FileInfo[] back_up_files = directory.GetFiles("*.sql").OrderByDescending(f => f.LastWriteTime).ToArray();
                    if (back_up_files.Length >= max_backups)
                    {
                        back_up_files.Last().Delete();
                    }
                }
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    mb.ExportInfo.AddCreateDatabase = true;
                    mb.ExportInfo.ExportTableStructure = true;
                    mb.ExportInfo.ExportRows = true;
                    mb.ExportToFile(dir + @"\" + file + ".sql");
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("We were unable to back up the database.", ErrorHandler.ErrorCode.BackUpDB, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
        }
        public static bool BackUp(string file_name)
        {
            try
            {
                if (!connect()) return false;
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    mb.ExportInfo.AddCreateDatabase = true;
                    mb.ExportInfo.ExportTableStructure = true;
                    mb.ExportInfo.ExportRows = true;
                    mb.ExportToFile(file_name);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("We were unable to back up the database.", ErrorHandler.ErrorCode.BackUpDB, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
        }
        public static bool executeRead(string query)
        {
            try
            {
                if (!connect()) return false;
                cmd.CommandText = query;
                data_reader = cmd.ExecuteReader();
                if (data_reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error reading from the database.", ErrorHandler.ErrorCode.ReadingFromDB, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
        }
        public static bool executeWrite(string query)
        {
            try
            {
                if (!connect()) return false;
                cmd.CommandText = query;
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    return true;
                }
                else return false;
            }
            catch (MySqlException ex)
            {
                ErrorHandler.Errors.displayError("Error writing into the database.", ErrorHandler.ErrorCode.WritingToDB, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool executeExists(string query)
        {
            try
            {
                if (!connect()) return false;
                cmd.CommandText = query;
                data_reader = cmd.ExecuteReader();
                if (data_reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error reading existance from the database.", ErrorHandler.ErrorCode.ExecuteExists, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static Object getOneValue(string query)
        {
            try
            {
                if (!connect()) return false;
                cmd.CommandText = query;
                data_reader = cmd.ExecuteReader();
                data_reader.Read();
                Object obj = data_reader.GetValue(0);
                data_reader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error reading from the database.", ErrorHandler.ErrorCode.ReadingFromDB, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static Object getOneValue(string query,string column_name)
        {
            try
            {
                if (!connect()) return false;
                cmd.CommandText = query;
                data_reader = cmd.ExecuteReader();
                data_reader.Read();
                Object obj = data_reader[column_name];
                data_reader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error reading from the database.", ErrorHandler.ErrorCode.ReadingFromDB, ErrorHandler.ErrorAction.Continue, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static int insertGetLastId(string query)
        {
            try
            {
                if (!connect()) return 0;
                cmd.CommandText = query;
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    return (int)cmd.LastInsertedId;
                }
                return 0;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error writing into the database.", ErrorHandler.ErrorCode.WritingToDB, ErrorHandler.ErrorAction.Continue, ex);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        private static bool dropDB()
        {
            try
            {
                if (!connectWithoutDbName()) return false;
                cmd.CommandText = "DROP DATABASE `" + db_name + "`;";
                int result = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.Errors.displayError("Error creating database.", ErrorHandler.ErrorCode.DropDB, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        private static bool createDB(string query)
        {
            try
            {
                if (!connectWithoutDbName()) return false;
                cmd.CommandText = query;
                int result = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                dropDB();
                ErrorHandler.Errors.displayError("Error creating database.", ErrorHandler.ErrorCode.CreateDB, ErrorHandler.ErrorAction.Exit, ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool DBExists()
        {
            try
            {
                if (!connectWithoutDbName()) return false;
                cmd.CommandText = "Show Databases like '" + db_name + "'";
                data_reader = cmd.ExecuteReader();
                if (data_reader.HasRows)
                    return true;
                return false;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }


}