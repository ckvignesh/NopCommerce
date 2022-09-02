using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Domain;
using Nop.Data;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Services;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public partial class LoyaltyPointsHistoryService : ILoyaltyPointsHistoryService
    {
        private readonly IRepository<LoyaltyMembership> _loyaltyMembershipRepository;
        public LoyaltyPointsHistoryService(IRepository<LoyaltyMembership> loyaltyMembershipRepository)
        {
            _loyaltyMembershipRepository = loyaltyMembershipRepository;
        }
        public virtual async Task InsertLoyaltyPointsHistoryAsync(LoyaltyMembership loyaltymembership)
        {
            await _loyaltyMembershipRepository.InsertAsync(loyaltymembership);
        }
        public virtual async Task<LoyaltyMembership>GetLoyaltyPointsHistoryByCustomerIdAsync(int customerId)
        { 
         var c =  await  _loyaltyMembershipRepository.Table.Where(x => x.CustomerId == customerId).OrderByDescending(p => p.CreatedOnUtc).FirstOrDefaultAsync();
            ;
            return c;
        }
    }

}
