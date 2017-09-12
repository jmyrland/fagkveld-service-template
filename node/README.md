# node.js service template

Pre requisites:
node.js v7+
npm v3.10+

## Getting started

1. Run `npm install`
2. Configure the service metadata in `server.js`
3. Run `npm start`
4. Happy coding :)

*Tip: for rapid development, install [nodemon](https://github.com/remy/nodemon).*

## Controller

There is a predefined controller named `ServiceController` which contains
all you need to get started.

This controller is already setup with [lowdb](https://github.com/typicode/lowdb) and
some examples of how to interact with this database. The IDs in this example is `string`s.

There is also an example of how to consume other services
(the endpoint named `GET /first-post`).

## Configuring the service router URL

If by any means the service router changes host, you can specify
the `ROUTER_URL` environment variable when running the server.

I.e.:
```
ROUTER_URL=http://192.168.75.93:3000 node server.js
```
