import socket
import threading
import time


# Función para enviar mensajes al servidor
def send_message(client_socket, message):
    client_socket.send(message.encode())


# Función para recibir mensajes del servidor
def receive_message(client_socket):
    data = client_socket.recv(1024).decode()
    return data


# Función para gestionar las respuestas del servidor en un hilo separado
def handle_responses(client_socket):
    while True:
        response = receive_message(client_socket)
        print(f"Respuesta recibida: {response}")


# Dirección y puerto del servidor
SERVER_ADDRESS = ("147.83.117.22", 50053)

# Crear tres clientes
client1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client3 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Conexión de los clientes al servidor
client1.connect(SERVER_ADDRESS)
client2.connect(SERVER_ADDRESS)
client3.connect(SERVER_ADDRESS)

# Hilo para gestionar las respuestas del servidor para cada cliente
thread1 = threading.Thread(target=handle_responses, args=(client1,))
thread2 = threading.Thread(target=handle_responses, args=(client2,))
thread3 = threading.Thread(target=handle_responses, args=(client3,))
thread1.start()
thread2.start()
thread3.start()

# Hacer login de los usuarios
users = [("sergio", "teo"), ("Ana", "teo"), ("Carlos", "teo")]
clients = [client1, client2, client3]

for i, (username, password) in enumerate(users):
    login_message = f"2/{username}*{password}"
    send_message(clients[i], login_message)

# Enviar mensaje al chat
for i, client in enumerate(clients):
    send_message(client, f"9/Hola, soy {users[i][0]}!")

# Esperar un tiempo para recibir invitaciones
time.sleep(5)

# Verificar si se recibieron invitaciones
invitations_received = [False, False, False]

for i, client in enumerate(clients):
    response = receive_message(client)
    if response.startswith("6/"):
        invitations_received[i] = True
        print(f"{users[i][0]} recibió una invitación para jugar.")

# Aceptar invitaciones y mover jugadores
for i, client in enumerate(clients):
    if invitations_received[i]:
        invitation_accept_message = f"6/1/{i+1}"
        send_message(client, invitation_accept_message)

# Si no se recibieron invitaciones, sergio invita a los otros dos jugadores y a pepa
if not any(invitations_received):
    invite_message = "5/sergio*Ana*Carlos*pepa"
    send_message(client1, invite_message)

# Mover jugadores y slimes
while True:
    for i, client in enumerate(clients):
        if invitations_received[i] or users[i][0] == "sergio":
            move_message = f"8/1/0/{users[i][0]}*0.4*0.8"
            send_message(client, move_message)

    # Esperar un poco antes de mover nuevamente
    time.sleep(1)

    # Comprobar si se recibe una entrada de consola para salir del bucle
    if input("Presiona Enter para desconectarte: "):
        break

# Desconexión de los clientes
for client in clients:
    send_message(client, "0")

# Esperar a que los hilos de respuesta finalicen
thread1.join()
thread2.join()
thread3.join()
