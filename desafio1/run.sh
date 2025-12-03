docker network create desafio1-network 2>/dev/null || echo "Rede já existe"

docker build -t desafio1-server ./server

docker build -t desafio1-client ./client

echo "build concluído"

docker run -d --name desafio1-server --network desafio1-network -p 8080:8080 desafio1-server

sleep 2

docker run -d --name desafio1-client --network desafio1-network \
  -e SERVER_URL=http://desafio1-server:8080 \
  -e INTERVAL=5 \
  desafio1-client

echo "conteineres rodando!"
echo ""
echo "ver logs do servidor: docker logs -f desafio1-server"
echo "ver logs do cliente: docker logs -f desafio1-client"

