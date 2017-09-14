# Post service

## Metadata

```
{
  "name": "post",     // FIXED    - don't change
  "port": 80          // VARIABLE - you choose
}
```

## Description

This service's responsibility is to store and serve idea posts.

The post model should have the following properties:
- `id`: (?) - unique post id - can be whatever type you choose.
- `userId`: (?) - unique user id - must be equal to the id type of the user service.
- `text`: (string) - the idea text
- `timestamp`: (integer) - created date (equal to javascript's `Date.now()`)

**All fields in the model (when returned as JSON) must conform to `lowerCamelCase` formats, to save a little ink during printouts.**

## Headers

- Content-Type: application/json

## Endpoints table

| Verb       | Endpoint                 | Description                  |
| ----------:|:------------------------ |:-----------------------------|
| POST       | /                        | Create a post.               |
| GET        | /list                    | Fetch all posts.             |
| GET        | /:postId                 | Fetch a single post.         |


## Endpoint descriptions

### POST /

**! Requires JSON data as body.**

##### Example body structure:
```
{
 "postText": "This is a great idea!!", // REQUIRED
 "userId": 1234                        // REQUIRED
}
```

##### Description:

This endpoint will create a post object with the given parameters: `postText` and `userId`.

The endpoint must validate the `postText`; it should be 10 characters or more.

The endpoint must validate that no required body parts is missing (hehe!).

If validation fails, this endpoint should return a `409 CONFLICT` response with an explanation
of what went wrong (JSON object with an `error` string), e.g.
`{ "error": "Missing 'userId'."}`

If the post is created successfully, it should be returned as a `200 OK` response, with the following JSON properties:
```
{
 "id": 1,                           // REQUIRED - may be any type
 "text": "This is a great idea!!",  // REQUIRED - string
 "userId": 1234,                    // REQUIRED - must be the same type as the ID in the 'user service'
 "timestamp": 1433167411668         // REQUIRED - must be an integer (equal to javascript's Date.now())
}
```

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with the created post as JSON.
- Returns `409 CONFLICT` with an error JSON.
- Returns `500 ERROR` if any errors occurred.

-------------------------------------------------------------------------------

### GET /list

##### Description:

This endpoint will return a `200 OK` response with a list of all posts
(including all properties) in the local database as a JSON array.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with a JSON array of all post objects.
- Returns `500 ERROR` if any errors occurred.

-------------------------------------------------------------------------------

### GET /:postId

**! Requires user ID as path parameter.**

##### Description:

This endpoint will return a `200 OK` response with the post matching the given
`postId` with the following JSON properties:
```
{
 "id": 1,                           // REQUIRED - may be any type
 "text": "This is a great idea!!",  // REQUIRED - string
 "userId": 1234,                    // REQUIRED - must be the same type as the ID in the 'user service'
 "timestamp": 1433167411668         // REQUIRED - must be an integer (equal to javascript's Date.now())
}
```

If the post was not found, a `404 NOT FOUND` response should be returned.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with a post as JSON matching the given `postId`.
- Returns `404 NOT` FOUND if the `postId` did not match any existing posts.
- Returns `500 ERROR` if any errors occurred.
