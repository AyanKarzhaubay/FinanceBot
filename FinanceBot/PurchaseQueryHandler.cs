using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceBot
{
    public class PurchaseQueryHandler
    {
        private readonly BotDbContext _dbContext;

        public PurchaseQueryHandler(BotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Purchase>> GetPurchasesAsync(int userId, DateTime startDate, DateTime endDate, string category = null)
        {
            var query = _dbContext.Purchases
                .Where(p => p.UserId == userId && p.Date >= startDate && p.Date <= endDate);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<decimal> GetTotalSpentAsync(int userId, DateTime startDate, DateTime endDate, string category = null)
        {
            var query = _dbContext.Purchases
                .Where(p => p.UserId == userId && p.Date >= startDate && p.Date <= endDate);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return Task.FromResult(query.Sum(p => p.Price));
        }
    }


}
