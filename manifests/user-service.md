# User service

## Metadata

```
{
  "name": "user",     // FIXED    - don't change
  "port": 80          // VARIABLE - you choose
}
```

## Description

This service's responsibility is to store and serve users.

The user model should have the following properties:
- `id`: (?) - unique user id - can be whatever type you choose.
- `email`: (string) - the user's email address
- `familyName`: (string) - the user's last name
- `givenName`: (string) - the user's first name

**All fields in the model (when returned as JSON) must conform to `lowerCamelCase` formats, to save a little ink during printouts.**


## Headers

- Content-Type: application/json

## Endpoints table

| Verb       | Endpoint                 | Description                           |
| ----------:|:------------------------ |:--------------------------------------|
| POST       | /                        | Create user (or update user)          |
| GET        | /list                    | Fetch all users.                      |
| GET        | /:userId                 | Fetch a single user.                  |


## Endpoint descriptions

### POST /

**! Requires JSON data as body.**

##### Example incoming data structure:
```
{
 "email": "someone@peanuts.no"  // REQUIRED
 "givenName": "Samuel"          // REQUIRED
 "familyName": "Jackson"        // REQUIRED
}
```

##### Description:

When a user is authenticated via the `auth` service, the service will send user data
to this endpoint. This endpoint should handle creating and updating users based on the
received data from the `auth` service. There should only be **one** user instance per
email address, thus if there is an existing user - this user should be updated with
the given data.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with the created user as JSON.
- Returns `500 ERROR` if any errors occurred.

-------------------------------------------------------------------------------

### GET /list

##### Description:

This endpoint will return a `200 OK` response with a list of all the users
in the local database as a JSON array.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with a JSON array of all user objects.
- Returns `500 ERROR` if any errors occurred.

-------------------------------------------------------------------------------

### GET /:userId

**! Requires user ID as path parameter.**

##### Description:

This endpoint will return a `200 OK` response with the user matching the given
`userId` as JSON.

If the user was not found, a `404 NOT FOUND` response should be returned.

If any exceptions occurs, a `500 ERROR` response should be returned.

##### Response status codes:

- Returns `200 OK` with a user as JSON matching the given `userId`.
- Returns `404 NOT` FOUND if the `userId` did not match any existing users.
- Returns `500 ERROR` if any errors occurred.
