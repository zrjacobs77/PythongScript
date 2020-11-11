
#!/usr/bin/env python

# imports the object socket for use.
# Documentation: https://docs.python.org/2/library/socket.html
import socket

# imports system 
import sys

# IP address of the server that we will connnect to
try:
    TCP_IP = sys.argv[1]

# if commandline input is null get it from the keyboard
except IndexError:
    TCP_IP = input("IP Address: ")

# the port of the server that we will connect to
try:
    TCP_PORT = sys.argv[2]

# if commandline input is null get it from the keyboard
except IndexError:
    TCP_PORT = int(input("Port #: "))
    

# when the server sends the data it will have to be within the size set in BUFFER_SIZE
BUFFER_SIZE = 10000

# data to be sent 
MESSAGE = input("Message: ")  #'Hello, World!'
b = bytes(MESSAGE,'utf-8')

# creates a socket object named s.
# Documentation for Socket can be found at: https://docs.python.org/2/library/socket.html
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# sends a connection request to the server at TCP_IP with port # TCP_PORT
s.connect((TCP_IP, TCP_PORT))

# sends the string in MESSAGE
s.send(b)

while 1:
    # the server will send data back it will go into "data" with a size equal to BUFFER_SIZE
    data = s.recv(BUFFER_SIZE)

    # recieves the length of the data
    length = s.recv(BUFFER_SIZE)

    # recieves the word count of the data
    count = s.recv(BUFFER_SIZE)
    
    if data and length and count:
        print("Data: ", data, " length: ", length, " count: ",count)
        break

# ends connection with the server
s.close()

#prints the data the server sends back
print ("received data:", data)
