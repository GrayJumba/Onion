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
  
    /// Interaction logic for StudentPicker.xaml
    /// </summary>
    public partial class StudentPicker : UserControl
    {
        public Style ComboBoxStyle
        {
            get { return Student_Combo.Style; }
            set { Student_Combo.Style = value; }
        }
        public bool ScrollButtonsVisible
        {
            set
            {
                if (value)
                {
                    button_section.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    button_section.Width = new GridLength(0);
                }
            }
        }
        public int SelectedStudentAutoId
        {
            get { return Convert.ToInt32(Student_Combo.SelectedValue); }
            set { Student_Combo.SelectedValue = value; }
        }
        public event SelectionChanged SelectedStudentChanged;
        public delegate void SelectionChanged(StudentPicker s, EventArgs e);

        int p = 0;
        public int LastItemIndex
        {
            get { return Student_Combo.Items.Count - 1; }
        }
        public int CurrentItemIndex
        {
            get { return p; }
            set
            {
                if (value > 0 && value < LastItemIndex)
                {
                    previous_button.IsEnabled = true;
                    next_button.IsEnabled = true;
                    p = value;
                    Student_Combo.SelectedIndex = p;
                    return;
                }
                if (value <= 0 && value < LastItemIndex)
                {
                    previous_button.IsEnabled = false;
                    next_button.IsEnabled = true;
                    p = 0;
                    Student_Combo.SelectedIndex = p;
                    return;
                }
                if (value > 0 && value >= LastItemIndex)
                {
                    previous_button.IsEnabled = true;
                    next_button.IsEnabled = false;
                    p = LastItemIndex;
                    Student_Combo.SelectedIndex = p;
                    return;
                }
                else
                {
                    previous_button.IsEnabled = false;
                    next_button.IsEnabled = false;
                }

            }
        }
        private void previous_button_Click(object sender, RoutedEventArgs e)
        {
            CurrentItemIndex--;
        }

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            CurrentItemIndex++;
        }
        public StudentPicker()
        {
            InitializeComponent();
            this.SelectedStudentChanged+=StudentPicker_selectionChanged;
            Student_Combo.ItemsSource = MySQLHandler.SDObjectIDs.StudentIDHandler.AllIDs;
            Student_Combo.SelectionChanged += Student_Combo_SelectionChanged;
         
        }

        private void StudentPicker_selectionChanged(StudentPicker s, EventArgs e)
        {
            return;
        }

        private void Student_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStudentChanged(this,new EventArgs());
        }
    }
}
