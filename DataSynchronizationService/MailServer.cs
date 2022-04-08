
namespace DataSynchronizationService
{
    public class MailServer
    {
        public string   Uri { get; }
        public int      Port { get; }
        public string   Name { get; }
        public string   Address { get; }
        public string   Login { get; }
        public string   Password { get; }
        public bool     UseEncryption { get; }

        public MailServer(string Uri, int Port, string Login, string Password, string Name, string Address, bool UseEncryption)
        {
            this.Uri = Uri;
            this.Port = Port;
            this.Name = Name;
            this.Address = Address;
            this.Login = Login;
            this.Password = Password;
            this.UseEncryption = UseEncryption;
        }
    }
}
