import socket

def main():
    ussr_pwd=[
        "sergio*teo",
        "Anna*teo",
        "Carlos*teo",
    ]
    sockets=[socket.socket(socket.AF_INET, socket.SOCK_STREAM) for i in range(len(ussr_pwd))]
    # create a socket object

    # get local machine name
    host = '192.168.56.102'

    port = 9050

    # connection to hostname on the port.
    [s.connect((host, port))  for s in sockets]

    # send a thank you message to the client.
    message = "2/pepa*pig"
    [s.sendall(message.encode())  for s in sockets]

    # receive data from the server
    datas = [s.recv(1024)  for s in sockets]

    [print('Received', repr(data.decode())) for data in datas]
    input("Acabar:")
    message="0/"
    [s.sendall(message.encode())  for s in sockets]

if __name__ == '__main__':
    main()
