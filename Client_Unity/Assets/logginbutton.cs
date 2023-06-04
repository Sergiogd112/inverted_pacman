using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class logginbutton : MonoBehaviour
{
    public Client cliente;
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    public string sceneName;
    public void Loggin()
    {
        if (cliente.Conectado)
        {
            if (!cliente.Logeado)
            {
                string name = nameField.text;
                string password = passwordField.text;
                if (name != "" & password != "")
                {

                    string message = "2/" + Regex.Escape(name) + "*" + Regex.Escape(password);
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                    cliente.server.Send(msg);
                    byte[] msg2 = new byte[10000000];
                    cliente.server.Receive(msg2);
                    UnityEngine.Debug.Log(Encoding.ASCII.GetString(msg2));
                    string[] Server_Error = Encoding.ASCII.GetString(msg2).Split('\x04');
                    if (Server_Error[0] == "")
                    {
                        //En caso de error cierra el socket
                        UnityEngine.Debug.Log("Not available, please connect again later");
                        //Del_Disconnect delegateDst = new Del_Disconnect(Disconnect);
                        cliente.server.Shutdown(SocketShutdown.Both);
                        cliente.server.Close();
                        //Attend.Abort();
                    }
                    else
                    {
                        string[] trozos = Server_Error[0].Split('/');
                        int code = Convert.ToInt32(trozos[0]);
                        if (code == 2)
                        {
                            int num2 = Convert.ToInt32(trozos[1]);
                            if (num2 == 1)
                            {
                                UnityEngine.Debug.Log("OK");
                                cliente.Logeado = true;
                                cliente.usuario = name;
                                SceneManager.LoadScene(sceneName);
                                ThreadStart ts = delegate { cliente.AtenderServidor(); };
                                cliente.atender = new Thread(ts);
                                cliente.atender.Start();
                            }
                            else
                            {
                                UnityEngine.Debug.Log("Not OK");
                                //Del_Disconnect delegateDst = new Del_Disconnect(Disconnect);
                                //passwordBox.Invoke(delegateDst, new object[] { });
                                cliente.server.Shutdown(SocketShutdown.Both);
                                cliente.server.Close();
                                //Attend.Abort();
                            }
                        }
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Fill in username and password please");
                }
            }
            else
                UnityEngine.Debug.Log("Session already started");
        }
    }
}
