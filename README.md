# GISA - Gestão Integral da Saúde do Associado

### Repositório contendo a POC do Trabalho de Conclusão do Curso de Especialização em Arquitetura de Software Distribuído pela PUC Minas

&nbsp;

[![POC @ YouTube](/docs/images/YouTube.png)](https://youtu.be/U5nhloZD7WA)

# Pré-requisitos

1. Ter o Docker instalado e configurado com o modo Swarm ativo.
2. Certificado digital válido. Este é opcional, porém, o proxy reverso (NGINX) foi configurado para procurar pelos arquivos **fullchain1.pem** e **privkey1.pem** no caminho /etc/letsencrypt/archive/poc-tcc-host.

&nbsp;

## Subindo a Stack

A solução é baseada em containers, portanto, com um único comando, todos os componentes podem ser iniciados com a instrução:

`docker stack deploy -c docker-stack-azure.yml gisa`

&nbsp;

# Macro Arquitetura

![Macro Arquitetura](docs/images/Diagrama%20de%20Macro%20Arquitetura.png)

# Componentes

## RabbitMQ
Mensageria

![RabbitMQ Connections](docs/images/RabbitMQ-Connections.png)

![RabbitMQ Queues](docs/images/RabbitMQ-Queues.png)

![RabbitMQ Queues - Processador de mensagens parado](docs/images/RabbitMQ-ServiceStopped.png)

## Redis e Redis Commander
Cache

![Redis Commander](docs/images/RedisCommander.png)

## MailDev
E-mails de confirmação/recusa de consulta

![MailDev](docs/images/MailDev.png)

## Zipkin
Rastreabilidade entre microserviços

![Zipkin](docs/images/ZipKin.png)

![Zipkin - Normal](docs/images/ZipKin-Normal.png)

![Zipkin - Normal](docs/images/ZipKin-Stopped.png)

## Azure Pipelines
CI/CD

![Pipelines](docs/images/Pipelines.png)

![Pilelines - Test Summary](docs/images/Pipelines-TestSummary.png)

![Pilelines - Test Coverage](docs/images/Pipelines-TestCoverage.png)

![Pilelines - Tasks](docs/images/Releases.png)

![Pilelines - Tasks](docs/images/Release-Tasks.png)


## Azure Application Insights
Logs e Telemetria

![Logs](docs/images/Appi-Performance.png)

![Telemetria](docs/images/Appi-LiveMetrics.png)

## Portainer
Gestão e monitoramento de containers

![Portainer](docs/images/Portainer.png)

## Referências

[Azure Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) - *Application Insights is a feature of Azure Monitor that provides extensible application performance management (APM) and monitoring for live web apps.*

[Azure Pipelines](https://azure.microsoft.com/en-us/services/devops/pipelines) - *Automate your builds and deployments with Pipelines so you spend less time with the nuts and bolts and more time being creative.*

[Dapr](https://dapr.io) - ***Dapr** is a portable, event-driven runtime that makes it easy for any developer to build resilient, stateless and stateful applications that run on the cloud and edge and embraces the diversity of languages and developer frameworks.*

[NGINX](https://nginx.com) - ***NGINX** is open source software for web serving, reverse proxying, caching, load balancing, media streaming, and more.*

[Portainer](https://www.portainer.io) - *At its heart, **Portainer** helps developers deploy cloud-native applications into containers simply, quickly and securely.*

[RabbitMQ](https://www.rabbitmq.com) - ***RabbitMQ** is the most widely deployed open source message broker.*

[Redis](https://redis.io) - ***Redis** is an open source (BSD licensed), in-memory data structure store, used as a database, cache, and message broker.*

[Zipkin](https://zipkin.io) - ***Zipkin** is a distributed tracing system. It helps gather timing data needed to troubleshoot latency problems in service architectures. Features include both the collection and lookup of this data.*