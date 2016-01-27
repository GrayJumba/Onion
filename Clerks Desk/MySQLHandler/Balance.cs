using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace MySQLHandler
{

}
namespace FeesDesk.MySQLHandler
{
    class Balance: MySQLHandlerTemplate
    {
        public static Balance Default { get { return _default; } }
        private static Balance _default = new Balance();
        public Balance()
            : base(
              "CALL `getBalancesAsAt`(@as_at,@student_auto_id, @class_stream_auto_id, @class_of);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@student_auto_id", null)
            , new MySqlParameter("@class_stream_auto_id", null)
            , new MySqlParameter("@class_of", null)
            , new MySqlParameter("@as_at", DateTime.Today))
        {

        }
        public void refreshDt(int student_auto_id, int class_stream_auto_id,int class_of ,DateTime as_at)
        {
            dtAdapter.SelectCommand.Parameters["@student_auto_id"].Value = student_auto_id;
            dtAdapter.SelectCommand.Parameters["@class_stream_auto_id"].Value = class_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@class_of"].Value = class_of;
            dtAdapter.SelectCommand.Parameters["@as_at"].Value = as_at;
            base.refreshDt();
        }
    }


}
