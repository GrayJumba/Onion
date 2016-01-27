using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsDesk.MySQLHandler
{
    class Student:Onion.MySQLHandler.MySQLHandlerTemplate
    {
         public static Student Default = new Student();
         public Student()
             : base(
            "SELECT `students`.`auto_id`,`students`.`admno`,`students`.`name`, `students`.`kcpe_mark`,`students`.`gender`,"
                        + "`students`.`image_filepath`,`students`.`guardian_name`, `students`.`guardian_phone_no`,stream_dfi_ufi(1,`students`.`class_stream_auto_id`) AS stream_ufi "
                        + " FROM  `students`  "
                        + " JOIN `class_stream` ON `students`.`class_stream_auto_id`=`class_stream`.`auto_id` "
                        + " WHERE  (IF( @req_auto_id=0 OR @req_auto_id IS NULL,1 ,0) OR (`students`.`auto_id` = @req_auto_id)) "
                        + " AND (IF(@req_class_stream_auto_id=0 OR @req_class_stream_auto_id IS NULL, 1,0) OR (`students`.`class_stream_auto_id` = @req_class_stream_auto_id)) "
                        + " AND (IF(@req_class_of=0 OR @req_class_of IS NULL, 1,0) OR (`class_stream`.`class_of` = @req_class_of)) "
                        +" ORDER BY `students`.`auto_id` DESC;"
             ,"INSERT INTO `students`"
                            +" (`auto_id`,`admno`,`name`,`kcpe_mark`,`gender`,`image_filepath`,`guardian_name`,`guardian_phone_no`,`class_stream_auto_id`)"
                            + " VALUES(@auto_id,@admno,@name,@kcpe_mark,@gender,@image_filepath,@guardian_name,@guardian_phone_no,stream_dfi_ufi(0,@stream_ufi);SELECT `auto_id`= LAST_INSERT_ID();"
            ,"UPDATE `students` SET `admno` = @admno,`name` = @name,`kcpe_mark` = @kcpe_mark,`gender` = @gender,`image_filepath` = @image_filepath,"
                            + " `guardian_name` = @guardian_name,`guardian_phone_no` = @guardian_phone_no,`class_stream_auto_id` = stream_dfi_ufi(0,@stream_ufi) WHERE `auto_id` = @auto_id;"
            ,"DELETE FROM `students` WHERE `auto_id` = @auto_id;"
            ,new MySqlParameter("@auto_id", MySqlDbType.UInt64, 11, "auto_id")
            , new MySqlParameter("@admno", MySqlDbType.VarChar, 45, "admno")
            , new MySqlParameter("@name", MySqlDbType.VarChar, 100, "name")
            , new MySqlParameter("@kcpe_mark", MySqlDbType.Int32, 11, "kcpe_mark")
            , new MySqlParameter("@gender", MySqlDbType.VarChar, 10, "gender")
            , new MySqlParameter("@image_filepath", MySqlDbType.VarChar, 300, "image_filepath")
            , new MySqlParameter("@guardian_name", MySqlDbType.VarChar, 45, "guardian_name")
            , new MySqlParameter("@guardian_phone_no", MySqlDbType.Int32, 11, "guardian_phone_no")
            , new MySqlParameter("@stream_ufi", MySqlDbType.VarChar, 45, "stream_ufi")
            , new MySqlParameter("@req_auto_id",0)
            , new MySqlParameter("@req_class_stream_auto_id",0)
             , new MySqlParameter("@req_class_of",0))
        {
        }
         public void refreshDt(int req_auto_id, int req_class_stream_auto_id, int req_class_of)
        {
            dtAdapter.SelectCommand.Parameters["@req_auto_id"].Value = req_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_stream_auto_id"].Value = req_class_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            base.refreshDt();
        }
    }
}
