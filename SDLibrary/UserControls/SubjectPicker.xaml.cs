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
    public partial class SubjectPicker : UserControl
    {
        public int selected_subject_code
        {
            get { return Convert.ToInt32(comboBox.SelectedValue); }
            set { comboBox.SelectedValue = value; }
        }
        public SubjectPicker()
        {
            InitializeComponent();
            comboBox.ItemsSource = MySQLHandler.SDObjectIDs.SubjectIDHandler.AllIDs;
        }
    }
}
