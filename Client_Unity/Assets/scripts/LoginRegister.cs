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
        //ThreadStart ts = delegate { AtenderServidor(); };
        //this.cliente.atender = new Thread(ts);
        //this.cliente.atender.Start();
    }
}
