# PacTask API Endpoints

## User endpoints

### 1) Register a new user

- Method: POST
- Route: /api/User/register
- Auth: none

Request body:

```json
{
  "username": "alice",
  "email": "alice@example.com",
  "password": "123456"
}
```

Successful response:

```json
{
  "username": "alice",
  "email": "alice@example.com",
  "token": "eyJhbGciOiJIUzUxMiJ9..."
}
```

Possible statuses:
- 200 OK
- 400 Bad Request if validation fails

### 2) Login user

- Method: POST
- Route: /api/User
- Auth: none

Request body:

```json
{
  "username": "alice",
  "email": "alice@example.com",
  "password": "123456"
}
```

Successful response:

```json
{
  "username": "alice",
  "email": "alice@example.com",
  "token": "eyJhbGciOiJIUzUxMiJ9..."
}
```

Possible statuses:
- 200 OK
- 401 Unauthorized for invalid credentials
- 400 Bad Request if model validation fails

### 3) Update current user

- Method: PUT
- Route: /api/User
- Auth: required

Request body:

```json
{
  "username": "alice_updated",
  "email": "alice.new@example.com",
  "password": "newPassword123"
}
```

Successful response:

```json
{
  "id": 1,
  "username": "alice_updated",
  "email": "alice.new@example.com",
  "environments": []
}
```

Possible statuses:
- 200 OK
- 400 Bad Request if validation fails or user not found
- 401 Unauthorized if token is invalid

## Environment endpoints

### 4) Get all environments for current user

- Method: GET
- Route: /api/Environment
- Auth: required

Successful response:

```json
[
  {
    "id": 1,
    "title": "Work"
  },
  {
    "id": 2,
    "title": "Personal"
  }
]
```

Possible statuses:
- 200 OK
- 404 Not Found if user is not found

### 5) Create environment

- Method: POST
- Route: /api/Environment
- Auth: required

Request body:

```json
{
  "title": "Work"
}
```

Successful response:
- Returns 201 Created with the request payload as the response body.

Example:

```json
{
  "title": "Work"
}
```

Possible statuses:
- 201 Created
- 400 Bad Request on validation errors
- 404 Not Found if user is missing

### 6) Update environment

- Method: PUT
- Route: /api/Environment/{id}
- Auth: required

Request body:

```json
{
  "title": "Updated Work"
}
```

Successful response:

```json
{
  "id": 1,
  "title": "Updated Work"
}
```

Possible statuses:
- 200 OK
- 401 Unauthorized if user does not own the environment
- 404 Not Found if environment is missing

### 7) Delete environment

- Method: DELETE
- Route: /api/Environment/{id}
- Auth: required

Successful response:

```json
{
  "id": 1,
  "title": "Updated Work"
}
```

Possible statuses:
- 200 OK
- 401 Unauthorized if user does not own the environment
- 404 Not Found if environment is missing

## Task endpoints

### 8) Get all tasks in an environment

- Method: GET
- Route: /api/Task/{id}
- Auth: required

This route expects the environment id and checks that the current user has access to that environment.

Successful response:

```json
[
  {
    "id": 1,
    "title": "Ship feature",
    "description": "Finish the dashboard release",
    "status": 0,
    "environmentId": 1
  }
]
```

Possible statuses:
- 200 OK
- 401 Unauthorized if user doesn't have access
- 404 Not Found if user doesn't exist

### 9) Get a single task by id

- Method: GET
- Route: /api/Task/{id}/unique
- Auth: required

Successful response:

```json
{
  "id": 1,
  "title": "Ship feature",
  "description": "Finish the dashboard release",
  "status": 0,
  "environmentId": 1
}
```

Possible statuses:
- 200 OK
- 401 Unauthorized if user doesn't own the task's environment
- 404 Not Found if task not found

### 10) Create task in environment

- Method: POST
- Route: /api/Task/{id}
- Auth: required

Request body:

```json
{
  "title": "Ship feature",
  "description": "Finish the dashboard release"
}
```

Successful response:
- Returns 201 Created but body is the original request payload (`CreateTaskEntityRequestDto`)

Possible statuses:
- 201 Created
- 400 Bad Request on validation failure
- 401 Unauthorized when user does not have access to environment
- 404 Not Found if user is missing

### 11) Update task

- Method: PUT
- Route: /api/Task/{id}
- Auth: required

Request body:

```json
{
  "title": "Ship feature",
  "description": "Finish the dashboard release and QA",
  "status": 1
}
```

Successful response:

```json
{
  "id": 1,
  "title": "Ship feature",
  "description": "Finish the dashboard release and QA",
  "status": 1,
  "environmentId": 1
}
```

Possible statuses:
- 200 OK
- 400 Bad Request if model state invalid or task cannot be updated
- 401 Unauthorized if user doesn't own the task
- 404 Not Found if task doesn't exist

### 12) Delete task

- Method: DELETE
- Route: /api/Task/{id}
- Auth: required

Successful response:

```json
{
  "id": 1,
  "title": "Ship feature",
  "description": "Finish the dashboard release",
  "status": 0,
  "environmentId": 1
}
```

Possible statuses:
- 200 OK
- 401 Unauthorized if user doesn't own the task
- 404 Not Found if task doesn't exist
- 400 Bad Request if delete fails
