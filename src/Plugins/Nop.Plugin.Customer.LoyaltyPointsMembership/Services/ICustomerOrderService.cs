using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public partial interface ICustomerOrderService
    {
        Task<decimal> GetSumofCustomerCompletedOrderByDate(int customerId, int numberOfDays);
    }
}