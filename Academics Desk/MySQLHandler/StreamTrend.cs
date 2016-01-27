using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicsDesk.MySQLHandler
{
    class StreamTermTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StreamTermTrend Default { get { return _default; } }
        private static StreamTermTrend _default = new StreamTermTrend();

        public static StreamTermTrend Results { get { return _results; } }
        private static StreamTermTrend _results = new StreamTermTrend();
        public StreamTermTrend()
            : base(
              @"SELECT CONCAT(`term`.`year`,' Term ',`term`.`number`) AS `term`,`stream_term_aggregates`.`average` FROM 
                        (
	                        SELECT * FROM `term` ORDER BY `year`,`number`
                        ) AS `term`
                        JOIN
                        (
	                        SELECT * FROM `stream_term_aggregates` WHERE `stream_auto_id`=@req_stream_auto_id
                        ) AS `stream_term_aggregates` 
                        ON `term`.`auto_id`=`stream_term_aggregates`.`term_auto_id`"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_stream_auto_id", null))
        {
        }
        public void refreshDt(int req_stream_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            base.refreshDt();
        }
    }

    class StreamExamTrend : Onion.MySQLHandler.MySQLHandlerTemplate
    {
        public static StreamExamTrend Default { get { return _default; } }
        private static StreamExamTrend _default = new StreamExamTrend();
        public static StreamExamTrend Results { get { return _results; } }
        private static StreamExamTrend _results = new StreamExamTrend();
        public StreamExamTrend()
            : base(
              @"SELECT CONCAT(`exam`.`name` ,' ',`term_year`,' Term ',`term_number`) AS `exam`,`stream_exam_aggregates`.`average` FROM 
                        (
	                        SELECT `exam`.`auto_id`,`exam`.`name`,`term`.`year` AS `term_year`,`term`.`number` AS `term_number` FROM `exam` JOIN `term` ON `exam`.`term_auto_id`=`term`.`auto_id` ORDER BY `start_date`
                        ) AS `exam`
                        JOIN
                        (
	                        SELECT * FROM `stream_exam_aggregates` WHERE `stream_auto_id`=@req_stream_auto_id
                        ) AS `stream_exam_aggregates` 
                        ON `exam`.`auto_id`=`stream_exam_aggregates`.`exam_auto_id`"
            , ""
            , ""
            , ""
            , new MySqlParameter("@req_stream_auto_id", null))
        {
        }
        public void refreshDt(int req_stream_auto_id)
        {
            dtAdapter.SelectCommand.Parameters["@req_stream_auto_id"].Value = req_stream_auto_id;
            base.refreshDt();
        }
    }
}
