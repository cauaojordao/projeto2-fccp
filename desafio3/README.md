# Desafio 3 - Docker Compose Orquestrando Serviços

## Descrição

Este desafio demonstra a orquestração de múltiplos serviços usando Docker Compose:
- **Web**: API .NET 8 que gerencia produtos
- **DB**: Banco de dados PostgreSQL
- **Cache**: Redis para cache de dados

## Funcionamento

1. Docker Compose orquestra os três serviços
2. O serviço `web` depende de `db` e `cache`
3. Todos os serviços se comunicam através de uma rede Docker interna
4. PostgreSQL usa volume para persistência de dados
5. A API usa Redis para cachear consultas de produtos

## Instruções de Execução

```bash
# Construir e iniciar todos os serviços
./up.sh

# Criar produto
curl -X POST http://localhost:8080/products -H "Content-Type: application/json" -d '{"name":"Notebook","price":2500.00}'

# Listar produtos
curl http://localhost:8080/products

# Ver logs
docker-compose logs -f

# Parar serviços
./down.sh

# Parar e remover volumes (limpar dados)
./clean.sh
```

## Endpoints
- `POST /products`: Cria um novo produto
- `GET /products`: Lista todos os produtos (com cache Redis)
