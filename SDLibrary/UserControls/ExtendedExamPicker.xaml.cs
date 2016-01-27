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
    public partial class ExtendedExamPicker : UserControl
    {
        public int SelectedExamAutoID;
        public int SelectedTermAuoID;
        public event SelectionChanged SelectedExamsChanged;
        public delegate void SelectionChanged(ExtendedExamPicker s, EventArgs e);
        bool event_running = false;
        MySQLHandler.ExamIDHandler examIDHandler = new MySQLHandler.ExamIDHandler();
        public ExtendedExamPicker()
        {
            InitializeComponent();
            this.SelectedExamsChanged += ExamPicker_selectionChanged;
            exam_Picker.comboBox.ItemsSource = examIDHandler.AllIDs;
            exam_Picker.comboBox.SelectionChanged += exam_Combo_SelectionChanged;
            term_Picker.comboBox.SelectionChanged += term_Combo_SelectionChanged;
        }

        private void ExamPicker_selectionChanged(ExtendedExamPicker s, EventArgs e)
        {
            return;
        }
        private void runEvent()
        {
            SelectedExamAutoID = Convert.ToInt32(exam_Picker.selected_exam_auto_id);
            SelectedTermAuoID=Convert.ToInt32( term_Picker.SelectedTermAutoId);
            SelectedExamsChanged(this, new EventArgs());
        }
        private void exam_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            runEvent();
            event_running = false;
        }

        private void term_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            examIDHandler.filter(Convert.ToInt32(term_Picker.comboBox.SelectedValue));
            exam_Picker.comboBox.Text = null;
            runEvent();
            event_running = false;
        }
    }
}
