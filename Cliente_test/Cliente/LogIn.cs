using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace Version_1
{
    public partial class LogIn : Form
    {
        Socket server;
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

            IPAddress address = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(address, 9050);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

                string user = userBox.Text;
                string password = passwordBox.Text;
                string message = "2/" + user + "/" + password;

                byte[] msg = Encoding.ASCII.GetBytes(message);
                server.Send(msg);
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                message = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show(message);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("No me he podido conectar");
                return;
            }







        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            Register form = new Register(); // Crear una instancia del nuevo formulario
            form.Show(); // Mostrar el nuevo formulario

        }
    }
}
