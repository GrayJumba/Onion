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
    /// Interaction logic for ExtendedStudentPicker.xaml
    /// </summary>
    public partial class ExtendedStudentPicker : UserControl
    {
        public int SelectedStudentAutoID;
        public int SelectedStreamAutoID;
        public int SelectedClassOf;
        public enum StudentSelectionType { One, Extended, OneOrExtended };
        public int getNumberOfStudents(){
            return Student_Picker.Student_Combo.Items.Count;
        }
        public StudentSelectionType SelectionType
        {
            set
            {
                if (value == StudentSelectionType.One)
                {
                    student_section.Width = new GridLength(1, GridUnitType.Star);
                    stream_section.Width = new GridLength(0);
                }
                else if (value == StudentSelectionType.Extended)
                {
                    student_section.Width = new GridLength(0);
                    stream_section.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    student_section.Width = new GridLength(2, GridUnitType.Star);
                    stream_section.Width = new GridLength(2, GridUnitType.Star);
                }
            }
        }
        public event SelectionChanged SelectedStudentsChanged;
        public delegate void SelectionChanged(ExtendedStudentPicker s, EventArgs e);
        bool event_running = false;
        MySQLHandler.StudentIDHandler studentIDHandler = new MySQLHandler.StudentIDHandler();
        public ExtendedStudentPicker()
        {
            InitializeComponent();
            this.SelectedStudentsChanged+=ExtendedStudentPicker_selectionChanged;
            Student_Picker.Student_Combo.ItemsSource = studentIDHandler.AllIDs;
            Student_Picker.Student_Combo.SelectionChanged += Student_Combo_SelectionChanged;
            Extended_Stream_Picker.stream_Picker.comboBox.SelectionChanged += Stream_Combo_SelectionChanged;
            Extended_Stream_Picker.Class_Picker.comboBox.SelectionChanged += Class_Combo_SelectionChanged;
        }
        public void NextStudent()
        {
            Student_Picker.CurrentItemIndex++;
        }
        private void ExtendedStudentPicker_selectionChanged(ExtendedStudentPicker s, EventArgs e)
        {
            return;
        }

        private void Student_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            runEvent();
            event_running = false;
        }
        private void runEvent()
        {
            SelectedStudentAutoID= Student_Picker.SelectedStudentAutoId;
            SelectedStreamAutoID= Extended_Stream_Picker.SelectedStreamAutoID;
            SelectedClassOf = Extended_Stream_Picker.SelectedClassOf;
            SelectedStudentsChanged(this, new EventArgs());
        }
        private void Stream_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            studentIDHandler.filter(Convert.ToInt32(Extended_Stream_Picker.stream_Picker.comboBox.SelectedValue), -1);
            Student_Picker.Student_Combo.Text = null;
            runEvent();
            event_running = false;
        } 

        private void Class_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (event_running) return;
            event_running = true;
            studentIDHandler.filter(-1, Convert.ToInt32(Extended_Stream_Picker.Class_Picker.comboBox.SelectedValue));
            Student_Picker.Student_Combo.Text = null;
            Extended_Stream_Picker.stream_Picker.comboBox.Text = null;
            runEvent();
            event_running = false;
        }
    }
}
