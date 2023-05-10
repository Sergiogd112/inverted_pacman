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
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Client cliente;

    private void Start() {
        Debug.Log(this.cliente.ip);
        IPAddress direc = IPAddress.Parse(this.cliente.ip);
        IPEndPoint ipep = new IPEndPoint(direc, this.cliente.puerto);
        Debug.Log("creando socket...");
        this.cliente.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            this.cliente.server.Connect(ipep);//Intentamos conectar el socket
            this.cliente.Conectado = true;
            Debug.Log("Conectado");
        }
        catch (SocketException ex)
        {
            //Si hay excepcion imprimimos error y salimos del programa con return 
            Debug.LogError(ex.ToString());
            return;
        }
        ThreadStart ts = delegate { AtenderServidor(); };
        this.cliente.atender = new Thread(ts);
        this.cliente.atender.Start();
    }
    private void AtenderServidor()   //ACABAR DE REVISAR
        {
            Debug.Log("AtenderServidor en marcha");
            while (true)
            {
                // Recibimos mensaje del servidor
                byte[] msg2 = new byte[100];
                this.cliente.server.Receive(msg2);
                this.cliente.Consultas = true;
                string[] error_servidor = Encoding.ASCII.GetString(msg2).Split('\x04');
                if (error_servidor[0] == "")
                {
                    Debug.Log("Servidor en tareas de mantenimiento, vuelva a conectarse más tarde");
                    // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                    this.cliente.server.Shutdown(SocketShutdown.Both);
                    this.cliente.server.Close();
                    this.cliente.atender.Abort();
                }

                else
                {
                    string[] trozos = error_servidor[0].Split('/');
                    Debug.Log(error_servidor[0]);
                    int codigo = Convert.ToInt32(trozos[0]);
                    switch (codigo)
                    {
                        case 0: //Resupesta a la desconexión
                            string mensaje1 = trozos[2];
                            int hack = Convert.ToInt32(trozos[1]);
                            if (hack == 1)
                            {
                                Debug.Log(mensaje1);
                            }
                            else
                            {
                                // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                this.cliente.server.Shutdown(SocketShutdown.Both);
                                this.cliente.server.Close();
                                this.cliente.atender.Abort();

                            }

                            break;

                        case 1: //Respuesta al registrar
                            int hackS = Convert.ToInt32(trozos[1]);
                            if (hackS == 1)
                            {
                                Debug.Log("Todo bien");
                                this.cliente.Logeado = true;
                            }
                            else
                            {
                                Debug.Log("Todo mal");
                                // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                this.cliente.server.Shutdown(SocketShutdown.Both);
                                this.cliente.server.Close();
                                this.cliente.atender.Abort();
                            }

                            break;

                        case 2: //Respuesta al iniciar sesión
                            int hack1 = Convert.ToInt32(trozos[1]);
                            if (hack1 == 1)
                            {
                                Debug.Log("Todo bien");
                                this.cliente.Logeado = true;
                            }
                            else
                            {
                                Debug.Log("Todo mal");
                                // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                                this.cliente.server.Shutdown(SocketShutdown.Both);
                                this.cliente.server.Close();
                                this.cliente.atender.Abort();
                            }
                            break;
                        case 3: //Respuesta a la consulta 1
                            string mensaje3 = trozos[1];
                            Debug.Log(mensaje3);
                            break;
                        case 4: //Respuesta a la consulta 2
                            this.cliente.nueva_connected_list = true;
                            string [] usuarios = trozos[2].Split(',');
                            // ActualizarConectados(usuarios);
                            Debug.Log(trozos[2]);
                            break;
                        case 5: //Respuesta a la consulta 3
                            string mensaje5 = trozos[1];
                            Debug.Log(mensaje5);
                            break;
                        case 6: //notificacion con la lista de conectados actualizada
                            int hack6 = Convert.ToInt32(trozos[1]);
                            if (hack6 == 0)
                                Debug.Log("No hay usuarios connectados");
                            else
                            {
                                string[] mensaje6 = new string[hack6];
                                for (int i = 0; i < hack6; i++)
                                {
                                    mensaje6[i] = (trozos[i + 2]);

                                }

                                // Del_ParaGrid delegado = new Del_ParaGrid(PonerEnGrid);
                                // dataGridView1.Invoke(delegado, new object[] { mensaje6, hack6 });
                                // this.Invoke(delegado, new object[] { mensaje6, hack6 });
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

                            //        //Debug.Log("Los jugadores a los que se ha invitado a la partida " + partida);
                            //        string[] invitados = new string[num_invitados];
                            //        for (int i = 0; i < num_invitados; i++)
                            //        {
                            //            invitados[i] = (trozos[i + 5].Split('\0')[0]);
                            //            //Debug.Log(invitados[i]);

                            //        }

                            //    }

                            //    else //respuesta para el invitado
                            //    {
                            //        string anfitrion = trozos[4];
                            //        string MessageBoxTitle = "Invitación de partida";
                            //        string MessageBoxContent = "Hola " + userBox.Text + " " + anfitrion + " te ha retado a una partida de" + juego + "\nDeseas aceptarla?";
                            //        string respuesta_invitation;
                            //        DialogResult result = Debug.Log(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
                            //        switch (result)
                            //        {
                            //            case DialogResult.Yes:
                            //                //Debug.Log("La partida se iniciará en unos instantes");
                            //                respuesta_invitation = "8/" + partida + "/" + anfitrion + "/" + userBox.Text + "/" + Convert.ToString(result) + "/1";
                            //                byte[] accept = System.Text.Encoding.ASCII.GetBytes(respuesta_invitation);
                            //                server.Send(accept);
                            //                break;
                            //            case DialogResult.No:
                            //                //Debug.Log("Juegas o eres un llorón?");
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

                            //        Debug.Log("La partida " + partida8 + "de Inverted-Pacman se canceló");
                            //    }
                            //    else if (hack8 == 1)//respuesta para anfitrion de los distintos invitados
                            //    {
                            //        string invitado = trozos[4];
                            //        string respuesta_inv = trozos[5];
                            //        Debug.Log("Respuesta del invitado " + invitado + "a la partida de Inverted-Pacman " + partida8 + "es " + respuesta_inv);
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
                            //        Debug.Log("La partida " + partida8 + "del juego " + juego + " se canceló");
                            //        Del_ParaFinalizarPartida delegado8 = new Del_ParaFinalizarPartida(FinalizarPartida);
                            //        this.Invoke(delegado8, new object[] { hack8, partida8 });
                            //    }
                            //    else if (hack8 == -1)//respuesta para el invitado si el anfitrion se desconnecta
                            //    {
                            //        Debug.Log("El anfitrion se desconecto");
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
                this.cliente.Consultas = false;
            }

        }
}
