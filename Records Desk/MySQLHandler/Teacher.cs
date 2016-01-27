using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsDesk.MySQLHandler
{
    class Teacher : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static Teacher Default = new Teacher();
        public Teacher()
            : base(
           "SELECT `teacher`.`auto_id`,`teacher`.`code`,`teacher`.`name`,`teacher`.`initials`, `teacher`.`gender`,`teacher`.`phone_number`, `teacher`.`filepath` "
                       +" FROM `teacher`;"
            , "INSERT INTO `teacher`(`auto_id`,`code`,`name`,`initials`,`gender`,`phone_number`,`filepath`)"
                        + " VALUES(@auto_id,@code,@name,@initials,@gender,@phone_number,@filepath);"
           , "UPDATE `teacher` SET `code` = @code,`name` = @name,`initials` = @initials,`gender` = @gender, "
                        +"`phone_number` = @phone_number,`filepath` = @filepath WHERE `auto_id` = @auto_id;"
           , "DELETE FROM `teacher` WHERE `auto_id` = @auto_id;"
           , new String[] {"auto_id", "code", "name", "initials", "gender", "phone_number", "filepath" })
        {
        }
    }
}