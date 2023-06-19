import socket
import multiprocessing

invi = False
idgame=0
endgame=False
def receive(s, name):
    while True:
        data = s.recv(1024)
        if len(data) > 0:
            print(name, "Received", repr(data.decode()))
            if(data.decode().split('/')[0]=="5"):
                print("Invitacion recibida")
                invi=True
                idgame=int(data.decode().split('/')[1])
            if(data.decode().split('/')[0]=="8" and data.decode().split('/')[1]=="3"):
                endgame=True
host = "147.83.117.22"
port = 50053
user="Ana"#input("nombre: ")
pwd="teo"#input("contraseña: ")
input("Start:")
my_socket=socket.socket(socket.AF_INET, socket.SOCK_STREAM)
my_socket.connect((host, port))
my_socket.sendall(("2/"+user + "*" + pwd).encode())
mythread=multiprocessing.Process(target=receive, args=(my_socket, user))
while True:
    if invi:
        print("Invitacion recibida")
        my_socket.sendall(("6/1"+str(idgame)).encode())
        break
while not(endgame):
            my_socket.sendall(("8/1/0/"+user+"*"+str(0.1)+"*"+str(0.2)).encode())
