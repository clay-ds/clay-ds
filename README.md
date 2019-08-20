# Technical Decisions

## General
Backend is built with ASP.NET Core 2.2 since it's the latest stable version.

## Authentication
Since the API should be able to support both web and mobile clients the decision was made to use Identity Server 4 as a STS. Auth itself is a huge topic so the in-memory version of IdS is used just to demonstrate the concept.

## Data storage
There're 2 types of storage in the application:
1. MySql + EF Core - mainly used since there's a requirement to know it. User permissions/offices/doors are stored there because all of these data types have an upper limit of an amount of entities (it's quite unlikely to have 1kk of offices/doors)
2. Mongo DB - used for storing dates when users enter doors. This data set can grow infinitely and fast by persising it in Mongo it'd be easier to distribute data when time comes.

## Testing
* Unit tests are built using XUnit. Only parts with logic inside are covered with unit tests for now.
* Integration tests are built with Postman, collection "DoorUnlocker #tests" is in /test folder.

## Other libraries
* AutoMapper - no comments
* CorrelationId - to be able to track HTTP request scope in logs
* FluentValidation - for validation
* Serilog + Seq - structured logging, prefer it over the ASP.NET Core default logging since don't believe that logging should be done through DI, it's a cross-cutting concept and so should remain static.

# How to run

Run `docker-compose up -d` in a root folder

# Working with App
Docker starts all the infrastructure at the following addresses:
* API - http://localhost:5344
* Identity Server - http://localhost:5345
* MySql - localhost:5341
* Mongo - localhost:5343

# Logging
All the logs go to Seq (run at http://localhost:5340)

# Verification
There's a postman collection "DoorUnlocker" in /test folder, it contains references to all the endpoints. Get Token request will save received token into an env variable.

List of doors:
There's one office (ID 1) and 2 doors (Tunnel: 1, Main: 2).

The following list of users is set up in IdS (password for all is "password"):
* OneDoor, ID: 1 - by default has access to Main door.
* TwoDoors, ID: 2 - by default has access to Main and Tunnel doors.
* NoDoors, ID: 2 - by default doesn't have any access.
* Admin, ID: 4 - has access to all doors