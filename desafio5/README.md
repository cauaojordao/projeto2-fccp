# Desafio 5 - Microsserviços com API Gateway

## Descrição

Este desafio demonstra uma arquitetura com **API Gateway** centralizando o acesso a dois microsserviços:
- **Users Service**: Fornece dados de usuários
- **Orders Service**: Fornece dados de pedidos
- **API Gateway**: Ponto único de entrada que orquestra chamadas aos serviços

## Funcionamento

1. API Gateway é o ponto único de entrada para todos os clientes (web, mobile, etc...)
2. Clientes fazem requisições apenas para o Gateway (porta 8080)
3. Gateway roteia requisições para os microsserviços apropriados:
   - `/users` -> Users Service
   - `/orders` -> Orders Service

## Instruções de Execução

```bash
# Construir e iniciar todos os serviços
./up.sh

# Testar gateway
curl http://localhost:8080/users
curl http://localhost:8080/orders

# Parar serviços
./down.sh
```

## Endpoints do Gateway

Todos os endpoints são acessados através do Gateway na porta 8080:

- `GET /users`: Lista todos os usuários
- `GET /users/{id}`: Busca usuário por ID
- `GET /orders`: Lista todos os pedidos
- `GET /orders/{id}`: Busca pedido por ID
- `GET /orders/user/{userId}`: Lista pedidos de um usuário
