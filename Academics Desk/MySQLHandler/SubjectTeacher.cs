using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class SubjectTeacher : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static SubjectTeacher Default { get { return _default; } }
        private static SubjectTeacher _default = new SubjectTeacher();


        public SubjectTeacher()
            : base(
              @"SELECT `done_subject`.`code` AS subject_code,`done_subject`.`abbreviation` AS subject_abbrev,teacher_dfi_ufi(1,`stream_teacher_subject`.`teacher_auto_id`) AS teacher_ufi FROM `done_subject` LEFT JOIN 
                 (SELECT * FROM `stream_teacher_subject`  WHERE `stream_auto_id` =@req_stream_auto_id) AS `stream_teacher_subject`
                 ON `done_subject`.`code`=`stream_teacher_subject`.`subject_code`"
            , ""
            , @"INSERT INTO `stream_teacher_subject`(`subject_code`,`stream_auto_id`,`teacher_auto_id`)
                VALUES(@subject_code,@req_stream_auto_id,teacher_dfi_ufi(0,@teacher_ufi)) ON DUPLICATE KEY UPDATE
                `subject_code`=@subject_code,`stream_auto_id`=@req_stream_auto_id,`teacher_auto_id`=teacher_dfi_ufi(0,@teacher_ufi); "
                            , ""
            , new String[] { "subject_code", "teacher_ufi" }
            , new MySqlParameter("@req_stream_auto_id", null))
        {
        }
        public void refreshDt(int req_stream_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            base.refreshDt();
        }
    }

}