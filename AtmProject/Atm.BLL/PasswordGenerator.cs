namespace Atm.BLL
{
    public static class PasswordGenerator
    {
        public static string Generate()
        {
            Random random = new Random();
            int passwordInt = random.Next(1000, 10000);

            return passwordInt.ToString();
        }
    }
}