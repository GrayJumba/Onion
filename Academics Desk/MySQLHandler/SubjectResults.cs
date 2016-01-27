using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    public class SubjectResults : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static SubjectResults Default { get { return _default; } }
        private static SubjectResults _default = new SubjectResults();
        public SubjectResults()
            : base(
             @"CALL `getSubjectResults`(@req_exam_auto_id, @req_term_auto_id, @req_subject_code);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_subject_code", null)
            , new MySqlParameter("@req_exam_auto_id", null)
            , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt(int req_subject_code, int req_exam_auto_id, int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_subject_code"].Value = req_subject_code;
            dtAdapter.SelectCommand.Parameters["@req_exam_auto_id"].Value = req_exam_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            base.refreshDt();
        }
    }
   
}
