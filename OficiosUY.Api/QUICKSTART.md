# 🚀 Quick Start Guide - Oficios UY API

## Start the API

```bash
dotnet run
```

The API will start at:
- **HTTPS**: https://localhost:5001
- **HTTP**: http://localhost:5000
- **Swagger**: https://localhost:5001/swagger

## Test the API (Using Swagger)

1. Open browser: `https://localhost:5001/swagger`

2. **Register a User**:
   - Endpoint: `POST /api/v1/auth/register`
   - Body:
   ```json
   {
     "name": "Juan Pérez",
     "email": "juan@example.com",
     "password": "password123",
     "role": "user"
   }
   ```

3. **Login**:
   - Endpoint: `POST /api/v1/auth/login`
   - Body:
   ```json
   {
     "email": "juan@example.com",
     "password": "password123"
   }
   ```
   - ✅ Cookies are set automatically

4. **Get Current User** (Protected):
   - Endpoint: `GET /api/v1/auth/me`
   - ✅ Works automatically with cookies

5. **Register a Provider**:
   ```json
   {
     "name": "Carlos Electricista",
     "email": "carlos@example.com",
     "password": "password123",
     "role": "provider"
   }
   ```

6. **Login as Provider** and **Create a Service**:
   - Endpoint: `POST /api/v1/services`
   - Body:
   ```json
   {
     "title": "Instalación Eléctrica",
     "description": "Instalación completa de sistemas eléctricos",
     "category": "Electricidad",
     "priceRange": "$2000 - $5000"
   }
   ```

7. **Get All Services**:
   - Endpoint: `GET /api/v1/services`

8. **Create a Booking** (as user):
   - Endpoint: `POST /api/v1/bookings`
   - Body:
   ```json
   {
     "serviceId": 1,
     "dateRequested": "2024-02-15T10:00:00Z"
   }
   ```

9. **Get User Bookings**:
   - Endpoint: `GET /api/v1/bookings/user/1`

## Test with Angular Frontend

1. Start the API: `dotnet run`
2. Start Angular app: `ng serve` (on port 4200)
3. CORS is already configured for `http://localhost:4200`
4. Cookies work automatically between frontend and backend

## Authentication Flow

```
1. User registers/logs in
   ↓
2. API sets HttpOnly cookies (access_token, refresh_token)
   ↓
3. Browser automatically sends cookies with each request
   ↓
4. API validates JWT from cookie
   ↓
5. Protected endpoints accessible
```

## Cookie Details

- **access_token**: Expires in 15 minutes
- **refresh_token**: Expires in 7 days
- **Attributes**: HttpOnly, Secure, SameSite=None

## Roles

- **user**: Can book services
- **provider**: Can create services + book services

## Common Endpoints

| Method | Endpoint | Auth | Role | Description |
|--------|----------|------|------|-------------|
| POST | /api/v1/auth/register | ❌ | - | Register new user |
| POST | /api/v1/auth/login | ❌ | - | Login user |
| POST | /api/v1/auth/logout | ❌ | - | Logout user |
| GET | /api/v1/auth/me | ✅ | - | Get current user |
| GET | /api/v1/users/{id} | ✅ | - | Get user by ID |
| PUT | /api/v1/users/{id} | ✅ | - | Update user |
| GET | /api/v1/services | ❌ | - | Get all services |
| POST | /api/v1/services | ✅ | provider | Create service |
| POST | /api/v1/bookings | ✅ | - | Create booking |
| GET | /api/v1/bookings/user/{id} | ✅ | - | Get user bookings |

## Troubleshooting

### CORS Issues
- Ensure Angular runs on `http://localhost:4200`
- Check `appsettings.json` → `CorsSettings:AllowedOrigins`

### Authentication Issues
- Check cookies in browser DevTools
- Verify JWT secret in `appsettings.json`
- Ensure HTTPS is used (cookies require Secure flag)

### Database Issues
- InMemory database resets on restart
- Data is not persisted between runs

## Environment Variables

Edit `appsettings.json`:

```json
{
  "JwtSettings": {
    "Secret": "your-secret-key-here",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "CorsSettings": {
    "AllowedOrigins": ["http://localhost:4200"]
  }
}
```

## Ready to Go! 🎉

Your Oficios UY backend API is fully functional and ready to integrate with the Angular frontend.
