Desenho de Solução 

Escopo da solução:

Desenhar arquitetura para suportar e disponibilizar os serviços de manutenção das informações do fluxo de caixa diário, com lançamentos 
de entrada e saída) e a geração de relatório do saldo consolidado por dia.

Requisitos funcionais:

- Serviço que faça a manutenção dos lançamentos;
- Serviço que gere o relatório consolidado diário.

Requisitos não-funcionais:

Não foram elencados requisitos não funcionais.
Mas, por tratar-se de um exercício, aplicamos ao caso concreto a escolha de requisitos de natureza não-funcionais, quanto a compatibilidade; escalabilidade; resiliência; confiança e segurança. Deixamos
de conhecer os requisitos quanto a performance; integração; ambiente e as eventuais restrições.

Quanto aos recursos de arquitetura:

Não há outras restrições ao uso da tecnologia senão ao uso do ambiente nuvem ("cloud") com recursos da Amazon WebServices - AWS

Quanto a solução:

O desenho da solução baseou-se, como dito, nos pilares da segurança; da possibilidade de escalabilidade horizontal; da disponilidade distribuída (nuvem) e redundância.
Além disso, o trabalho não se orientou em relação ao custo benefício da sua aplicação e, primou-se pela utilização de serviços gerenciados (Software as a Service - SaaS).
Seria possível implementar a solução utilizando-se serviços de plataforma (Platform as a Service - PaaS) ou mesmo com apenas serviços de infraestrutura (Infrastructure as a Service - IaaS) mas optou-se
pelo ambiente gerenciado ao invés do ambiente com gerenciamento próprio. Para distinguir, o ambiente gerenciado ou SaaS é de responsabilidade do provedor do serviço, principalmente quanto a sua disponibilidade,
manutenção, atualização e monitoramento. Exemplo é o serviço de banco de dados (Amazon RDS); do Amazon Fargate para gerenciamento de containers ou mesmo do Elastic Container Service para escalonamento horizontal de aplicações.
O ambiente com gerenciamento, onde o mesmo é projeto com componentes de PaaS (banco de dados em máquinas virtuais) e IaaS, como máquinas virtuais (EC2), sub-redes, etc que dependem de pessoal capacitado para
criar a infraestrutura e manter o seu gerenciamento, prevendo manutenção preventivas e intervenções de contingência, e a execução de "patchs" de atualização dos recursos que suportam a aplicação.

O projeto arquitetural aplica-se também a um ambiente distribuído, com a utilização da padrão de microsserviços.
Mesmo que a aplicação seja projetada para o seu escalonamento horizontal, a estratégia dependerá de configuração. Logo, procurou-se definir uma arquitetura que possibilite o fácil escalonamento, caso necessário. Isso porquê,
de início, entendemos que não há requisitos de escalonamento e, por conta das funcionalidades, o aplicativo não possui o respectivo apelo, visto que as operações de manutenção de lançamentos do movimento de caixa
e a geração de relatório não justificaria, por si só, a sua escalabilidade ou mesmo a necessidade de ser distribuído, podendo ser construído com arquitetura de monolito.

Mas, para fins de exercício, optou-se pelo modelo de microsserviços por conta da experiência atual do cliente nas demais aplicações que possui.
Mesmo critério foi utilizado para se eleger o ambiente de nuvem para o qual o mesmo foi planejado.

No quesito segurança, aplicamos conceitos e a previsão de operacionalização com autenticação OIDC (OpenID Connect) que permite um ambiente federado de autenticação e autorização, com a utilização de recurso de login único (SSO - Single Sign On) ou mesmo de
autenticação em provedores SAML como os existentes em redes sociais (Facebook, Google, etc).
Mesmo não sendo um ambiente conectado a provedores de autenticação externos (Facebook, Google, etc), isso não impede a sua operacionalização com a utilização dos recursos do Amazon Identity Center, que pode agregar usuários, grupos e recursos de acesso, em
substituição ao referido provedor externo.
Nesse contexto, sabendo que o ambiente de execução foi moldado para uma realidade diferente da implementação da aplicação, visto que esse ambiente não existe de fato, os testes e verificações locais de segurança ficaram prejudicados por não
haver um ambiente federado que pudesse suportar as rotinas de testes com o tráfego de tokens de sessão e claims.
E, desenvolver uma aplicação de autenticação e autorização que emulasse o comportamento estaria fora do escopo e tempo desse desafio.

Após as considerações acima, passamos a justificar o uso de cada recurso e ao final, descrever o fluxo de utilização dos mesmos.

Quanto aos recursos internos para garantir segurança de acesso, descobrimento dos serviços, guarda de senhas e certificados:

Amazon DNS - Necessário um serviço de DNS (Domain Name System) próprio para evitar a captura de servidores de e-mail e o envio de mensagens maliciosas ao e-mail corporativo através dos registros de domínio público. 

Amazon Certificate Manager - O serviço oferece a guarda e consulta de certificados para o estabelecimento de conexões seguras, através do protocolo HTTPS, garantindo a troca de informações através de mTLS (Mutual Transport Layer Security).

Amazon Secrets Manager - A aplicação precisará obter credenciais para o acesso aos recursos, como banco de dados, etc. Logo, há a previsão da contratação de um serviço de guarda e acesso seguro dessas informações
para que as mesmas não fiquem armazenadas na aplicação ou nas suas configurações, o que facilitaria a sua captura desautorizada e o seu uso fora da aplicação, para causar vazamentos ou prejuízos internos.

Amazon Shield - Oferece uma proteção contra ataques de negação de serviços (Distributed Denial of Service - DDoS) na borda ou entrada do ambiente corporativo.

Amazon WAF - WAF é acrônimo de "web application firewall" e como o nome sugere, é um firewal de entrada que permite a definição de políticas de ingresso e egresso de pacotes entre o ambiente externo e interno.

Amazon Elastic IP - Oferece um IP estático fixo que garante que, se os serviços se reiniciarem, eles permaneçam endereçáveis e, portanto, acessíveis externamente. Quando os serviços são reiniciados, sejam manualmente ou automaticamente, por
falhas, um novo IP é fornecido ao serviço e o mesmo endereça a máquina para o acesso externo. Quem resolve o domínio para o endereço IP é o servidor de DNS (Domain Name System). Logo, se o IP mudar, as máquinas não serão mais endereçadas por conta
da mudança e o serviço terá a aparência de ter sido interrompido às vistas dos usuários. Assim, o recurso de IP elástico permite o escalonamento futuro e garante a acessibilidade dos serviços publicados para a internet.


Quanto aos fluxos:

Estamos prevendo dois fluxos no desenho: um para os acessos aos serviços disponibilizados e, outro, de acesso e publicação das imagens de deploy das evoluções dos serviços, para serem atualizados no gerenciador de containers caso
a equipe de desenvolvimento disponibilize nova versão da aplicação.

No fluxo de consumo dos serviços, que inicia-se com o usuário através dos canais disponíveis nos equipamentos (devices) realiza a sua autenticação para obter um token de acesso.
Essa operação acontece mediante utilização do serviço do recurso do Amazon Cognito, que é um serviço de autenticação e autorização que permite o acesso federado, o login único (SSO - Single Sign On) e a autenticação e autorização
por provedor externo (Facebook, Google, etc), onde delega-se a esses provedores a tarefa de validar as credenciais do usuário e informar os recursos que poderá acessar, sem que a organização precise gerenciar esses aspectos, ou mesmo, 
manter esses serviços e credenciais.

Essa etapa de autenticação e autorização faz uso dos serviços do Amazon Identity Center para manter credenciais dos usuários ou para criar políticas de acesso, caso não seja adotado um Provedor de Identidades.
É previsto também o serviço do Amazon CloudTrail para realizar o rastreamento dos acessos requeridos e concedidos, bem como para identificar algum ataque malicioso.

Uma vez autenticado e de posse do token de acesso, o usuário poderá consumir os serviços da API, realizando a requisição adequada que passará pelo gateway de internet (IGW - Internet Gateway) que destinará a requisição
para a aplicação de acordo com o caminho externo; passando pelo balanceador de carga (ALB - Application Load Balance), que elege a instância da aplicação que possui menos carga de trabalho.

As aplicações (API de serviços) são executadas por intermédio do serviço do Amazon Fargate, que é um provedor de infraestrutura para a execução de containers. Esse containers, no caso, onde rodam as aplicações, encontram-se
dentro de um conjunto (cluster) de containers, gerenciados por um serviço de escalonamento, chamado ECS (Elastic Container Service). O serviço do ECS é responsável por levantar quantas instâncias da aplicação forem 
configuradas, ou mesmo baixá-las em caso de atualizações ou problemas, bem como de descalonamento.
Assim, se a estratégia for de escalar horizontalmente as aplicações, que consiste em replicar a aplicação para conferir disponibilidade e resiliência, o ECS é configurado com o número de instâncias necessárias, os recursos necessários, bem
como, dos critérios de escalonamento, quando for automático.

Tanto o Amazon Fargate com ECS executam num contexto de uma sub-rede pública (acessível pela internet), com endereçamento limitado.

As instâncias em execução acessam em concorrência o serviço da Amazon RDS (Relational Database Service), que consiste num serviço (Saas) de banco de dados relacional, no caso, com a engine do PostgreSQL. O serviço do RDS é
criado para operar no modo Multi-AZ, que consiste na configuração de um banco primária, que é replicado de forma síncrona numa segunda instância, garantindo redundância de dados e alta disponibilidade.
Como dito, o ambiente da aplicação é escalonável, mas depende da adoção das estratégias frente a necessidade. Assim, caso haja a opção pelo escalonamento da aplicação, entendo que o banco de dados não deve sofrer
escalonamento horizontal das suas instâncias por conta da integridade dos seus dados, há de se adotar outra estratégia para o aumento da capacidade do banco de dados, como a sua segregação por domínio, a sua clusterização
em partições, ou mesmo, a realização do processo de "sharding" das informações.
De nada adianta escalar aplicações para suportar um volume maior de requisições, se as mesmas acessam o mesmo banco de dados, sobrecarregando as conexões e os recursos do gerenciador de banco de dados.

===================================================================================================================

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
