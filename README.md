Quanto ao modelo arquitetural:
==================================================================

A proposta de solução de arquitetura encontra-se em:

CashFlow\Models\Solution Design.drawio
CashFlow\Models\Solution Design.pdf
CashFlow\Models\Solution Design.png


Inicialização da aplicação:
==================================================================

Para que os dados sejam persistidos, é necessário criar a pasta na máquina local:

C:\cashflow\postgresql

Para construir o projeto:

- Acessa a pasta de contexto da aplicação (considerando que você está na raiz do repositório):

cd .\Application\CashFlow

- Executar o comando para construir e levantar os serviços:

docker-compose up -d

- Para baixar os serviços, execute:

docker-compose down


Procedimento de teste:
==================================================================

- Necessário levantar o serviço do PostgreSQL:

cd .\Application\CashFlow
docker-compose up -d dbpostgres

- Executar rotinas de teste:

cd .\Application\CashFlow
dotnet test

Consumo da API:
==================================================================

* Consulta por ID:
GET /api/v1/cashmoviment/1

* Adiciona movimento caixa:
POST /api/v1/cashmoviment

Exemplo payload:
{
	"id": 0,
	"createAt": "2023-08-21T09:32:12.000Z",
	"historic": "Recebimento fornecedor cod.: 8993",
	"value": 564.98,
	"nature": 0
}

* Atualiza movimento caixa:
PATCH /api/v1/cashmoviment

Exemplo payload:
{
	"id": 1,
	"createAt": "2023-08-21T09:32:12.000Z",
	"historic": "Recebimento fornecedor cod.: 8993",
	"value": 564.98,
	"nature": 0
}

* Remove um movimento do caixa:
DELETE /api/v1/cashmoviment/1

* Consulta saldo do caixa consolidado por dia:
GET /api/v1/cashmoviment/balance?start=21/08/2023&end=22/08/2023

- A especificação da API pode ser consultada em:

http://localhost:5047/swagger/index.html

* No projeto, encontra-se o arquivo JSON da especificação OpenAPI.


Data: 23/08/2023