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
    public partial class ExtendedStreamPicker : UserControl
    {
        public int SelectedStreamAutoID;
        public int SelectedClassOf;
      
        public event SelectionChanged SelectedClassesChanged;
        public delegate void SelectionChanged(ExtendedStreamPicker s, EventArgs e);
        bool event_running = false;
        MySQLHandler.StreamIDHandler streamIDHandler = new MySQLHandler.StreamIDHandler();
        public ExtendedStreamPicker()
        {
            InitializeComponent();
            this.SelectedClassesChanged += StreamPicker_selectionChanged;
            stream_Picker.comboBox.ItemsSource = streamIDHandler.AllIDs;
            stream_Picker.comboBox.SelectionChanged += Stream_Combo_SelectionChanged;
            Class_Picker.comboBox.SelectionChanged += Class_Combo_SelectionChanged;
        }

        private void StreamPicker_selectionChanged(ExtendedStreamPicker s, EventArgs e)
        {
            return;
        }
        private void runEvent()
        {
            SelectedStreamAutoID = Convert.ToInt32(stream_Picker.comboBox.SelectedValue);
            SelectedClassOf=Convert.ToInt32( Class_Picker.comboBox.SelectedValue);
            
            SelectedClassesChanged(this, new EventArgs());
        }
        private void Stream_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            runEvent();
            event_running = false;
        }

        private void Class_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            streamIDHandler.filter(Convert.ToInt32(Class_Picker.comboBox.SelectedValue));
            stream_Picker.comboBox.Text = null;
            runEvent();
            event_running = false;
        }
    }
}
