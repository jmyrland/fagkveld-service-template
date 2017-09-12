const request = require('request');

const ROUTER_URL = process.env.ROUTER_URL || 'http://192.168.75.93:3000';

const ServiceHelper = {

  registerService: (serviceMetadata) => {

    const attemptRegistration = function() {

      console.info('## Attempting to register service.. ' + ROUTER_URL);

      registerService(serviceMetadata)
        .then(() => {
          console.info('## Service registered successfully!');
        })
        .catch((err) => {
          console.error('!! Error occured while registering service: ', err.message);
          console.info('## Retrying in 2 seconds..');

          setTimeout(function() {
            attemptRegistration(serviceMetadata);
          }, 2000);
        });
    };

    attemptRegistration();
  },

  getService: (serviceName) => {
    if (!serviceName || serviceName.length == 0) {
      return Promise.reject(new Error('The service name have to be specified.'));
    }

    const serviceUri = `${ROUTER_URL}/${serviceName}`;

    return checkService(serviceUri, serviceName)
      .then(() => createService(serviceUri))
  }

};

const createService = (uri) => {
  return {
    get: (path) => new Promise((resolve, reject) => {
      const endpoint = `${uri}${path}`;
      request.get(endpoint, (error, response, body) => {
        if (error) {
          return reject(error);
        }
        try {
          const result = JSON.parse(body);
          resolve(result);
        }catch(ex) {
          reject(new Error(`Failed to parse JSON from [${endpoint}] - statusCode: [${response.statusCode}].`))
        }
      });
    }),
  }
};

const checkService = (serviceUri, serviceName) => new Promise((resolve, reject) => {
  request.get(`${serviceUri}/test`, (error, response) => {
    if (error) {
      return reject(error);
    }
      console.log(response.statusCode)
    if (!response || response.statusCode == 502) {
      return reject(new Error(`Service with name [${serviceName}] has not been registered.`))
    }
    return resolve();
  })
});

const registerService = (serviceData) => new Promise((resolve, reject) => {
  request.post(ROUTER_URL + '/register', { json: true, body: serviceData }, function (error, response, body) {
    if (error) {
      reject(error);
      return;
    }

    if (response.statusCode != 204) {
      error = new Error('Service was not registered correctly, expected statuscode 204 but was ' + response.statusCode);
      reject(error);
      return;
    }

    resolve();
  });
});

module.exports = ServiceHelper;
