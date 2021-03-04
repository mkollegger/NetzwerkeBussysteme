#// (C) 2020 Michael Kollegger
#// 
#// Kontakt mike@fotec.at / www.fotec.at
#// 
#// Erstversion vom 04.03.2020 20:27
#// Entwickler      Michael Kollegger
#// Projekt         NeBu

import socket
import sys

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Connect the socket to the port where the server is listening
server_address = ('127.0.0.1', 13000)
print ('connecting to %s port %s' % server_address)
sock.connect(server_address)
try:
    
    # Send data
    message = 'Hallo Py.'
    print ('sending "%s"' % message)
    
    b = bytearray()
    b.extend(map(ord, message))
    sock.sendall(b)

    # Look for the response
    amount_received = 0
    amount_expected = len(message)
    
    while amount_received < amount_expected:
        data = sock.recv(16)
        amount_received += len(data)
        print('received "%s"' % data)

finally:
    print ('closing socket')
    sock.close()