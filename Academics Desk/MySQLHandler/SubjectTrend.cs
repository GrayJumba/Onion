using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class SchoolSubjectTrend: Onion.MySQLHandler.MySQLHandlerTemplate
    {
          public static SchoolSubjectTrend Default { get { return _default; } }
        private static SchoolSubjectTrend _default = new SchoolSubjectTrend();

        public SchoolSubjectTrend()
            : base(
              @"CALL `subjectSchoolTrend`(@use_exam, @req_subject_code);"
            , ""
            , ""
            , ""
             ,new MySqlParameter("@use_exam", null)
            , new MySqlParameter("@req_subject_code", null))
        {
        }
        public void refreshDt(bool use_exam,int req_subject_code)
        {
            dtAdapter.SelectCommand.Parameters["@use_exam"].Value = use_exam ? 1 : 0;
            dtAdapter.SelectCommand.Parameters["@req_subject_code"].Value = req_subject_code;
            base.refreshDt();
        }
    }
}
