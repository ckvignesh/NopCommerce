using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Models;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Services;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Payments;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Components
{
    /// <summary>
    /// Represents the view component to display payment info in public store
    /// </summary>
    [ViewComponent(Name = "LoyaltyPointsMembership")]
    public class LoyaltyPointsMembershipComponent : NopViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly ILoyaltyPointsHistoryService _loyaltyPointsHistoryService;
        private readonly ICustomerService _customerService;

        public LoyaltyPointsMembershipComponent(IWorkContext workContext, ILoyaltyPointsHistoryService loyaltyPointsHistoryService, ICustomerService customerService)
        {
            _workContext = workContext;
            _loyaltyPointsHistoryService = loyaltyPointsHistoryService;
            _customerService = customerService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await GetLoyaltyPointsMembershipModelAsync();
            return View("~/Plugins/Customer.LoyaltyPointsMembership/Views/LoyaltyPointsMembership.cshtml", model);
        }
        public async Task<LoyaltyPointsMembershipModel> GetLoyaltyPointsMembershipModelAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (await _customerService.IsRegisteredAsync(customer))
            {
                var customerId = customer.Id;
                var loyaltyPointsHistory = await _loyaltyPointsHistoryService.GetLoyaltyPointsHistoryByCustomerIdAsync(customerId);             
                if (loyaltyPointsHistory != null)
                {
                    var mem = loyaltyPointsHistory.CurrentMembership;
                    return new LoyaltyPointsMembershipModel
                    {
                        Membership = mem
                    };
                }
                else
                {
                    return new LoyaltyPointsMembershipModel
                    {
                        Membership = "Silver Membership" 
                    };
                }
            }
            else
            {
                return new LoyaltyPointsMembershipModel
                {
                    Membership = "Please have account to get memebership"
                };
            }
        }
    }
}