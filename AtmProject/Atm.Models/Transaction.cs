namespace Atm.Models
{
    public class Transaction
    {
        public int UserId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        public DateTime DateTime { get; set; }
    }
}