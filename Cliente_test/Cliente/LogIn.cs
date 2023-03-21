using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Version_1
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //No hace nada, dejar vacío
        }

        private void acceptButton_Click(object sender, EventArgs e) //Al apretar el botón accept de Log In
        {
            string user = userBox.Text;
            string password = passwordBox.Text;
            string complete = "2/" + user + "/" + password;


            MessageBox.Show(complete);
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            Register form = new Register(); // Crear una instancia del nuevo formulario
            form.Show(); // Mostrar el nuevo formulario

        }
    }
}
