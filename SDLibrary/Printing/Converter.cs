using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLibrary.Reports
{
    public class Converter
    {
        public ObservableCollection<Point> Points
        {
            get { return points; }
        }
        ObservableCollection<Point> points = new ObservableCollection<Point>();
        DataTable dt;
        string x_col;
        string y_col;

        public Converter(DataTable dt, string x_col, string y_col)
        {
            this.dt = dt;
            this.x_col = x_col;
            this.y_col = y_col;
            refresh();
        }
        public void refresh()
        {
            points.Clear();
            foreach (DataRow row in dt.Rows)
            {
                Point point = new Point();
                point.X = row[x_col].ToString();
                point.Y = row[y_col] == DBNull.Value ? 0 : row[y_col];
                points.Add(point);
            }
        }
    }
    public class Point
    {
        public string X { get; set; }
        public object Y { get; set; }
    }
}
