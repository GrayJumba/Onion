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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AdvancedSubjectPicker : UserControl
    {
        MySQLHandler.SubjectIDHandler subjectIDHandler = new MySQLHandler.SubjectIDHandler();

        public void SetFilter(Int16 req_is_done_by_school, Int16 req_is_junior_compulsory, Int16 req_is_senior_compulsory)
        {
            subjectIDHandler.filter(req_is_done_by_school, req_is_junior_compulsory, req_is_senior_compulsory);
        }
        public int selected_subject_code
        {
            get { return Convert.ToInt32(subjectPicker.comboBox.SelectedValue); }
            set { subjectPicker.comboBox.SelectedValue = value; }
        }
        public AdvancedSubjectPicker()
        {
            InitializeComponent();
            subjectPicker.comboBox.ItemsSource = subjectIDHandler.AllIDs;
        }
    }
}
