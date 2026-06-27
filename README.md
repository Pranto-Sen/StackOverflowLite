# StackOverflow Lite API

A simplified **StackOverflow-style Question & Answer REST API** built with **ASP.NET Core 9**, following **Clean Architecture** and **CQRS (MediatR)** principles.

This project allows users to register, ask questions, answer questions, vote on questions and answers, accept the best answer, and earn reputation based on community interactions.


---

# Technologies

- ASP.NET Core 9
- C#
- PostgreSQL
- Entity Framework Core
- ASP.NET Identity
- JWT Authentication
- MediatR
- FluentValidation
- Redis
- Docker
- Swagger / OpenAPI

---

# Architecture

## The project follows **Clean Architecture**.

```
StackOverflowLite
│
├── StackOverflowLite.API
│
├── StackOverflowLite.Application
│
├── StackOverflowLite.Domain
│
├── StackOverflowLite.Persistence
│
├── StackOverflowLite.Infrastructure
│
├── docker-compose.yml
│
├── Dockerfile
│
└── README.md
```

### API

Handles HTTP Requests and Responses.

### Application

Contains

- Commands
- Queries
- DTOs
- Validators
- Handlers

### Domain

Contains

- Entities
- Enums
- Business Models

### Persistence

Contains

- DbContext
- EF Core Configurations
- Repository Implementations
- Migrations

### Infrastructure

Contains

- JWT Service
- Redis Service
- Current User Service
- External Services

---


# Prerequisites

Before running this project, install the following:

- .NET 9 SDK
- Docker Desktop
- PostgreSQL
- Redis
- Visual Studio 2022 / VS Code

---

# Clone Repository

```bash
https://github.com/Pranto-Sen/StackOverflowLite.git
```

Go inside project

```bash
cd StackOverflowLite
```

---

# Configuration

Open

```
StackOverflowLite.API/appsettings.json
```

Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=stackoverflowlite;Username=postgres;Password=1234"
  },

  "Jwt": {
    "Key": "THIS_IS_A_VERY_SECRET_KEY_123456",
    "Issuer": "StackOverflowLite",
    "Audience": "StackOverflowLiteUsers"
  },

  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

---

# Running Locally


## Step 1

Build Project

```bash
dotnet build
```

---

## Step 2

Create Migration (Skip if already exists)

```bash
dotnet ef migrations add InitialCreate \
--project StackOverflowLite.Persistence \
--startup-project StackOverflowLite.API
```

---

## Step 3

Apply Migration

```bash
dotnet ef database update \
--project StackOverflowLite.Persistence \
--startup-project StackOverflowLite.API
```
---

## Step 4
If you want to run the API locally, start a Redis container on port **6380**:

```bash
docker run -d --name redis-local -p 6380:6379 redis
```
---
## Step 5

Run API

```bash
dotnet run --project StackOverflowLite.API
```

---

# Running With Docker




## Start Containers

```bash
docker compose up --build
```




---

## Docker Services

Docker starts the following services automatically.

| Service | Port |
|----------|------|
| API | 8080 |
| PostgreSQL | 5432 |
| Redis | 6379 |

---

## Access Swagger

After running the application



Docker

```
http://localhost:8080/swagger
```

(Port may vary depending on your settings.)



---


# Authentication Flow

Before creating Questions, Answers, Voting, or any other protected resource, you must first create an account and obtain a JWT token.


# Step 1 - Register

Creates a new user account.

### Endpoint

```http
POST /api/auth/register
```

### Authorization

❌ No Authentication Required

---

## Request Body

```json
{
  "userName": "pranto",
  "email": "pranto@gmail.com",
  "password": "Password123!"
}
```




---

# Step 2 - Login

Login using your registered email and password.

### Endpoint

```http
POST /api/auth/login
```



## Request Body

```json
{
  "email": "pranto@gmail.com",
  "password": "Password123!"
}
```

---

## Successful Response

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9......",
  "userId": "57cadb67-c947-479a-b027-2dad89a0db9a",
  "userName": "pranto",
  "email": "pranto@gmail.com"
}
```

Copy the **token** from the response. 

You will use this token for every protected endpoint.

---

## Invalid Credentials

```json
{
  "success": false,
  "message": "Invalid credentials."
}
```

---

# Step 3 - Authorize Swagger

Open Swagger.

Click the **Authorize** button located at the top-right corner.

Paste the token in the following format:

```text
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

> **Important:**  
> No need to use **Bearer** keyword , Just paste the JWT token.



Now all protected endpoints can be accessed.

---

# Step 4 - Get Logged-in User Profile

Returns information about the currently authenticated user.

### Endpoint

```http
GET /api/auth/profile
```



## Successful Response

```json
{
  "id": "57cadb67-c947-479a-b027-2dad89a0db9a",
  "userName": "pranto",
  "email": "pranto@gmail.com",
  "reputation": 0
}
```

Initially, every new user starts with:

```
Reputation = 0
```

As users receive votes and accepted answers, the reputation value automatically changes.

---



# Tags & Questions

After successfully logging in and authorizing with your JWT token, you can start creating Tags and Questions.



# Step 5 - Create Tag

Creates a new tag.

## Endpoint

```http
POST /api/tags
```



---

## Request Body

```json
{
  "name": "ASP.NET Core"
}
```


---

# Step 6 - Get All Tags

Returns every available tag.

## Endpoint

```http
GET /api/tags
```



---

## Success Response

```json
[
    {
        "id": "fd2d4d37-fb63-4db0-b59c-5dbaf32a74fd",
        "name": "ASP.NET Core"
    },
    {
        "id": "963b7d20-3af2-495d-81dd-f8564fd0bd91",
        "name": "Redis"
    },
    {
        "id": "7a84844f-4d5e-4f52-9b77-76dcdbad7d7a",
        "name": "Docker"
    }
]
```


---

# Step 7 - Create Question

## Endpoint

```http
POST /api/questions
```



---

## Request Body

```json
{
    "title": "How to use Redis with ASP.NET Core?",
    "description": "I want to cache my API response using Redis. What is the best approach?",
}
```



---

# Step 8 - Get All Questions

Returns every question.

## Endpoint

```http
GET /api/questions
```


---

## Success Response

```json
[
    {
        "id": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf",
        "title": "How to use Redis with ASP.NET Core?",
        "description": "I want to cache my API response using Redis.",
        "author": "pranto",
        "viewCount": 12,
        "acceptedAnswerId": null,
        "createdAt": "2026-06-26T14:35:51Z"
    }
]
```

> **Note:**  
> This endpoint is cached using **Redis** to improve performance.  
> The cache is automatically cleared whenever a question is created, updated, or deleted.

---

# Step 9 - Get Question By Id

Returns details of a specific question.

## Endpoint

```http
GET /api/questions/{questionId}
```


---

## Success Response

```json
{
    "id": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf",
    "title": "How to use Redis with ASP.NET Core?",
    "description": "I want to cache my API response using Redis.",
    "author": "pranto",
    "viewCount": 12,
    "acceptedAnswerId": null,
    "createdAt": "2026-06-26T14:35:51Z"
}
```

> **Note:**  
> This endpoint is cached using **Redis** to track how many times a question has been viewed.
>Instead of updating PostgreSQL every time someone opens a question, the application stores the view count in **Redis**.
>This approach is much faster and reduces unnecessary database writes.

---

# Step 10 - Set Questions Tag


## Endpoint

```http
POST /api/tag/assign
```
## Request Body
```
{
  "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "tagIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```


---

# Step 11 - Filter Questions by Tag

Returns questions belonging to a specific tag.

## Endpoint

```http
GET /api/questions/tag/{tagName}
```

Example

```http
GET /api/questions/tag/Docker
```

---

## Success Response

```json
[
    {
        "id": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf",
        "title": "How to use Redis with ASP.NET Core?",
        "author": "pranto"
    },
    {
        "id": "cc5bcab4-76c4-45e8-ae5f-09f5a930bb1c",
        "title": "Redis Cache Expiration",
        "author": "john"
    }
]
```



# Step 12 - Create Answer

Creates a new answer for a question.

## Endpoint

```http
POST /api/answers
```

---

## Request Body

```json
{
    "questionId": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf",
    "content": "You can use Redis by injecting IDistributedCache into your service."
}
```

---

# Step 13 - Get Answers By Question

Returns every answer belonging to a question.

## Endpoint

```http
GET /api/answers/question/{questionId}
```

Example

```http
GET /api/answers/question/70cf4570-8a69-4d72-9d43-fb8a2c4d4baf
```



## Success Response

```json
[
    {
        "id": "2b74d35b-17af-4fc4-9328-056d65cb35d8",
        "content": "You can use Redis by injecting IDistributedCache.",
        "author": "john",
        "isAccepted": false,
        "createdAt": "2026-06-26T14:55:12Z"
    },
    {
        "id": "6d7db12f-4e94-4628-bae8-6cf0efadf70a",
        "content": "Redis works very well with ASP.NET Core.",
        "author": "alice",
        "isAccepted": false,
        "createdAt": "2026-06-26T15:02:43Z"
    }
]
```

If an accepted answer exists:

```json
[
    {
        "id": "2b74d35b-17af-4fc4-9328-056d65cb35d8",
        "content": "You can use Redis by injecting IDistributedCache.",
        "author": "john",
        "isAccepted": true
    }
]
```


---

## Cannot Delete Accepted Answer

If the answer is currently accepted.

```json
{
    "success": false,
    "message": "Unaccept the answer before deleting it."
}
```




# Step 14 - Accept Answer

## Endpoint

```http
POST /api/answers/accept
```


## Request Body

```json
{
    "answerId": "2b74d35b-17af-4fc4-9328-056d65cb35d8"
}
```



Database changes

Answer Table

```
IsAccepted = true
```

Question Table

```
AcceptedAnswerId = 2b74d35b-17af-4fc4-9328-056d65cb35d8
```

Answer owner's reputation

```
+15
```

---

## Cannot Accept Own Answer

```json
{
    "success": false,
    "message": "Cannot accept your own answer."
}
```

---

## Not Question Owner

```json
{
    "success": false,
    "message": "Only question owner can accept answer."
}
```

---

# Step 15 - Switch Accepted Answer

Suppose Answer A

```
Accepted
```



```
AcceptedAnswerId = A
```

Now the question owner accepts Answer B.

The application automatically performs: 

```
Answer A
IsAccepted = false
```



```
Answer B
IsAccepted = true
```

Then

```
AcceptedAnswerId = AnswerB
```

Reputation

Answer A

```
-15
```

Answer B

```
+15
```

No manual action is required.

---

# Step 16 - Unaccept Answer

Removes the accepted answer.

## Endpoint

```http
POST /api/answers/unaccept
```



## Request Body

```json
{
    "answerId": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf"
}
```



Database changes

Answer

```
IsAccepted = false
```

Question

```
AcceptedAnswerId = null
```

Reputation

```
-15
```

---



# Question Voting

---

# Step 17 - Vote Question

## Endpoint

```http
POST /api/votes/question
```


---

## Upvote Request

```json
{
    "questionId": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf",
    "voteType": 1
}
```

> **voteType**
>
> - `1` = Upvote
> - `-1` = Downvote


---

### Database Changes

QuestionVote Table

```
VoteType = Upvote
```

Question Owner Reputation

```
+5
```

---

# Downvote Question

```json
{
    "questionId": "70cf4570-8a69-4d72-9d43-fb8a2c4d4baf",
    "voteType": -1
}
```

Question Owner Reputation

```
-1
```

---

# Cannot Vote Own Question

```json
{
    "success": false,
    "message": "You cannot vote on your own question."
}
```



---

# Answer Voting

---

# Step 18 - Vote Answer

## Endpoint

```http
POST /api/votes/answer
```


## Upvote Answer

```json
{
    "answerId": "2b74d35b-17af-4fc4-9328-056d65cb35d8",
    "voteType": 1
}
```



---

### Database Changes

AnswerVote Table

```
VoteType = Upvote
```

Answer Owner Reputation

```
+10
```

---

# Downvote Answer

```json
{
    "answerId": "2b74d35b-17af-4fc4-9328-056d65cb35d8",
    "voteType": -1
}
```

Answer Owner Reputation

```
-2
```

---

# Cannot Vote Own Answer

```json
{
    "success": false,
    "message": "You cannot vote on your own answer."
}
```

---



