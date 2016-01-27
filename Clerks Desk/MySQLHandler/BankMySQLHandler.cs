using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace FeesDesk.MySQLHandler
{
    class Bank : MySQLHandlerTemplate
    {
        public static Bank Default = new Bank();
        public Bank():base(
            "SELECT `bank`.`auto_id`,`bank`.`name`,`bank`.`visible` FROM `bank`"
            , "INSERT INTO `bank` (`name`,`visible`) VALUES(@name, @visible);"
            ,"UPDATE `bank` SET `name` = @name,`visible` = @visible  WHERE `auto_id` = @auto_id;"
            ,""
            ,new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@name" ,MySqlDbType.VarChar, 45, "name")
             ,new MySqlParameter("@visible", MySqlDbType.Int16, 11, "visible"))
        {
           dt.Columns["visible"].DefaultValue=1;
        }
    }
  
}
