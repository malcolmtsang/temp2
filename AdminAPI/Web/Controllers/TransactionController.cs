using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using Web.Models;
using Newtonsoft.Json;
using System.Linq;




namespace Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int accountNumber, DateTime startDate, DateTime endDate)
        {
            var response = await Client.GetAsync($"api/account/{accountNumber}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("Error", "Invalid account number.");
                return View();
            }

            var result = response.Content.ReadAsStringAsync().Result;
            var account = JsonConvert.DeserializeObject<Account>(result);
            IEnumerable<Transaction> transactions = account.Transactions.OrderBy(x => x.TransactionTimeUtc);
            var filter = FilterTransaction(transactions, startDate, endDate);

            return View(filter);
        }

        public IEnumerable<Transaction> FilterTransaction(IEnumerable<Transaction> transactions, DateTime startDate, DateTime endDate)
        {
            List<Transaction> filter = transactions.ToList();

            foreach (Transaction transaction in transactions)
            {
                if (transaction.TransactionTimeUtc < startDate.ToUniversalTime() || transaction.TransactionTimeUtc > endDate.ToUniversalTime())
                {
                    filter.Remove(transaction);
                }
            }

            return filter;
        }
    }
}
