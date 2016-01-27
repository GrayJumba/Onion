using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class MarkSheet : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static MarkSheet Default { get { return _default; } }
        private static MarkSheet _default = new MarkSheet();
        public MarkSheet()
            : base(
              "CALL `getMarkSheet`(@req_exam_auto_id,@req_term_auto_id, @req_stream_auto_id, @req_class_of);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", DateTime.Today.Year)
            , new MySqlParameter("@req_exam_auto_id", null)
             , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt( int req_exam_auto_id,int req_term_auto_id,int req_stream_auto_id, int req_class_of)
        {
            dt.Columns.Clear();
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            base.refreshDt();
        }
    }
    
}
