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
using System.Windows.Shapes;

namespace TeachersDesk
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
            //bankDg.DataContext = MySQLHandler.Bank.Default.Dt;
        }

        private void ok_Button_Click(object sender, RoutedEventArgs e)
        {
            //MySQLHandler.Bank.Default.saveChanges();
            //MySQLHandler.Bank.Default.refreshDt();
            //MySQLHandler.AllObjectIDs.BankUFIHandler.refresh();
        }
     
    }
}
