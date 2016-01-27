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
    class PaymentMethod: MySQLHandlerTemplate
    {
        public static PaymentMethod Default = new PaymentMethod();
        public PaymentMethod()
            : base(
            "SELECT `payment_method`.`auto_id`,`payment_method`.`name`,`payment_method`.`allowed` FROM `payment_method`"
            , "INSERT INTO `payment_method` (`name`,`allowed`) VALUES(@name, @allowed);"
            , "UPDATE `payment_method` SET `name` = @name,`allowed` = @allowed  WHERE `auto_id` = @auto_id;"
            ,""
            ,new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@name" ,MySqlDbType.VarChar, 45, "name")
             , new MySqlParameter("@allowed", MySqlDbType.Int16, 11, "allowed"))
        {
           dt.Columns["allowed"].DefaultValue=1;
        }
    }
    
}
