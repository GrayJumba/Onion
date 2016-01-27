using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace FeesDesk.MySQLHandler
{
    class Credit : MySQLHandlerTemplate
    {
        private static Credit _default = new Credit();
        public static Credit Default { get { return _default; } }

        public Credit()
            : base(
              "SELECT `fee_credit`.`auto_id`,`fee_credit`.`student_auto_id`,`fee_credit`.`description`,`fee_credit`.`date`,`fee_credit`.`amount` FROM `fee_credit`;"
            , "INSERT INTO `fee_credit`(`auto_id`,`student_auto_id`,`description`,`date`,`amount`)"
                                    + "VALUES(@auto_id,@student_auto_id,@description,@date,@amount);"
            , "UPDATE `fee_credit` SET `auto_id` = @auto_id,`student_auto_id` = @student_auto_id,`description` = @description "
                                    + " ,`date` = @date,`amount` = @amount WHERE `auto_id` = @auto_id;"
            , "DELETE FROM `fee_credit` WHERE `auto_id` = @auto_id;"
            , new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@student_auto_id", MySqlDbType.UInt64, 11, "student_auto_id")
            , new MySqlParameter("@description", MySqlDbType.VarChar, 45, "description")
            , new MySqlParameter("@date", MySqlDbType.Date, 11, "date")
            , new MySqlParameter("@amount", MySqlDbType.Double, 11, "amount"))
        {
            dt.Columns["date"].DefaultValue = DateTime.Today;
        }
    }
}
