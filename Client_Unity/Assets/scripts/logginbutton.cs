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

/// <summary>
/// This is the script for the loggin button.
/// </summary>
public class logginbutton : MonoBehaviour
{
    public Client cliente; // This is the client.
    public TMP_InputField nameField; // This is the name field.
    public TMP_InputField passwordField; // This is the password field.
    public string sceneName; // This is the scene name.
    /// <summary>
    /// This is called when the user clicks the loggin button.
    /// </summary>
    public void Loggin()
    {
        if (cliente.Conectado) // If the client is connected.
        {
            if (!cliente.Logeado) // If the client is not logged in.
            {
                string name = nameField.text; // Get the name.
                string password = passwordField.text;   // Get the password.
                if (name != "" & password != "")
                {

                    string message = "2/" + Regex.Escape(name) + "*" + Regex.Escape(password); // Create the message.
                    // Send the message to the server.
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                    cliente.server.Send(msg);
                    byte[] msg2 = new byte[10000000];
                    cliente.server.Receive(msg2);
                    UnityEngine.Debug.Log(Encoding.ASCII.GetString(msg2));
                    string[] Server_Error = Encoding.ASCII.GetString(msg2).Split('\x04');
                    if (Server_Error[0] == "") // If the server is not available.
                    {
                        UnityEngine.Debug.Log("Not available, please connect again later");
                        cliente.server.Shutdown(SocketShutdown.Both);
                        cliente.server.Close();
                    }
                    else // If the server is available.
                    {
                        string[] trozos = Server_Error[0].Split('/');
                        int code = Convert.ToInt32(trozos[0]);
                        if (code == 2) // If the code is 2.
                        {
                            int num2 = Convert.ToInt32(trozos[1]);
                            if (num2 == 1) // If the code is 1.
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
