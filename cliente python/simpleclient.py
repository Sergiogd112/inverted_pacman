import socket
import multiprocessing

invi = False
idgame=0
def receive(s, name):
    while True:
        data = s.recv(1024)
        if len(data) > 0:
            print(name, "Received", repr(data.decode()))
            if(data.decode().split('/')[0]=="5"):
                print("Invitacion recibida")
                invi=True
                idgame=int(data.decode().split('/')[1])


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
    # host = "147.83.117.22"
    host = "147.83.117.22"
    port = 50053
    a=input("Start:")
    # connection to hostname on the port.
    [s.connect((host, port)) for s in sockets]
    # send a thank you message to the client.
    [
        s.sendall(("2/"+name + "*" + pwd).encode())
        for s, (name, pwd) in zip(sockets, name_pwd)
    ]
    # receive data from the server
    processes = [
        multiprocessing.Process(target=receive, args=(s, name))
        for s, (name, pwd) in zip(sockets, name_pwd)
    ]
    [thread.start() for thread in processes]
    invitar=input("Intitar: ")
    message="5/Ana*Carlos*pepa"
    sockets[0].sendall(message.encode())
    # while True:
    #     if invi:
    #         print("Invitacion recibida")
    #         for i in range(1,len(sockets)):
    #             sockets[i].sendall(("6/1"+str(idgame)).encode())
    #         break
    
    input("Acabar:")
    [thread.terminate() for thread in processes]
    message = "0/"
    [s.sendall(message.encode()) for s in sockets]


if __name__ == "__main__":
    main()
