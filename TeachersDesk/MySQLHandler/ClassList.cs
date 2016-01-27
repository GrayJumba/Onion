using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersDesk.MySQLHandler
{
    class ClassList: Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ClassList Default { get { return _default; } }
        private static ClassList _default = new ClassList();
        public ClassList()
            : base(
              "CALL `getClassListData`(@purpose, @class_stream_auto_id, @class_of);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@class_stream_auto_id", null)
            , new MySqlParameter("@class_of", DateTime.Today.Year)
            , new MySqlParameter("@purpose", "General"))
        {
                dt = SmartDesk.MySQLHandler.Transformer.transformTable(dt, "admno", "abbreviation", "blank");
        }
        public void refreshDt(string class_stream_auto_id,string class_of,string purpose)
        {
            dt.PrimaryKey = null;
            dt.Columns.Clear();
            dtAdapter.SelectCommand.Parameters["@class_stream_auto_id"].Value = class_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@class_of"].Value = class_of;
            dtAdapter.SelectCommand.Parameters["@purpose"].Value = purpose;
            base.refreshDt();
            if (purpose == "General" || purpose == "Junior Subject Selection" || purpose == "Senior Subject Selection" || purpose == "Senior Marks Entry" || purpose == "Junior Marks Entry")
            dt = SmartDesk.MySQLHandler.Transformer.transformTable(dt, "admno", "abbreviation", "blank");
        }
    }


}
