# Rating Notification ServiceProvider

 Armut / Homerun Backend Assignment

## Overview

All Projects Coded with C# / .Net Core

Rating Service && Notification Service functionality implemented and Used with OpenAPI

Both Services were Unit and Integration tested (TestContainers).

Structured logging implemented to all services.

A continuous integration made by .github/workflow. 

Implemented a rate-limiting mechanism.

### Possible Improvements
 For general i could implement API Gateway for handling these two service and maybe future services.
 
 I Could Add Data Service which is basically doing CRUD operations but it will be the only service who interacts with the DB. 
 If a service has a business with a database, it will talk to the data service instead of talking directly to the database. I think it is important for reliability.
 

## Installation

It has docker-compose.yaml file. If we run command below in correct path , it will start Both services, PostgreSql and Rabbitmq.

    docker-compose up
     

## Projects

### HomeRun.Shared

 It is a shared library class. it has helper methods, Custom exceptions but more importantly BaseRepository based on Generic Repository Pattern.
 I created this library because of maintainability and scalability. BaseRepository creadted with EntityFramework DBContext and we can use it for future services.
 
 Also i created IRepository interface which contains basically crud functions. If non-relation databases needed like mongoDB, we can create anohter repository based on this Irepository
 and All of the future services can use IRepository instance comes from DI. and whether its relational or non-relational if we give proper Dependency Injections it will be suited to all available    services.

### HomeRun.RatingService

Allows users to submit a rating for a service provider and fetch the
average rating for a specific service provider. This service will persist the rating data
and notify the notification service when a new rating is submitted.

I have Created a Web Api  with .Net and used a PostgreSql to store RatingService informations.

For test cases when db creates, it creates 3 ServiceProvider instances for test cases and their id's 1, 2, 3

This service uses Serilog for structured logging. And it uses middleware for global exception handling.

When New ratings created, it sends message to RabbitMq with amqp connection as a publisher and notifies the Notification Service.

#### Rating Service OpenAPI

    https://localhost:5001/swagger/index.html 


### HomeRun.NotificationService

Allows clients to fetch a list of new notifications that have been
submitted since the last time the endpoint was called.

When new rating submitted, Notification Service receives message and logs them. Also for simulation purposes, it keeps values locally.
When endpoint called for SpecificProvider Id , it returns new rating notification associated with specified id.

It is Web Api but it has background service in it which listening to specified rabbitmq Container. 


#### Notification Service OpenAPI

    https://localhost:7001/swagger/index.html 

## Sample Scenario

#### Please Read This !
 As i mentioned above,  I have created 3 sample ServiceProvider instances  and their id's are 1 , 2, 3 .
 When trying scenarios assume that we have only 3 Provider and if you try to use other than these ones, You will probably get BadRequest and Reasonable message like No Service Provider Found with specified Id.
 

User Rates for a specific Service Provider with url https://localhost:5001/rating

    {
      "serviceProviderId": 1,
      "ratingValue": 5
    }


and response will be like that:

    {
        "result": {
            "serviceProviderId": 1,
            "ratingValue": 5,
            "id": 1,
            "createdAt": "2023-08-06T22:04:00.5635349Z"
        },
        "isSuccess": true,
        "message": "Rating created successfully"
    }

Also we can Get Average rating for specified serviceProvider

    https://localhost:5001/rating/avg/1
And it returns the average rating for specific Service Provider Id
    
    {
        "result": 5,
        "isSuccess": true,
        "message": "Average Rating is: 5"
    }


When we look at the RatingService logs from Logs folder or console, we can see that it created instance, sent it to the rabbitMQ.
    
    2023-08-07 01:04:00 [2023-08-06 22:04:00Z] [INF] HomeRun.RatingService.RatingController Rating created successfully: {"ServiceProviderId": 1, "RatingValue": 5, "Id": 1, "CreatedAt":     "2023-08-06T22:04:00.5635349Z", "$type": "Rating"}
    2023-08-07 01:04:00 [2023-08-06 22:04:00Z] [INF] HomeRun.RatingService.MessageProducer Message sent to the queue successfully: {"ServiceProviderId": 1, "RatingValue": 5, "Id": 1, "CreatedAt": "2023-08-06T22:04:00.5635349Z", "$type": "Rating"}
    2023-08-07 01:04:00 [2023-08-06 22:04:00Z] [INF] Serilog.AspNetCore.RequestLoggingMiddleware HTTP POST /rating responded 200 in 617.4901 ms



When we look at the Notification service and check the logs we can see that our background service received Rabbitmq message from Rating Service
    
    2023-08-07 01:03:07 [2023-08-06 22:03:07Z] [INF] HomeRun.NotificationService.NotificationProccessor Notification Service is working.
    2023-08-07 01:04:01 [2023-08-06 22:04:01Z] [INF] HomeRun.NotificationService.NotificationProccessor Product message received: {"ServiceProviderId":1,"RatingValue":5,"Id":1,"CreatedAt":"2023-08-06T22:04:00.5635349Z"}


If we want to get new notifications for specific service provider,

#### GET
    https://localhost:7001/notification/1
And response will be like that. ( It also logs notifications which has been sended to users) 
    
    [
      {
        "id": "46baa5b1-e704-4fd4-af0f-f3cd3060cc51", // GUID
        "ratingId": 1,
        "serviceProviderId": 1,
        "ratingValue": 5,
        "createdAt": "2023-08-07T00:53:06.4136243Z"
      }
    ]

#### NOTE: 
Notification Service keeps notifications locally.  when we call the endpoint, it removes showed notifications from the list. if you call the endpoint twice for same ServiceProvider, second one will return Empty list becase it assumes that notifications have been readed.









