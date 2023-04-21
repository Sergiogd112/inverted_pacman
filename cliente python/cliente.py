import socket

import threading


def receive(s, name):
    while True:
        data = s.recv(1024)
        if len(repr(data.decode()))!=0:
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
    host = "192.168.56.102"

    port = 9050

    # connection to hostname on the port.
    [s.connect((host, port)) for s in sockets]

    # send a thank you message to the client.
    [
        s.sendall((name + "*" + pwd).encode())
        for s, (name, pwd) in zip(sockets, name_pwd)
    ]

    # receive data from the server
    threads = [
        threading.Thread(target=receive, args=(s, name))
        for s, (name, pwd) in zip(sockets, name_pwd)
    ]
    [thread.start() for thread in threads]

    input("Acabar:")
    [thread.kill() for thread in threads]
    message = "0/"
    [s.sendall(message.encode()) for s in sockets]


if __name__ == "__main__":
    main()
