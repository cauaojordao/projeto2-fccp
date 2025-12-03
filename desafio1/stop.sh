echo "parando e removendo containers..."

docker stop desafio1-server desafio1-client 2>/dev/null
docker rm desafio1-server desafio1-client 2>/dev/null
docker network rm desafio1-network

echo "containers e rede removidos."

