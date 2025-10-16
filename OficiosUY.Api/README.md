# Oficios UY - Backend API

Backend API for **Oficios UY**, a platform connecting users with independent service providers in Uruguay.

## Tech Stack

- **.NET 9** Web API
- **Entity Framework Core** (InMemory for MVP)
- **JWT Authentication** with HttpOnly cookies
- **FluentValidation** for request validation
- **Swagger/OpenAPI** for API documentation
- **BCrypt** for password hashing

## Architecture

Clean/Layered architecture with separation of concerns:

```
OficiosUY.Api/
├── Configuration/       # App settings classes
├── Controllers/         # API endpoints
├── Data/               # DbContext
├── DTOs/               # Data transfer objects
├── Entities/           # Domain models
├── Middleware/         # Custom middleware
├── Services/           # Business logic
│   └── Implementations/
└── Validators/         # FluentValidation rules
```

## Running the Project

```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger`

## API Endpoints

### Authentication (`/api/v1/auth`)
- `POST /register` - Register new user
- `POST /login` - Login user
- `POST /logout` - Logout user
- `POST /refresh` - Refresh access token
- `GET /me` - Get current user info (requires auth)

### Users (`/api/v1/users`)
- `GET /{id}` - Get user by ID (requires auth)
- `PUT /{id}` - Update user (requires auth)

### Services (`/api/v1/services`)
- `GET /` - Get all services
- `POST /` - Create service (requires provider role)

### Bookings (`/api/v1/bookings`)
- `POST /` - Create booking (requires auth)
- `GET /user/{userId}` - Get user bookings (requires auth)

## Authentication Flow

1. **Register/Login**: Returns JWT tokens in HttpOnly cookies (`access_token`, `refresh_token`)
2. **Authenticated Requests**: JWT automatically read from cookie
3. **Token Refresh**: Use `/auth/refresh` endpoint when access token expires
4. **Logout**: Clears authentication cookies

## Cookie Configuration

- **HttpOnly**: Yes (prevents XSS attacks)
- **Secure**: Yes (HTTPS only)
- **SameSite**: None (allows cross-origin requests from Angular frontend)
- **Access Token Expiry**: 15 minutes
- **Refresh Token Expiry**: 7 days

## CORS Configuration

Configured to allow requests from Angular frontend at `http://localhost:4200` with credentials.

## User Roles

- **user**: Regular users who book services
- **provider**: Service providers who offer services

## Database

Using **InMemory** database for MVP. Data is reset on application restart.

### Entities

- **User**: Id, Name, Email, PasswordHash, Role, RefreshToken, RefreshTokenExpiry
- **Service**: Id, Title, Description, Category, PriceRange, ProviderId
- **Booking**: Id, UserId, ServiceId, DateRequested, Status

## Configuration

Edit `appsettings.json` to configure:
- JWT secret, issuer, audience, token expiration
- CORS allowed origins
- Logging levels

## Testing with Swagger

1. Navigate to `https://localhost:5001/swagger`
2. Register a new user via `/api/v1/auth/register`
3. Login via `/api/v1/auth/login` (cookies are set automatically)
4. Use authenticated endpoints (Swagger will send cookies automatically)

## Integration with Angular Frontend

The API is designed to work seamlessly with Angular 20 SSR:
- JWT tokens stored in HttpOnly cookies
- CORS configured for `http://localhost:4200`
- Credentials included in requests
- Consistent JSON response format

## Error Handling

Global error handling middleware catches all exceptions and returns consistent error responses:

```json
{
  "success": false,
  "message": "Error description",
  "error": "Technical details"
}
```

## Security Features

✅ Password hashing with BCrypt  
✅ JWT with refresh token mechanism  
✅ HttpOnly cookies (XSS protection)  
✅ Secure & SameSite cookie attributes  
✅ Role-based authorization  
✅ Input validation with FluentValidation  
✅ CORS configuration  

## Next Steps for Production

- Replace InMemory database with SQL Server/PostgreSQL
- Add email verification
- Implement password reset
- Add rate limiting
- Configure HTTPS certificates
- Add logging (Serilog)
- Implement caching (Redis)
- Add health checks
- Configure environment-specific settings
