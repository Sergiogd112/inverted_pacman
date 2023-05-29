using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "MainGameManager", menuName = "ScriptableObjects/MainGameManager", order = 1)]
public class MainGameManager : ScriptableObject
{
    public Client client;
    public bool host = false;
    public string[] nombres = new string[4];
    public float[][] ppositions = new float[4][2];
    public float[][] epositions = new float[4][2];
    public int[] ids = new int[4];
    public int[] points = new int[4];
    public int[] health = new int[4];
    public Thread atentder;
    public bool timeouts = false;
    public int spawn = 0;
    public float[] spawnpos = new float[2];
    public List<string> lognames=new List<string>();
    public List<int> logids=new List<int>();
    public List<int> logcodes=new List<int>();

    public void AtenderPartida()
    {
        UnityEngine.Debug.Log("Atender partida en marcha");
        while (true)
        {
            byte[] msg2 = new byte[1000];
            this.server.Receive(msg2);
            this.Consultas = true;
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
            switch(scode){
                case 0:
                    DecodeFull(elements[2]);
                    break;
                case 1:
                    DecodePlayers(Convert.ToInt32(elements[2]), elements[3]);
                    break;
                case 2:
                    DecodeEnemy(Convert.ToInt32(elements[2]),elements[3]);
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
                    epositions[i][0] = Convert.ToFloat(properties[1]);
                    epositions[i][1] = Convert.ToFloat(properties[2]);
                }
                break;
            case 1:
                if (host)
                {
                    break;
                }
                string[] properties = data[i].Split('*');
                spawn = Convert.ToInt32(properties[0]);
                spawnpos[0] = Convert.ToFloat(properties[1]);
                spawnpos[1] = Convert.ToFloat(properties[2]);
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
                    ppositions[i][0] = Convert.ToFloat(properties[1]);
                    ppositions[i][1] = Convert.ToFloat(properties[2]);
                    points[i]=Convert.ToInt32(properties[3]);
                    health[i]=Convert.ToInt32(properties[4]);
                    names[i]=properties[0];
                }
                break;
            case 1:
                if (host)
                {
                    break;
                }
                string[] properties = data[i].Split('*');
                logcodes.Add(1);
                lognames.Add(properties[1]);
                logids.Add(properties[2]);
                break;                
            case 2:
                if (host)
                {
                    break;
                }
                string[] properties = data[i].Split('*');
                logcodes.Add(3);
                lognames.Add(properties[1]);
                logids.Add(properties[2]);
                break;                
            case 3:
                if (host)
                {
                    break;
                }
                string[] properties = data[i].Split('*');
                logcodes.Add(3);
                lognames.Add(properties[1]);
                logids.Add(properties[2]);
                break;                
        }
    }
    public void DecodeFull(string s)
    {
        string[] data=s.Split('|');
        DecodeEnemy(0,data[1]);
        DecodePlayers(0,data[1]);
    }
}
