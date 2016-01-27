using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.MySQLHandler
{
    #region Object ID template classes

    public class IDHandler
    {
        public class ObjectID
        {
           
            public string dfi { get; set; }
            public string ufi { get; set; }
            public ObjectID(string dfi, string ufi)
            {
                this.dfi = dfi;
                this.ufi = ufi;
            }
        }
        protected bool isInitialized = false;
        protected string dfi_columns;
        protected string ufi_columns;
        protected string filters;
        protected string table_name;
        protected string query = null;
        protected ObservableCollection<ObjectID> all_ids = new ObservableCollection<ObjectID>();
        public  virtual ObservableCollection<ObjectID> AllIDs
        {
            get
            {
                if (!isInitialized) refresh();
                return all_ids;
            }
        }
        public IDHandler(string query)
        {
            this.query = query;
        }
        public IDHandler(string table_name, string dfi_columns, string ufi_columns, string filters)
        {
            this.table_name = table_name;
            this.dfi_columns = dfi_columns;
            this.ufi_columns = ufi_columns;
            this.filters = filters;

        }
        public bool initialize()
        {
            if (isInitialized) return true;
            else if (refresh())
            {
               isInitialized = true;
               return true;
            }
            return false;
        }
        public virtual bool refresh()
        {
            if (this.query == null)
            {
                this.query = "SELECT " + dfi_columns + " AS `dfi`," + ufi_columns + " AS `ufi` FROM " + table_name;
                if (!String.IsNullOrWhiteSpace(this.filters))
                {
                    query += " WHERE " + this.filters;
                }
                query += ";";
            }
            if (Onion.MySQLHandler.MySQLHelper.executeRead(this.query))
            {
                this.all_ids.Clear();
                while (MySQLHelper.data_reader.Read())
                {
                    this.all_ids.Add(new ObjectID(MySQLHelper.data_reader.GetString("dfi"), MySQLHelper.data_reader.GetString("ufi")));
                }
                return true;
            }
            return false;
        }
    }
    public class UFIHandler
    {
        bool isInitialized = false;
        string ufi_columns;
        string table_name;
        string filters;
        string query;
        private ObservableCollection<string> all_ufis = new ObservableCollection<string>();
        public ObservableCollection<string> AllUFIs
        {
            get
            {
                if (!isInitialized) refresh();
                return all_ufis;
            }
            set
            {
                all_ufis = value;
            }
        }
        public UFIHandler(string table_name, string ufi_columns, string filters)
        {
            this.table_name = table_name;
            this.ufi_columns = ufi_columns;
            this.filters = filters;
        }
        public UFIHandler(string query)
        {
            this.query = query;
        }
        public virtual bool refresh()
        {
            if (query == null)
            {
                this.query = "SELECT " + ufi_columns + " AS `ufi` FROM " + table_name;
                if (!String.IsNullOrWhiteSpace(this.filters))
                {
                    query += " WHERE " + this.filters;
                }
                query += ";";
            }
            if (MySQLHelper.executeRead(this.query))
            {
                this.all_ufis.Clear();
                while (MySQLHelper.data_reader.Read())
                {
                    this.all_ufis.Add(MySQLHelper.data_reader.GetString("ufi"));
                }
                return true;
            }
            return false;
        }
        public bool initialize()
        {
            if (isInitialized) return true;
            else if (refresh())
            {
                isInitialized = true;
                return true;
            }
            return false;
        }
    }
    #endregion
}
