using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Data;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public class LoyaltyRewardPointsService: ILoyaltyRewardPointsService
    {
        private readonly IRepository<RewardPointsHistory> _rewardPointsHistoryRepository;
        public LoyaltyRewardPointsService(IRepository<RewardPointsHistory> rewardPointsHistoryRepository)
        {
            _rewardPointsHistoryRepository = rewardPointsHistoryRepository;
        }
        public virtual async Task<RewardPointsHistory> GetRewardPointsByCustomerIdAsync(int customerId)
        {
            if (customerId == 0)
                return null;

            return await _rewardPointsHistoryRepository.Table
                .Where(o => o.CustomerId == customerId).OrderByDescending(p => p.CreatedOnUtc).FirstOrDefaultAsync();
        }
        //public virtual async Task InsertRewardPointsHistoryEntryAsync(RewardPointsHistory rewardPointsHistory)
        //{
        //    await _rewardPointsHistoryRepository.InsertAsync(rewardPointsHistory);
        //}
    }
}
