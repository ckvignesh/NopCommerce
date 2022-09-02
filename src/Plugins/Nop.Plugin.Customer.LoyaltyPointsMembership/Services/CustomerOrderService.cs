using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Data;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly IRepository<Order>_orderRepository;
        public CustomerOrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<decimal> GetSumofCustomerCompletedOrderByDate(int customerId, int numberOfDays)
        {
            int oc = (int)OrderStatus.Complete;
            int od = (int)ShippingStatus.Delivered;
            var startdate = DateTime.UtcNow.Date.AddDays(-numberOfDays);
            var order = _orderRepository.Table.Where(o => o.CustomerId == customerId && o.OrderStatusId == oc);
            var orderwithinrange = order.Where(o=>o.PaidDateUtc >= startdate && o.PaidDateUtc <= DateTime.UtcNow.Date);
            return await orderwithinrange.SumAsync(o => o.OrderTotal);
        }
    }
}
