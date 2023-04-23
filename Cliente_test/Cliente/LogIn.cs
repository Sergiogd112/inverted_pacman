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
        delegate void Del_ParaConectados(string[] mensaje, int codigo);
        delegate void Del_ParaEmpezarPartida(string[] mensaje, int codigo);
        delegate void Del_ParaFinalizarPartida(string[] mensaje, int codigo);
        delegate void DelegadoParaEnviarChatMain(string autor, string mensaje);
        public string usuario;
        public int num_conn;
        bool Conectado = false;
        bool Logeado = false;
        bool Consultas = false;
        string ip = "192.168.56.102";
        //string ip = "147.83.117.22";
        int puerto = 50053;
        public string mensaje_chat;
        public string chat_autor;
        string juego = "Inverted-Pacman";
        int startGame;
        List<string> Conectados = new List<string>();
        public LogIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.BackgroundImage=Properties.Resources.distorsionada;
            //this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            //Custom main background
            //this.BackColor = Color.FromArgb(244, 164, 96);
            //0,89,76 green
            //42,42,46 grey
            this.ForeColor = Color.Black;
            // Custom title
            //Custom LogIn
            //Custom buttons
            //conn.BackColor = Color.FromArgb(0,50,137);
            //desconn.BackColor = Color.FromArgb(0,50,137);
            //jugar.BackColor = Color.FromArgb(0, 50, 137);
            acceptButton.BackColor = Color.FromArgb(135, 206, 235);
            registerButton.BackColor = Color.FromArgb(135, 206, 235);
            Enviar_Consulta.BackColor = Color.FromArgb(135, 206, 235);
            //conn.FlatAppearance.BorderSize = 0;
            Desconectar_Btn.FlatAppearance.BorderSize = 0;
            //jugar.FlatAppearance.BorderSize = 0;
            dataGridView1.BackgroundColor = Color.FromArgb(135, 206, 235);
            passwordBox.UseSystemPasswordChar = true;


            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse(ip);
            IPEndPoint ipep = new IPEndPoint(direc, puerto);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                connect_status.BackColor = Color.Green;
                connect_status.Text = "Conectado";
                connect_status.ForeColor = Color.Black;
                Conectado = true;
                loading_text.Text = "Connexión establecida\nComprobando credenciales";

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                loading_text.Text = "No he podido conectar con el servidor" + ex.ErrorCode;

                return;
            }
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
        }

        public void Desconectar()
        {
            connect_status.BackColor = default(Color);
            num_usuarios.Text = "No disponible";
            //dataGridView1.Columns[0].HeaderText = "Usuarios connectados";
            connect_status.Text = "Desconectado";
            connect_status.BackColor = Color.Red;
            loading_text.Text = "";
            userBox.Clear();
            passwordBox.Clear();
            dataGridView1.Rows.Clear();
            Conectado = false;
            Logeado = false;
        }

        public void ActualizarConectados(string[] mensaje)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 1;
            int i;
            for (i = 0; i < num_conn; i++)
            {
                if (mensaje[i] != usuario)
                    dataGridView1.Rows.Add(mensaje[i]);
            }
        }

        public void PonerEnGrid(string[] mensaje, int hack)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 1;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(135, 206, 235);
            dataGridView1.GridColor = Color.FromArgb(8, 20, 50);
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.EnableHeadersVisualStyles = false;
            //dataGridView1.Columns[0].HeaderText = "Usuarios connectados "+Convert.ToString(hack);
            //num_usuarios.Text = Convert.ToString(hack);
            //dataGridView1.Columns[0].HeaderCell.Style.BackColor = Color.FromArgb(62, 120, 138);
            //dataGridView1.Columns[0].HeaderCell.Style.ForeColor = Color.FromArgb(41, 44, 51);
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(135, 206, 235);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(135, 206, 235);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 206, 235);
            dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Green;

            loading_text.Text = "Bienvenido de nuevo " + mensaje[hack - 1];

            int i;
            num_conn = hack;
            ActualizarConectados(mensaje);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        public void anadir_Conectados(string[] mensaje, int hack)
        {
            int i;
            for (i = 0; i < hack; i++)
            {
                Conectados.Add(mensaje[i]);
            }
        }
        public List<string> GetConectados()
        {
            return Conectados;
        }
        private void AtenderServidor()   //ACABAR DE REVISAR
        {
            while (true)
            {
                // Recibimos mensaje del servidor
                byte[] msg2 = new byte[100];
                server.Receive(msg2);
                Consultas = true;
                string[] error_servidor = Encoding.ASCII.GetString(msg2).Split('\x04');
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
                    MessageBox.Show(error_servidor[0]);
                    int codigo = Convert.ToInt32(trozos[0]);
                    switch (codigo)
                    {
                        case 0: //Resupesta a la desconexión
                            string mensaje1 = trozos[2];
                            int hack = Convert.ToInt32(trozos[1]);
                            if (hack == 1)
                            {
                                loading_text.Text = (mensaje1);
                            }
                            else
                            {
                                Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                passwordBox.Invoke(delegado, new object[] { });
                                server.Shutdown(SocketShutdown.Both);
                                server.Close();
                                atender.Abort();

                            }

                            break;

                        case 1: //Respuesta al registrar
                            int hackS = Convert.ToInt32(trozos[1]);
                            if (hackS == 1)
                            {
                                MessageBox.Show("Todo bien");
                                Logeado = true;
                            }
                            else
                            {
                                MessageBox.Show("Todo mal");
                                Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                passwordBox.Invoke(delegado, new object[] { });
                                server.Shutdown(SocketShutdown.Both);
                                server.Close();
                                atender.Abort();
                            }

                            break;

                        case 2: //Respuesta al iniciar sesión
                            int hack1 = Convert.ToInt32(trozos[1]);
                            if (hack1 == 1)
                            {
                                MessageBox.Show("Todo bien");
                                Logeado = true;
                            }
                            else
                            {
                                MessageBox.Show("Todo mal");
                                Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                passwordBox.Invoke(delegado, new object[] { });
                                server.Shutdown(SocketShutdown.Both);
                                server.Close();
                                atender.Abort();
                            }
                            break;
                        case 3: //Respuesta a la consulta 1
                            string mensaje3 = trozos[1];
                            MessageBox.Show(mensaje3);
                            break;
                        case 4: //Respuesta a la consulta 2
                            string mensaje4 = trozos[1];
                            MessageBox.Show(mensaje4);
                            break;
                        case 5: //Respuesta a la consulta 3
                            string mensaje5 = trozos[1];
                            MessageBox.Show(mensaje5);
                            break;
                        case 6: //notificacion con la lista de conectados actualizada
                            int hack6 = Convert.ToInt32(trozos[1]);
                            if (hack6 == 0)
                                MessageBox.Show("No hay usuarios connectados");
                            else
                            {
                                string[] mensaje6 = new string[hack6];
                                for (int i = 0; i < hack6; i++)
                                {
                                    mensaje6[i] = (trozos[i + 2]);

                                }

                                Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                                dataGridView1.Invoke(delegado, new object[] { mensaje6, hack6 });
                                this.Invoke(delegado, new object[] { mensaje6, hack6 });
                            }
                            break;
                        //case 7:
                        //    int hack7 = Convert.ToInt32(trozos[1]);
                        //    int partida = Convert.ToInt32(trozos[3]);
                            
                           
                        //    if (hack7 == 0)// respuesta para el anfitrion
                        //    {
                        //        Del_ParaEmpezarPartida delegado7 = new Del_ParaEmpezarPartida(juego, IniciarPartida);
                        //        this.Invoke(delegado7, new object[] { hack7, partida });
                        //        int num_invitados = Convert.ToInt32(trozos[4]);

                        //        //MessageBox.Show("Los jugadores a los que se ha invitado a la partida " + partida);
                        //        string[] invitados = new string[num_invitados];
                        //        for (int i = 0; i < num_invitados; i++)
                        //        {
                        //            invitados[i] = (trozos[i + 5].Split('\0')[0]);
                        //            //MessageBox.Show(invitados[i]);

                        //        }

                        //    }

                        //    else //respuesta para el invitado
                        //    {
                        //        string anfitrion = trozos[4];
                        //        string MessageBoxTitle = "Invitación de partida";
                        //        string MessageBoxContent = "Hola " + userBox.Text + " " + anfitrion + " te ha retado a una partida de" + juego + "\nDeseas aceptarla?";
                        //        string respuesta_invitation;
                        //        DialogResult result = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
                        //        switch (result)
                        //        {
                        //            case DialogResult.Yes:
                        //                //MessageBox.Show("La partida se iniciará en unos instantes");
                        //                respuesta_invitation = "8/" + partida + "/" + anfitrion + "/" + userBox.Text + "/" + Convert.ToString(result) + "/1";
                        //                byte[] accept = System.Text.Encoding.ASCII.GetBytes(respuesta_invitation);
                        //                server.Send(accept);
                        //                break;
                        //            case DialogResult.No:
                        //                //MessageBox.Show("Juegas o eres un llorón?");
                        //                respuesta_invitation = "8/" + partida + "/" + anfitrion + "/" + userBox.Text + "/" + Convert.ToString(result) + "/0";
                        //                byte[] decline = System.Text.Encoding.ASCII.GetBytes(respuesta_invitation);
                        //                server.Send(decline);
                        //                break;
                        //        }

                        //    }
                        //    break;


                        //case 8:
                        //    int hack8 = Convert.ToInt32(trozos[1]);
                        //    int partida8 = Convert.ToInt32(trozos[3]);
                        //    int startGame  = 0;

                        //    if (hack8 == 0)// respuesta para invitado
                        //    {

                        //        MessageBox.Show("La partida " + partida8 + "de Inverted-Pacman se canceló");
                        //    }
                        //    else if (hack8 == 1)//respuesta para anfitrion de los distintos invitados
                        //    {
                        //        string invitado = trozos[4];
                        //        string respuesta_inv = trozos[5];
                        //        MessageBox.Show("Respuesta del invitado " + invitado + "a la partida de Inverted-Pacman " + partida8 + "es " + respuesta_inv);
                        //        if (respuesta_inv == "Yes")
                        //        {
                        //            string partidaStart = "10/" + invitado + "/" + partida8;
                        //            startGame = startGame + 1;

                        //            byte[] gameStart = System.Text.Encoding.ASCII.GetBytes(partidaStart);
                        //            server.Send(gameStart);
                        //            /* if (startGame == 1)
                        //             {
                        //                 DelegadoParaEmpezarPartida delegado8 = new DelegadoParaEmpezarPartida(IniciarPartida);
                        //                 this.Invoke(delegado8, new object[] {hack8, partida8 });
                        //             }
                        //             */

                        //        }



                        //    }
                        //    else if (hack8 == 2)// respuesta para anfitrion para cancelar la partida
                        //    {
                        //        MessageBox.Show("La partida " + partida8 + "del juego " + juego + " se canceló");
                        //        Del_ParaFinalizarPartida delegado8 = new Del_ParaFinalizarPartida(FinalizarPartida);
                        //        this.Invoke(delegado8, new object[] { hack8, partida8 });
                        //    }
                        //    else if (hack8 == -1)//respuesta para el invitado si el anfitrion se desconnecta
                        //    {
                        //        MessageBox.Show("El anfitrion se desconecto");
                        //    }



                        //    break;

                        //case 9:
                        //    chat_autor = trozos[1];
                        //    mensaje_chat = trozos[2];

                        //    DelegadoParaEnviarChatMain delegado9 = new DelegadoParaEnviarChatMain(EnviarChat);
                        //    this.Invoke(delegado9, new object[] { chat_autor, mensaje_chat });

                        //    break;

                        //case 10:
                        //    int hack10 = Convert.ToInt32(trozos[1]);
                        //    int partida10 = Convert.ToInt32(trozos[2]);
                        //    Del_ParaEmpezarPartida delegado10 = new Del_ParaEmpezarPartida(IniciarPartida);
                        //    this.Invoke(delegado10, new object[] { hack10, partida10 });
                        //    break;
                    }

                }
                Consultas = false;
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
                    }
                    else
                    {
                        MessageBox.Show("Rellene nombre de usuario y contraseña por favor");
                    }
                }
                else
                    MessageBox.Show("Sesión ya iniciada");
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                if (!Logeado)
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

        //private void Conectar_Click(object sender, EventArgs e)
        //{
        //    if (!Conectado_Click)
        //    {
        //        //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
        //        //al que deseamos conectarnos
        //        IPAddress direc = IPAddress.Parse(ipBox.Text);
        //        IPEndPoint ipep = new IPEndPoint(direc, Convert.ToInt32(portBox.Text));


        //        //Creamos el socket 
        //        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        try
        //        {
        //            server.Connect(ipep);//Intentamos conectar el socket
        //            this.BackColor = Color.Green;
        //            MessageBox.Show("Conectado");
        //            Conectado = true;

        //        }
        //        catch (SocketException ex)
        //        {
        //            //Si hay excepcion imprimimos error y salimos del programa con return 
        //            MessageBox.Show("No he podido conectar con el servidor");
        //            return;
        //        }
        //        Conectado_Click = true;
        //    }
        //    else
        //        MessageBox.Show("Conexión ya establecida");


        //    //pongo en marcha el thread que atendera los mensajes del servidor
        //    //ThreadStart ts = delegate { AtenderServidor(); };
        //    //atender = new Thread(ts);
        //    //atender.Start();

        //}

        private void Enviar_Consulta_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                if (Logeado & !Consultas)
                {
                    if (Consulta1.Checked)  //La consulta 1 es la función que devuelve el ranking
                    {
                        string mensaje = "3/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        byte[] msg2 = new byte[1000];
                        server.Send(msg);
                        //server.Receive(msg2);                        //Recibimos la respuesta del servidor
                        //string[] respuesta = Encoding.ASCII.GetString(msg2).Split('\0')[0].Split('/');
                        //int n = Convert.ToInt32(respuesta[0]);
                        //string[] filas = respuesta[1].Split(',');
                        //string[] celdas;
                        //Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                        //dataGridView1.Columns.Clear();
                        //dataGridView1.Rows.Clear();
                        //dataGridView1.Columns.Add("Pos", "Posición");
                        //dataGridView1.Columns.Add("usuario", "Usuario");
                        //dataGridView1.Columns.Add("pts", "Puntos");
                        //connlbl.Text = mensaje;
                        //for (int i = 0; i < n; i++)
                        //{
                        //    celdas = (filas[i].Split('*'));
                        //    dataGridView1.Rows.Add( Convert.ToString(i), celdas[0], celdas[1]);

                        //}

                    }

                    else if (Consulta2.Checked)  //La consulta 2 es la función que devuelve los jugadores online
                    {
                        string mensaje = "4/";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        //byte[] msg2 = new byte[100];
                        //server.Send(msg);
                        //server.Receive(msg2);           //Recibimos la respuesta del servidor
                        //string[] respuesta = Encoding.ASCII.GetString(msg2).Split('\0')[0].Split('/');
                        //int n = Convert.ToInt32(respuesta[0]);
                        //string[] filas = respuesta[1].Split(',');
                        //string[] celdas;
                        //Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                        //dataGridView1.Columns.Clear();
                        //dataGridView1.Rows.Clear();
                        //dataGridView1.Columns.Add("usuarios", "usuarios");
                        //connlbl.Text = mensaje;
                        //for (int i = 0; i < n; i++)
                        //{
                        //    celdas = (filas[i].Split('*'));
                        //    dataGridView1.Rows.Add(Convert.ToString(n), celdas[0], celdas[1]);

                        //}
                    }
                    else if (Consulta3.Checked)  //La consulta 3 es la función que crea una partida
                    {
                        string mensaje = "5/1*2*3*4";
                        // Enviamos al servidor el nombre tecleado
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        byte[] msg2 = new byte[100];
                        server.Send(msg);
                        //server.Receive(msg2);    //Recibimos la respuesta del servidor
                        //string respuesta = Encoding.ASCII.GetString(msg2);
                        //if(respuesta == "error")
                        //{
                        //    MessageBox.Show("Ha habido un error");
                        //}
                        //else
                        //{
                        //    int n = Convert.ToInt32(respuesta);
                        //    if (n == 1)
                        //    {
                        //        MessageBox.Show("Se ha creado la partida con éxito");
                        //    }
                        //    if (n == 0)
                        //    {
                        //        MessageBox.Show("No se ha podido crear la partida");
                        //    }

                        //}

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
            if (Logeado)
            {
                string mensaje = "0/";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                atender.Abort();
                server.Send(msg);
                connect_status.BackColor = default(Color);
                num_usuarios.Text = "No disponible";
                //dataGridView1.Columns[0].HeaderText = "Usuarios connectados";
                connect_status.Text = "Desconectado";
                connect_status.BackColor = Color.Red;
                loading_text.Text = "";
                userBox.Clear();
                passwordBox.Clear();
                dataGridView1.Rows.Clear();
                Conectado = false;
                Logeado = false;
            }
            else
                MessageBox.Show("No hay ninguna sesión iniciada");

        }

        private void Close_Btn_Click(object sender, EventArgs e)
        {
            if (Conectado)
            {
                MessageBox.Show("Por favor asegurese de desconectarse del servidor antes de cerrar la ventana");
            }
            else
            {
                atender.Abort();
                this.Close();
            }
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Conectado)
            {
                MessageBox.Show("Por favor asegurese de desconectarse del servidor antes de cerrar la ventana");
                e.Cancel = true;
            }
            else
                atender.Abort();
        }
    }
}
