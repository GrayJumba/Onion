using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsDesk.MySQLHandler
{
    class Stream : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static Stream Default = new Stream();
        public Stream()
            : base(
           "SELECT `class_stream`.`auto_id`,`class_stream`.`stream_name`,`class_stream`.`class_of`,teacher_dfi_ufi(1,`class_stream`.`teacher_auto_id`) as teacher_ufi FROM `class_stream`;"
           , "INSERT INTO `class_stream` (`stream_name`,`class_of`,`teacher_auto_id`)VALUES(@stream_name,@class_of,teacher_dfi_ufi(0,@teacher_ufi));"
           , "UPDATE `class_stream` SET `stream_name` = @stream_name,`class_of` = @class_of,`teacher_auto_id` =teacher_dfi_ufi(0,@teacher_ufi) WHERE `auto_id` = @auto_id;"
           , "DELETE FROM `class_stream` WHERE `auto_id` = @auto_id;"
           , new String[] { "auto_id", "stream_name", "class_of", "teacher_ufi" })
        {
        }
    }
}
