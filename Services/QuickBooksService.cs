using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using QuickBooksIntegrationAPI.Models;

namespace QuickBooksIntegrationAPI.Services
{
    public class QuickBooksService
    {
        private readonly IConfiguration _config;
        private readonly string _authUrl;

        public QuickBooksService(IConfiguration config)
        {
            _config = config;
            var clientId = _config["QuickBooks:ClientId"];
            var redirectUri = _config["QuickBooks:RedirectUri"];
            _authUrl = $"https://appcenter.intuit.com/connect/oauth2?client_id={clientId}&response_type=code&scope=com.intuit.quickbooks.accounting&redirect_uri={redirectUri}";
        }

        public string GetAuthUrl()
        {
            return _authUrl;
        }

        public async Task<QuickBooksAuthResponse> ExchangeCodeForTokenAsync(string code)
        {
            var clientId = _config["QuickBooks:ClientId"];
            var clientSecret = _config["QuickBooks:ClientSecret"];
            var redirectUri = _config["QuickBooks:RedirectUri"];

            var client = new RestClient("https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer");
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"))}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUri);

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<QuickBooksAuthResponse>(response.Content);
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ErrorMessage}");
            }
        }

        // Method to get balance sheet data
        public async Task<List<BalanceSheet>> GetBalanceSheetAsync()
        {
            // Example code to fetch data from QuickBooks API
            // You would replace this with actual API calls and data processing

            var balanceSheetData = new List<BalanceSheet>
            {
                new BalanceSheet { AccountName = "Cash", OpeningBalance = 1000.00M, ClosingBalance = 1200.00M },
                new BalanceSheet { AccountName = "Accounts Receivable", OpeningBalance = 2000.00M, ClosingBalance = 1800.00M }
                // Add more accounts as needed
            };

            return await Task.FromResult(balanceSheetData);
        }
    }

    public class QuickBooksAuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IdToken { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
        public string Scope { get; set; }
    }
}
