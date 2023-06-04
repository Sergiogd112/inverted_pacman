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

[CreateAssetMenu(fileName = "MainGameManager", menuName = "ScriptableObjects/MainGameManager", order = 1)]
public class MainGameManager : ScriptableObject
{
    public Client client;
    public bool host = false;
    public string[] names = new string[4];
    public float[,] ppositions = new float[4, 2];
    public float[,] epositions = new float[4, 2];
    public int[] ids = new int[4];
    public int[] points = new int[4];
    public int[] health = new int[4];
    public Thread atender;
    public bool timeouts = false;
    public int spawn = 0;
    public float[] spawnpos = new float[2];
    public List<string> lognames = new List<string>();
    public List<int> logids = new List<int>();
    public List<int> logcodes = new List<int>();

    public void AtenderPartida()
    {
        UnityEngine.Debug.Log("Atender partida en marcha");
        while (true)
        {
            byte[] msg2 = new byte[1000];
            this.client.server.Receive(msg2);

            string[] error_servidor = Encoding.ASCII.GetString(msg2).Split('\x04');
            if (error_servidor[0] == "")
            {
                UnityEngine.Debug.Log("Servidor en tareas de mantenimiento, vuelva a conectarse m�s tarde");
                this.client.server.Shutdown(SocketShutdown.Both);
                this.client.server.Close();
                this.atender.Abort();
                break;
            }
            string[] elements = error_servidor[0].Split('/');
            UnityEngine.Debug.Log(error_servidor[0]);
            int codigo = Convert.ToInt32(elements[0]);
            if (codigo != 8)
            {
                continue;
            }
            int scode = Convert.ToInt32(elements[1]);
            switch (scode)
            {
                case 0:
                    DecodeFull(elements[2]);
                    break;
                case 1:
                    DecodePlayers(Convert.ToInt32(elements[2]), elements[3]);
                    break;
                case 2:
                    DecodeEnemy(Convert.ToInt32(elements[2]), elements[3]);
                    break;
            }
        }
    }
    public void DecodeEnemy(int code, string data)
    {

        switch (code)
        {
            case 0:
                string[] elements = data.Split(',');
                for (int i = 0; i < 4; i++)
                {
                    string[] properties = elements[i].Split('*');
                    ids[i] = Convert.ToInt32(properties[0]);
                    epositions[i, 0] = float.Parse(properties[1]);
                    epositions[i, 1] = float.Parse(properties[2]);
                }
                break;
            case 1:
                if (host)
                {
                    break;
                }
                string[] values = data.Split('*');
                spawn = Convert.ToInt32(values[0]);
                spawnpos[0] = float.Parse(values[1]);
                spawnpos[1] = float.Parse(values[2]);
                break;
        }
    }
    public void DecodePlayers(int code, string data)
    {

        switch (code)
        {
            case 0:
                string[] elements = data.Split(',');
                for (int i = 0; i < 4; i++)
                {
                    string[] properties = elements[i].Split('*');
                    ppositions[i, 0] = float.Parse(properties[1]);
                    ppositions[i, 1] = float.Parse(properties[2]);
                    points[i] = Convert.ToInt32(properties[3]);
                    health[i] = Convert.ToInt32(properties[4]);
                    names[i] = Regex.Unescape(properties[0]);
                }
                break;
            case 1:
                if (host)
                {
                    break;
                }
                string[] log1 = data.Split('*');
                logcodes.Add(1);
                lognames.Add(Regex.Unescape(log1[1]));
                logids.Add(Convert.ToInt32(log1[2]));
                break;
            case 2:
                if (host)
                {
                    break;
                }
                string[] log2 = data.Split('*');
                logcodes.Add(3);
                lognames.Add(Regex.Unescape(log2[1]));
                logids.Add(Convert.ToInt32(log2[2]));
                break;
            case 3:
                if (host)
                {
                    break;
                }
                string[] log3 = data.Split('*');
                logcodes.Add(3);
                lognames.Add(Regex.Unescape(log3[1]));
                logids.Add(Convert.ToInt32(log3[2]));
                break;
        }
    }
    public void DecodeFull(string s)
    {
        string[] data = s.Split('|');
        DecodeEnemy(0, data[1]);
        DecodePlayers(0, data[1]);
    }
}
