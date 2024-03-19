# permissionManagement

This project is an application developed in .NET Core 6, using an architecture based on patterns such as Repository, Unit of Work, and CQRS (Command Query Responsibility Segregation).

# Key Features
Repository and UnitOfWork: We utilize the Repository pattern along with Unit of Work to facilitate data access and maintain consistency in database operations.
CQRS: We implement the CQRS pattern to separate read and write operations, allowing us to optimize scalability and performance of our application in the Permission module.

# Testing
The project includes a comprehensive set of unit tests to ensure code quality and application functionality.

# Docker
We provide a Dockerfile ready to generate a Docker image of the application. This streamlines the deployment and execution of the application in any Docker-compatible environment.

# Configuration
We offer a configmap.yml file that can be used to update the application configuration, such as appsettings.json, in Kubernetes environments.

# Kubernetes Deployment
An included deployment.yml file outlines how to deploy the Docker image to a Kubernetes cluster. This enables a quick and scalable deployment of the application in a containerized orchestration environment.

# Recommendations
We recommend building and developing this project using Visual Studio 2022 for an optimized development experience.