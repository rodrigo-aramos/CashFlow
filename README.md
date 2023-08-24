__Quanto ao modelo arquitetural:__<br>
==================================================================<br>
<br>
A proposta de solução de arquitetura encontra-se em:<br>
<br>
CashFlow\Models\Solution Design.drawio<br>
CashFlow\Models\Solution Design.pdf<br>
CashFlow\Models\Solution Design.png<br>
<br>
<br>
__Inicialização da aplicação__:<br>
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
<br>
- Para baixar os serviços, execute:<br>
<br>
docker-compose down<br>
<br>
<br>
<br>
__Procedimento de teste__: <br>
==================================================================<br>
<br>
- Necessário levantar o serviço do PostgreSQL:<br>
<br>
cd .\Application\CashFlow <br>
docker-compose up -d dbpostgres<br>
<br>
<br>
- Executar rotinas de teste:<br>
<br>
cd .\Application\CashFlow <br>
dotnet test<br>
<br>
<br>
__Consumo da API__: <br>
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
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"id": 0,<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"createAt": "2023-08-21T09:32:12.000Z",<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"historic": "Recebimento fornecedor cod.: 8993",<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"value": 564.98,<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"nature": 0<br>
}<br>
<br>
* Atualiza movimento caixa:<br>
PATCH /api/v1/cashmoviment<br>
<br>
Exemplo payload:<br>
{<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"id": 1,<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"createAt": "2023-08-21T09:32:12.000Z",<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"historic": "Recebimento fornecedor cod.: 8993",<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"value": 564.98,<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"nature": 0<br>
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
