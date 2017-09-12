const ServiceHelper = require('../lib/ServiceHelper')
const FileSync = require('lowdb/adapters/FileSync')
const shortid = require('shortid')
// Docs: https://github.com/typicode/lowdb
const low = require('lowdb')

const adapter = new FileSync('db.json')
const db = low(adapter)

// :: Set defaults for the database
db.defaults({ items: [] }).write()

const ServiceController = (server) => {
  // :: Service consuption example
  server.get('/first-post', getFirstPost);

  // :: Create/POST example
  server.post('/', createItem);

  // :: Multiple results example
  server.get('/list', getItems);

  // :: Single result example
  server.get('/:id', getSingleItem);
};

const getFirstPost = (req, res, next) => {
  ServiceHelper.getService('post')
    .then(service => service.get('/list'))
    .then(posts => posts[0])
    .then(firstPost => res.send(firstPost))
    .catch(err => {
      console.error(err);
      next(err);  // Let `next` handle the error..
    })
};

const createItem = (req, res) => {
  const { text } = req.body;

  // Basic validation
  if (!text) {
    return res.status(409).send({
      error: 'Missing text.'
    });
  }

  // Construct the item
  const item = {
    id: shortid.generate(),
    timestamp: Date.now(),
    text: text,
  };

  // Save the item to the DB
  db.get('items')
    .push(item)
    .write();

  // Respond with the created item
  res.send(item);
}

const getItems = (req, res) => {
  const items = db.get('items').value();
  res.send(items || []);
}

const getSingleItem = (req, res) => {
  const item = db.get('items')
    .find({ id: req.params.id })
    .value();

  if (!item) {
    return res.status(404).send();
  }

  res.send(item);
}

module.exports = ServiceController;
