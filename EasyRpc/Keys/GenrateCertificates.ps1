openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout Server.key -out Server.crt
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout Client.key -out Client.crt