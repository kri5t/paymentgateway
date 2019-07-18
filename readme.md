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

##### Logging
Added serilog for logging options. It is highly customizable and can
be customized to send logs to azure, file, console etc.