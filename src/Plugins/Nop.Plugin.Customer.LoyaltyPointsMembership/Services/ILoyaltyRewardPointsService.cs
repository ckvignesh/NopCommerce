using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public partial interface ILoyaltyRewardPointsService
    {
        Task<RewardPointsHistory> GetRewardPointsByCustomerIdAsync(int customerId);
        //Task InsertRewardPointsHistoryEntryAsync(RewardPointsHistory rewardPointsHistory);
    }

}
