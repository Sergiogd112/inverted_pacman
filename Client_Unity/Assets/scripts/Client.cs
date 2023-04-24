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
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Client", order = 1)]

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


}
