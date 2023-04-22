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
        # with open(self.username + ".txt", "w") as f:
        #     f.write("")
        self.connected = False
        self.loggedin = False
        self.querying = False

    def connect(self, host="", port=0):
        if not self.connected:
            self.s.connect((host, port))
            # self.process = multiprocessing.Process(target=self.recieve, args=())
            # self.process.start()
            self.connected = True
            print(self.username, " is connected")
        else:
            print(self.username, " is already connected")

    def register(self):
        self.s.sendall(
            ("1/" + self.username + "*" + self.password + "*" + self.email).encode()
        )
        print(
            "sent: ",
            "1/" + self.username + "*" + self.password + "*" + self.email,
            " as ",
            ("2/" + self.username + "*" + self.password + "*" + self.email).encode(),
        )
        self.loggedin = True
        self.querying = True
        data = self.s.recv(1024)
        if len(data) > 0:
            text = repr(data.decode())
            if "1" == text[0]:
                print(self.username, " register response is:", text)
            elif "2" == text[0]:
                print(self.username, " login response is:", text)
            elif "3" == text[0]:
                print(self.username, " ranking is response is:", text)
            elif "4" == text[0]:
                print(self.username, " Connected list response is:", text)
            elif "5" == text[0]:
                print(self.username, " new match response is:", text)

            self.querying = False

    def login(self):
        self.s.sendall(("2/" + self.username + "*" + self.password).encode())
        print(
            "sent: ",
            "2/" + self.username + "*" + self.password,
            " as ",
            ("2/" + self.username + "*" + self.password).encode(),
        )
        self.loggedin = True
        self.querying = True
        data = self.s.recv(1024)
        if len(data) > 0:
            text = repr(data.decode())
            if "1" == text[0]:
                print(self.username, " register response is:", text)
            elif "2" == text[0]:
                print(self.username, " login response is:", text)
            elif "3" == text[0]:
                print(self.username, " ranking is response is:", text)
            elif "4" == text[0]:
                print(self.username, " Connected list response is:", text)
            elif "5" == text[0]:
                print(self.username, " new match response is:", text)

            self.querying = False
    def recieve(self):
        i = 0
        while True:
            
            if i > 1000:
                self.querying = False
                i = -1
            i += 1

    def disconnect(self):
        if self.connected:
            i = 0
            while self.querying and i < 100:
                i += 1
                sleep(0.1)

            disconect_msg = "0/"
            self.s.sendall(disconect_msg.encode())
            # self.process.terminate()
            self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
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
        self.df = self.df.assign(connected=False, loggedin=False, querying=False)

    def update_query(self):
        for n, user in enumerate(self.users):
            self.df.querying[n] = user.querying

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
            sleep(0.5)

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
            sleep(0.5)

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
            user.connect("192.168.56.102", 9050)
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
            self.update_query()
        print("exiting...")
        self.disconnect(True)


def auto(manager):
    print("Starting auto")
    print("--------------------------------")
    print("Connecting...")
    print("--------------------------------")
    manager.connect(True)
    print("--------------------------------")
    print("Registering...")
    print("--------------------------------")
    manager.register(True)
    manager.update_query()
    print(manager.df)
    input("Disconect")
    print("--------------------------------")
    print("Disconnecting...")
    print("--------------------------------")
    manager.disconnect(True)
    sleep(10)
    print("--------------------------------")
    print("Connecting...")
    print("--------------------------------")
    manager.connect(True)
    sleep(10)
    print("--------------------------------")
    print("Logging in...")
    print("--------------------------------")
    manager.loggin(True)
    manager.update_query()
    print(manager.df)
    sleep(10)
    input("Continue")
    print("--------------------------------")
    print("few disconnects...")
    print("--------------------------------")
    for i in range(5):
        manager.users[i].disconnect()
        manager.df.connected[i] = False
        manager.df.loggedin[i] = False
        sleep(1)
    manager.update_query()
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
    manager.update_query()
    print(manager.df)
    sleep(10)
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
