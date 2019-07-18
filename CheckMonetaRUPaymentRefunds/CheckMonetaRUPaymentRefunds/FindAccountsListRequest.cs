using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckMonetaRUPaymentRefunds.FindAccountsList
{
    public class FindAccountsListRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
