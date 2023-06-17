using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class registerbutton : MonoBehaviour
{
    public Client cliente;
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    public TMP_InputField repeatpasswordField;
    public TMP_InputField emailField;
    public string sceneName;
    public void Register()
    {
        if (cliente.Conectado)
        {
            if (!cliente.Logeado)
            {
                string name = nameField.text;
                string password = passwordField.text;
                string newpassword = repeatpasswordField.text;
                string email = emailField.text;
                if (name != "" & password != "" & newpassword != "" & email != "")
                {

                    string message = "1/" + Regex.Escape(name) + "*" + Regex.Escape(password) + "*" + email;
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
                        if (code == 1)
                        {
                            int num2 = Convert.ToInt32(trozos[1]);
                            if (num2 == 1)
                            {
                                UnityEngine.Debug.Log("OK");
                                cliente.Logeado = true;
                                SceneManager.LoadScene(sceneName);
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
                    UnityEngine.Debug.Log("Fill in username, password and email please");
                }
            }
            else
                UnityEngine.Debug.Log("User already registered");
        }
    }
}
