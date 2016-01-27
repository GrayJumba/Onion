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
    /// Interaction logic for LabelledTextBox.xaml
    /// </summary>
    public partial class LabelledTextBox : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(LabelledTextBox));
        public string Label
        {
            get { return label.Content.ToString(); }
            set { label.Content = value; }
        }
        public string Value
        {
                get{   return this.GetValue(ValueProperty) as string;     }
                set {  this.SetValue(ValueProperty, value);         }
        }
        public LabelledTextBox()
        {
            InitializeComponent();
        }
    }
}
