using System;
using Newtonsoft.Json.Linq;

namespace CheckMonetaRUPaymentRefunds
{
    public static class JsonTemplates
    {
        private const string RootNodeName = "Envelope";

        private static JProperty GetHeader(string login, string password)
        {
            return new JProperty("Header",
                         new JObject(
                            new JProperty("Security",
                                new JObject(
                                    new JProperty("UsernameToken",
                                        new JObject(
                                            new JProperty("Username", login),
                                            new JProperty("Password", password)))))));
        }

        public static string VerifyPayment(string login, string password, string payer, string payee, decimal amount, string clientTransaction, string paymentToken, string desciption)
        {
            return PaymentInner("VerifyPaymentRequest", login, password, payer, payee, amount, clientTransaction, paymentToken, desciption);
        }

        public static string Payment(string login, string password, string payer, string payee, decimal amount, string clientTransaction, string paymentToken, string desciption)
        {
            return PaymentInner("PaymentRequest", login, password, payer, payee, amount, clientTransaction, paymentToken, desciption);
        }

        private static string PaymentInner(string methodName, string login, string password, string payer, string payee, decimal amount, string clientTransaction, string paymentToken, string desciption)
        {
            var header = GetHeader(login, password);
            var body = new JProperty("Body",
                        new JObject(
                            new JProperty(methodName,
                                new JObject(
                                    new JProperty("version", "VERSION_2"),
                                    new JProperty("payer", payer),
                                    new JProperty("payee", payee),
                                    new JProperty("amount", amount),
                                    new JProperty("isPayerAmount", "true"),
                                    new JProperty("clientTransaction", clientTransaction),
                                    new JProperty("description", desciption),
                                    new JProperty("operationInfo",
                                        new JObject(
                                            new JProperty("attribute",
                                                new JArray(
                                                    new JObject(
                                                        new JProperty("key", "PAYMENTTOKEN"),
                                                        new JProperty("value", paymentToken))
                                                        ))))))));

            var requestObject = new JObject(
                                   new JProperty(RootNodeName,
                                       new JObject(header, body)));

            return requestObject.ToString();
        }

        public static string FindAccountsList(string login, string password)
        {
            var header = GetHeader(login, password);
            var body = new JProperty("Body", 
                new JObject(
                    new JProperty("FindAccountsListRequest",
                        new JObject()
            )));

            var requestObject = new JObject(
                new JProperty(RootNodeName,
                    new JObject(header, body)));

            return requestObject.ToString();
        }
    }
}
