using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Receipts.xaml
    /// </summary>
    public partial class ReceiptSingular : UserControl
    {
        public static ReceiptSingular thisUserControl;

        private static Onion.Controls.DtTraversor dt_traversor = new Onion.Controls.DtTraversor(MySQLHandler.Receipt.Default.Dt);
        public Onion.Controls.DtTraversor Dt_Traversor
        {
            get { return dt_traversor;}
        }
        public ReceiptSingular()
        {
            thisUserControl = this;
            InitializeComponent();
            dt_traversor.registerButtons(next_button, previous_button);
            dt_traversor.JumbToEnd();
        }
    }

    public class NumberToWordsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (String.IsNullOrEmpty(value.ToString())) return "Null";
                string result = NumberToWords(System.Convert.ToInt32(value));
                return result.Substring(0, 1).ToUpper() + result.Substring(1);
            }
            catch (Exception)
            {
                return "Invalid Number or number too large";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return NumberToWords(System.Convert.ToInt32(value));
        }


        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }


    }


}
