import socket
import multiprocessing
from time import sleep
invi = False
idgame = 0


def receive(s, name):
    while True:
        data = s.recv(1024)
        if len(data) > 0:
            print(name, "Received", repr(data.decode()))
            if data.decode().split("/")[0] == "6":
                print("Invitacion recibida")
                names = data.decode().split("/")[2]
                invi = True
                idgame = int(data.decode().split("/")[1])
                s.sendall(("6/1/" + str(idgame)).encode())
            if data.decode().split("/")[0] == "7":
                print("Empieza el juego")
                s.sendall(("7/1/" + names+"\0").encode())
                sleep(.5)
                print("Enviando posicion")
                s.sendall(("8/1/0/" + name+"*-1,1*+1,0\0").encode())
                    # sleep(.5)
                    # s.sendall(("8/1/0/" + name+"*-1,5*+1.3").encode())
                    # sleep(.5)
            if data.decode().split("/")[0] == "8":
                print("Han mandado posicion")


def main():
    name_pwd = [
        ("sergio", "teo"),
        ("Ana", "teo"),
        ("Carlos", "teo"),
    ]
    sockets = [
        socket.socket(socket.AF_INET, socket.SOCK_STREAM) for i in range(len(name_pwd))
    ]

    # create a socket object
    # get local machine name
    # host = "192.168.56.102"
    host = "147.83.117.22"
    port = 50053
    a = input("Start:")
    # connection to hostname on the port.
    [s.connect((host, port)) for s in sockets]
    # send a thank you message to the client.
    [
        s.sendall(("2/" + name + "*" + pwd).encode())
        for s, (name, pwd) in zip(sockets, name_pwd)
    ]
    # receive data from the server
    processes = [
        multiprocessing.Process(target=receive, args=(s, name))
        for s, (name, pwd) in zip(sockets, name_pwd)
    ]
    [thread.start() for thread in processes]

    input("Acabar:")
    [thread.terminate() for thread in processes]
    message = "0/"
    [s.sendall(message.encode()) for s in sockets]


if __name__ == "__main__":
    main()
