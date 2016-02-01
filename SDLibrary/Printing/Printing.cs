using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Printing;
using CloneExtensions;

namespace SmartDesk.Printing
{
    public class Printing
    {
        static PrintDialog printDialog;
        
        public static void PrintGrid(DataGrid param,string DocTitle)
        {
            printDialog= new PrintDialog();
            if (printDialog.ShowDialog() == false)
                return;

            string documentTitle = DocTitle;
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            CustomDataGridDocumentPaginator paginator = new CustomDataGridDocumentPaginator(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
      
            printDialog.PrintDocument(paginator, "Grid");
        }
        public static void PrintVisualWithoutDialog(Grid g)
        {
            FrameworkElement e = g as FrameworkElement;
            Transform originalScale = e.LayoutTransform;   
            printDialog = new PrintDialog();
            //get selected printer capabilities
            PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
            g.Height = Convert.ToDouble("29.7cm");
            MessageBox.Show(g.Height+"");
            //get scale of the print wrt to screen of WPF visual
            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / g.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
            g.ActualHeight);
            //Transform the Visual to scale
            g.LayoutTransform = new ScaleTransform(scale, scale);
            //get the size of the printer page

            Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            //update the layout of the visual to the printer page size.

            g.Measure(sz);
            g.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            //now print the visual to printer to fit on the one page.
           
            printDialog.PrintVisual(g, "Onion Smart Solutions");

            e.LayoutTransform = originalScale;
        }
        public static void PrintVisual(Grid g)
        {
            printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == false)
                return;
            FrameworkElement e = g as FrameworkElement;
            Transform originalScale = e.LayoutTransform;   
           
            //get selected printer capabilities
            PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
            //get scale of the print wrt to screen of WPF visual
          
            double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / g.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
            g.ActualHeight);
            //Transform the Visual to scale
            g.LayoutTransform = new ScaleTransform(scale, scale);
            //get the size of the printer page

            Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            //update the layout of the visual to the printer page size.

            g.Measure(sz);
            g.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            //now print the visual to printer to fit on the one page.
           
            printDialog.PrintVisual(g, "Onion Smart Solutions");

            e.LayoutTransform = originalScale;
        }

    }
}
