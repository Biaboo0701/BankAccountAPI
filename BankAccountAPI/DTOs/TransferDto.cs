namespace BankAccountAPI.DTO
{
    public class TransferDto
    {
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
