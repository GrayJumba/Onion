using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Onion.MySQLHandler;
namespace FeesDesk.MySQLHandler
{
   
    class Receipt:MySQLHandlerTemplate
    {
        public static Receipt Default = new Receipt();
        public Receipt():base(
            "SELECT `receipt`.`auto_id`,`receipt`.`receipt_no`,`receipt`.`date`,`receipt`.`received_from`,`receipt`.`payment_method`,`receipt`.`bank`,`receipt`.`ref_no`,`receipt`.`amount`,"
                                        + "`receipt`.`processed`,`receipt`.`student_auto_id` FROM `receipt` "
                                        + " WHERE `receipt`.`receipt_no`=IFNULL(@req_receipt_no,`receipt`.`receipt_no`) "
                                        + " AND`receipt`.`payment_method`=IFNULL(@req_payment_method,`receipt`.`payment_method`) "
                                        + " AND `receipt`.`date` BETWEEN IFNULL(@start_date,`receipt`.`date`) AND IFNULL(@end_date,`receipt`.`date`) ;"
             ,"INSERT INTO `receipt` (`receipt_no`,`date`,`received_from`,`payment_method`,`bank`,`ref_no`,`amount`,`processed`,`student_auto_id`)"
                                        + "VALUES((SELECT CONCAT(`prefix`,`next_receipt_number`,`suffix`) FROM `receipt_number_gen`),@date,@received_from,@payment_method,@bank,@ref_no,@amount,@processed,@student_auto_id);"
                                        + "UPDATE `receipt_number_gen` SET `next_receipt_number`=`next_receipt_number`+1;"
            ,"UPDATE `receipt` SET `receipt_no` = @receipt_no,`date` = @date,`received_from` = @received_from,`payment_method` = @payment_method,"
                                        + "`bank` = @bank,`ref_no` = @ref_no,`amount` =@amount,`processed` = @processed,`student_auto_id` = @student_auto_id WHERE `auto_id` = @auto_id;"
            ,"DELETE FROM `receipt` WHERE `auto_id` = @auto_id;"
            ,new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@receipt_no", MySqlDbType.VarChar, 45, "receipt_no")
            , new MySqlParameter("@date", MySqlDbType.Date, 45, "date")
            , new MySqlParameter("@received_from", MySqlDbType.VarChar, 45, "received_from")
            , new MySqlParameter("@payment_method", MySqlDbType.VarChar, 45, "payment_method")
            , new MySqlParameter("@bank", MySqlDbType.VarChar, 45, "bank")
            , new MySqlParameter("@ref_no", MySqlDbType.VarChar, 45, "ref_no")
            , new MySqlParameter("@amount", MySqlDbType.Double, 11, "amount")
            , new MySqlParameter("@processed", MySqlDbType.Int32, 1, "processed")
            , new MySqlParameter("@student_auto_id", MySqlDbType.Int32, 11, "student_auto_id")
            , new MySqlParameter("@req_receipt_no",null)
             , new MySqlParameter("@req_payment_method", null)
            , new MySqlParameter("@start_date",null)
             , new MySqlParameter("@end_date",DateTime.Today))
        {
            this.dt.Columns["date"].DefaultValue = DateTime.Today;

        }
        public void refreshDt(string req_receipt_no, string req_payment_method, string start_date, string end_date)
        {
            dtAdapter.SelectCommand.Parameters["@req_receipt_no"].Value = req_receipt_no;
            dtAdapter.SelectCommand.Parameters["@req_payment_method"].Value = req_payment_method;
            dtAdapter.SelectCommand.Parameters["@start_date"].Value = start_date;
            dtAdapter.SelectCommand.Parameters["@end_date"].Value = end_date;
            base.refreshDt();
        }
        public override bool saveChanges()
        {
            bool value = base.saveChanges();
            MySQLHandler.AllObjectIDs.ReceiptUFIHandler.refresh();
            return value;
        }
    }
    class ReceiptNumberGenerator
    {
        public static int next_rct_no;
        public static string prefix;
        public static string suffix;
        public static void refresh()
        {
            string query = "SELECT * FROM `receipt_number_gen`;";
            if (Onion.MySQLHandler.MySQLHelper.executeRead(query))
            {
                if (Onion.MySQLHandler.MySQLHelper.data_reader.Read())
                {
                    next_rct_no = Onion.MySQLHandler.MySQLHelper.data_reader.GetInt32("next_receipt_number");
                    prefix = Onion.MySQLHandler.MySQLHelper.data_reader.GetString("prefix");
                    suffix = Onion.MySQLHandler.MySQLHelper.data_reader.GetString("suffix");

                }
            }
        }
        public static bool saveChanges()
        {
            string query = "UPDATE  `receipt_number_gen` SET `next_receipt_number`='" + next_rct_no + "' ,`prefix`='" + prefix + "' ,`suffix`='" + suffix + "';";
            return Onion.MySQLHandler.MySQLHelper.executeWrite(query);
        }
    }
}
