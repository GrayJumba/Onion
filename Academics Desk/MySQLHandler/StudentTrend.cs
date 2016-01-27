using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class StudentTermTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StudentTermTrend Default { get { return _default; } }
        private static StudentTermTrend _default = new StudentTermTrend();
        public static StudentTermTrend ReportCard { get { return _reportcard; } }
        private static StudentTermTrend _reportcard = new StudentTermTrend();
        public StudentTermTrend()
            : base(
              @"SELECT CONCAT(`term`.`year`,' Term ',`term`.`number`) AS `term`,`student_term_aggregates`.`average` FROM 
                        (
	                        SELECT * FROM `term` ORDER BY `year`,`number`
                        ) AS `term`
                        JOIN
                        (
	                        SELECT * FROM `student_term_aggregates` WHERE `student_auto_id`=@req_student_auto_id
                        ) AS `student_term_aggregates` 
                        ON `term`.`auto_id`=`student_term_aggregates`.`term_auto_id`"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_student_auto_id", null))
        {
        }
        public void refreshDt(int req_student_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            base.refreshDt();
        }
    }

    class StudentExamTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StudentExamTrend Default { get { return _default; } }
        private static StudentExamTrend _default = new StudentExamTrend();
        public static StudentExamTrend ResultSlip { get { return _resultSlip; } }
        private static StudentExamTrend _resultSlip = new StudentExamTrend();
        public StudentExamTrend()
            : base(
              @"SELECT CONCAT(`exam`.`name` ,' ',`term_year`,' Term ',`term_number`) AS `exam`,`student_exam_aggregates`.`average` FROM 
                        (
	                        SELECT `exam`.`auto_id`,`exam`.`name`,`term`.`year` AS `term_year`,`term`.`number` AS `term_number` FROM `exam` JOIN `term` ON `exam`.`term_auto_id`=`term`.`auto_id` ORDER BY `start_date`
                        ) AS `exam`
                        JOIN
                        (
	                        SELECT * FROM `student_exam_aggregates` WHERE `student_auto_id`=@req_student_auto_id
                        ) AS `student_exam_aggregates` 
                        ON `exam`.`auto_id`=`student_exam_aggregates`.`exam_auto_id`"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_student_auto_id", null))
        {
        }
        public void refreshDt(int req_student_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            base.refreshDt();
        }
    }
}
