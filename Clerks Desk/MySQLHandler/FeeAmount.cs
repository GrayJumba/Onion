using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace FeesDesk.MySQLHandler
{
    class FeeAmount : MySQLHandlerTemplate
    {
      
        public static FeeAmount Form1 = new FeeAmount(DateTime.Today.Year,1);
        public static FeeAmount Form2 = new FeeAmount(DateTime.Today.Year, 2);
        public static FeeAmount Form3 = new FeeAmount(DateTime.Today.Year, 3);
        public static FeeAmount Form4 = new FeeAmount(DateTime.Today.Year, 4);
        private int form;
        public FeeAmount(int year,int form):base(
            "SELECT `rel_fee_amount`.`auto_id`, `All_Terms`.`auto_id` AS `term_auto_id`,`All_Terms`.`number` AS `term`,`rel_fee_amount`.`amount` FROM (SELECT `term`.`auto_id`,`term`.`year`,`term`.`number` FROM `term`  WHERE `term`.`year`= @year) AS `All_Terms` "
                                        + " LEFT JOIN (SELECT *  FROM  `fee_amount` WHERE `fee_amount`.`form`= @form)  AS `rel_fee_amount`  ON `All_Terms`.`auto_id`= `rel_fee_amount`.`term_auto_id`  ;"
            ,""
            ,"INSERT INTO `fee_amount` (`term_auto_id`,`form`,`amount`) VALUES (@term_auto_id,@form,@amount) ON DUPLICATE KEY UPDATE `amount`=@amount;"
            ,""

            ,new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@term_auto_id", MySqlDbType.UInt64, 11, "term_auto_id")
            , new MySqlParameter("@term", MySqlDbType.Int32, 1, "term")
            , new MySqlParameter("@amount", MySqlDbType.Double, 10, "amount")
            , new MySqlParameter("@form",form)
            , new MySqlParameter("@year", year))
        {
            this.form = form;
        }
        public void refreshDt(int year)
        {
            dtAdapter.SelectCommand.Parameters["@year"].Value = year;
            base.refreshDt();
        }
    }
}
