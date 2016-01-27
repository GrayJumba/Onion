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
    public partial class ClassPicker : UserControl
    {
        public int SelectedClassOf
        {
            get { return Convert.ToInt32(comboBox.SelectedValue); }
            set { comboBox.SelectedValue = value; }
        }
        public ClassPicker()
        {
            InitializeComponent();
            comboBox.ItemsSource = MySQLHandler.SDObjectIDs.ClassIDHandler.AllIDs;
        }
    }
}
