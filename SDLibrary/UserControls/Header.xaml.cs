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
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
            string sch_name = MySQLHandler.Settings.Default["school_name"];
            string sch_address = MySQLHandler.Settings.Default["school_address"];
            string sch_phone = MySQLHandler.Settings.Default["school_phone"];
            string sch_email = MySQLHandler.Settings.Default["school_email"];

            SchoolName.Text = sch_name;
            SchoolDetails.Text = sch_address + ", " + sch_phone + "\n" + sch_email + ".\n";
        }
    }
}
