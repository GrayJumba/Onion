using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace FeesDesk.MySQLHandler
{
    class Invoice:MySQLHandlerTemplate
    {
        public static Invoice Default { get { return _default; } }
        private static Invoice _default = new Invoice();
        public Invoice()
            : base(
              "SELECT `invoice`.`auto_id`,`invoice`.`student_auto_id`,`invoice`.`description`,`invoice`.`date`,`invoice`.`amount` FROM `invoice`;"
            , "INSERT INTO `invoice`(`auto_id`,`student_auto_id`,`description`,`date`,`amount`)"
                                    +"VALUES(@auto_id,@student_auto_id,@description,@date,@amount);"
            , "UPDATE `invoice` SET `auto_id` = @auto_id,`student_auto_id` = @student_auto_id,`description` = @description "
                                    +" ,`date` = @date,`amount` = @amount WHERE `auto_id` = @auto_id;"
            , "DELETE FROM `invoice` WHERE `auto_id` = @auto_id;"
            ,new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@student_auto_id", MySqlDbType.UInt64, 11, "student_auto_id")
            , new MySqlParameter("@description", MySqlDbType.VarChar, 45, "description")
            , new MySqlParameter("@date", MySqlDbType.Date, 11, "date")
            , new MySqlParameter("@amount", MySqlDbType.Double, 11, "amount"))
        {
            dt.Columns["date"].DefaultValue = DateTime.Today;
        }
    }
}
