using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    public class StreamResults : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StreamResults Default { get { return _default; } }
        private static StreamResults _default = new StreamResults();
        public StreamResults()
            : base(
             @"CALL `getStreamResults`(@req_exam_auto_id, @req_term_auto_id);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_exam_auto_id", null)
            , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt( int req_exam_auto_id, int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            base.refreshDt();
        }
    }
  
   
}
