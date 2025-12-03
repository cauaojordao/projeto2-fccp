echo "parando e removendo containers..."

docker stop users-service info-service 2>/dev/null
docker rm users-service info-service 2>/dev/null
docker network rm desafio4-network

echo "containers e rede removidos"
