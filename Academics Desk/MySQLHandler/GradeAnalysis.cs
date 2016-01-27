using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class GradeAnalysis : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static GradeAnalysis Default { get { return _default; } }
        private static GradeAnalysis _default = new GradeAnalysis();
        public GradeAnalysis()
            : base(
              "CALL `getGradeAnalysis`(@req_exam_auto_id, @req_term_auto_id, @req_stream_auto_id, @req_class_of);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_stream_auto_id", null)
            , new MySqlParameter("@req_class_of", DateTime.Today.Year)
            , new MySqlParameter("@req_exam_auto_id", null)
            , new MySqlParameter("@req_term_auto_id", null))
        {
            dt = SmartDesk.MySQLHandler.Transformer.transformTable(dt, "subject", "grade", "count");
        }
        public void refreshDt(int req_stream_auto_id, int req_class_of, int req_exam_auto_id, int req_term_auto_id)
        {
            dt.PrimaryKey = null;
            dt.Columns.Clear();
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_class_of"].Value = req_class_of;
            base.refreshDt();
            dt = SmartDesk.MySQLHandler.Transformer.transformTable(dt, "subject", "grade", "count");
        }
    }


   
}
