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
    /// Interaction logic for TermPicker.xaml
    /// </summary>
    public partial class TermPicker : UserControl
    {
        public int SelectedTermAutoId
        {
            get { return Convert.ToInt32(comboBox.SelectedValue); }
            set { comboBox.SelectedValue = value; }
        }
        public Style ComboBoxStyle
        {
            get { return comboBox.Style; }
            set { comboBox.Style = value; }
        }
      
        public TermPicker()
        {
            InitializeComponent();
            comboBox.ItemsSource = MySQLHandler.SDObjectIDs.TermIDHandler.AllIDs;
        }
    }
}
