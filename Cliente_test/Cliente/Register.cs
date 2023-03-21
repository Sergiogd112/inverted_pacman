using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Version_1
{
    public partial class Register : Form
    {
        Socket server;
        public Register()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, EventArgs e)
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
                string email = addressBox.Text;
                string password = passwordBox.Text;
                string message = "1/" + user + "/" + email + "/" + password;

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
    }
}
