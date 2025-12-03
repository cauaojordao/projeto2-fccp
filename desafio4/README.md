# Desafio 4 - Microsserviços Independentes

## Descrição

Este desafio demonstra dois microsserviços independentes que se comunicam via HTTP:
- **Users Service**: Fornece dados de usuários na porta 8080
- **Info Service**: Consome o Users Service e exibe informações combinadas na porta 8081 
- Aqui não é necessário o uso de banco de dados ou docker-compose, os dados são simulados em memória.

## Funcionamento

1. Users Service expõe endpoints para listar e buscar usuários
2. Info Service faz requisições HTTP para o Users Service e combina as informações
3. Os serviços se comunicam usando o nome do container como hostname

## Instruções de Execução

```bash
# Criar rede e iniciar serviços
./run.sh

# Users Service
curl http://localhost:8080/users

# Info Service
curl http://localhost:8081/info

# Parar serviços
./stop.sh
```

## Endpoints

### Users Service (porta 8080)
- `GET /users`: Lista todos os usuários
- `GET /users/{id}`: Busca usuário por ID

### Info Service (porta 8081)
- `GET /info`: Lista usuários com informações combinadas
- `GET /info/{id}`: Informações detalhadas de um usuário
