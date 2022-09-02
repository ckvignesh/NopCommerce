using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public partial interface ICustomerInfoServices
    {
        Task<List<Core.Domain.Customers.Customer>> GetAllCustomers();
        Task<List<Core.Domain.Customers.Customer>?> GetCustomersWithBirtdayAsToday();
    }
}
