namespace BankAccountAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}