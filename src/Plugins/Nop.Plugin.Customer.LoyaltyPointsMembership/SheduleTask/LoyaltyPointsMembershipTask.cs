using System.Threading.Tasks;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Services;
using Nop.Services.Customers;
using Nop.Services.ScheduleTasks;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.SheduleTask
{
    /// <summary>
    /// Represents a schedule task to synchronize contacts
    /// </summary>
    public class LoyaltyPointsMembershipTask : IScheduleTask
    {
        private readonly ILoyaltyPointsMembershipService _loyaltyPointsMembershipService;
        public LoyaltyPointsMembershipTask(ILoyaltyPointsMembershipService loyaltyPointsMembershipService)
        {
            _loyaltyPointsMembershipService = loyaltyPointsMembershipService;
        }
        public async Task ExecuteAsync()
        {
            await _loyaltyPointsMembershipService.LoyaltyPointSheduleTask();
        }
    }
}