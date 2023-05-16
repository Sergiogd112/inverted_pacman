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
using System.Diagnostics;

[CreateAssetMenu(fileName = "this", menuName = "ScriptableObjects/this", order = 1)]

public class Client  : ScriptableObject
{
    public Socket server;
    public Thread atender;
    public string usuario;
    public int num_conn;
    public bool Conectado = false;
    public bool Logeado = false;
    public bool Consultas = false;
    public string ip;
    //string ip = "147.83.117.22";
    public int puerto;
    public bool nueva_connected_list = false;

    public int numplayergame = 0;
    public int StartAtender()
    {
        return 0;
    }
    public void AtenderServidor()   //ACABAR DE REVISAR
    {
        UnityEngine.Debug.Log("AtenderServidor en marcha");
        while (true)
        { 
            // Recibimos mensaje del servidor
            byte[] msg2 = new byte[100];
            this.server.Receive(msg2);
            this.Consultas = true;
            string[] error_servidor = Encoding.ASCII.GetString(msg2).Split('\x04');
            if (error_servidor[0] == "")
            {
                UnityEngine.Debug.Log("Servidor en tareas de mantenimiento, vuelva a conectarse m�s tarde");
                // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                this.server.Shutdown(SocketShutdown.Both);
                this.server.Close();
                this.atender.Abort();
            }

            else
            {
                string[] trozos = error_servidor[0].Split('/');
                UnityEngine.Debug.Log(error_servidor[0]);
                int codigo = Convert.ToInt32(trozos[0]);
                switch (codigo)
                {
                    case 0: //Resupesta a la desconexi�n
                        string mensaje1 = trozos[2];
                        int hack = Convert.ToInt32(trozos[1]);
                        if (hack == 1)
                        {
                            UnityEngine.Debug.Log(mensaje1);
                        }
                        else
                        {
                            // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                            this.server.Shutdown(SocketShutdown.Both);
                            this.server.Close();
                            this.atender.Abort();

                        }

                        break;

                    case 1: //Respuesta al registrar
                        int hackS = Convert.ToInt32(trozos[1]);
                        if (hackS == 1)
                        {
                            UnityEngine.Debug.Log("Todo bien");
                            this.Logeado = true;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Todo mal");
                            // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                            this.server.Shutdown(SocketShutdown.Both);
                            this.server.Close();
                            this.atender.Abort();
                        }

                        break;

                    case 2: //Respuesta al iniciar sesi�n
                        int hack1 = Convert.ToInt32(trozos[1]);
                        if (hack1 == 1)
                        {
                            UnityEngine.Debug.Log("Todo bien");
                            this.Logeado = true;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Todo mal");
                            // Del_ParaDesconectar delegado = new Del_ParaDesconectar(Desconectar);
                            this.server.Shutdown(SocketShutdown.Both);
                            this.server.Close();
                            this.atender.Abort();
                        }
                        break;
                    case 3: //Respuesta a la consulta 1
                        string mensaje3 = trozos[1];
                        UnityEngine.Debug.Log(mensaje3);
                        break;
                    case 4: //Respuesta a la consulta 2
                        this.nueva_connected_list = true;
                        string[] usuarios = trozos[2].Split(',');
                        // ActualizarConectados(usuarios);
                        UnityEngine.Debug.Log(trozos[2]);
                        break;
                    case 5: //Respuesta a la consulta 3
                        string mensaje5 = trozos[1];
                        UnityEngine.Debug.Log(mensaje5);
                        break;
                    case 6: //notificacion con la lista de conectados actualizada
                        int hack6 = Convert.ToInt32(trozos[1]);
                        if (hack6 == 0)
                            UnityEngine.Debug.Log("No hay usuarios connectados");
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
                        //        string MessageBoxTitle = "Invitaci�n de partida";
                        //        string MessageBoxContent = "Hola " + userBox.Text + " " + anfitrion + " te ha retado a una partida de" + juego + "\nDeseas aceptarla?";
                        //        string respuesta_invitation;
                        //        DialogResult result = UnityEngine.Debug.Log(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
                        //        switch (result)
                        //        {
                        //            case DialogResult.Yes:
                        //                //Debug.Log("La partida se iniciar� en unos instantes");
                        //                respuesta_invitation = "8/" + partida + "/" + anfitrion + "/" + userBox.Text + "/" + Convert.ToString(result) + "/1";
                        //                byte[] accept = System.Text.Encoding.ASCII.GetBytes(respuesta_invitation);
                        //                server.Send(accept);
                        //                break;
                        //            case DialogResult.No:
                        //                //Debug.Log("Juegas o eres un llor�n?");
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

                        //        UnityEngine.Debug.Log("La partida " + partida8 + "de Inverted-Pacman se cancel�");
                        //    }
                        //    else if (hack8 == 1)//respuesta para anfitrion de los distintos invitados
                        //    {
                        //        string invitado = trozos[4];
                        //        string respuesta_inv = trozos[5];
                        //        UnityEngine.Debug.Log("Respuesta del invitado " + invitado + "a la partida de Inverted-Pacman " + partida8 + "es " + respuesta_inv);
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
                        //        UnityEngine.Debug.Log("La partida " + partida8 + "del juego " + juego + " se cancel�");
                        //        Del_ParaFinalizarPartida delegado8 = new Del_ParaFinalizarPartida(FinalizarPartida);
                        //        this.Invoke(delegado8, new object[] { hack8, partida8 });
                        //    }
                        //    else if (hack8 == -1)//respuesta para el invitado si el anfitrion se desconnecta
                        //    {
                        //        UnityEngine.Debug.Log("El anfitrion se desconecto");
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
            this.Consultas = false;
        }

    }

}
