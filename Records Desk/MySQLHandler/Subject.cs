using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RecordsDesk.MySQLHandler
{
    class Subject: Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static Subject Default = new Subject();
        public Subject()
            : base(
           "SELECT `subject`.`code`, `subject`.`name`,`subject`.`abbreviation`, `subject`.`group`, `subject`.`is_done_by_school`,"
                    +" `subject`.`is_junior_compulsory`, `subject`.`is_senior_compulsory` "
                    +" FROM `subject` WHERE (IF(@req_done_by_school_only=0,1,0) OR `subject`.`is_done_by_school`=1);"
           , ""
           , "UPDATE `subject` SET`name` = name,`abbreviation` = @abbreviation,`group` = @group,`is_done_by_school` = @is_done_by_school,"
                    +"`is_junior_compulsory` = @is_junior_compulsory,`is_senior_compulsory` = @is_senior_compulsory WHERE `code` = @code;"
           , ""
           , new String[] { "code", "name", "abbreviation", "group", "is_done_by_school", "is_junior_compulsory", "is_senior_compulsory" }, new MySqlParameter("@req_done_by_school_only", 1))
        {
        }
        public void refreshDt(bool req_done_by_school_only)
        {
            dtAdapter.SelectCommand.Parameters["@req_done_by_school_only"].Value = req_done_by_school_only;
            base.refreshDt();
        }
    }
}
