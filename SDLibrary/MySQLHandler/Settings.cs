using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDesk.MySQLHandler
{
    class Settings
    {
        Dictionary<string,string> dic = new Dictionary<string,string>();
        public static Settings Default = new Settings();

        public Settings()
        {
            string sql = "SELECT `key`,`value` FROM `settings`;";
            if(Onion.MySQLHandler.MySQLHelper.executeRead(sql))
            {
                while (Onion.MySQLHandler.MySQLHelper.data_reader.Read())
                {
                    dic.Add(Onion.MySQLHandler.MySQLHelper.data_reader["key"].ToString(), Onion.MySQLHandler.MySQLHelper.data_reader["value"].ToString());
                }
     
            }
        }
        public String this[String key]
        {
            get
            {
                if (dic.ContainsKey(key))
                    return dic[key];
                else return "";
               
            }
            set
            {
                if (dic.ContainsKey(key))
                dic[key] = value;
            }
        }
        public bool saveSettings()
        {
            string[] keys=dic.Keys.ToArray();
             string sql = "";
            foreach (string s in dic.Keys )
            {
                dic.Keys.ToList();
                sql += "UPDATE `settings` SET `value`='" + dic[s] + "' WHERE `key`='" + s + "';";
            }

            return Onion.MySQLHandler.MySQLHelper.executeWrite(sql);

        }

    }
}
