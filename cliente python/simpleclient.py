import socket
import multiprocessing


def receive(s, name):
    while True:
        data = s.recv(1024)
        if len(data) > 0:
            print(name, "Received", repr(data.decode()))


def main():
    name_pwd = [
        ("sergio", "teo"),
        ("Anna", "teo"),
        ("Carlos", "teo"),
    ]
    sockets = [
        socket.socket(socket.AF_INET, socket.SOCK_STREAM) for i in range(len(name_pwd))
    ]
    # create a socket object
    # get local machine name
    host = "147.83.117.22"
    port = 5053
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
    input("Acabar:")
    [thread.terminate() for thread in processes]
    message = "0/"
    [s.sendall(message.encode()) for s in sockets]


if __name__ == "__main__":
    main()
