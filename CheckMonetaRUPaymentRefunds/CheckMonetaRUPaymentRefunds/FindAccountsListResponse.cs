using System.Collections.Generic;

namespace CheckMonetaRUPaymentRefunds.FindAccountsList
{
    public class FindAccountsListResponse
    {
        public List<Account> Account { get; set; }
    }

    public class Account
    {
        public decimal Balance { get; set; }
        public string Alias { get; set; }
        public string Currency { get; set; }
        public long Id { get; set; }
        public int Type { get; set; }
        public decimal AvailableBalance { get; set; }
        public int Status { get; set; }

        public override string ToString()
        {
            return $"Название: {Alias} \nБаналс: {Balance} \nДоступный баланс: {AvailableBalance}";
        }
    }

    public class Body
    {
        public FindAccountsListResponse FindAccountsListResponse { get; set; }
    }

    public class Envelope
    {
        public Body Body { get; set; }
    }

    public class RootObject
    {
        public Envelope Envelope { get; set; }
    }
}
