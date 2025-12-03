# Desafio 2 - Volumes e Persistência

## Descrição

Este desafio demonstra a persistência de dados usando volumes Docker com SQLite. Os dados são armazenados em um volume
Docker, permitindo que persistam mesmo após a remoção do container.

## Funcionamento

1. Um volume Docker é criado para armazenar o banco de dados SQLite
2. O container `app` cria e gerencia o banco de dados no volume
3. Mesmo após remover o container, os dados persistem no volume

## Instruções de Execução

```bash
# Executar aplicação
./run.sh

# Criar usuário
curl -X POST http://localhost:8080/users \
  -H "Content-Type: application/json" \
  -d '{"name":"João","email":"joao@example.com"}'

# Ver usuários
curl http://localhost:8080/users

# Parar container (dados persistem no volume)
./stop.sh

# Executar novamente e verificar persistência
./run.sh
curl http://localhost:8080/users

# Teste completo de persistência
./test.sh

# Limpar tudo (incluindo volume)
./clean.sh
```

## Verificar Volume

```bash
# Ver informações do volume
docker volume inspect desafio2-data

# Ver conteúdo do volume
docker run --rm -v desafio2-data:/data alpine ls -la /data

# Ver logs do container
docker logs desafio2-app
```

## Endpoints

- `GET /`: Informações sobre a aplicação
- `POST /users`: Cria um novo usuário
- `GET /users`: Lista todos os usuários
