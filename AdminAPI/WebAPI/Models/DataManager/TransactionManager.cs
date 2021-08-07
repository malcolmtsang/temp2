using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.Data;
using WebAPI.Models.Repository;

namespace WebAPI.Models.DataManager
{
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        private readonly MCBAContext _context;
        public TransactionManager(MCBAContext context)
        {
            _context = context;
        }

        public int Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.TransactionID;
        }

        public int Delete(int id)
        {
            _context.Transactions.Remove(_context.Transactions.Find(id));
            _context.SaveChanges();

            return id;
        }

        public Transaction Get(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;

        }

        public IEnumerable<Transaction> GetTransactions(int accountNumber, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate > endDate)
            {
                endDate = null; 
            }

            IEnumerable<Transaction> transactions;

            if (startDate == null && endDate == null)
            {
                transactions = _context.Transactions.OrderByDescending(t => t.TransactionTimeUtc).Where(t => t.AccountNumber == accountNumber);
            }
            else if (startDate != null && endDate == null)
            {
                transactions = _context.Transactions.OrderByDescending(t => t.TransactionTimeUtc)
                    .Where(t => t.AccountNumber == accountNumber && t.TransactionTimeUtc >= startDate);
            }
            else
            {
                transactions = _context.Transactions.OrderByDescending(t => t.TransactionTimeUtc)
                    .Where(t => t.AccountNumber == accountNumber && t.TransactionTimeUtc >= startDate && t.TransactionTimeUtc.Date <= endDate);
            }

            return transactions;
        }

    }
}
