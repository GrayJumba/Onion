using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class ClassTermTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ClassTermTrend Default { get { return _default; } }
        private static ClassTermTrend _default = new ClassTermTrend();
        public static ClassTermTrend Results { get { return _results; } }
        private static ClassTermTrend _results = new ClassTermTrend();
        public ClassTermTrend()
            : base(
              @"SELECT CONCAT(`term`.`year`,' Term ',`term`.`number`) AS `term`,`class_term_aggregates`.`average` FROM 
                        (
	                        SELECT * FROM `term` ORDER BY `year`,`number`
                        ) AS `term`
                        JOIN
                        (
	                        SELECT * FROM `class_term_aggregates` WHERE `class_of`=@req_class_of
                        ) AS `class_term_aggregates` 
                        ON `term`.`auto_id`=`class_term_aggregates`.`term_auto_id`"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_class_of", null))
        {
        }
        public void refreshDt(int req_class_of)
        {
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            base.refreshDt();
        }
    }

    class ClassExamTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ClassExamTrend Default { get { return _default; } }
        private static ClassExamTrend _default = new ClassExamTrend();
        public static ClassExamTrend Results { get { return _results; } }
        private static ClassExamTrend _results = new ClassExamTrend();
        public ClassExamTrend()
            : base(
              @"SELECT CONCAT(`exam`.`name` ,' ',`term_year`,' Term ',`term_number`) AS `exam`,`class_exam_aggregates`.`average` FROM 
                        (
	                        SELECT `exam`.`auto_id`,`exam`.`name`,`term`.`year` AS `term_year`,`term`.`number` AS `term_number` FROM `exam` JOIN `term` ON `exam`.`term_auto_id`=`term`.`auto_id` ORDER BY `start_date`
                        ) AS `exam`
                        JOIN
                        (
	                        SELECT * FROM `class_exam_aggregates` WHERE `class_of`=@req_class_of
                        ) AS `class_exam_aggregates` 
                        ON `exam`.`auto_id`=`class_exam_aggregates`.`exam_auto_id`"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_class_of", null))
        {
        }
        public void refreshDt(int req_class_of)
        {
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            base.refreshDt();
        }
    }
    class FormTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static FormTrend Form1 { get { return form1; } }
        private static FormTrend form1 = new FormTrend(1,DateTime.Today.Year);
        public static FormTrend Form2 { get { return form2; } }
        private static FormTrend form2 = new FormTrend(2, DateTime.Today.Year);
        public static FormTrend Form3 { get { return form3; } }
        private static FormTrend form3 = new FormTrend(3, DateTime.Today.Year);
        public static FormTrend Form4 { get { return form4; } }
        private static FormTrend form4 = new FormTrend(4, DateTime.Today.Year);
        public FormTrend(int form,int cur_year)
            : base(
              @"SELECT CONCAT(`term`.`year`,' Term ',`term`.`number`) AS `term`,`class_term_aggregates`.`average` FROM 
                        (
	                        SELECT * FROM `term` ORDER BY `year`,`number`
                        ) AS `term`
                        JOIN
                        (
	                        SELECT * FROM `class_term_aggregates` WHERE `class_of`=@cur_year+(4-@req_class)
                        ) AS `class_term_aggregates` 
                        ON `term`.`auto_id`=`class_term_aggregates`.`term_auto_id`"
            , ""
            , ""
            , ""
            ,new MySqlParameter("@cur_year", cur_year)
            , new MySqlParameter("@req_class", form))
        {
        }
        public void refreshDt(int cur_year)
        {
            dtAdapter.SelectCommand.Parameters["@cur_year"].Value = cur_year;
            base.refreshDt();
        }
    }
}
