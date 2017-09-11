# ASP.NET MVC Service template 

## Getting started

1. Open the `Global.asax`
2. Configure the service according to your manifest.
3. Press play and happy coding! :)

## Controller

There is a predefined controller named `ServiceController` which contains 
all you need to get started.

This controller is already setup with [LiteDB](http://www.litedb.org/) and 
some examples of how to interact with this database.

There is also an example of how to consume other services 
(the action named `ServiceExample()`).

## Configuring the service router URL

If by any means the service router changes host, you can update this 
by changing the `ROUTER_URL` constant in the `ServiceHelper` class.