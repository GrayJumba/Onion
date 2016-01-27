using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.MySQLHandler
{
    public class MySQLHandlerTemplate
    {
        /// <summary>
        /// Tabular
        /// </summary>

        public DataTable Dt { get { return dt; } }
        protected DataTable dt = new DataTable();
        protected MySqlDataAdapter dtAdapter = new MySqlDataAdapter();
        public MySQLHandlerTemplate(string select_query, string insert_query, string update_query, string delete_query, params MySqlParameter[] parameters)
        {
            construct(select_query, insert_query, update_query, delete_query, parameters);
        }
        private void construct(string select_query, string insert_query, string update_query, string delete_query,MySqlParameter[] parameters)
        {
            dtAdapter.SelectCommand = new MySqlCommand(select_query, MySQLHelper.getDefaultConnection());
            dtAdapter.UpdateCommand = new MySqlCommand(update_query, MySQLHelper.getDefaultConnection());
            dtAdapter.DeleteCommand = new MySqlCommand(delete_query, MySQLHelper.getDefaultConnection());
            dtAdapter.InsertCommand = new MySqlCommand(insert_query, MySQLHelper.getDefaultConnection());
            dtAdapter.SelectCommand.Parameters.AddRange(parameters);
            dtAdapter.InsertCommand.Parameters.AddRange(parameters);
            dtAdapter.UpdateCommand.Parameters.AddRange(parameters);
            dtAdapter.DeleteCommand.Parameters.AddRange(parameters);
            dtAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            dtAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            dtAdapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
            refreshDt();
        }
        public MySQLHandlerTemplate(string select_query, string insert_query, string update_query, string delete_query,string[] simple_parameters ,params MySqlParameter[] parameters)
        {
            List<MySqlParameter> par = parameters.ToList();
            foreach (string s in simple_parameters)
            {
                par.Add(new MySqlParameter() { ParameterName = "@" + s, SourceColumn = s });
            }
            construct(select_query, insert_query, update_query, delete_query, par.ToArray()); 
        }
        public void initialize()
        {

        }
        protected void refreshConnection()
        {
            if (dtAdapter.SelectCommand.Connection.State == ConnectionState.Open)
            {
                dtAdapter.SelectCommand.Connection.Close();
            }

            if (dtAdapter.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                dtAdapter.UpdateCommand.Connection.Close();
            }
            if (dtAdapter.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                dtAdapter.DeleteCommand.Connection.Close();
            }
            if (dtAdapter.InsertCommand.Connection.State == ConnectionState.Open)
            {
                dtAdapter.InsertCommand.Connection.Close();
            }
        }
        public virtual bool saveChanges()
        {
            try
            {
                refreshConnection();
                dtAdapter.Update(this.dt);
                dt.AcceptChanges();                     
                return true;               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(ex.Message+"We have detected errors in your input.Continue anyway?", "Smart Desk", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    dtAdapter.ContinueUpdateOnError = true;
                    dtAdapter.Update(this.dt);
                    dt.AcceptChanges();
                    dtAdapter.ContinueUpdateOnError = false;
                    return true;
                }
                return false;
            }
        }
        public virtual  void refreshDt()
        {          
            refreshConnection();
            dt.Clear();
            dtAdapter.Fill(dt);
        }

    }
}
