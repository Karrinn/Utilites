using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CheckMonetaRUPaymentRefunds.FindAccountsList
{
    public static class Requests
    {
        private const string Url = "https://moneta.ru/services";

        public static async Task<RootObject> FindAccountsList(FindAccountsList.FindAccountsListRequest findAccountsListRequest)
        {
            using (var client = new HttpClient())
            {
                var findAccountsList = await client.PostAsync(Url, new StringContent(
                    JsonTemplates.FindAccountsList(findAccountsListRequest.Login, findAccountsListRequest.Password),
                    Encoding.UTF8, "application/json"));

                var findAccountsListResult = await findAccountsList.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<RootObject>(findAccountsListResult);
            }
        }

    }
}