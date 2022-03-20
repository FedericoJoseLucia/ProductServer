<h3 align="center">Product Server - Sample Project</h3>

  <p align="center">
    A sample project made to demonstrate multiple coding patters, best practices and technology usages.
    Written on c# with .Net framework.
</p>


<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#implementation-details">Implementation Details</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
    </li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

The project was built using coding patterns as Domain Driver Desing, Mediator, CQRS, Test Driven Development, and Event Sourcing. It exposes a REST API with Create, Update, GetById and GetById (Master) operations.


<!-- IMPLEMENTATION DETAILS -->
## Implementation Details

Endpoints:
- POST: /api/Product/Create
- PUT: /api/Product/Update
- GET: /api/Product/GetById/{id}
- GET: /api/Product/GetById/Master/{id}

The chosen database engine is SQL Server, using Entity Framework Core as ORM.

Create and Update operations have custom validations (made with FluentValidations library) on the application layer. Any other
unexpected error will be catched by an exception middleware located on the API layer and transmitted as an internal error.

On Create, the product aggregate publishes a domain event (using mediatr notifications). This event is handled in the application layer where the product model is cached for future GetById (Master) requests. 

The GetById operation returns some basic properties of the product model.
The GetById (Master) operation returns those same properties but also adds the last created product (catched) id and external code and an external service product stock and department.

The external service was mocked easily with mockapi.io. For security reasons the endpoint isn't pushed.

The seedwork folders located around the project include common cross-project reusable files, some from microsoft docs, others custom written.

The project development was realized using a minimal GitFlow aproach (It's a small project), and Test Driven Development. In the tests folder you can find unit and integration tests made to test different functionalities.

<!-- GETTING STARTED -->
## Getting Started

The "appsettings.json" file contains configuration settings
1) Replace ConnectionStrings/SqlServerConnection with your SQL Server Connection String.
2) Replace ProductExternalServerAPI/BaseAddress with your external service api uri.
3) Replace LogDirectory with yo
4) Run!

<!-- CONTACT -->
## Contact

Federico Jose Lucia - federicojoselucia@gmail.com

Project Link: [https://github.com/FedericoJoseLucia/productServer](https://github.com/FedericoJoseLucia/productServer)
