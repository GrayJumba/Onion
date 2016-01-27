using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class SchoolTermTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static SchoolTermTrend Default { get { return _default; } }
        private static SchoolTermTrend _default = new SchoolTermTrend();

        public static SchoolTermTrend Result { get { return _result; } }
        private static SchoolTermTrend _result = new SchoolTermTrend();
        public SchoolTermTrend()
            : base(
              @"SELECT CONCAT(`term`.`year`,' Term ',`term`.`number`) AS `term`,`school_term_aggregates`.`average` FROM 
                        (
	                        SELECT * FROM `term` ORDER BY `year`,`number`
                        ) AS `term`
                        JOIN
                        (
	                        SELECT * FROM `school_term_aggregates`
                        ) AS `school_term_aggregates`
                        ON `term`.`auto_id`=`school_term_aggregates`.`term_auto_id`"
            , ""
            , ""
            , "")
        {
        }
    }

    class SchoolExamTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static SchoolExamTrend Default { get { return _default; } }
        private static SchoolExamTrend _default = new SchoolExamTrend();

        public static SchoolExamTrend Result { get { return _result; } }
        private static SchoolExamTrend _result = new SchoolExamTrend();
        public SchoolExamTrend()
            : base(
              @"SELECT CONCAT(`exam`.`name` ,' ',`term_year`,' Term ',`term_number`) AS `exam`,`school_exam_aggregates`.`average` FROM 
                        (
	                        SELECT `exam`.`auto_id`,`exam`.`name`,`term`.`year` AS `term_year`,`term`.`number` AS `term_number` FROM `exam` JOIN `term` ON `exam`.`term_auto_id`=`term`.`auto_id` ORDER BY `start_date`
                        ) AS `exam`
                        JOIN
                        (
	                        SELECT * FROM `school_exam_aggregates`
                        ) AS `school_exam_aggregates` 
                        ON `exam`.`auto_id`=`school_exam_aggregates`.`exam_auto_id`"
            , ""
            , ""
            , "")
        {
        }
    }
}
