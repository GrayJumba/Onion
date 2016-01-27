using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartDesk.Controls
{
    /// <summary>
    /// Interaction logic for MultipleStudentPicker.xaml
    /// </summary>
    public partial class StreamPicker : UserControl
    {
        public int SelectedStreamAutoID;
        public event SelectionChanged SelectedStreamChanged;
        public delegate void SelectionChanged(StreamPicker s, EventArgs e);
        bool event_running = false;
        public Style ComboBoxStyle
        {
            get { return comboBox.Style; }
            set { comboBox.Style = value; }
        }
        public StreamPicker()
        {
            InitializeComponent();
            this.SelectedStreamChanged += StreamPicker_selectionChanged;
            comboBox.ItemsSource = MySQLHandler.SDObjectIDs.StreamIDHandler.AllIDs;
            comboBox.SelectionChanged += Stream_Combo_SelectionChanged;
        }

        private void StreamPicker_selectionChanged(StreamPicker s, EventArgs e)
        {
            return;
        }
        private void runEvent()
        {
            SelectedStreamAutoID = Convert.ToInt32(comboBox.SelectedValue);
            SelectedStreamChanged(this, new EventArgs());
        }
        private void Stream_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            runEvent();
            event_running = false;
        }
    }
}
