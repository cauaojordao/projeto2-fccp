# Desafio 1 - Containers em Rede

## Descrição

Este desafio demonstra a comunicação entre dois containers através de uma rede Docker customizada:
- **Container Server**: Servidor web .NET 8 Minimal API rodando na porta 8080
- **Container Client**: Cliente que faz requisições HTTP de 10 em 10 segundos para o servidor

## Funcionamento

1. Uma rede Docker customizada é criada (`desafio1-network`)
2. O container `server` executa um servidor web Minimal API na porta 8080
3. O container `client` faz requisições HTTP periódicas (a cada 10 segundos) para o servidor
4. Os containers se comunicam usando o nome do serviço como hostname (`server`)

## Instruções de Execução

```bash
# Criar a rede e subir os containers
./run.sh

# Ver logs do servidor (em outro terminal)
docker logs -f desafio1-server

# Ver logs do cliente (em outro terminal)
docker logs -f desafio1-client

# Testar servidor
curl http://localhost:8080

# Parar e remover
./stop.sh
```

## Endpoints do Servidor

- `GET /`: Retorna mensagem "Servidor rodando!"

