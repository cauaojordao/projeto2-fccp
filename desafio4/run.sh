docker network create desafio4-network 2>/dev/null || echo "Rede já existe"

docker build -t desafio4-users-service ./users-service
docker build -t desafio4-info-service ./info-service

docker run -d --name users-service --network desafio4-network -p 8080:8080 desafio4-users-service

sleep 2

docker run -d --name info-service --network desafio4-network -p 8081:8081 \
  -e USERS_SERVICE_URL=http://users-service:8080 \
  desafio4-info-service

echo "serviços rodando!"
echo "Users Service: http://localhost:8080"
echo "Info Service: http://localhost:8081"
