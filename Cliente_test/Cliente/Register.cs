using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Version_1
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            string user = userBox.Text;
            string address = addressBox.Text;
            string password = passwordBox.Text;
            string complete = "1/" + user + "/" + address + "/" + password;


            MessageBox.Show(complete);
        }
    }
}
