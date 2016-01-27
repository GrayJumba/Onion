using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Onion.Controls
{
    public class DtTraversor : INotifyPropertyChanged
    {
        public delegate void SelectionChanged(DtTraversor d, EventArgs e);
        public event SelectionChanged selectionChanged;
        Button next_button = new Button();
        Button prev_button = new Button();
        public DataTable dt;
        int p = 0;
        public int LastRowIndex
        {
            get { return dt.Rows.Count - 1; }
        }
        public int CurrentRowIndex
        {
            get { return p; }
            set
            {
                if (value > 0 && value < LastRowIndex)
                {
                    prev_button.IsEnabled = true;
                    next_button.IsEnabled = true;
                    p = value;
                    selectionChanged(this, new EventArgs());
                    return;
                }
                if (value <= 0 && value < LastRowIndex)
                {
                    prev_button.IsEnabled = false;
                    next_button.IsEnabled = true;
                    p = 0;
                    selectionChanged(this, new EventArgs());
                    return;
                }
                if (value > 0 && value >= LastRowIndex)
                {
                    prev_button.IsEnabled = true;
                    next_button.IsEnabled = false;
                    p = LastRowIndex;
                    selectionChanged(this, new EventArgs());
                    return;
                }
                else
                {
                    prev_button.IsEnabled = false;
                    next_button.IsEnabled = false;
                }

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public DtTraversor(DataTable dt)
        {
            this.dt = dt;
            dt.RowChanged += dt_RowChanged;
            selectionChanged += DtTraversor_selectionChanged;
        }
        void DtTraversor_selectionChanged(DtTraversor d, EventArgs e)
        {
            AllPropertiesChanged();
        }
        public void registerButtons(Button next_button, Button prev_button)
        {
            this.next_button = next_button;
            this.prev_button = prev_button;
            next_button.Click += next_button_Click;
            prev_button.Click += prev_button_Click;
            CurrentRowIndex = p;
        }
        public Object this[String fieldName]
        {
            get
            {
                    if (dt.Rows.Count != 0)
                        return dt.Rows[CurrentRowIndex][fieldName];
                return null;
            }
            set
            {
                    if (value == null)
                        value = DBNull.Value;
                    dt.Rows[CurrentRowIndex][fieldName] = value;
                    this.NotifyPropertyChanged("Item[]");
            }
        }
        private void AllPropertiesChanged()
        {
            this.NotifyPropertyChanged("Item[]");
        }
        public void InitiateAllPropertiesChanged()
        {
            AllPropertiesChanged();
        }
        void dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Row != getCurrentRow()) return;
            else
                this.NotifyPropertyChanged("Item[]");
        }
        public DataRow getCurrentRow()
        {
            if (CurrentRowIndex >= 0 && CurrentRowIndex < dt.Rows.Count)
                return dt.Rows[CurrentRowIndex];
            else return null;
        }
        public void Next()
        {
            CurrentRowIndex++;
        }
        public void Previous()
        {
            CurrentRowIndex--;
        }
        public void JumpToStart()
        {
            CurrentRowIndex = 0;
        }
        public void JumbToEnd()
        {
            CurrentRowIndex = LastRowIndex;
        }
        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }
        private void prev_button_Click(object sender, RoutedEventArgs e)
        {
            Previous();
        }
    }
}
