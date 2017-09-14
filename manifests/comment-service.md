# Comment service

## Metadata

```
{
  "name": "comment",  // FIXED    - don't change
  "port": 80          // VARIABLE - you choose
}
```

## Description

This service's responsibility is to store and serve comments for posts.

The comment model should have the following properties:
- `id`: (?) - unique comment id - can be whatever type you choose.
- `userId`: (?) - unique user id - must be equal to the id type of the user service.
- `postId`: (?) - unique post id - must be equal to the id type of the post service.
- `text`: (string) - the comment text
- `timestamp`: (integer) - created date (equal to javascript's `Date.now()`)

**All fields in the model (when returned as JSON) must conform to `lowerCamelCase` formats, to save a little ink during printouts.**

## Headers

- Content-Type: application/json

## Endpoints table

| Verb       | Endpoint                 | Description                            |
| ----------:|:------------------------ |:---------------------------------------|
| POST       | /:postId                 | Create a comment for a post.           |
| GET        | /list                    | Fetch all comments.                    |
| GET        | /list/:postId            | Fetch all comments for a single post.  |


## Endpoint descriptions

### POST /:postId

**! Requires `postId` path parameter.**
**! Requires JSON data as body.**

##### Example body structure:
```
{
 "commentText": "This is my comment.", // REQUIRED
 "userId": 1234                        // REQUIRED
}
```

##### Description:

This endpoint will create a comment object with the given parameters: `commentText`, `postId` and `userId`.

The endpoint must validate that no required path/body parts is missing (hehe!).

If validation fails, this endpoint should return a `409 CONFLICT` response with an explanation
of what went wrong (JSON object with an `error` string), e.g.
`{ "error": "Missing 'userId'."}`

If the post is created successfully, it should be returned as a `200 OK` response, with the following JSON properties:
```
{
 "id": 1,                           // REQUIRED - may be any type
 "text": "This is a great idea!!",  // REQUIRED - string
 "userId": 1234,                    // REQUIRED - must be the same type as the ID in the 'user service'
 "postId": 1234,                    // REQUIRED - must be the same type as the ID in the 'post service'
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

This endpoint will return a `200 OK` response with a list of all comments for all posts
(including all properties) in the local database as a JSON array.

If no comments is found, an empty JSON array should be returned.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with a JSON array of all post objects.
- Returns `500 ERROR` if any errors occurred.

-------------------------------------------------------------------------------

### GET /list/:postId

**! Requires `postId` path parameter.**

##### Description:

This endpoint will return a `200 OK` response with a list of all comments for the
given `postId` (including all properties) in the local database as a JSON array.

If no comments is found, an empty JSON array should be returned.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with a JSON array of all post objects.
- Returns `500 ERROR` if any errors occurred.
