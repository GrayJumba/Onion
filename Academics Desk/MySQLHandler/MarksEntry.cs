using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class MarksEntry : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        private int req_student_auto_id, req_stream_auto_id, req_class_of, req_exam_auto_id, req_subject_code;
        public static MarksEntry Default { get { return _default; } }
        private static MarksEntry _default = new MarksEntry();
        public MarksEntry()
            : base(
             @" CALL `get_exam_marks_template`(@req_student_auto_id, @req_stream_auto_id,@req_class_of,@req_exam_auto_id,null);
                    SELECT `exam_marks_template`.`auto_id` AS `student_auto_id`, `exam_marks_template`.`admno`,`exam_marks_template`.`name`, `exam_mark`.`mark` FROM
                    (SELECT * FROM `exam_marks_template` WHERE `subject_code`=@req_subject_code) AS `exam_marks_template`
                    LEFT JOIN (SELECT `student_auto_id` as `student_auto_id` ,`mark` FROM `exam_mark` WHERE `exam_auto_id`=@req_exam_auto_id AND `subject_code`=@req_subject_code) AS `exam_mark`
                    ON `exam_marks_template`.`auto_id`=`exam_mark`.`student_auto_id`;"
            , ""
            , @"INSERT INTO `exam_mark` (`student_auto_id`,`exam_auto_id`,`subject_code`,`mark`) 
                  VALUES (@student_auto_id,@req_exam_auto_id,@req_subject_code,@mark) ON duplicate KEY UPDATE `mark`=@mark;
                 "
            , ""
            , new MySqlParameter("@student_auto_id", MySqlDbType.Int32, 11, "student_auto_id")
            , new MySqlParameter("@mark", MySqlDbType.Int32, 11, "mark")
            , new MySqlParameter("@exam_auto_id", MySqlDbType.Int32, 11, "exam_auto_id")
            , new MySqlParameter("@subject_code", MySqlDbType.Int32, 11, "subject_code")
            , new MySqlParameter("@req_student_auto_id", null)
            , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", DateTime.Today.Year)
            , new MySqlParameter("@req_exam_auto_id", null)
            , new MySqlParameter("@req_subject_code", null))
        {
        }
        public void refreshDt(int req_student_auto_id, int req_stream_auto_id, int req_class_of, int req_exam_auto_id, int req_subject_code)
        {
            this.req_student_auto_id = req_student_auto_id;
            this.req_stream_auto_id = req_stream_auto_id;
            this.req_class_of = req_class_of;
            this.req_exam_auto_id = req_exam_auto_id;
            this.req_subject_code = req_subject_code;
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_subject_code"].Value = req_subject_code;
            base.refreshDt();
        }
        public override bool saveChanges()
        {
            bool result= base.saveChanges();
            string query = "CALL `computeAllAffected`(" + req_exam_auto_id + "," + req_student_auto_id + "," + req_stream_auto_id + ", " + req_class_of + "," + req_subject_code + ");";
           Onion.MySQLHandler.MySQLHelper.executeWrite(query);
            return result;
        }
        public double getExamProgress(int exam_auto_id)
        {
            string query = "CALL `getExamProgress`(" + exam_auto_id + ");";
            return Convert.ToDouble(Onion.MySQLHandler.MySQLHelper.getOneValue(query, "progress"));
        }

        public static int getMarksOutOf(int exam_auto_id)
        {
            string query = "SELECT `out_of` FROM `exam` where `auto_id`="+exam_auto_id;
            return Convert.ToInt32(Onion.MySQLHandler.MySQLHelper.getOneValue(query, "out_of"));
 
        }
    }

    public class StudentMarksEntry : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        private int req_student_auto_id, req_exam_auto_id;
        public static StudentMarksEntry Default { get { return _default; } }
        private static StudentMarksEntry _default = new StudentMarksEntry();
        public StudentMarksEntry()
            : base(
              @"CALL `get_exam_marks_template`(@req_student_auto_id, null,null,@req_exam_auto_id,null);
                        SELECT subject.abbreviation AS subject_abbrev , subject.code AS subject_code,subject_marks.mark FROM 
                        `exam_marks_template`
                        LEFT JOIN 
                        (SELECT `exam_mark`.`subject_code`,  `exam_mark`.`mark`
                        FROM `exam_mark` WHERE `exam_mark`.`student_auto_id`=@req_student_auto_id AND `exam_mark`.`exam_auto_id`=@req_exam_auto_id) AS subject_marks
                        ON  `exam_marks_template`.`subject_code`=`subject_marks`.`subject_code` 
                        JOIN `subject` ON `exam_marks_template`.`subject_code`= `subject`.`code`   ORDER BY `exam_marks_template`.`subject_code`;"
            , ""
            , @"INSERT INTO `exam_mark` (`student_auto_id`,`exam_auto_id`,`subject_code`,`mark`) 
                  VALUES (@req_student_auto_id,@req_exam_auto_id,@subject_code,@mark) ON duplicate KEY UPDATE `mark`=@mark;"
            , ""
            , new string[] { "subject_code","mark"}
            , new MySqlParameter("@req_student_auto_id", null)
              , new MySqlParameter("@req_exam_auto_id", null))
        {
        }
        public void refreshDt(int req_student_auto_id, int req_exam_auto_id)
        {
            this.req_student_auto_id = req_student_auto_id;
            this.req_exam_auto_id = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            base.refreshDt();
        }
        public override bool saveChanges()
        {
            bool result = base.saveChanges();
            string query = "CALL `computeAllAffected`("+req_exam_auto_id+","+req_student_auto_id +",null, null, null);";
            Onion.MySQLHandler.MySQLHelper.executeWrite(query);
            return result;
        }
    }

}
