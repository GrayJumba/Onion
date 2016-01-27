using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class JuniorSubjectSelection : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static JuniorSubjectSelection Default { get { return _default; } }
        private static JuniorSubjectSelection _default = new JuniorSubjectSelection();
        public JuniorSubjectSelection()
            : base(
              "CALL `get_req_student`(@req_student_auto_id, @req_stream_auto_id,@req_class_of);"
                        +"SELECT req_student.auto_id AS student_auto_id,req_student.admno,req_student.name, IFNULL(junior_doing.doing,0) AS doing   FROm req_student LEFT JOIN (SELECT student_auto_id,doing FROM  student_junior_subject_doing WHERE subject_code=@req_subject_code) AS junior_doing  "
                        +"ON req_student.auto_id= junior_doing.student_auto_id"
            , ""
            , @"INSERT IGNORE INTO `junior_subject_selection`(`student_auto_id`,`subject_code`) VALUES(@student_auto_id ,@req_subject_code) ;
                    DELETE FROM `junior_subject_selection` WHERE `student_auto_id` =IF(@doing=0 ,@student_auto_id,null) && `subject_code` =IF(@doing=0 ,@req_subject_code,null);"
            , ""
            , new string[] { "student_auto_id" }
            , new MySqlParameter("@req_student_auto_id", null)
            , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", DateTime.Today.Year)
             , new MySqlParameter("@req_subject_code", null))
        {
            
        }
        public void refreshDt(int req_student_auto_id, int req_stream_auto_id, int req_class_of, int req_subject_code)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            dtAdapter.SelectCommand.Parameters["@req_subject_code"].Value = req_subject_code;
            base.refreshDt();
        }
    }


    public class StudentJuniorSubjectSelection : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StudentJuniorSubjectSelection Default { get { return _default; } }
        private static StudentJuniorSubjectSelection _default = new StudentJuniorSubjectSelection();
        public StudentJuniorSubjectSelection()
            : base(
              @"SELECT `subject`.`code` AS `subject_code`,`abbreviation` As `subject`, IF(`student_data`.`subject_code` IS NULL,0,1) AS doing FROM 
                    (SELECT `code`,`abbreviation` FROM `done_subject` WHERE `is_junior_compulsory`=0  ORDER BY `code`) AS `subject`
                    LEFT JOIN 
                    (SELECT `subject_code`FROM junior_subject_selection WHERE `student_auto_id` =@req_student_auto_id) AS `student_data`
                    ON  `subject`.`code`= `student_data`.`subject_code`  ORDER BY `subject`.`code`;"
            , ""
             , @"INSERT IGNORE INTO `junior_subject_selection`(`student_auto_id`,`subject_code`) VALUES(@req_student_auto_id ,@subject_code) ;
                    DELETE FROM `junior_subject_selection` WHERE `student_auto_id` =IF(@doing=0 ,@req_student_auto_id,null) && `subject_code` =IF(@doing=0 ,@subject_code,null);"
            , ""
            , new string[] { "subject_code", "doing" }
            , new MySqlParameter("@req_student_auto_id", null))
        {
        }
        public void refreshDt(int req_student_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.UpdateCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            base.refreshDt();
        }

    }
    class SeniorSubjectSelection : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static SeniorSubjectSelection Default { get { return _default; } }
        private static SeniorSubjectSelection _default = new SeniorSubjectSelection();
        public SeniorSubjectSelection()
            : base(
              "CALL `get_req_student`(@req_student_auto_id, @req_stream_auto_id,@req_class_of);"
                        + "SELECT req_student.auto_id  AS student_auto_id,req_student.admno,req_student.name, IFNULL(senior_doing.doing,0) AS doing  "
                        +" FROM req_student LEFT JOIN (SELECT student_auto_id,doing FROM  student_senior_subject_doing WHERE subject_code=@req_subject_code) AS senior_doing "
                        + "ON req_student.auto_id= senior_doing.student_auto_id"
            , ""
            , @"INSERT IGNORE INTO `senior_subject_selection`(`student_auto_id`,`subject_code`) VALUES(@student_auto_id ,@req_subject_code) ;
                        DELETE FROM `senior_subject_selection` WHERE `student_auto_id` =IF(@doing=0 ,@student_auto_id,null) && `subject_code` =IF(@doing=0 ,@req_subject_code,null);"
            , ""
            , new string[] { "student_auto_id"}
            , new MySqlParameter("@req_student_auto_id", null)
            , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", null)
             , new MySqlParameter("@req_subject_code", null))
        {

        }
        public void refreshDt(int req_student_auto_id, int req_stream_auto_id, int req_class_of, int req_subject_code)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            dtAdapter.SelectCommand.Parameters["@req_subject_code"].Value = req_subject_code;
            base.refreshDt();
        }
    }

    public class StudentSeniorSubjectSelection : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StudentSeniorSubjectSelection Default { get { return _default; } }
        private static StudentSeniorSubjectSelection _default = new StudentSeniorSubjectSelection();
        public StudentSeniorSubjectSelection()
            : base(
              @"SELECT `subject`.`code` AS `subject_code`,`abbreviation` As `subject`, IF(`student_data`.`subject_code` IS NULL,0,1) AS doing FROM 
                    (SELECT `code`,`abbreviation` FROM `done_subject` WHERE `is_senior_compulsory`=0 ) AS `subject`
                    LEFT JOIN 
                    (SELECT `subject_code`FROM senior_subject_selection WHERE `student_auto_id` =@req_student_auto_id) AS `student_data`
                    ON  `subject`.`code`= `student_data`.`subject_code` ORDER BY `subject`.`code`;"
            , ""
             , @"INSERT IGNORE INTO `senior_subject_selection`(`student_auto_id`,`subject_code`) VALUES(@req_student_auto_id ,@subject_code) ;
                    DELETE FROM `senior_subject_selection` WHERE `student_auto_id` =IF(@doing=0 ,@req_student_auto_id,null) && `subject_code` =IF(@doing=0 ,@subject_code,null);"
            , ""
            , new string[] { "subject_code", "doing" }
            , new MySqlParameter("@req_student_auto_id", null))
        {
        }
        public void refreshDt(int req_student_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.UpdateCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            base.refreshDt();
        }

    }
}
