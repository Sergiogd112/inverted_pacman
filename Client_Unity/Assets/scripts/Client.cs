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

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "this", menuName = "ScriptableObjects/this", order = 1)]

public class Client : ScriptableObject
{
    public Socket server; // This is the server.
    public Thread atender; // This is the thread.
    public string usuario; // This is the username.
    public int num_conn; // This is the number of connections.
    public bool Conectado = false; // This is used to check if the client is connected.
    public bool Logeado = false; // This is used to check if the client is logged in.
    public bool Consultas = false; // This is used to check if the client is making a query.
    public string ip="147.83.117.22"; // This is the ip.
    public int puerto=50053; // This is the port.
    public bool updated_conected_list = false; // This is used to check if the connected list has been updated.
    public string[] connected; // This is the list of connected users.
    public int numplayergame = 0; // This is the number of players in the game.
    public string amfitrion; // This is the host.
    public string[] invitados; // This is the list of guests.
    public int idpartida; // This is the id of the game.
    public bool invitado = false; // This is used to check if the client is a guest.
    public string pscene; // This is the previous scene.
    public int invitationres = 0; // This is the invitation response.
    public ChatData chatdata; // This is the chat data.
    public int delete = 0; // This is used to store the result of the delete account query.
    public int StartAtender()
    {
        return 0;
    }
    /// <summary>
    /// This is called when the client is created. It manages the connection to the server.
    /// </summary>
    public void AtenderServidor()
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
                    case 0: //Respuesta al desconectar
                        string mensaje1 = elements[2];
                        int hack = Convert.ToInt32(elements[1]);
                        if (hack == 1)
                        {
                            UnityEngine.Debug.Log(mensaje1);
                        }
                        else
                        {
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
                    case 7: //notificacion de que se ha unido un jugador a la partida
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
                    case 10: // notificacion de actualizacion de chat
                        string[] messages = elements[2].Split(",");
                        for (int i = chatdata.users.Count(); i < messages.Length; i++)
                        {
                            string[] data = messages[i].Split("*");
                            UnityEngine.Debug.Log(data[0]);
                            UnityEngine.Debug.Log(data[1]);
                            UnityEngine.Debug.Log(data[2]);
                            chatdata.addMessage(Regex.Unescape(data[0]), Regex.Unescape(data[1]), Regex.Unescape(data[2]));
                        }
                        chatdata.updated_chat = true;
                        break;
                    case 11: // respuesta a la consulta de eliminar cuenta
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
