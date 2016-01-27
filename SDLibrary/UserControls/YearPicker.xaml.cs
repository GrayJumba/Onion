using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for YearPicker.xaml
    /// </summary>
    public partial class YearPicker : UserControl
    {
        public DatePicker datePicker;
        public event YearChanged SelectedYearChanged;
        public delegate void YearChanged(YearPicker w, EventArgs e);
        public int SelectedYear
        {
            get {
                 return datePicker.SelectedDate.Value.Year;
            }
            set { datePicker.SelectedDate = new DateTime(value, 1, 1);}
        }
        public DateTime SelectedDate
        {
            get
            {
                return datePicker.SelectedDate.Value;
            }
            set { datePicker.SelectedDate = value; }
        }
        public YearPicker()
        {
            InitializeComponent();
            this.SelectedYearChanged += default_SelectedYearChanged;
            datePicker = year_picker;
            year_picker.CalendarOpened += DatePicker_CalendarOpened;
        }
        private void default_SelectedYearChanged(YearPicker y, EventArgs e)
        {

        }
        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            var datepicker = sender as DatePicker;
            if (datepicker != null)
            {
                var popup = datepicker.Template.FindName("PART_Popup", datepicker) as Popup;
                if (popup != null && popup.Child is Calendar)
                {
                    Calendar calendar = (Calendar)popup.Child;
                    calendar.DisplayMode = CalendarMode.Decade;
                    calendar.Visibility = System.Windows.Visibility.Visible;
                    calendar.DisplayModeChanged -= calender_DisplayModeChanged;
                    calendar.DisplayDateChanged -= calender_DisplayDateChanged;
                    calendar.DisplayModeChanged += calender_DisplayModeChanged;
                    calendar.DisplayDateChanged += calender_DisplayDateChanged;

                }
            }
        }
        public void calender_DisplayModeChanged(object sender, EventArgs e)
        {
            var calendar = sender as Calendar;
            if (calendar.DisplayMode != CalendarMode.Decade)
            {
                calendar.DisplayMode = CalendarMode.Decade;
            }
        }
        public void calender_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            int diff = e.RemovedDate.Value.Year - calendar.DisplayDate.Year;
            if (diff == -10 || diff == 10)
                return;
            datePicker.SelectedDate = datePicker.DisplayDate;
            SelectedYearChanged(this, new EventArgs());
            calendar.Visibility = System.Windows.Visibility.Hidden;
        }
      
    }
}
