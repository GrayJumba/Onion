using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    public class ReportCardDetails : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ReportCardDetails Default { get { return _default; } }
        private static ReportCardDetails _default = new ReportCardDetails();
        public ReportCardDetails()
            : base(
             @"CALL `getReportCardDetails`(@req_student_auto_id,@req_term_auto_id)"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_student_auto_id", null)
            , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt(int req_student_auto_id,int req_term_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = req_student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = req_term_auto_id;
            base.refreshDt();
        }
    }
    public class ReportCardMarks : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static ReportCardMarks  Default { get { return _default; } }
        private static ReportCardMarks _default =new ReportCardMarks();
        public ReportCardMarks ()
            : base(
              @"CALL `getReportCardMarks`(@req_student_auto_id,@req_term_auto_id);"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_student_auto_id", null)
             , new MySqlParameter("@req_term_auto_id", null))
        {
        }
        public void refreshDt(int student_auto_id, int term_auto_id)
        {
            dt.Columns.Clear();
            dtAdapter.SelectCommand.Parameters["@req_student_auto_id"].Value = student_auto_id;
            dtAdapter.SelectCommand.Parameters["@req_term_auto_id"].Value = term_auto_id;
            base.refreshDt();
        }
    }
    
}