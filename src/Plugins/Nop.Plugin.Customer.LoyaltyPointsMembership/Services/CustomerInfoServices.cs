using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public class CustomerInfoServices : ICustomerInfoServices
    {
        private readonly IRepository<Core.Domain.Customers.Customer> _customerRepository;
        private readonly IRepository<Core.Domain.Common.GenericAttribute> _genericAttributeRepository;
        public CustomerInfoServices(IRepository<Core.Domain.Customers.Customer> customerRepository, IRepository<Core.Domain.Common.GenericAttribute> genericAttributeRepository)
        {
            _customerRepository = customerRepository;
            _genericAttributeRepository = genericAttributeRepository;
        }
        public async Task<List<Core.Domain.Customers.Customer>> GetAllCustomers()
        {
            return await _customerRepository.Table.ToListAsync();
        }
        public async Task<List<Core.Domain.Customers.Customer>?> GetCustomersWithBirtdayAsToday()
        {
            var hasSuchKey = await _genericAttributeRepository.Table.AnyAsync(o => o.KeyGroup == "Customer" && o.Key == "DateOfBirth");
            if (hasSuchKey)
            {
                var entities = _genericAttributeRepository.Table.Where(o => o.KeyGroup == "Customer" && o.Key == "DateOfBirth").ToList();
                var customerIds = new List<int>();
                foreach (var entity in entities)
                {
                    //Ensure expected Data format = "yyyy-MM-dd"
                    var d = entity.Value.Split("-");
                    if (d[1] == DateTime.UtcNow.Month.ToString("D2") && d[2] == DateTime.UtcNow.Day.ToString())
                    {
                        customerIds.Add(entity.EntityId);
                    }
                }
                var s = await _customerRepository.Table.Where(o => customerIds.Contains(o.Id)).ToListAsync();
                return s;
            }
            return null;
        }
    }
}
