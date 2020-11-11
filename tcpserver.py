#!/usr/bin/env python

# imports object socket for use
# Documentation: https://docs.python.org/2/library/socket.html
import socket

# IP address that the client needs to connect to
TCP_IP = '127.0.0.1'

# the port that the client will need to connect to
TCP_PORT = 50005

# Sets the size that data sent by the client can be.
BUFFER_SIZE = 20  # Normally 1024, but we want fast response

# creates a socket object named s. Sockets are used to connect two computers.
#
# Documentation for Socket can be found at: "https://docs.python.org/2/library/socket.html"
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# binds the socket to the address and port. TCP_IP and TCP_PORT
s.bind(('', TCP_PORT))

# "Listen for connections made to the socket. The backlog argument specifies the maximum
# number of queued connections and should be at least 0; the maximum value is system-dependent
# (usually 5), the minimum value is forced to 0."
#
# from documentation on socket.listen(backlog) it essentially sets the server to wait for a new
# connection and then continues when it gets 1.
s.listen(1)

# accept returns two variables, conn (which is a new socket), and address (the IP address of the
# connection)
#
# Accept() is used to accept a connection and define variables that helps to reply to the connection
conn, addr = s.accept()

# prints the address that connected to the server
print ('Connection address:', addr)

# continious loop, loops until data has a value
while 1:
    # when server recieves data from a connection it will store it in data,
    # which is restricted in size by BUFFER_SIZE
    data = conn.recv(BUFFER_SIZE)
    length = 0
    count = 0
    # if there is no data break out of the loop.
    if data:
        
        # gets the remote address for which the socket is connected to.
        #s.getpeername()
        
        # gets the length of data
        length = len(data)
        length = str(length)
        
        # gets the word count of data
        count = len(data.split())
        count = str(count)
        
        # prints the data recieved by the client
        print ("received data:", data, "Words: ", count, "Length: ", length)
        break
    
# returns the data to confirm the data wasn't lost or corrupted
conn.send(data)  # echo

#sends the length of the data back to client
conn.send(length)

# sends the number of words back to the client
conn.send(count)
    
# ends the connection with the client.
conn.close()
