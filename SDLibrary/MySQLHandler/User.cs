using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Onion.MySQLHandler;


namespace SmartDesk.MySQLHandler
{

    public class UserHandler : MySQLHandlerTemplate
    {
        public static UserHandler Default { get { return _default; } }
        private static UserHandler _default = new UserHandler();
        public UserHandler()
            : base(
             "SELECT `user`.`auto_id`, `user`.`name`,`user`.`password`,`user`.`type` FROM `user`"
            , "INSERT INTO `user`(`name`,`password`,`type`)VALUES(@name,@password,@type);"
            , "UPDATE `user` SET `name` = @name,`password` = @password,`type`=@type WHERE `auto_id` = @auto_id;"
            , "DELETE FROM `user`  WHERE `auto_id` = @auto_id;"
            , new MySqlParameter("@student_auto_id", null)
            , new MySqlParameter("@class_stream_auto_id", null)
            , new MySqlParameter("@class_of", null)
            , new MySqlParameter("@as_at", DateTime.Today))
        {
        }
    }
    public class User{

        //write them in order of their seniority- descending order
        public static string[] user_types = { "administrator", "academics", "records", "fees", "teacher", "principal", "library" };
        
        /// <summary>
        /// Singular
        /// </summary>
        int auto_id;
        public string name;
        string password;
        public string type;
        public bool isLoggedIn = false;
       
        public  bool logIn(string name,string password)
        {
            string query = "SELECT `user`.`auto_id`, `user`.`name`,`user`.`password`,`user`.`type` FROM `user` WHERE `name`='" + name + "' AND `password`='" + password + "';";
            if(MySQLHelper.executeRead(query))
            {
                if(MySQLHelper.data_reader.Read())
                {
                    this.type = MySQLHelper.data_reader.GetString("type");
                    if (!user_types.ToList().Exists(x => x == this.type))
                        return false;
                    this.auto_id = MySQLHelper.data_reader.GetInt32("auto_id");
                    this.name = MySQLHelper.data_reader.GetString("name");

                    this.password = MySQLHelper.data_reader.GetString("password");

                    this.isLoggedIn = true;
                   

                    return true;
                }
            }
            return false;
        }

        public void logOut()
        {
            name = "";
            password = "";
            type = "";
            isLoggedIn = false;
        }
        public bool addUser(string name, string password,string type)
        {          
            string query = "INSERT INTO `user`(`name`,`password`,`type`)VALUES('" + name + "','" + password + "','" + type + "');";
            return MySQLHelper.executeWrite(query);
        }
        public bool updateUserPassword(string old_password, string new_password)
        {
            string query = "IF EXISTS(SELECT * FROM `user` WHERE `name`='" + name + "' AND `password`='" + old_password + "') UPDATE`user` SET `password`='" + password + "';";
            if (MySQLHelper.executeWrite(query))
            {
                this.password = new_password;
                return true;
            }
            return false;
        }
        public bool updateUserDetails(string name, string password, string type)
        {
            string query = "UPDATE`user` SET `name`='" + name + "',`password`='" + password + "',`type`='" + type + "' WHERE auto_id='"+this.auto_id+"';";
            if(MySQLHelper.executeWrite(query))
            {
                this.name = name;
                this.password = password;
                this.type = type;
                return true;
            }
            return false;
        }

        public bool checkType(string type)
        {
            if (isLoggedIn)
            {
                if (this.type == type)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("You are not allowed to do this operation. Please login as " + type + " user to proceed.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("You are not logged in. Please login to proceed.");
                return false;
            }
        }
        public bool checkRights(string type)
        {
            if (isLoggedIn)
            {
                if (!user_types.ToList().Exists(x => x == this.type))
                {
                    MessageBox.Show("You are not recognized as a valid user. Please login as " + type + " user to proceed.");
                    return false;
                }
                if (user_types.ToList().IndexOf(type) >= user_types.ToList().IndexOf(this.type))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("You do not have permission to do this operation. Please login as " + type + " user to proceed.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("You are not logged in. Please login to proceed.");
                return false;
            }
        }

       
    }
}
