__Desenho de Solução__
<br>
<br>
__Escopo da solução__:
<br>
<br>
Desenhar arquitetura para suportar e disponibilizar os serviços de manutenção das informações do fluxo de caixa diário, com lançamentos 
de entrada e saída, e a geração de relatório do saldo consolidado por dia.<br>
<br>
Requisitos funcionais:<br>
<br>
- Serviço que faça a manutenção dos lançamentos;<br>
- Serviço que gere o relatório consolidado diário.<br>
<br>
Requisitos não-funcionais:<br>
<br>
Não foram elencados outros requisitos não-funcionais, senão a linguagem da aplicação (NET C#) e o ambiente de nuvem da organização (AWS). Mas, por tratar-se de um exercício, aplicamos ao caso concreto a escolha de requisitos de natureza não-funcionais, quanto a compatibilidade; escalabilidade; resiliência; confiança e segurança. Deixamos de conhecer os requisitos quanto a performance; integração; ambiente e as eventuais restrições adicionadas pelos Stakeholders.<br>
<br>
Quanto aos recursos de arquitetura:<br>
<br>
Não há outras restrições ao uso da tecnologia senão ao uso do ambiente nuvem ("cloud") com recursos da Amazon WebServices - AWS.<br>
<br>
Quanto ao desenho da solução:<br>
<br>
O desenho da solução baseou-se, como dito, nos pilares da segurança; da possibilidade de escalabilidade horizontal; da disponilidade distribuida e redundância.<br>
Além disso, o trabalho não se orientou apenas no melhor custo benefício dos recursos escolhidos e, sim, também pela utilização de serviços gerenciados (Software as a Service - SaaS) em detrimento dos não gerenciados (Platform as a Service - PaaS / Infrastructure as a Service - IaaS). Seria possível implementar a solução utilizando-se serviços de plataforma (PaaS) ou mesmo com apenas serviços de infraestrutura (Infrastructure as a Service - IaaS) mas optou-se pelo ambiente gerenciado ao invés do ambiente com gerenciamento próprio. Para distinguir, o ambiente gerenciado (SaaS, ou semi-gerenciado: PaaS), o gerenciamento é de responsabilidade do provedor do serviço em nuvem, principalmente quanto a sua disponibilidade, manutenção, atualização e monitoramento. Exemplo é o serviço de banco de dados (Amazon RDS); do Amazon Fargate para gerenciamento de containers ou mesmo do Amazon ECS (Elastic Container Service) para escalonamento horizontal de aplicações, as quais o usuário preocupa-se apenas em implementar a sua utilização, sem preocupar-se com a sua criação, manutenção e monitoriamento.<br>
O ambiente com gerenciamento, onde o mesmo é projetado com componentes de PaaS (banco de dados em máquinas virtuais) e IaaS, como máquinas virtuais (EC2), sub-redes, etc que dependem de pessoal capacitado para criar a infraestrutura e manter o seu gerenciamento, provendo manutenções preventivas, corretivas e intervenções de contingência.<br>
<br>
O projeto arquitetural aplica-se também a um ambiente distribuido, com a utilização da padrão de microsserviços.<br>
Mesmo que a aplicação seja projetada para o seu escalonamento horizontal, a estratégia dependerá de configuração. Logo, procurou-se definir uma arquitetura que possibilite o fácil escalonamento, caso necessário. Isso porquê, de início, entendemos que não há requisitos de escalonamento e, por conta das funcionalidades, o aplicativo não possui a referida prerrogativa, visto que as operações de manutenção de lançamentos do movimento de caixa e a geração de relatório não justificaria, por si só, a sua escalabilidade ou mesmo a necessidade de ser distribuído, podendo ser construído com arquitetura de monolito. Claro que, se a aplicação for um produto SaaS, multi-clientes e com separação de dados (multi-tenant), o cenário mudaria e, então, estariamos diante de um cenário de microsserviços com estratégia de escalonamento.<br>
<br>
Mas, para fins de exercício, optou-se pelo modelo de microsserviços por conta da experiência atual do cliente nas demais aplicações que possui. O mesmo critério foi utilizado para se eleger o ambiente de nuvem para o qual o mesmo foi planejado.<br>
<br>
No quesito segurança, aplicamos os conceitos e a previsão de operacionalização com autenticação OIDC (OpenID Connect) que permite um ambiente federado de autenticação e autorização, com a utilização de recurso de login único (SSO - Single Sign On) ou mesmo de autenticação em provedores SAML como os existentes em redes sociais (Facebook, Google, etc).<br>
Mesmo não sendo um ambiente conectado a provedores de autenticação externos (Facebook, Google, etc), isso não impede a sua operacionalização com a utilização dos recursos do Amazon Identity Center, que pode agregar usuários, grupos e recursos de acesso, em substituição ao referido provedor externo.<br>
Nesse contexto, sabendo que o ambiente de execução foi moldado para uma realidade diferente da implementação da aplicação, visto que esse ambiente não existe de fato, os testes e verificações locais de segurança ficaram prejudicados por não haver um ambiente federado que pudesse suportar as rotinas de testes com o tráfego de tokens de sessão e claims. Além disso, desenvolver uma aplicação de autenticação e autorização que emulasse o comportamento estaria fora do escopo e tempo desse desafio.<br>
<br>
Após as considerações acima, passamos a justificar o uso de cada recurso e ao final, descrever o fluxo de utilização dos mesmos.<br>
<br>
Quanto aos recursos internos para garantir segurança de acesso, descobrimento dos serviços, guarda de senhas e certificados:<br>
<br>
Amazon DNS - Necessário um serviço de DNS (Domain Name System) próprio para evitar a captura de servidores de e-mail e o envio de mensagens maliciosas ao e-mail corporativo através dos registros de domínio público. <br>
<br>
Amazon Certificate Manager - O serviço oferece a guarda e consulta de certificados para o estabelecimento de conexões seguras, através do protocolo HTTPS, garantindo a troca de informações através de mTLS (Mutual Transport Layer Security).<br>
<br>
Amazon Secrets Manager - A aplicação precisará obter credenciais para o acesso aos recursos, como banco de dados, etc. Logo, há a previsão da contratação de um serviço de guarda e acesso seguro dessas informações para que as mesmas não fiquem armazenadas na aplicação ou nas suas configurações, o que facilitaria a sua captura desautorizada e o seu uso fora da aplicação, para causar vazamentos ou prejuízos internos.<br>
<br>
Amazon Shield - Oferece uma proteção contra ataques de negação de serviço (Distributed Denial of Service - DDoS) na borda ou entrada do ambiente corporativo.<br>
<br>
Amazon WAF - WAF é acrônimo de "web application firewall", e como o nome sugere, é um firewal de entrada que permite a definição de políticas de egresso e ingresso de pacotes entre o ambiente externo e interno.<br>
<br>
Amazon Elastic IP - Oferece um IP estático fixo que garante que, se os serviços se reiniciarem, eles permaneçam endereçáveis e, portanto, acessíveis externamente. Quando os serviços são reiniciados, sejam manualmente ou automaticamente, por falhas, um novo IP é fornecido ao serviço e o mesmo endereça a máquina para o acesso externo. Quem resolve o domínio para o endereço IP é o servidor de DNS (Domain Name System). Logo, se o IP mudar, as máquinas não serão mais endereçadas por conta
da mudança e o serviço terá a aparência de ter sido interrompido às vistas dos usuários, numa condição difícil de se tomar conhecimento, senão por reclamações dos usuários, apenas. Assim, o recurso de IP elástico permite o escalonamento futuro e garante a acessibilidade dos serviços publicados para a internet.
<br>
<br>
Quanto aos fluxos:<br>
<br>
Estamos prevendo dois fluxos no desenho: um para os acessos aos serviços disponibilizados e, outro, de acesso e publicação das imagens de deploy das evoluções dos serviços, para serem atualizados no gerenciador de containers caso a equipe de desenvolvimento disponibilize nova versão da aplicação.<br>
<br>
No fluxo de consumo dos serviços, que inicia-se com o usuário através dos canais disponíveis nos equipamentos, realizando a sua autenticação para obter um token de acesso e autorização de uso de recursos.<br>
Essa operação acontece mediante utilização do serviço do recurso do Amazon Cognito, que é um serviço de autenticação e autorização que permite o acesso federado, o login único (SSO - Single Sign On) e a autenticação e autorização por provedor externo (Facebook, Google, etc), onde delega-se a esses provedores a tarefa de validar as credenciais do usuário e informar os recursos que poderá acessar, sem que a organização precise gerenciar esses aspectos, ou mesmo, de manter esses serviços e credenciais.<br>
<br>
Essa etapa de autenticação e autorização faz uso dos serviços do Amazon Identity Center para manter credenciais dos usuários ou para criar políticas de acesso, caso não seja adotado um Provedor de Identidades.<br>
É previsto também o serviço do Amazon CloudTrail para realizar o rastreamento das sessões dos acessos realizados, bem como para identificar algum ataque malicioso.<br>
<br>
Uma vez autenticado e de posse do token de acesso, o usuário poderá consumir os serviços da API, realizando a requisição adequada que passará pelo gateway de internet (IGW - Internet Gateway) que destinará a requisição para a aplicação de acordo com o caminho externo; passando pelo balanceador de carga (ALB - Application Load Balance), que elege a instância da aplicação que possui menos carga de trabalho.<br>
<br>
As aplicações (API de serviços) são executadas por intermédio do serviço do Amazon Fargate, que é um provedor de infraestrutura para a execução de containers. Esse containers, no caso, onde rodam as aplicações, encontram-se dentro de um conjunto (cluster) de containers, gerenciados por um serviço de escalonamento, chamado ECS (Elastic Container Service). O serviço do ECS é responsável por levantar quantas instâncias da aplicação forem configuradas, ou mesmo baixá-las em caso de atualizações ou problemas, bem como de descalonamento por redução de demanda.<br>
Assim, se a estratégia for de escalar horizontalmente as aplicações, que consiste em replicar a aplicação para conferir disponibilidade e resiliência, o ECS é configurado com o número de instâncias necessárias, os recursos necessários, bem como, dos critérios de escalonamento, quando for de forma automática.<br>
<br>
Tanto o Amazon Fargate e o ECS, executam num contexto de uma sub-rede pública (acessível pela internet), com endereçamento limitado.<br>
As instâncias em execução acessam em concorrência o serviço do Amazon RDS (Relational Database Service), que consiste num serviço (SaaS) de banco de dados relacional, no caso, com o motor do PostgreSQL. O serviço do RDS é criado para operar no modo Multi-AZ, que consiste na configuração de um banco primária, que é replicado de forma síncrona numa segunda instância, garantindo redundância de dados e alta disponibilidade.<br>
Como dito, o ambiente da aplicação é escalonável, mas depende da adoção das estratégias frente a necessidade. Assim, caso haja a opção pelo escalonamento da aplicação, entendemos que o banco de dados não deve sofrer escalonamento horizontal das suas instâncias por conta da integridade dos seus dados, há de se adotar outra estratégia para o aumento da capacidade do banco de dados, como a sua segregação por domínio, a sua clusterização em partições, ou mesmo, a realização do processo de "sharding" das informações.<br>
De nada adianta escalar aplicações para suportar um volume maior de requisições, se as mesmas acessam o mesmo banco de dados, sobrecarregando as conexões e os recursos do gerenciador de banco de dados.<br>
<br>
O segundo fluxo, consiste no fluxo de construção e publicação. Os serviços do gerenciador de containers e instancias (Fargate/ECS) carregam e executam instâncias contidas na forma de imagens de container, padrão Docker (motor para criação e gerenciamentode containers).<br>
Essas imagens, depois de criadas no ambiente do repositório de fontes do GitHub, através do recurso de actions, as mesmas são versionadas e salvas no registro de imagens.<br>
Os containers criados com o Docker podem ser registrados na própria plataforma do Docker, que é proprietária e pública para não assinantes.<br>
Para contornar essa possível falha de segurança, que é manter imagens das aplicações da organização em repositório de imagens de acesso público, sugerimos a adoção do serviço do Amazon ECR (Elastic Container Registry) para armazenar as imagens da aplicação.<br>
<br>
Logo, o fluxo de publicação importa inicialmente, na realização de "commits" de alterações de código em uma versão estável, testada e funcional, a qual dará origem a uma imagem de execução, denominada imagem container. Essa imagem é elevada (push) para o registro de imagens (ECR) e ficará disponível para ser carregada (pull) e executada no ambiente dos containers.<br>
<br>
Para o acesso remoto, a entrada faz-se pelo serviço da Amazon PrivateLink (Endpoints) que consiste num endpoint de acesso a VPC (Virtual Private Cloud) através de uma conexão privada.<br>
<br>
Para realizar o acesso por conexão privada pelo GitHub, repositório sugerido, utilizando o recurso de "Actions", este acesso depende da existência de um usuário sistêmico com credenciais junto ao Amazon IAM (Identity and Access Management) com privilégios limitados.<br>
<br>
O processo de publicação de novas versões não é automatizado e depende da intervenção humana, previamente definida mediante uma estratégia de manutenção.<br>
<br>
O respectivo acesso possibilitará o registro da nova imagem da aplicação no ECR, para ser consumido pelo gerenciador de containers (Fargate/ECS).<br>
<br>
<br>
Quanto a aplicação implementada:<br>
<br>
Observação: ao invés de utilizarmos os termos de débito e crédito como sugerido no desafio, utilizamos os termos entrada e saída, respectivamente.<br>
<br>
A aplicação foi implementada utilizando os princípios do DDD (Domain-Driven Design), utilizando-se como padrão estrutural de projeto o Repositório (Repository) com unidade de trabalho atômica (Unit Of Work). A aplicação sugerida possui três camadas, as quais vamos enumerar de forma decrescente para entendimento.<br>
<br>
A terceira camada, a de infraestrutura, responsável por agregar as funcionalidades de acesso à base de dados, com utilização de um motor de ORM (Object Relational Mapping) implementada pelo EF (Entity Framework), com os mapeamentos das entidades do banco de dados utilizado, no caso, relacional PostgreSQL. A camada de infraestrutura foi concebida sob o padrão de unidade de trabalho (Unit Of Work) pela utilização de classe de contexto de dados (Context in collections) com classes com a responsabilidade de executar ações padrões sobre cada entidade, considerando um repositório de operação específico para cada categoria de dado.<br>
<br>
A segunda camada, a de domínio, responsável por segregar as entidades de negócio, os serviços para a realização das regras de negócio do domínio; as classes de transporte (DTO - Data Transfer Object) e todos elementos cujo comportamento afetam ou são afetadas pelo domínio de negócio.<br>
<br>
A segunda camada, a de aplicação, consiste na implementação parcial do padrão de arquitetura MVC (Model View Controller), onde são expostos os controles de execução das funcionalidades. Essa camada expõe uma API (Application Programming Interface), no padrão Rest (Representational state transfer), com a definição de endpoints endereçáveis para a execução de tarefas sobre recursos.<br>
<br>
A aplicação foi concebida para servir aplicações clientes, como aplicações móveis (mobile), desktop ou web. Ou seja, a camada de apresentação não faz parte do escopo do projeto e não foi desenvolvida no exercício.<br>
<br>
Além das referidas camadas de aplicação implementada, foi criada uma aplicação de teste das funcionalidades da aplicação principal.<br>
<br>
Para oportunizar os testes da aplicação em ambiente simulado, foi criado um ambiente de containers locais para executar um banco de dados relacional containerizado, e a execução de um container da aplicação Rest. Esse ambiente de teste permite que a aplicação receba requisições locais em porta específica e realize a comunicação com o gerenciador de banco de dados de forma independente, em outro container.<br>
<br>
<br>
Para finalizar, o que faltou?<br>
<br>
Em virtude do foco: arquitetura de solução e, do tempo, deixamos de:<br>
- criar as validações da regra de negócio no modelos da requisição, ou seja, validar a existência de um histórico válido para o lançamento; uma data de registro válida; um valor monetário positivo; a natureza da operação nos limites da enumeração criada (0-Entrada / 1-Saída);<br>
- configurar a validação das credenciais e obtenção de token e claims recebidos em JWT (Json Web Tokens), bem como a definição das claims necessárias para acessar os recursos do sistema;<br>
- documentar melhor a API através da colocação de atributos nos métodos de chamada com informações adicionais para ser gerado no Swagger.<br>
<br>
<br>
<br>
__Instruções para execução e organização__<br>
<br>
<br>
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
