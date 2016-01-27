using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace FeesDesk.MySQLHandler
{
     class Statement : MySQLHandlerTemplate
    {
        public static Statement Default { get { return _default; } }
        private static Statement _default = new Statement();
        public Statement()
            : base(
              "SELECT `fee_transaction`.`student_auto_id`,`fee_transaction`.`date`,`fee_transaction`.`trans_type`,`fee_transaction`.`trans_number`"
                        +",`fee_transaction`.`description`,`fee_transaction`.`amount` FROM `fee_transaction` "
                        +" WHERE `fee_transaction`.`student_auto_id`=@student_auto_id AND `fee_transaction`.`date` <= @as_at ORDER BY `fee_transaction`.`date` ASC;"
            , ""
            , ""
            , ""
            , new MySqlParameter("@student_auto_id", 1)
            , new MySqlParameter("@as_at", DateTime.Today))
        {

        }
        public void refreshDt(UInt64 student_auto_id,DateTime as_at)
        {
            dtAdapter.SelectCommand.Parameters["@student_auto_id"].Value = student_auto_id;
            dtAdapter.SelectCommand.Parameters["@as_at"].Value = as_at;
            base.refreshDt();
        }
    }
}
