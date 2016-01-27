using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onion.MySQLHandler;

namespace TeachersDesk.MySQLHandler
{

    public class AllObjectIDs
    {
        public static UFIHandler PaymentMethodUFIHandler { get { return payment_method_UFIHandler; }}
        private static UFIHandler payment_method_UFIHandler = new UFIHandler("`payment_method`", "`payment_method`.`name`", "`allowed`=1");

        public static UFIHandler BankUFIHandler { get { return bank_UFIHandler; }}
        private static UFIHandler bank_UFIHandler = new UFIHandler("`bank`", "`bank`.`name`", "`visible`=1");
        public static UFIHandler ReceiptUFIHandler { get { return receipt_UFIHandler; } }
        private static UFIHandler receipt_UFIHandler = new UFIHandler("`receipt`", "`receipt`.`receipt_no`", null);
    }
}
