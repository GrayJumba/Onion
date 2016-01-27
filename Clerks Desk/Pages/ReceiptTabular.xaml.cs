using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for ReceiptTabular.xaml
    /// </summary>
    public partial class ReceiptTabular : UserControl
    {
        public static ReceiptTabular user_control;
        private DataRowView rowBeingEdited = null;
        public ReceiptTabular()
        {
            user_control = this;
            InitializeComponent();
            ReceiptDg.DataContext = MySQLHandler.Receipt.Default.Dt;
        }
       
       private void refresh()
       {
           string start_date;
           string end_date;
           if (startDatePicker.SelectedDate == null) start_date = null; else start_date = startDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
           if (endDatePicker.SelectedDate == null) end_date = null; else end_date = endDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
           MySQLHandler.Receipt.Default.refreshDt((string)ReceiptCombo.SelectedValue, (string)PaymentMethodCombo.SelectedValue, start_date, end_date);
       }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (this.IsInitialized)
            {
                refresh();
            }
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                refresh();
            }
        }

        private void ReceiptDg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                rowBeingEdited.EndEdit();
            }
        }

        private void ReceiptDg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView rowView = e.Row.Item as DataRowView;
            rowBeingEdited = rowView;
        }

       
     
    }
}
