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

/// <summary>
/// This is the script for the login and register buttons.
/// </summary>
public class LoginRegister : MonoBehaviour
{
    public Client cliente; // This is the client object.

    private void Start() {
        if(cliente.Conectado){
            return;
        }
        Debug.Log(this.cliente.ip);
        IPAddress direc = IPAddress.Parse(this.cliente.ip); // Get the ip address.
        IPEndPoint ipep = new IPEndPoint(direc, this.cliente.puerto); // Create the ip endpoint.
        Debug.Log("creando socket...");
        this.cliente.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Create the socket.
        try
        {
            this.cliente.server.Connect(ipep);// Connect the socket.
            this.cliente.Conectado = true;
            Debug.Log("Conectado");
        }
        catch (SocketException ex)
        {
            // If the server is not available.
            Debug.LogError(ex.ToString());
        }

    }
}
