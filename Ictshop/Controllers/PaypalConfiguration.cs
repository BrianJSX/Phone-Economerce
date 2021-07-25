using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace Ictshop.Controllers
{
    public class PaypalConfiguration
    {

        public PaypalConfiguration()
        {
            var config = GetConfig();
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential("AZlshcHExTAGkjjDqJ0RUBExSMFKghrirKX9A_LKPhgZSERlECq1T2QvCadqcxRPCe1spHptKfFAf2Hz", "EJwWeZprp8ktlA2F0vcM-bLCEZatKSDrEs7jnkyezxA0cC95YzW5YXqUtDljz5EEjQnb1ArLb-jzXdKP", GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            var apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}