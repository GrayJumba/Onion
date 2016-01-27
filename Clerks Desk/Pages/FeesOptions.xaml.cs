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

namespace FeesDesk
{
    /// <summary>
    /// Interaction logic for FeesOptions.xaml
    /// </summary>
    public partial class FeesOptions : UserControl
    {
        public FeesOptions()
        {
            InitializeComponent();
            PaymentMethodDg.DataContext = MySQLHandler.PaymentMethod.Default.Dt;
            refreshPage();
        }
        private void refreshPage()
        {
            MySQLHandler.ReceiptNumberGenerator.refresh();
            next_rct_no_text.Text = MySQLHandler.ReceiptNumberGenerator.next_rct_no.ToString();
            prefix_text.Text = MySQLHandler.ReceiptNumberGenerator.prefix;
            suffix_text.Text = MySQLHandler.ReceiptNumberGenerator.suffix;

            MySQLHandler.PaymentMethod.Default.refreshDt();

           
        }
        private void fees_ok_Button_Click(object sender, RoutedEventArgs e)
        {
            int next_rct_no;
            if (int.TryParse(next_rct_no_text.Text, out next_rct_no))
            {
                MySQLHandler.ReceiptNumberGenerator.prefix = prefix_text.Text;
                MySQLHandler.ReceiptNumberGenerator.suffix = suffix_text.Text;
                MySQLHandler.ReceiptNumberGenerator.next_rct_no = next_rct_no;
                MySQLHandler.ReceiptNumberGenerator.saveChanges();
            }
            else MessageBox.Show("Error. Next receipt number must be a number.");

            MySQLHandler.PaymentMethod.Default.saveChanges();
            MySQLHandler.AllObjectIDs.PaymentMethodUFIHandler.refresh();
            refreshPage();
        }
    }
}
