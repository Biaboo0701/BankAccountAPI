using BankAccountAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BankAccountAPI.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, BankingContext dbContext)
        {
            var path = context.Request.Path.Value;

            // Ignorar autenticação para rotas específicas
            if (path.StartsWith("/api/accounts/register") ||
                path.StartsWith("/api/accounts/balance") ||
                path.StartsWith("/api/accounts/statement") ||
                path.StartsWith("/api/accounts/transfer"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                return;
            }

            if (context.Request.Path.StartsWithSegments("/api/accounts/register"))
            {
                await _next(context);
                return;
            }


            var authHeader = context.Request.Headers["Authorization"].ToString();
            var encodedCredentials = authHeader.Replace("Basic ", "");
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.UTF8.GetString(decodedBytes).Split(':');

            if (credentials.Length != 2)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var email = credentials[0];
            var password = credentials[1];

            var user = await dbContext.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next(context);
        }
    }
}
