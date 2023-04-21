import socket
from time import sleep
import multiprocessing
import pandas as pd


class User:
    def __init__(self, username=None, password=None, email=None):
        self.username = username
        self.password = password
        self.email = email
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        with open(self.username + ".txt", "w") as f:
            f.write("")
        self.connected = False
        self.loggedin = False

    def connect(self, host="", port=0):
        if not self.connected:
            self.s.connect((host, port))
            self.process = multiprocessing.Process(target=self.recieve, args=())
            self.process.start()
            self.connected = True
            print(self.username, " is connected")
        else:
            print(self.username, " is already connected")

    def register(self):
        self.s.sendall(
            ("1/" + self.username + "*" + self.password + "*" + self.email).encode()
        )
        self.loggedin = True

    def login(self):
        self.s.sendall(("2/" + self.username + "*" + self.password).encode())
        self.loggedin = True

    def recieve(self):
        while True:
            data = self.s.recv(1024)
            if len(data) > 0:
                with open(self.name + ".txt", "a") as f:
                    f.write(repr(data.decode()) + "\n---\n---\n")

    def disconnect(self):
        if self.connected:
            disconect_msg = "0/"
            self.s.sendall(disconect_msg.encode())
            self.process.terminate()
            self.connected = False
            self.loggedins = False
            print(self.username + " is disconnected")
        else:
            print(self.username, " is not connected")


class Manager:
    def __init__(self):
        self.df = pd.read_csv("sampleusers.txt")
        self.users = [
            User(row.Username, row.password, row.email) for _, row in self.df.iterrows()
        ]
        self.df=self.df.assign(connected=False, loggedin=False)

    def register(self, all=False):
        if all:
            users = self.users
            idxs = list(self.df.index)
        else:
            print("Please select user indexes separated by spaces or [a]ll for all")
            print(self.df.query("loggedin==False")[["Username", "email"]])
            text = input(">>")
            if "a" == text[0]:
                users = self.users
                idxs = list(self.df.index)
            else:
                idxs = [int(i) for i in text.split(" ")]
                users = [self.users[int(i)] for i in idx]
        for user, idx in zip(users, idxs):
            user.register()
            self.df.loggedin[idx] = True

    def login(self, all=False):
        if all:
            users = self.users
            idxs = list(self.df.index)
        else:
            print("Please select user indexes separated by spaces or [a]ll for all")
            print(self.df.query("loggedin==False")[["Username", "email"]])
            text = input(">>")
            if "a" == text[0]:
                users = self.users
                idxs = list(self.df.index)
            else:
                idxs = [int(i) for i in text.split(" ")]
                users = [self.users[int(i)] for i in idx]
        for user, idx in zip(users, idxs):
            user.login()
            self.df.loggedin[idx] = True

    def connect(self, all=False):
        if all:
            users = self.users
            idxs = list(self.df.index)
        else:
            print("Please select user indexes separated by spaces or [a]ll for all")
            print(self.df.query("connected==False")[["Username", "email"]])
            text = input(">>")
            if "a" == text[0]:
                users = self.users
                idxs = list(self.df.index)
            else:
                idxs = [int(i) for i in text.split(" ")]
                users = [self.users[int(i)] for i in idx]
        for user, idx in zip(users, idxs):
            user.connect("192.168.56.102", 9060)
            self.df.connected[idx] = True
            self.df.loggedin[idx] = True

    def disconnect(self, all=False):
        if all:
            users = self.users
            idxs = list(self.df.index)
        else:
            print("Please select user indexes separated by spaces or [a]ll for all")
            print(self.df.query("connected==True")[["Username", "email"]])
            text = input(">>")
            if "a" == text[0]:
                users = self.users
                idxs = list(self.df.index)
            else:
                idxs = [int(i) for i in text.split(" ")]
                users = [self.users[int(i)] for i in idx]
        for user, idx in zip(users, idxs):
            user.disconnect()
            self.df.connected[idx] = True
            self.df.loggedin[idx] = False

    def display(self, code=None):
        print(
            "Wich table do you want:\n"
            + "[a] all\n"
            + "[c] connected\n"
            + "[nc] not connected\n"
            + "[l] logged in\n"
            + "[nl] not logged in\n"
        )
        text = input(">>")
        if "a" in text:
            print(self.df[["Username", "email"]])
            return
        if "c" in text:
            if len(text) == 2:
                print(self.df.query("connected==False")[["Username", "email"]])
                return
            print(self.df.query("connected==True")[["Username", "email"]])
            return
        if "c" in text:
            if len(text) == 2:
                print(self.df.query("loggedin==False")[["Username", "email"]])
                return
            print(self.df.query("loggedin==True")[["Username", "email"]])
            return

    def run(self):
        self.connect(all=True)
        while True:
            print(
                "Commands:\n"
                + "[d]isplay\n"
                + "[c]onnect\n"
                + "[r]egister\n"
                + "[l]ogin\n"
                + "[d]isconnect\n"
                + "[q]uit"
            )
            text = input(">>")
            if "d" == text[0]:
                self.display()
            elif "c" == text[0]:
                self.connect(all=False)
            elif "r" == text[0]:
                self.register()
            elif "l" == text[0]:
                self.login()
            elif "d" == text[0]:
                self.disconnect()
            elif "q" == text[0]:
                break
        print("exiting...")
        self.disconnect(True)


def auto(manager):
    print("Starting auto")
    print("--------------------------------")
    print("Connecting...")
    print("--------------------------------")
    manager.connect(True)
    sleep(1)
    print("--------------------------------")
    print("Registering...")
    print("--------------------------------")
    manager.register(True)
    print(manager.df)
    sleep(1)
    print("--------------------------------")
    print("Disconnecting...")
    print("--------------------------------")
    manager.disconnect(True)
    sleep(1)
    print("--------------------------------")
    print("Connecting...")
    print("--------------------------------")
    manager.connect(True)
    sleep(1)
    print("--------------------------------")
    print("Logging in...")
    print("--------------------------------")
    manager.loggin(True)
    print(manager.df)
    sleep(1)
    print("--------------------------------")
    print("few disconnects...")
    print("--------------------------------")
    for i in range(5):
        manager.users[i].disconnect()
        manager.df.connected[i] = False
        manager.df.loggedin[i] = False
        sleep(1)
    print(manager.df)
    print("--------------------------------")
    print("Reconnecting and reloging...")
    print("--------------------------------")
    for i in range(5):
        manager.users[i].connect()
        manager.df.connected[i] = True
        manager.users[i].login()
        manager.df.loggedin[i] = True
        sleep(1)
    print(manager.df)
    sleep(1)
    print("--------------------------------")
    print("Disconnecting...")
    print("--------------------------------")
    manager.disconnect(True)


def main():
    manager = Manager()
    text = input("[m]anual or [a]uto (default is auto): ")
    if "m" not in text:
        auto(manager)
    else:
        manager.run()


if __name__ == "__main__":
    main()
