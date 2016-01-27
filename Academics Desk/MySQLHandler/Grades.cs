using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class OverallGrade : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static OverallGrade Default { get { return _default; } }
        private static OverallGrade _default = new OverallGrade();
        public OverallGrade()
            : base(
              "SELECT `grade`.`name` as `grade`,`grade`.`points`,`general_grade`.`lower_bound`,`general_grade`.`comment`,"
                    +"IF(`grade`.`points`=12,100,`general_grade2`.`lower_bound`-1) AS`upper_bound`"
                    +"FROM `grade` LEFT JOIN (SELECT * FROM  `general_grade` WHERE `general_grade`.`form`=@req_form) AS `general_grade` ON `grade`.`points`= `general_grade`.`grade_points` "
                    +"LEFT JOIN (SELECT * FROM  `general_grade` WHERE `general_grade`.`form`=@req_form) AS `general_grade2` ON `grade`.`points`= `general_grade2`.`grade_points`-1 ORDER BY  grade.points DESC;"
            , ""
            , "INSERT INTO `general_grade`(`grade_points`,`form`,`lower_bound`,`comment`)VALUES(@points,@req_form,@lower_bound,@comment) "
                    + " ON duplicate KEY UPDATE `lower_bound`=@lower_bound ,`comment`=@comment ;"
            , ""
            , new string[]{"points","lower_bound","comment"}
            ,new MySqlParameter("@req_form",4))
        {
        }
        public void refreshDt(int req_form)
        {
            dtAdapter.SelectCommand.Parameters["@req_form"].Value = req_form;
            base.refreshDt();
        }
    }
    class SubjectGrade : Onion.MySQLHandler.MySQLHandlerTemplate
    {

        public static SubjectGrade Default { get { return _default; } }
        private static SubjectGrade _default = new SubjectGrade();
        public SubjectGrade()
            : base(
              @"SELECT `grade`.`name` as `grade`,`grade`.`points` as `points`,`subject_grade`.`lower_bound`, `subject_grade`.`comment` ,
                    IF(`grade`.`points`=12,100,`subject_grade2`.`lower_bound`-1) AS`upper_bound` FROM `grade`   
                     LEFT JOIN (SELECT  * FROM `subject_grade` WHERE `subject_grade`.`form`=@req_form AND `subject_grade`.`subject_code`=@req_subject_code) AS `subject_grade` ON `grade`.`points`=`subject_grade`.`grade_points`
                     LEFT JOIN (SELECT  * FROM `subject_grade` WHERE `subject_grade`.`form`=@req_form AND `subject_grade`.`subject_code`=@req_subject_code) AS `subject_grade2` ON `grade`.`points`= `subject_grade2`.`grade_points`-1 ORDER BY  grade.points DESC;"
            , ""
            , "INSERT INTO `subject_grade`(`subject_code`,`form`,`grade_points`,`lower_bound`,`comment`)"
                    +"VALUES(@req_subject_code,@req_form,@points,@lower_bound,@comment) ON duplicate key UPDate `lower_bound`=@lower_bound,`comment`=@comment"
            , ""
            , new string[] {"points","lower_bound","comment"}
            , new MySqlParameter("@req_form", 4)
            , new MySqlParameter("@req_subject_code", 101))
        {
        }
        public void refreshDt(int req_form, int req_subject_code)
        {
            dtAdapter.SelectCommand.Parameters["@req_form"].Value = req_form;
            dtAdapter.SelectCommand.Parameters["@req_subject_code"].Value = req_subject_code;
            base.refreshDt();
        }
    }
   

}
