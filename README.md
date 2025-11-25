# Product Management

## 1 - Arquitetura
O projeto foi orgnizado nas seguintes camadas:
```
ProductManagement.Domain          → Regras de negócio, entidades e contratos
ProductManagement.Application     → Casos de uso, DTOs e serviços de aplicação
ProductManagement.Infrastructure  → Persistência (EF Core), conexões e RabbitMQ
ProductManagement.API             → Controllers, DI e exposição REST
ProductManagement.Tests           → Testes unitários (xUnit + Moq)
```
Principais decisões:

- Domain contém apenas regras e entidades puras.
- Infrastructure implementa repositórios concretos, DbContext e serviços de mensageria.
- Application orquestra regras de forma independente do transporte/dados.
- API fica fina, mantendo controllers apenas como endpoints, sem regra de negócio dentro.
- Comunicação assíncrona ocorre via RabbitMQ, usando fila product.created.

## 2 - Fluxo do sistema
### Criação do produto
- POST /products
- Produto é persistido no banco via EF Core
- Evento product.created é publicado no RabbitMQ
- Um serviço baseado em RabbitMqConsumer<T> escuta a fila e armazena logs na tabela ProductEvents, garantindo rastreabilidade.

## 3 - Execução
### Clonagem do repositório
```
git clone https://github.com/l-chocolate/ProductManagement
cd ProductManagement
```
### Subir ambiente
```
docker compose up --build
```
### Acesso a API
A API sobe em:
```
http://localhost:8080
```
Swagger UI disponível em:
```
http://localhost:8080/swagger
```

## 4 - Testes
O projeto contém testes para:
- Controllers
- Services
- MessageHandler
- Mock de RabbitMQ
- Lógica de domínio

Para executar:
```
dotnet test
```

### 5 - Front-End
Foi criado um projeto em Angular que pode ser acessado em:
```
http://localhost:4200/products
```

