# Avatar service

## Metadata

```
{
  "name": "avatar",   // FIXED    - don't change
  "port": 80          // VARIABLE - you choose
}
```

## Description

This service is responsible of keeping track of, and serving avatars based on a given user id.

At the start of the service, the service will scrape the html from the URL 'https://peanuts.no/menneskene/' and
extract the email (`.employee-list-block__employee__email a href`) and image url (`.employee-list-block__employee__image (background-image)`) fields for each member (`.employee-list-block__employee`) in the organization. These values will be cached in a dictionary - with the email as a key and the image url as a value, i.e.:

```
{
  "jorn.myrland@peanuts.no": "http://www.peanuts.no/globalassets/ansattbilder/jorn-andre-myrland.jpg?width=300&height=300&transform=downfit",
  "frode.hetland@peanuts.no": "http://www.peanuts.no/globalassets/ansattbilder/frode-hetland.jpg?width=300&height=300&transform=downfit",
  // ETC...
}
```

## Dependencies

- User service
- https://peanuts.no/menneskene/

## Headers

- Content-Type: image/jpg

## Endpoints table

| Verb       | Endpoint                 | Description                                           |
| ----------:|:------------------------ |:------------------------------------------------------|
| GET        | /:userId                 | Return avatar for user, based on the scraped HTML.    |


## Endpoint descriptions

### GET /:userId

**! Requires user ID as path parameter.**

**! Important: `Content-Type: image/jpg` must be set for the response.**

##### Description:

Note, you don't have the email for the given user in this request, so you are depending
on making a request to the `user` service to retreive the email to lookup the email.

When you have the email, you can lookup the image url in you're dictionary - then **proxy
the image data (from the image url) to the client response**. Note, proxy != redirect.
This response have the status code `200 OK` and contain a header `Content-Type: image/jpg`.

If the given userId can't be matched to any image, return a `404 NOT FOUND` response.

If any error occurs, return a `500 ERROR` response.

To reduce "lookup time", an image cache by id should be implemented.


##### Response status codes:

- Returns `200 OK` with image data
- Returns `404 NOT` FOUND if the `userId` did not match any existing users.
- Returns `500 ERROR` if any errors occurred.
