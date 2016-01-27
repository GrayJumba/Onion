using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    public class ClassResults : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ClassResults Default { get { return _default; } }
        private static ClassResults _default = new ClassResults();
        public ClassResults()
            : base(
             @"CALL `getClassResults`(@req_exam_auto_id, @req_term_auto_id, @req_stream_auto_id, @req_class_of);"
            , ""
            , ""
            , ""
             , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", null)
            , new MySqlParameter("@req_exam_auto_id", null)
            , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt(int req_stream_auto_id, int req_class_of, int req_exam_auto_id, int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            base.refreshDt();
        }
    }
    public class ClassResultsAggr : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ClassResultsAggr Default { get { return _default; } }
        private static ClassResultsAggr _default = new ClassResultsAggr();
        public ClassResultsAggr()
            : base(
             @"CALL `getClassResultsAggr`(@req_exam_auto_id, @req_term_auto_id, @req_stream_auto_id, @req_class_of);"
            , ""
            , ""
            , ""
             , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", null)
            , new MySqlParameter("@req_exam_auto_id", null)
            , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt(int req_stream_auto_id, int req_class_of, int req_exam_auto_id, int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            base.refreshDt();
        }
    }
}
