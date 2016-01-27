using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class Exam : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static Exam Default { get { return _default; } }
        private static Exam _default = new Exam();
        public Exam()
            : base(
              @"SELECT `exam`.`auto_id`,`exam`.`name`,`exam`.`term_auto_id`,term_dfi_ufi(1,`exam`.`term_auto_id`) AS `term_ufi`,`exam`.`start_date`,`exam`.`out_of`,GROUP_CONCAT(`exam_class`.`featured_class`) AS featured_classes
                        FROM `exam` LEFT JOIN `exam_class` ON `exam`.`auto_id`=`exam_class`.`exam_auto_id`
                         WHERE (IF(@req_exam_auto_id=0 OR @req_exam_auto_id IS NULL,1,0) OR `exam`.`auto_id`=@req_exam_auto_id) AND
                         (IF(@req_term_auto_id=0 OR @req_term_auto_id IS NULL,1,0) OR `exam`.`term_auto_id`=@req_term_auto_id)
                         GROUP BY `exam`.`auto_id`;"
            , @"INSERT INTO `exam`(`auto_id`,`name`,`term_auto_id`,`start_date`,`out_of`)
                    VALUES(@auto_id,@name,term_dfi_ufi(0,@term_ufi),@start_date,@out_of); CALL `insertExamClasses`(LAST_INSERT_ID(), @featured_classes);"
            , @"UPDATE `exam` SET `auto_id` = @auto_id,`name` = @name,`term_auto_id` = term_dfi_ufi(0,@term_ufi),`start_date`=@start_date,`out_of` = @out_of
                    WHERE `auto_id` = @auto_id;CALL `insertExamClasses`(@auto_id, @featured_classes);"
            , "DELETE FROM `exam` WHERE `auto_id` = @auto_id;"
            , new string[] { "auto_id", "name", "term_auto_id","term_ufi","start_date", "out_of", "featured_classes" }
            , new MySqlParameter("@req_exam_auto_id", 0)
            , new MySqlParameter("@req_term_auto_id", 0))
        {
        }
        public void refreshDt(int req_exam_auto_id, int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            base.refreshDt();
        }
        
    }
    class Formulae : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static Formulae Default { get { return _default; } }
        private static Formulae _default = new Formulae();
        public Formulae()
            : base(
              @"SELECT `exam_auto_id` , `name` as exam_name ,`account_for` FROM `exam_view` WHERE `term_auto_id`=@req_term_auto_id AND `featured_class`=@req_class;"
            , @""
            , @" UPDATE `exam_class` SET `account_for`=@account_for WHERE `exam_auto_id`=@exam_auto_id AND `featured_class`=@req_class;"
            , ""
            , new string[] { "exam_auto_id","account_for" }
            , new MySqlParameter("@req_class", 0)
            , new MySqlParameter("@req_term_auto_id", 0))
        {
        }
        public void refreshDt(int req_class, int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class"].Value = req_class;
            base.refreshDt();
        }

    }

}
