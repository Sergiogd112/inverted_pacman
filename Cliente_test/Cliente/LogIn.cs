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
        Thread atender;
        delegate void Del_ParaGrid(string[] mensaje, int codigo);
        delegate void Del_ParaDesconectar();
        bool Conectado = false;
        bool Conectado_Click = false;
        bool Logeado = false;
        bool changeALLOW = false;
        public LogIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //No hace nada, dejar vacío
        }

        public void Desconectar()
        {
            if (changeALLOW)
            {

                this.BackColor = Color.Gray;
                userBox.Clear();
                passwordBox.Clear();
                connlbl.Text = " ";
                dataGridView1.Rows.Clear();
                changeALLOW = false;
            }


        }

        public void PonerEnGrid(string[] mensaje, int hack)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 1;
            dataGridView1.ColumnHeadersVisible = true;
            string c = Convert.ToString(hack);
            dataGridView1.Rows.Add("Hay " + c + " usuario/s conectado/s");
            int i;
            for (i = 0; i < hack; i++)
            {
                dataGridView1.Rows.Add(mensaje[i]);
            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void AtenderServidor()
        {
            while (true)
            {
                // Recibimos mensaje del servidor
                byte[] msg2 = new byte[100];
                server.Receive(msg2);
                string[] error_servidor = Encoding.ASCII.GetString(msg2).Split('\0');
                if (error_servidor[0] == "")
                {
                    MessageBox.Show("Servidor en tareas de mantenimiento, vuelva a conectarse más tarde");
                    Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                    passwordBox.Invoke(delegado, new object[] { });
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    atender.Abort();
                }

                else
                {
                    string[] trozos = error_servidor[0].Split('/');
                    int codigo = Convert.ToInt32(trozos[0]);
                    switch (codigo)
                    {
                        case 0: //Resupesta a la desconexión
                            string mensaje1 = trozos[2].Split('\0')[0];
                            int hack = Convert.ToInt32(trozos[1]);
                            if (hack == 1)
                            {
                                MessageBox.Show(mensaje1);
                            }
                            else
                            {
                                MessageBox.Show(mensaje1);
                                Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                passwordBox.Invoke(delegado, new object[] { });
                                server.Shutdown(SocketShutdown.Both);
                                server.Close();
                                atender.Abort();

                            }

                            break;

                        case 1: //Respuesta al registrar
                            string mensaje = trozos[1].Split('\0')[0];
                            MessageBox.Show(mensaje);
                            break;
                        case 2: //Respuesta al iniciar sesión
                            string mensaje2 = trozos[2].Split('\0')[0];
                            int hack1 = Convert.ToInt32(trozos[1]);
                            if (hack1 == 0)
                            {
                                MessageBox.Show(mensaje2);
                                Logeado = true;
                            }
                            else
                            {
                                MessageBox.Show(mensaje2);
                            }
                            break;
                        case 3: //Respuesta a la consulta 1
                            string mensaje3 = trozos[1].Split('\0')[0];
                            MessageBox.Show(mensaje3);
                            break;
                        case 4: //Respuesta a la consulta 2
                            string mensaje4 = trozos[1].Split('\0')[0];
                            MessageBox.Show(mensaje4);
                            break;
                        case 5: //Respuesta a la consulta 3
                            string mensaje5 = trozos[1].Split('\0')[0];
                            MessageBox.Show(mensaje5);
                            break;
                        case 6:
                            int hack6 = Convert.ToInt32(trozos[1]);
                            if (hack6 == 0)
                                MessageBox.Show("No hay usuarios connectados");
                            else
                            {
                                string[] mensaje6 = new string[hack6];
                                for (int i = 0; i < hack6; i++)
                                {
                                    mensaje6[i] = (trozos[i + 2].Split('\0')[0]);

                                }

                                Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                                dataGridView1.Invoke(delegado, new object[] { mensaje6, hack6 });
                            }
                            break;
                    }

                }
            }

        }
        private void acceptButton_Click(object sender, EventArgs e) //Al apretar el botón accept de Log In
        {

            if (Conectado)
            {
                if (!Logeado)
                {
                    if (userBox.Text != "" & passwordBox.Text != "")
                    {
                        string mensaje = "2/" + userBox.Text + "*" + passwordBox.Text;
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                        changeALLOW = true;
                        byte[] msgLog = new byte[100];
                        server.Receive(msgLog);
                        string[] Log = Encoding.ASCII.GetString(msgLog).Split('\0');
                        if (Log[0] == "0")
                        {
                            MessageBox.Show("Contraseña incorrecta");
                        }
                        //Recibimos la respuesta del servidor
                        if (Log[0] == "1")
                        {
                            Logeado = true;
                            Conectado = true;
                            MessageBox.Show("Se ha iniciado sesion");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Rellene nombre de usuario y contraseña por favor");
                    }
                }
                else
                    MessageBox.Show("Sesión ya iniciada");
            }
            else
                MessageBox.Show("Inicie conexión");
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                if (userBox.Text != "" & passwordBox.Text != "" & emailBox.Text != "")
                {
                    string mensaje = "1/" + userBox.Text + "*" + passwordBox.Text + "*" + emailBox.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                {
                    MessageBox.Show("Rellene nombre de usuario y contraseña por favor");
                }
            }
            else
                MessageBox.Show("Inicie conexión");

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

        private void Conectar_Click(object sender, EventArgs e)
        {
            if (!Conectado_Click)
            {
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse(ipBox.Text);
                IPEndPoint ipep = new IPEndPoint(direc, Convert.ToInt32(portBox.Text));


                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket
                    this.BackColor = Color.Green;
                    MessageBox.Show("Conectado");
                    Conectado = true;

                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
                Conectado_Click = true;
            }
            else
                MessageBox.Show("Conexión ya establecida");


            //pongo en marcha el thread que atendera los mensajes del servidor
            //ThreadStart ts = delegate { AtenderServidor(); };
            //atender = new Thread(ts);
            //atender.Start();

        }

        private void Desconectar_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                if (Logeado)
                {
                    string mensaje = "0/" + userBox.Text + "/" + passwordBox.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    Conectado_Click = false;
                    Conectado = false;
                    Logeado = false;
                    changeALLOW = true;
                }
                else
                    MessageBox.Show("No hay ninguna sesión iniciada");

            }
            else
            {
                MessageBox.Show("No hay ninguna conexión no establecida");
            }
        }

        private void Enviar_Consulta_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                if (Logeado)
                {
                    if (Consulta1.Checked)  //La consulta 1 es la función que devuelve el ranking
                    {
                        string mensaje = "3/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        byte[] msg2 = new byte[1000];
                        server.Send(msg);
                        server.Receive(msg2);                        //Recibimos la respuesta del servidor
                        string[] respuesta = Encoding.ASCII.GetString(msg2).Split('\0')[0].Split('/');
                        int n = Convert.ToInt32(respuesta[0]);
                        string[] filas = respuesta[1].Split(',');
                        string[] celdas;
                        Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Add("Pos", "Posición");
                        dataGridView1.Columns.Add("usuario", "Usuario");
                        dataGridView1.Columns.Add("pts", "Puntos");
                        connlbl.Text = mensaje;
                        for (int i = 0; i < n; i++)
                        {
                            celdas = (filas[i].Split('*'));
                            dataGridView1.Rows.Add( Convert.ToString(i), celdas[0], celdas[1]);

                        }

                    }

                    else if (Consulta2.Checked)  //La consulta 2 es la función que devuelve los jugadores online
                    {
                        string mensaje = "4/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        byte[] msg2 = new byte[100];
                        server.Send(msg);
                        server.Receive(msg2);           //Recibimos la respuesta del servidor
                        string[] respuesta = Encoding.ASCII.GetString(msg2).Split('\0')[0].Split('/');
                        int n = Convert.ToInt32(respuesta[0]);
                        string[] filas = respuesta[1].Split(',');
                        string[] celdas;
                        Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Add("usuarios", "usuarios");
                        connlbl.Text = mensaje;
                        for (int i = 0; i < n; i++)
                        {
                            celdas = (filas[i].Split('*'));
                            dataGridView1.Rows.Add(Convert.ToString(n), celdas[0], celdas[1]);

                        }
                    }
                    else if (Consulta3.Checked)  //La consulta 3 es la función que crea una partida
                    {
                        string mensaje = "5/1*2*3*4";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        byte[] msg2 = new byte[100];
                        server.Send(msg);
                        server.Receive(msg2);    //Recibimos la respuesta del servidor
                        string respuesta = Encoding.ASCII.GetString(msg2);
                        if(respuesta == "error")
                        {
                            MessageBox.Show("Ha habido un error");
                        }
                        else
                        {
                            int n = Convert.ToInt32(respuesta);
                            if (n == 1)
                            {
                                MessageBox.Show("Se ha creado la partida con éxito");
                            }
                            if (n == 0)
                            {
                                MessageBox.Show("No se ha podido crear la partida");
                            }
                            
                        }

                    }
                    else
                        MessageBox.Show("Ninguna consulta seleccionada");
                }
                else
                    MessageBox.Show("Inicie sesión");
            }
            else if (!Conectado)

                MessageBox.Show("Inicie conexión");
        }

        private void Desconectar_Btn_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                if (Logeado)
                {
                    string mensaje = "0/";
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    Conectado_Click = false;
                    Conectado = false;
                    Logeado = false;
                    changeALLOW = true;
                }
                else
                    MessageBox.Show("No hay ninguna sesión iniciada");

            }
            else
            {
                MessageBox.Show("No hay ninguna conexión no establecida");
            }
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Conectado)
            {
                MessageBox.Show("Por favor asegurese de desconectarse del servidor antes de cerrar la ventana");
                e.Cancel = true;
            }
        }
    }
}
