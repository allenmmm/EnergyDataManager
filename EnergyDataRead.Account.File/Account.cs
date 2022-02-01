namespace EnergyDataReader.Account.File
{
    public class Account
    {
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public Account(
            string accountId,
            string firstName,
            string lastName)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
        }

        public Account(Account account)
        {
            AccountId = account.AccountId;
            FirstName = account.FirstName;
            LastName = account.LastName;
        }
    }
}
