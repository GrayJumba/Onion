using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Onion.MySQLHandler
{
    public partial class DBCredentialsForm : Form
    {
        public DBCredentialsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string hostname = textBox3.Text;
            string db_name = textBox4.Text;
            if (hostname == "" || hostname == null)
            {
                MessageBox.Show("The host name cannot be empty.");
                return;
            }
            if (db_name == "" || db_name == null)
            {
                MessageBox.Show("The host name cannot be empty.");
                return;
            }
            if(username=="" ||username ==null)
            {
                MessageBox.Show("The user name cannot be empty.");
                return;
            }
            else
            {
                this.Visible = false;
                MySQLHandler.MySQLHelper.ConfirmCredentials(hostname, username, password);
            }
        }
    }
}
