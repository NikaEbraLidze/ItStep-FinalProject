namespace Atm.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PersonalNumber { get; set; }
        public required string Password { get; set; }
        public decimal Balance { get; set; }
    }
}