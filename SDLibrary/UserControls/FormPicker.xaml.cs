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
    public partial class FormPicker : UserControl
    {
        public int SelectedForm
        {
            set {  comboBox.SelectedIndex = value - 1; }
            get { return comboBox.SelectedIndex + 1;}
        }
        public event SelectionChanged SelectedFormChanged;
        public delegate void SelectionChanged(FormPicker s, EventArgs e);
        public FormPicker()
        {
            InitializeComponent();
            comboBox.SelectionChanged += comboBox_SelectionChanged;
            SelectedFormChanged += FormPicker_SelectedFormChanged;
        }

        void FormPicker_SelectedFormChanged(FormPicker s, EventArgs e)
        {
           
        }

        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFormChanged(this, new EventArgs());
        }
    }
}
