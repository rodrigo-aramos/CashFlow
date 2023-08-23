Quanto ao modelo arquitetural:<br>
==================================================================<br>
<br>
A proposta de solução de arquitetura encontra-se em:<br>
<br>
CashFlow\Models\Solution Design.drawio<br>
CashFlow\Models\Solution Design.pdf<br>
CashFlow\Models\Solution Design.png<br>
<br>
<br>
Inicialização da aplicação:<br>
==================================================================<br>
<br>
Para que os dados sejam persistidos, é necessário criar a pasta na máquina local:<br>
<br>
C:\cashflow\postgresql<br>
<br>
Para construir o projeto:<br>
<br>
- Acessa a pasta de contexto da aplicação (considerando que você está na raiz do repositório):<br>
<br>
cd .\Application\CashFlow<br>
<br>
- Executar o comando para construir e levantar os serviços:<br>
<br>
docker-compose up -d<br>
<br>
- Para baixar os serviços, execute:<br>
<br>
docker-compose down<br>
<br>
<br>
Procedimento de teste:<br>
==================================================================<br>
<br>
- Necessário levantar o serviço do PostgreSQL:<br>
<br>
cd .\Application\CashFlow<br>
docker-compose up -d dbpostgres<br>
<br>
- Executar rotinas de teste:<br>
<br>
cd .\Application\CashFlow<br>
dotnet test<br>
<br>
Consumo da API:<br>
==================================================================<br>
<br>
* Consulta por ID:<br>
GET /api/v1/cashmoviment/1<br>
<br>
* Adiciona movimento caixa:<br>
POST /api/v1/cashmoviment<br>
<br>
Exemplo payload:<br>
{<br>
<space><space><space>"id": 0,<br>
	"createAt": "2023-08-21T09:32:12.000Z",<br>
	"historic": "Recebimento fornecedor cod.: 8993",<br>
	"value": 564.98,<br>
	"nature": 0<br>
}<br>
<br>
* Atualiza movimento caixa:<br>
PATCH /api/v1/cashmoviment<br>
<br>
Exemplo payload:<br>
{<br>
	"id": 1,<br>
	"createAt": "2023-08-21T09:32:12.000Z",<br>
	"historic": "Recebimento fornecedor cod.: 8993",<br>
	"value": 564.98,<br>
	"nature": 0<br>
}<br>
<br>
* Remove um movimento do caixa:<br>
DELETE /api/v1/cashmoviment/1<br>
<br>
* Consulta saldo do caixa consolidado por dia:<br>
GET /api/v1/cashmoviment/balance?start=21/08/2023&end=22/08/2023<br>
<br>
- A especificação da API pode ser consultada em:<br>
<br>
http://localhost:5047/swagger/index.html<br>
<br>
* No projeto, encontra-se o arquivo JSON da especificação OpenAPI.<br>
<br>
<br>
Data: 23/08/2023<br>
<br>
<br>