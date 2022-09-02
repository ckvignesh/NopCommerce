using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Domain;


namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public partial interface ILoyaltyPointsHistoryService
    {
        Task InsertLoyaltyPointsHistoryAsync(LoyaltyMembership loyaltymembership);
        Task<LoyaltyMembership> GetLoyaltyPointsHistoryByCustomerIdAsync(int customerId);
    }
    
}
