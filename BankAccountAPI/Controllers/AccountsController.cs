using BankAccountAPI.Data;
using BankAccountAPI.DTO;
using BankAccountAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BankAccountAPI.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly BankingContext _context;

        public AccountsController(BankingContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var account = new Account
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                AccountNumber = new Random().Next(100000, 999999).ToString()
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new { account.AccountNumber });
        }

        [HttpGet("balance")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> GetBalance([FromQuery] string accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null) return NotFound("Conta não encontrada");

            return Ok(new { Balance = account.Balance });
        }

        [HttpGet("statement")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> GetStatement([FromQuery] string accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null) return NotFound("Conta não encontrada");

            var transactions = new List<object>
    {
        new { Date = DateTime.Now, Amount = -50.00, Type = "Transfer" },
        new { Date = DateTime.Now.AddDays(-1), Amount = 200.00, Type = "Deposit" }
    };

            return Ok(transactions);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferDto request)
        {
            // Buscar a conta remetente
            var sender = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.SourceAccount);
            if (sender == null) return NotFound("Conta origem não encontrada");

            // Buscar a conta destinatária
            var recipient = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.DestinationAccount);
            if (recipient == null) return NotFound("Conta destino não encontrada");

            // Verificar se a conta remetente tem saldo suficiente
            if (sender.Balance < request.Amount) return BadRequest("Saldo insuficiente");

            // Atualizar os saldos das contas
            sender.Balance -= request.Amount;
            recipient.Balance += request.Amount;

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();

            return Ok("Transferência realizada com sucesso");
        }



        private async Task<Account> Authenticate(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic "))
            {
                return null;
            }

            var encodedCredentials = authorizationHeader.Replace("Basic ", "");
            var decodedBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.UTF8.GetString(decodedBytes).Split(':');

            if (credentials.Length != 2)
            {
                return null;
            }

            var email = credentials[0];
            var password = credentials[1];

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
            {
                return null;
            }

            return account;
        }
    }  
}
