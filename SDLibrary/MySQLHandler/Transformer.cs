using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDesk.MySQLHandler
{
   public class Transformer
    {
        public static DataTable transformTable(DataTable dt,string pivot_column,string stretch_column,string load_column)
        {
            DataTable new_dt = new DataTable(dt.TableName);
            List<string> other_columns = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != load_column && column.ColumnName != stretch_column)
                {
                    new_dt.Columns.Add(column.ColumnName,column.DataType);
                    if (column.ColumnName != pivot_column)
                    {
                        other_columns.Add(column.ColumnName);

                    }
                }
                
            }
            Type load_column_type = dt.Columns[load_column].DataType;
            new_dt.PrimaryKey = new DataColumn[] {new_dt.Columns[pivot_column]};
            foreach (DataRow row in dt.Rows)
            {
                string column_name=row[stretch_column].ToString();
                if (!new_dt.Columns.Contains(column_name))
                {
                    new_dt.Columns.Add(column_name, load_column_type);
                }
                DataRow new_row = new_dt.Rows.Find(row[pivot_column]);
                bool row_exists = false;
                if (new_row == null)
                {
                    
                    new_row = new_dt.NewRow();
                    new_row[pivot_column] = row[pivot_column];
                }
                else
                {
                    row_exists = true;
                }

                for (int i = 0; i < other_columns.Count; i++)
                {
                    new_row[other_columns[i]] = row[other_columns[i]];
                } 
                new_row[column_name] = row[load_column];
                if (!row_exists)
                {
                    new_dt.Rows.Add(new_row);
                }
            }
            return new_dt;
        }
    }
}
