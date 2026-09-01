# PacTask API Models and DTOs

## Domain model

### UserEntity

```csharp
public class UserEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<EnvironmentEntity> Environments { get; set; } = new List<EnvironmentEntity>();
}
```

### EnvironmentEntity

```csharp
public class EnvironmentEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    public int UserId { get; set; }
    public UserEntity? User { get; set; }
}
```

### TaskEntity

```csharp
public class TaskEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskEntityStatus Status { get; set; } = TaskEntityStatus.NotDone;
    public int EnvironmentId { get; set; }
    public EnvironmentEntity? Environment { get; set; }
}
```

### TaskEntityStatus

```csharp
public enum TaskEntityStatus
{
    NotDone,
    Done
}
```

## Request DTOs

### RegisterUserRequestDto

```csharp
public class RegisterUserRequestDto
{
    [Required]
    [MaxLength(32)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
```

### LoginUserRequestDto

```csharp
public class LoginUserRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
```

### UpdateUserRequestDto

```csharp
public class UpdateUserRequestDto
{
    [MaxLength(32)]
    public string? Username { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Password { get; set; }
}
```

### CreateEnvironmentRequestDto

```csharp
public class CreateEnvironmentRequestDto
{
    [Required]
    [MaxLength(32)]
    public string Title { get; set; } = string.Empty;
}
```

### UpdateEnvironmentRequestDto

```csharp
public class UpdateEnvironmentRequestDto
{
    [Required]
    [MaxLength(32)]
    public string Title { get; set; } = string.Empty;
}
```

### CreateTaskEntityRequestDto

```csharp
public class CreateTaskEntityRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Description { get; set; } = string.Empty;
}
```

### UpdateTaskEntityRequestDto

```csharp
public class UpdateTaskEntityRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public TaskEntityStatus Status { get; set; } = TaskEntityStatus.NotDone;
}
```

## Response DTOs

### LoggedUserDto

```csharp
public class LoggedUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
```

### UserDto

```csharp
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<EnvironmentEntity> Environments { get; set; } = new List<EnvironmentEntity>();
}
```

### EnvironmentDto

```csharp
public class EnvironmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}
```

### TaskDto

```csharp
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskEntityStatus Status { get; set; } = TaskEntityStatus.NotDone;
    public int EnvironmentId { get; set; }
}
```

## Database relationship notes

The relationship mapping is defined in ApplicationDBContext:

- User -> Environments: one-to-many, cascade delete
- Environment -> Tasks: one-to-many, cascade delete

This means deleting a user removes all their environments and tasks; deleting an environment removes all its tasks.

## Security notes

- Passwords are hashed using ASP.NET Core `PasswordHasher<UserEntity>`.
- JWT tokens are created with `TokenService.CreateToken()`.
- Tokens contain `email` and `username` claims.
- Token validation is configured in Program.cs with issuer, audience, and signing key from appsettings.json.
