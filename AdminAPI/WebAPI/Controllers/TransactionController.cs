using System;
using System.Collections.Generic;
using WebAPI.Models.DataManager;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models.DataController
{
    [ApiController, Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionManager _repo;
        public TransactionController(TransactionManager repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}"), HttpGet("{id}/{start}"), HttpGet("{id}/{start}/{end}")]
        public IEnumerable<Transaction> GetTransactions(int id, DateTime? start = null, DateTime? end = null)
        {
            return _repo.GetTransactions(id, start, end);
        }
    }
}
