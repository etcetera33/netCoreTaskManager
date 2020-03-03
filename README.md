# Task manager via Dmytro Poliit

## Application architecture
![SSO](img/Architecture.png)

### Application is designed as SOA project. Let`s view its components.

## Components
1. API - component, which handles all the incoming requests.
2. Front end
3. Identity Server - component, which is responsible for the authorization && authentication.
Authorization via implicit flow is implemented as follows
![SSO](img/SSO.png)
4. Redis
5. MS SQL Database
6. Enitity Observer is used to track changes and to push an event to the notification service if specific changes were made
7. Notification Service is used to notify the user of some events
8. All the images are saved to the Azure blob storage


### All of the images are build via Gitlab pipelines, pushed to the Azure Container Registry and than pulled by K8s
![SSO](img/CD.png)


## Technologies
1.  .Net Core Web API
    1. Authorization w JWT and Roles
    2. Layered architecture, DAL, Repository pattern o Unit of Work
    3. Entity Framework
    4. SSO (implicit and credentials grant flow)
    5. Unit Tests
2. Containerization
   1. Docker
   2. Docker-compose
   3. CI/CD
   4. Building images and pushing to Azure Container Registry via Gitlab Pipelines
3.  Service Oriented Architecture
4.  Caching - Redis
5.  MassTransit
6.  Kubernetes
7.  Azure
    1.  Azure blob storage
    2.  Azure container registry
8.  Frontend - SPA Angular application v8