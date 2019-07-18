# Payment gateway

## Setup
To run the API you only need to run:

`docker-compose up --build -d`

It will build the solution using the Dockerfile and run the unit tests. 
When the build completes it will run the docker containers in detached mode.

## Documentation
Added swagger for documentation:

https://localhost/swagger/index.html

## Assumptions
##### Credit card information
In my version of the API I assume that we do not want to deal with storing
credit card information from the user. This leaves us out of liability should
the worst think happen and a dataleak occurs. Therefor I am only storing the
information that I need for receipt.

##### Only one user
Right now I assume only one user. As this is only testing. Therefore
I assume that all the entries in the DB are for one user.

## Architecture
I have used Command Query Responsibility Segregation(CQRS) for my access 
to the data layer. This allows me to easily communicate to other developers, 
what endpoints have side-effects and which doesn't. It is also easier to 
reuse specific parts of my core application in other endpoints or other 
parts of future applications. For example internal APIs or message queue 
handlers.

I am a firm believer in micro services and I try to follow domain driven 
design principles when designing the micro services. This ensures that 
the organization have a shared language and that the code reflects this 
language. It also ensures that a service has a certain part of the domain 
as responsibility and (if done right) reduces the amount of calls between 
services.

I have used the repository pattern to decouple the data access layer from 
the business logic. This makes it easier to implement new data layers. 
And also decouples the tie between a specific database object and the 
business logic.

## Future work
##### Authentication
I have not had time to set-up authentication on the endpoints. If I were 
to set-up the authentication I would make use of the following technologies:

**IdentityServer4:** This is a highly flexible self-hosted solution that 
is free to use. It works well with ASP.Net Core Identity and gives you 
everything you need from an authentication server out of the box. It is 
maintained by two industry experts and makes it easy to handle OIDC plus 
they provide several client-side libraries as well. It also comes with a 
middleware that makes it easy to secure APIs built in dotnet core.

**SAAS solution:** 
If customization of the authentication server is not needed, I would 
probably go with a SAAS solution from either AWS or similar, where 
everything is handled for me. Since this takes away complexity and 
allows me to focus on the implementation of the application, while 
knowing the application is secure.

##### Test coverage
I do not have the best test coverage and some of the tests can benefit 
from even greater granularity. The basic business logic is under test, 
which is the most important. I have shown how the business logic, commands
and repository can be tested.

##### Bank simulator
The bank simulator is very barebones. The simulator could have been more 
elaborate. And fx been implemented using webhooks - If the service was 
handled asynchronous. I have experience from implementing the Stripe API
and this was how they handled it.

If this was to be handled via webhooks. We would need a way to notify
our vendor when the transaction went through. All of this would free up
resources on our end. And would help us processing more requests.

##### Application metrics
Right now we are using prometheus for application metrics. And grafana to 
visualize it. It is a great platform that comes with a lot of plugins.

If a locally hosted solution is not great. Azure also has application monitor
that can help both with logging and error handling.

## Technologies
##### MediatR:
Used as a mediation layer to connect the controller layer to the data 
layer. It creates an abstraction between the two, that makes it easier 
to handle dependency injection and a lose coupling.

##### Automapper: 
Used for mapping data result models to the response models of the 
controllers. This is used to control the response models and make sure 
that new properties will not be forgotten when adding them to the data 
layer result models.

##### Swagger: 
Used for automatic documentation of the endpoint. It takes the XML 
comments in the code and presents them to the consumers of the API.

##### Entity Framework: 
Is used as an ORM for the database. This eases the communication between 
database and data layer in the application. Also it provides me with 
LINQ support which is a powerful querying language.

##### XUnit: 
Is used for testing purposes. It is very similar to NUnit. I prefer 
this framework because I have most experience with it, but it also 
seems to have a bit less boilerplate. As it follows the set-up of a 
class, instead of having decorators to control the different methods 
as NUnit does.

##### Moq: 
Mocking framework used in the unit tests, to abstract out parts that 
is not important to the part of the code I am testing.

##### Serilog:
Added serilog for logging options. It is highly customizable and can
be customized to send logs to azure, file, console etc.