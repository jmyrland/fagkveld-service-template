const express = require('express');
const bodyParser = require('body-parser');
const ServiceHelper = require('./lib/ServiceHelper');

const app = express();
app.use(bodyParser());

// :: Setup controllers
require('./core/ServiceController')(app);

const port = process.env.PORT || 3000;

const server = app.listen(port, () => {

  console.log('## Service up and running on port [%s]', port);

  // :: Register service
  const serviceInfo = {
    address: '127.0.0.1',
    name: 'node-service-template',
    port: port
  };
  ServiceHelper.registerService(serviceInfo);

});

module.exports = server;
