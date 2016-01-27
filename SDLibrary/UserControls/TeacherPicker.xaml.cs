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
    /// Interaction logic for TeacherPicker.xaml
    /// </summary>
    public partial class TeacherPicker : UserControl
    {
        public Style ComboBoxStyle
        {
            get { return comboBox.Style; }
            set { comboBox.Style = value; }
        }
        public TeacherPicker()
        {
            InitializeComponent();
            comboBox.ItemsSource = MySQLHandler.SDObjectIDs.TeacherIDHandler.AllIDs;
        }
    }
}
