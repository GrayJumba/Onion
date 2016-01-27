using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    public class ResultSlip : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ResultSlip Default { get { return _default; } }
        private static ResultSlip _default = new ResultSlip();
        public ResultSlip()
            : base(
             @"CALL `get_exam_marks_template`(@req_student_auto_id, null,null,@req_exam_auto_id,null);
                        SELECT subject.abbreviation AS subject_abbrev ,`percentage`,`grade_name`,`comment`,`stream_position`,`class_position` FROM 
                        `exam_marks_template`
                         LEFT JOIN  
                         (SELECT `subject_code`,`mark` ,`percentage`,`grade_name`,`comment`,`stream_position`,`class_position`
                         FROM `marks_view` WHERE `marks_view`.`student_auto_id`=@req_student_auto_id AND `marks_view`.`exam_auto_id`=@req_exam_auto_id) AS subject_marks 
                         ON `exam_marks_template`.`subject_code`=`subject_marks`.`subject_code` 
                         JOIN `subject` ON `subject`.`code` =`exam_marks_template`.`subject_code` ORDER BY `exam_marks_template`.`subject_code`;"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_student_auto_id", null)
              , new MySqlParameter("@req_exam_auto_id", null))
        {
        }
        public void refreshDt(int student_auto_id, int exam_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = exam_auto_id;
            base.refreshDt();
        }
    }
    public class ResultSlipAggr : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ResultSlipAggr Default { get { return _default; } }
        private static ResultSlipAggr _default = new ResultSlipAggr();
        public ResultSlipAggr()
            : base(
              @"CALL `get_exam_marks_template`(@req_student_auto_id, null,null,@req_exam_auto_id,null);
                    SELECT `average`,`average_grade`,`comment`,`agg_cluster_grade`,`improvement`,`kcpe_improvement_index`,`stream_position`,`class_position`  FROM 
                    (SELECT DISTINCT(`auto_id`) FROM `exam_marks_template`)AS `exam_marks_template`
                    LEFT JOIN  
                    (SELECT * FROM `student_exam_aggregates` WHERE `student_auto_id`=@req_student_auto_id AND `exam_auto_id`= @req_exam_auto_id) AS `marks_aggr` 
                    ON `exam_marks_template`.`auto_id`=`marks_aggr`.`student_auto_id` ;"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_student_auto_id", null)
              , new MySqlParameter("@req_exam_auto_id", null))
        {
        }
        public void refreshDt(int student_auto_id, int exam_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = exam_auto_id;
            base.refreshDt();
        }
    }
}