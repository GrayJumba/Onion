using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;
namespace SmartDesk.MySQLHandler
{
    public class SDObjectIDs
    {
        public static IDHandler StudentIDHandler { get { return student_IDHandler; } } 
        private static IDHandler student_IDHandler = new IDHandler("`students`", "`auto_id`", "CONCAT(`students`.`admno`,' ',`students`.`name`)",null);
        public static StudentIDHandler Synced_StudentIDHandler { get { return synced_Student_IDHandler; } }
        private static StudentIDHandler synced_Student_IDHandler = new StudentIDHandler();

        public static StreamIDHandler Synced_StreamIDHandler { get { return synced_Stream_IDHandler; } }
        private static StreamIDHandler synced_Stream_IDHandler = new StreamIDHandler();

        public static UFIHandler StudentUFIHandler { get { return student_UFIHandler; }}
        private static UFIHandler student_UFIHandler = new UFIHandler("`students`", "CONCAT(`students`.`admno`,' ',`students`.`name`)", null);
        public static IDHandler TeacherIDHandler { get { return teacher_IDHandler; } }
        private static IDHandler teacher_IDHandler = new IDHandler("`teacher`", "`auto_id`", "CONCAT(`teacher`.`code`,' ',`teacher`.`name`)", null);
        public static IDHandler SubjectIDHandler { get { return subject_IDHandler; } }
        private static IDHandler subject_IDHandler = new IDHandler("`done_subject`", "`code`", "`done_subject`.`abbreviation`", null);
        public static IDHandler StreamIDHandler { get { return stream_IDHandler; } }
        private static IDHandler stream_IDHandler = new IDHandler("SELECT `auto_id` AS dfi ,stream_dfi_ufi(1,`auto_id`) AS ufi FROM `class_stream`");
        public static IDHandler ClassIDHandler { get { return class_IDHandler; } }
        private static IDHandler class_IDHandler = new IDHandler("SELECT DISTINCT `class_of`AS `dfi`, CONCAT('Form ',YEAR(CURDATE())-`class_of`+4) AS `ufi` FROM `class_stream` WHERE `class_of` BETWEEN YEAR(CURDATE()) AND YEAR(CURDATE())+3"
            + " UNION SELECT DISTINCT `class_of`AS `dfi`, CONCAT('Class of ',`class_of`) AS ufi FROM  (SELECT * FROM `class_stream` ORDER BY `class_of` DESC) AS `class_stream` WHERE `class_of` NOT BETWEEN YEAR(CURDATE()) AND YEAR(CURDATE())+3");
        public static IDHandler ExamIDHandler { get { return exam_IDHandler; } }
        private static IDHandler exam_IDHandler = new IDHandler("SELECT `exam`.`auto_id` as dfi, CONCAT( `exam`.`name`,' Term ',`term`.`number`,' ' ,`term`.`year`) AS ufi FROM `exam` JOIN `term` ON `exam`.`term_auto_id`=`term`.`auto_id` ORDER BY `exam`.`auto_id` DESC;");

        public static IDHandler TermIDHandler { get { return term_IDHandler; } }
        private static IDHandler term_IDHandler = new IDHandler("`term`", "`auto_id`", "term_dfi_ufi(1,`auto_id`)", null);
    }

    public class StreamIDHandler : IDHandler
    {

        public class StreamObjectID : ObjectID
        {
            public int class_of;
            public StreamObjectID(string dfi, string ufi, int class_of)
                : base(dfi, ufi)
            {
                this.class_of = class_of;
            }
        }
        public int req_class_of = 0;
        public override ObservableCollection<ObjectID> AllIDs
        {
            get
            {
                if (!isInitialized) refresh();
                return all_ids;
            }
        }
        protected List<StreamObjectID> unfiltered_ids = new List<StreamObjectID>();
        public StreamIDHandler()
            : base("SELECT `auto_id` AS dfi ,CONCAT(YEAR(CURDATE())-`class_of`+4,' ',`stream_name`) AS ufi ,class_of FROM `class_stream` WHERE `class_of` BETWEEN YEAR(CURDATE()) AND YEAR(CURDATE())+3 "
            + "UNION SELECT `auto_id` AS dfi ,CONCAT(`class_of`,' ',`stream_name`) AS ufi ,class_of  FROM  (SELECT * FROM `class_stream` ORDER BY `class_of` DESC) AS `class_stream` WHERE `class_of` NOT BETWEEN YEAR(CURDATE()) AND YEAR(CURDATE())+3")
        {
        }
        public void filter(int class_of)
        {
            req_class_of = class_of;
            all_ids.Clear();
            if (req_class_of > 0)
            {
                foreach (StreamObjectID object_id in unfiltered_ids)
                {
                    if (object_id.class_of == req_class_of) all_ids.Add(object_id);
                }
            }
            else
            {
                foreach (StreamObjectID object_id in unfiltered_ids)
                {
                    all_ids.Add(object_id);
                }
            }
        }
        public override bool refresh()
        {
            if (Onion.MySQLHandler.MySQLHelper.executeRead(this.query))
            {
                this.unfiltered_ids.Clear();
                while (MySQLHelper.data_reader.Read())
                {
                    this.unfiltered_ids.Add(new StreamObjectID(MySQLHelper.data_reader.GetString("dfi"), MySQLHelper.data_reader.GetString("ufi"),
                        MySQLHelper.data_reader.GetInt32("class_of")));
                }
                filter(req_class_of);
                return true;
            }
            return false;
        }
    }
    public class ExamIDHandler : IDHandler
    {

        public class ExamObjectID : ObjectID
        {
            public int term_auto_id;
            public ExamObjectID(string dfi, string ufi, int term_auto_id)
                : base(dfi, ufi)
            {
                this.term_auto_id = term_auto_id;
            }
        }
        public int req_term_auto_id = 0;
        public override ObservableCollection<ObjectID> AllIDs
        {
            get
            {
                if (!isInitialized) refresh();
                return all_ids;
            }
        }
        protected List<ExamObjectID> unfiltered_ids = new List<ExamObjectID>();
        public ExamIDHandler()
            : base("SELECT `exam`.`auto_id` as dfi, CONCAT( `exam`.`name`,' Term ',`term`.`number`,' ' ,`term`.`year`) AS ufi,`exam`.`term_auto_id`  FROM `exam` JOIN `term` ON `exam`.`term_auto_id`=`term`.`auto_id`;")
        {
        }
        public void filter(int term_auto_id)
        {
            req_term_auto_id = term_auto_id;
            all_ids.Clear();
            if (req_term_auto_id > 0)
            {
                foreach (ExamObjectID object_id in unfiltered_ids)
                {
                    if (object_id.term_auto_id == req_term_auto_id) all_ids.Add(object_id);
                }
            }
            else
            {
                foreach (ExamObjectID object_id in unfiltered_ids)
                {
                    all_ids.Add(object_id);
                }
            }
        }
        public override bool refresh()
        {
            if (Onion.MySQLHandler.MySQLHelper.executeRead(this.query))
            {
                this.unfiltered_ids.Clear();
                while (MySQLHelper.data_reader.Read())
                {
                    this.unfiltered_ids.Add(new ExamObjectID(MySQLHelper.data_reader.GetString("dfi"), MySQLHelper.data_reader.GetString("ufi"),
                        MySQLHelper.data_reader.GetInt32("term_auto_id")));
                }
                filter(req_term_auto_id);
                return true;
            }
            return false;
        }
    }
    public class StudentIDHandler:IDHandler
    {

        public class StudentObjectID : ObjectID
        {
            public int stream_auto_id;
            public int class_of;
            public StudentObjectID(string dfi, string ufi, int stream_auto_id, int class_of)
                : base(dfi, ufi)
            {
                this.stream_auto_id = stream_auto_id;
                this.class_of = class_of;
            }
        }
        public int req_stream_auto_id=-1;
        public int req_class_of = -1;
        public override ObservableCollection<ObjectID> AllIDs
        {
            get
            {
                if (!isInitialized) refresh();
                return all_ids;
            }
        }
        protected List<StudentObjectID> unfiltered_ids = new List<StudentObjectID>();
        public StudentIDHandler()
            : base("SELECT `student_basic_info`.`auto_id` AS dfi ,CONCAT(`student_basic_info`.`admno`,' ',`student_basic_info`.`name`) as ufi, "
                    +" `student_basic_info`.`class_stream_auto_id` ,`student_basic_info`.`class_of` FROM `student_basic_info`;")
        {
        }
        public void filter(int stream_auto_id, int class_of)
        {
            req_class_of = class_of;
            req_stream_auto_id = stream_auto_id;
            all_ids.Clear();
            if (stream_auto_id > 0)
            {
                foreach (StudentObjectID object_id in unfiltered_ids) 
                {
                    if (object_id.stream_auto_id == stream_auto_id) all_ids.Add(object_id);
                }
            }
            else if (class_of > 0)
            {
                foreach (StudentObjectID object_id in unfiltered_ids)
                {
                    if (object_id.class_of == class_of) all_ids.Add(object_id);
                }
            }
            else
            {
                foreach (StudentObjectID object_id in unfiltered_ids)
                {
                   all_ids.Add(object_id);
                }
            }
        }
        public override bool refresh()
        {
            if (Onion.MySQLHandler.MySQLHelper.executeRead(this.query))
            {
                this.unfiltered_ids.Clear();
                while (MySQLHelper.data_reader.Read())
                {
                    this.unfiltered_ids.Add(new StudentObjectID(MySQLHelper.data_reader.GetString("dfi"), MySQLHelper.data_reader.GetString("ufi"),
                        MySQLHelper.data_reader.GetInt32("class_stream_auto_id"), MySQLHelper.data_reader.GetInt32("class_of")));
                }
                filter(req_stream_auto_id, req_class_of);
                return true;
            }
            return false;
        }
    }

    public class SubjectIDHandler : IDHandler
    {

        public class SubjectObjectID : ObjectID
        {
            public bool is_done_by_school;
            public bool is_junior_compulsory;
            public bool is_senior_compulsory;
            public SubjectObjectID(string dfi, string ufi, bool is_done_by_school, bool is_junior_compulsory, bool is_senior_compulsory)
                : base(dfi, ufi)
            {
                this.is_done_by_school = is_done_by_school;
                this.is_junior_compulsory=is_junior_compulsory;
                this.is_senior_compulsory=is_senior_compulsory;
            }
        }
        public Int16 req_is_done_by_school;
        public Int16 req_is_junior_compulsory;
        public Int16 req_is_senior_compulsory;
        public override ObservableCollection<ObjectID> AllIDs
        {
            get
            {
                if (!isInitialized) refresh();
                return all_ids;
            }
        }
        protected List<SubjectObjectID> unfiltered_ids = new List<SubjectObjectID>();
        public SubjectIDHandler()
            : base("SELECT `subject`.`code` as dfi,`subject`.`abbreviation` as ufi,`subject`.`is_done_by_school`,`subject`.`is_junior_compulsory`,"
                            +" `subject`.`is_senior_compulsory` FROM `subject`;")
        {
        }
        public void filter(Int16 req_is_done_by_school, Int16 req_is_junior_compulsory, Int16 req_is_senior_compulsory)
        {
            this.req_is_done_by_school = req_is_done_by_school;
            this.req_is_junior_compulsory = req_is_junior_compulsory;
            this.req_is_senior_compulsory = req_is_senior_compulsory;
            
            all_ids.Clear();
            foreach (SubjectObjectID object_id in unfiltered_ids)
            {
                bool failed = false;
                if (req_is_done_by_school == 0 || req_is_done_by_school==1)
                {
                    if (object_id.is_done_by_school != Convert.ToBoolean(req_is_done_by_school)) failed = true;
                }
                if (req_is_junior_compulsory == 0 || req_is_junior_compulsory == 1)
                {
                    if (object_id.is_junior_compulsory != Convert.ToBoolean(req_is_junior_compulsory)) failed = true;
                }
                if (req_is_senior_compulsory == 0 || req_is_senior_compulsory == 1)
                {
                    if (object_id.is_senior_compulsory != Convert.ToBoolean(req_is_senior_compulsory)) failed = true;
                } 
                if (!failed) all_ids.Add(object_id);
            }
        }
        public override bool refresh()
        {
            if (Onion.MySQLHandler.MySQLHelper.executeRead(this.query))
            {
                this.unfiltered_ids.Clear();
                while (MySQLHelper.data_reader.Read())
                {
                    this.unfiltered_ids.Add(new SubjectObjectID(MySQLHelper.data_reader.GetString("dfi"), MySQLHelper.data_reader.GetString("ufi"),
                        MySQLHelper.data_reader.GetBoolean("is_done_by_school"), MySQLHelper.data_reader.GetBoolean("is_junior_compulsory"), MySQLHelper.data_reader.GetBoolean("is_senior_compulsory")));
                }
                filter(req_is_done_by_school, req_is_junior_compulsory, req_is_senior_compulsory);
                return true;
            }
            return false;
        }
    }
}
