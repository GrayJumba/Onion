using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    public class TopStudents : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static TopStudents Default { get { return _default; } }
        private static TopStudents _default = new TopStudents();
        public TopStudents()
            : base(
             @"CALL `getTopStudents`(@req_exam_auto_id, @req_term_auto_id, @req_stream_auto_id, @req_class_of);"
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
    public class TopStudentsPerSubject : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static TopStudentsPerSubject Default { get { return _default; } }
        private static TopStudentsPerSubject _default = new TopStudentsPerSubject();
        public TopStudentsPerSubject()
            : base(
             @"CALL `getTopStudentsPerSubject`(@req_exam_auto_id, @req_term_auto_id, @req_stream_auto_id, @req_class_of);"
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
    public class MostImproved : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static MostImproved Default { get { return _default; } }
        private static MostImproved _default = new MostImproved();
        public MostImproved()
            : base(
             @"CALL `getMostImproved`(@req_exam_auto_id, @req_term_auto_id, @req_stream_auto_id, @req_class_of);"
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
