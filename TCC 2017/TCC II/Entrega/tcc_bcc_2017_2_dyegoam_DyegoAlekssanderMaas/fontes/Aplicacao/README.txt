Instruções para configuração do Mosquitto: https://store.docker.com/images/eclipse-mosquitto
docker pull eclipse-mosquitto
docker run -it -p 1884:1884 -p 9001:9001 eclipse-mosquitto

Instruções para configuração do postgres:
-editar connection string no app.config