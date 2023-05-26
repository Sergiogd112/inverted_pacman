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
        Socket Server;
        Thread Attend;
        //Creamos una delegate para la desconexión del servidor
        delegate void Del_Disconnect();
        public string User;
        public int Num_conn;
        //Variable boleana para la conexión del servidor
        bool Connect = false;
        //Variable boleana para login en BBDD
        bool LoggedIn = false;
        //Variable boleana para las consultas al servidor
        bool Queries = false;
        string Ip = "147.83.117.22";
        int Port = 50053;
        public bool New_Connected_List = false;
        string[] Users;
        List<string> Connected = new List<string>();
        public LogIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ForeColor = Color.Black;
            acceptButton.BackColor = Color.FromArgb(135, 206, 235);
            registerButton.BackColor = Color.FromArgb(135, 206, 235);
            Enviar_Consulta.BackColor = Color.FromArgb(135, 206, 235);
            Desconectar_Btn.FlatAppearance.BorderSize = 0;
            dataGridView1.BackgroundColor = Color.FromArgb(135, 206, 235);
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Invitar";
            checkColumn.HeaderText = "Invitar";
            checkColumn.Width = 50;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10;
            dataGridView1.Columns.Add(checkColumn);
            dataGridView1.Columns.Add("Nombre", "Nombre");

            passwordBox.UseSystemPasswordChar = true;


            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse(Ip);
            IPEndPoint ipep = new IPEndPoint(direc, Port);
            dataGridView1.ColumnCount = 2;


            //Creamos el socket 
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Intentamos conectar el socket
                Server.Connect(ipep);
                connect_status.BackColor = Color.Green;
                connect_status.Text = "Connected";
                connect_status.ForeColor = Color.Black;
                Connect = true;
                loading_text.Text = "Connection established\nChecking credentials";

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                loading_text.Text = "I couldn't connect to the server" + ex.ErrorCode;

                return;
            }
            ThreadStart ts = delegate { AttendServer(); };
            Attend = new Thread(ts);
            Attend.Start();

        }


        public void Disconnect()
        {
            connect_status.BackColor = default(Color);
            num_usuarios.Text = "Not available";
            connect_status.Text = "Disconnected";
            connect_status.BackColor = Color.Red;
            loading_text.Text = "";
            userBox.Clear();
            passwordBox.Clear();
            dataGridView1.Rows.Clear();
            Connect = false;
            LoggedIn = false;
        }
        /// <summary>
        /// Añade una fila a la tabla de conectados
        /// </summary>
        /// <param name="row"></param>
        private void AddRow(string s)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action<string>(AddRow), new object[] { s });
                return;
            }

            dataGridView1.Rows.Add(false,s);
        }
        /// <summary>
        /// Vacia la tabla de conectados
        /// </summary>
        private void ClearGrid()
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(ClearGrid));
                return;
            }

            dataGridView1.Rows.Clear();
        }

        /// <summary>
        /// Actualiza en la tabla de conectados
        /// </summary>
        /// <param name="message"></param>
        public void UpdateConnected(string[] message)
        {
            string[] row= new string[1];
            ClearGrid();
            foreach (string s in message)
            {
                AddRow(s);
            }            
        }
        /// <summary>
        /// Atiende todas las respuestas del servidor
        /// </summary>
        private void AttendServer()   
        {
            while (true)
            {
                // Recibe mensaje del servidor
                byte[] msg2 = new byte[10000000];
                Server.Receive(msg2);
                Queries = true;
                //MessageBox.Show(Encoding.ASCII.GetString(msg2));
                string[] Server_Error = Encoding.ASCII.GetString(msg2).Split('\x04');
                if (Server_Error[0] == "")
                {
                    //En caso de error cierra el socket
                    MessageBox.Show("Not available, please connect again later");
                    Del_Disconnect delegateDst = new Del_Disconnect(Disconnect);
                    passwordBox.Invoke(delegateDst, new object[] { });
                    Server.Shutdown(SocketShutdown.Both);
                    Server.Close();
                    Attend.Abort();
                }

                else
                {
                    string[] trozos = Server_Error[0].Split('/');
                    int code = Convert.ToInt32(trozos[0]);
                    switch (code)
                    {
                        case 1: 
                            //Respuesta al registrar
                            int num1 = Convert.ToInt32(trozos[1]);
                            if (num1 == 1)
                            {
                                MessageBox.Show("OK");
                                LoggedIn = true;
                            }
                            else
                            {
                                MessageBox.Show("Not OK");
                                Del_Disconnect delegateDst = new Del_Disconnect(Disconnect);
                                passwordBox.Invoke(delegateDst, new object[] { });
                                Server.Shutdown(SocketShutdown.Both);
                                Server.Close();
                                Attend.Abort();
                            }

                            break;

                        case 2: 
                            //Respuesta al iniciar sesión
                            int num2 = Convert.ToInt32(trozos[1]);
                            if (num2 == 1)
                            {
                                MessageBox.Show("OK");
                                LoggedIn = true;
                            }
                            else
                            {
                                MessageBox.Show("Not OK");
                                Del_Disconnect delegateDst = new Del_Disconnect(Disconnect);
                                passwordBox.Invoke(delegateDst, new object[] { });
                                Server.Shutdown(SocketShutdown.Both);
                                Server.Close();
                                Attend.Abort();
                            }
                            break;
                        case 3: 
                            //Respuesta a la consulta Ranking
                            string message3 = trozos[1];
                            MessageBox.Show(message3);
                            break;
                        case 4: 
                            //Respuesta a la consulta Conectados
                            this.New_Connected_List = true;
                            Users = trozos[2].Split(',');
                            UpdateConnected(Users);
                            break;
                        case 5: 
                            //Respuesta crear partida
                            int num5 = Convert.ToInt32(trozos[1]);
                            if (num5 == 0)
                            {
                                MessageBox.Show("Failed to invite user");
                            }

                            if (num5 == 1)
                            {
                                int id_partida = Convert.ToInt32(trozos[2]);
                                MessageBox.Show("User has been invited");
                            }

                            break;
                        case 6: 
                            //Notificación invitación partida
                            int num6 = Convert.ToInt32(trozos[1]);
                            string creator_name = Convert.ToString(trozos[2]);
                            string message6 = "You have been invited to the game created by" + creator_name + ", would you like to join?";
                            string caption = "Game invitation";
                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                            DialogResult result;

                            result = MessageBox.Show(message6, caption, buttons);
                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {
                                //Acepta la invitación
                                string message = "6/" +trozos[1]+ "/1" ;
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                                Server.Send(msg);
                            }
                            else
                            {
                                //Rechaza invitación
                                string message = "6/" + trozos[1] + "/0";
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                                Server.Send(msg);
                            }
                            break;
                        default:
                            MessageBox.Show(Server_Error[0]);
                            break;
                    }

                }
                Queries = false;
            }

        }
        private void acceptButton_Click(object sender, EventArgs e) //Al apretar el botón accept de Log In
        {
            if (Connect)
            {
                if (!LoggedIn)
                {
                    if (userBox.Text != "" & passwordBox.Text != "")
                    {

                        string message = "2/" + userBox.Text + "*" + passwordBox.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                        Server.Send(msg);
                    }
                    else
                    {
                        MessageBox.Show("Fill in username and password please");
                    }
                }
                else
                    MessageBox.Show("Session already started");
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (Connect)
            {
                if (!LoggedIn)
                {
                    if (userBox.Text != "" & passwordBox.Text != "" & emailBox.Text != "")
                    {

                        string mensaje = "1/" + userBox.Text + "*" + passwordBox.Text + "*" + emailBox.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        Server.Send(msg);
                    }
                    else
                    {
                        MessageBox.Show("Fill in username and password please");
                    }
                }
                else
                    MessageBox.Show("Start Connection");
            }
        }

        private void Mostrar_Contraseña_CheckedChanged(object sender, EventArgs e)
        {
            string text = passwordBox.Text;
            if (Mostrar_Contraseña.Checked)
            {
                passwordBox.UseSystemPasswordChar = false;
                passwordBox.Text = text;

            }
            else
            {
                passwordBox.UseSystemPasswordChar = true;
                passwordBox.Text = text;
            }
        }


        private void Enviar_Consulta_Click(object sender, EventArgs e)
        {
            if (Connect)
            {
                if (LoggedIn & !Queries)
                {
                    if (Consulta1.Checked)  //La consulta 1 es la función que devuelve el ranking
                    {
                        string message = "3/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                        byte[] msg2 = new byte[1000];
                        Server.Send(msg);
                    }

                    else if (Consulta2.Checked)  //La consulta 2 es la función que devuelve los jugadores online
                    {
                        string message = "4/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                    }
                    else if (Consulta3.Checked)  //La consulta 3 es la función que crea una partida
                    {
                        int count = 0;
                        string[] names = new string[3];
                        bool selected= false;
                        for (int i=0;i<dataGridView1.Rows.Count;i++)
                        {
                            if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value))
                            {
                                names[count]= dataGridView1.Rows[i].Cells[1].Value.ToString();
                                count += 1;
                            }
                        }
                        if (count < 3 | count > 3)
                        {
                            MessageBox.Show("selecciona tres nombres");
                        }
                        else
                        {
                            string message = "5/"+string.Join("*",names);
                            // Enviamos al servidor el nombre tecleado
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                            byte[] msg2 = new byte[100];
                            Server.Send(msg);
                        }
                    }
                    else
                        MessageBox.Show("No query selected");
                }
                else
                    MessageBox.Show("Login");
            }
            else if (!Connect)

                MessageBox.Show("Start Connection");
        }

        private void Desconectar_Btn_Click(object sender, EventArgs e)
        {
            if (LoggedIn)
            {
                string message = "0/";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                Attend.Abort();
                Server.Send(msg);
                connect_status.BackColor = default(Color);
                num_usuarios.Text = "Not available";
                connect_status.Text = "Disconnected";
                connect_status.BackColor = Color.Red;
                loading_text.Text = "";
                userBox.Clear();
                passwordBox.Clear();
                dataGridView1.Rows.Clear();
                Connect = false;
                LoggedIn = false;
            }
            else
                MessageBox.Show("No session started");

        }

        private void Close_Btn_Click(object sender, EventArgs e)
        {
            if (Connect)
            {
                MessageBox.Show("Please make sure to disconnect from the server before closing the window");
            }
            else
            {
                Attend.Abort();
                this.Close();
            }
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Connect)
            {
                MessageBox.Show("Please make sure to disconnect from the server before closing the window");
                e.Cancel = true;
            }
            else
                try
                {
                    Attend.Abort();
                }
                catch (Exception ex)
                {
                }
        }
    }
}
