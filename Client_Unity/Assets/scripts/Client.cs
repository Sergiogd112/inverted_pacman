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
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "this", menuName = "ScriptableObjects/this", order = 1)]

public class Client : ScriptableObject
{
    public Socket server;
    public Thread atender;
    public string usuario;
    public int num_conn;
    public bool Conectado = false;
    public bool Logeado = false;
    public bool Consultas = false;
    public string ip="147.83.117.22";
    public int puerto=50053;
    public bool updated_conected_list = false;
    public string[] connected;
    public int numplayergame = 0;
    public string amfitrion;
    public string[] invitados;
    public int idpartida;
    public bool invitado = false;
    public string pscene;
    public int invitationres = 0;
    public ChatData chatdata;
    public int delete = 0;
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
            byte[] msg2 = new byte[1000];
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
                string[] elements = error_servidor[0].Split('/');
                UnityEngine.Debug.Log(error_servidor[0]);
                int codigo = Convert.ToInt32(elements[0]);
                switch (codigo)
                {
                    case 0: //Resupesta a la desconexi�n
                        string mensaje1 = elements[2];
                        int hack = Convert.ToInt32(elements[1]);
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
                            SceneManager.LoadScene("Login");
                        }

                        break;

                    case 1: //Respuesta al registrar
                        int hackS = Convert.ToInt32(elements[1]);
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
                        int hack1 = Convert.ToInt32(elements[1]);
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
                        string mensaje3 = elements[1];
                        UnityEngine.Debug.Log(mensaje3);
                        break;
                    case 4: //Respuesta a la consulta 2
                        this.updated_conected_list = true;
                        connected = elements[2].Split(',');
                        // ActualizarConectados(usuarios);
                        UnityEngine.Debug.Log(connected);
                        updated_conected_list = true;
                        break;
                    case 5: //Respuesta a la consulta 3
                        string mensaje5 = elements[1];
                        UnityEngine.Debug.Log(mensaje5);
                        break;
                    case 6: //notificacion con la lista de conectados actualizada
                        string[] jugadores = elements[2].Split(',');
                        amfitrion = jugadores[0];
                        invitados = jugadores[1].Split('*');
                        idpartida = Convert.ToInt32(elements[1]);
                        invitado = true;

                        break;
                    case 7:
                        if (elements[1] == "1")
                        {
                            atender.Abort();
                            invitationres = 1;
                        }
                        else
                        {
                            invitationres = -1;
                        }
                        break;
                    case 10:
                        string[] messages = elements[2].Split(",");
                        for (int i = chatdata.users.Count(); i < Convert.ToInt32(elements[1]); i++)
                        {
                            string[] data = messages[i].Split("*");
                            chatdata.addMessage(Regex.Unescape(data[0]), Regex.Unescape(data[1]), Regex.Unescape(data[2]));
                        }
                        chatdata.updated_chat = true;
                        break;
                    case 11:
                        if (elements[1] == "1")
                        {
                            delete = 1;
                            Logeado = false;
                            usuario = "";

                            atender.Abort();
                        }
                        else
                        {
                            delete = -1;
                        }
                        break;
                    default:
                        break;

                }

            }
            this.Consultas = false;
        }

    }

}
