using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion
{
    class Floats
    {
        public static Floats Default=new Floats();
        string code;
        bool activated=false;
        public bool Activated
        {
            get { return activated; }
        }
        public Floats()
        {
            string sql = "SELECT `4C557F74BAC70A6F7CEE9A9A0` FROM `temp` WHERE `0109B24394D99FFC95133EEEA`='BDFAB225396127A85A6C6D85D'";
            code = Onion.MySQLHandler.MySQLHelper.getOneValue(sql).ToString();
            activated = isOk(this.code); ;
        }
        private bool isOk(string code)
        {
            return (Numbers.NumberBuilder.isInterger(SmartDesk.MySQLHandler.Settings.Default["school_name"], code));
        }
        public bool Activate()
        {
            if (isOk(code))
            {
                activated = true;
                return true;
            }
            else
            {
                activated = false;
                return false;
            }
        }
        public bool updateCode(string code)
        {
            System.Threading.Thread.Sleep(5000);
            if (isOk(code))
            {
                string sql = "UPDATE `temp` SET `4C557F74BAC70A6F7CEE9A9A0`='" + code + "'";
                if(MySQLHandler.MySQLHelper.executeWrite(sql))
                {
                    System.Windows.MessageBox.Show("The key was entered successfully.");
                    activated = true;
                    return true;
                }
                return false;
            }
            else
            {
                System.Windows.MessageBox.Show("The key you entered is not correct.");
                return false;
            }
        }
    }
}
