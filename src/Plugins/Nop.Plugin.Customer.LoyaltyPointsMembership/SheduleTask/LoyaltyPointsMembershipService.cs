using Nop.Core;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Domain;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Services;
using Nop.Services.Customers;
using Nop.Services.Orders;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.SheduleTask
{
    public class LoyaltyPointsMembershipService : ILoyaltyPointsMembershipService
    {
        private readonly ILoyaltyRewardPointsService _loyaltyrewardpointService;
        private readonly ILoyaltyPointsHistoryService _loyaltyPointsHistoryService;
        private readonly ICustomerInfoServices _customerInfoService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerOrderService _customerOrderService;
        private readonly IMembershipChangeMessageService _membershipChangeMessageService;
        private readonly IRewardPointService _rewardPointService;

        public LoyaltyPointsMembershipService(ICustomerInfoServices customerInfoService,
            ICustomerService customerService,
            ILoyaltyRewardPointsService loyaltyrewardpointService,
            ILoyaltyPointsHistoryService loyaltyPointsHistoryService,
            IWorkContext workContext,
            ICustomerOrderService customerOrderService,
            IMembershipChangeMessageService membershipChangeMessageService,
            IRewardPointService rewardPointService)
        {
            _customerInfoService = customerInfoService;
            _loyaltyrewardpointService = loyaltyrewardpointService;
            _loyaltyPointsHistoryService = loyaltyPointsHistoryService;
            _customerService = customerService;
            _workContext = workContext;
            _customerOrderService = customerOrderService;
            _membershipChangeMessageService = membershipChangeMessageService;
            _rewardPointService = rewardPointService;

        }
        public async Task LoyaltyPointSheduleTask()
         {
            string[] membership;
            var customers = await _customerInfoService.GetAllCustomers();
            var birthdayCustomers = await _customerInfoService.GetCustomersWithBirtdayAsToday();
            if (birthdayCustomers != null)
            {
                foreach (var birthdayCustomer in birthdayCustomers)
                {
                    if (await _customerService.IsRegisteredAsync(birthdayCustomer))
                    {
                        await _rewardPointService.AddRewardPointsHistoryEntryAsync(birthdayCustomer, 100, birthdayCustomer.RegisteredInStoreId, "Earned birthday reward points of 100");
                        await _membershipChangeMessageService.SendMembershipChangeMessageAsync(birthdayCustomer, (await _workContext.GetWorkingLanguageAsync()).Id, birthdayCustomer.Email, "BirthdayRewardPointsMessageTemplate");
                    }
                }
                var todayRegisterdCustomers = customers.Where(v => (DateTime.UtcNow.Date - v.CreatedOnUtc.Date).Days % 365 == 0);
                foreach (var customer in todayRegisterdCustomers)
                {
                    if (await _customerService.IsRegisteredAsync(customer))
                    {
                        var customerId = customer.Id;
                        var orderTotal = await _customerOrderService.GetSumofCustomerCompletedOrderByDate(customerId, 365);
                        if (orderTotal >= decimal.Parse(500.ToString()))
                        {
                            membership = new string[] { "Golden Member, Silver Member" };
                            await _rewardPointService.AddRewardPointsHistoryEntryAsync(customer, 100, customer.RegisteredInStoreId, "Earned membership upgrage points of 100");
                            await _membershipChangeMessageService.SendMembershipChangeMessageAsync(customer, (await _workContext.GetWorkingLanguageAsync()).Id, customer.Email, "MembershipUpgradeMessageTemplate");
                        }
                        else
                        {
                            membership = new string[] { "Silver Member" };
                            await _membershipChangeMessageService.SendMembershipChangeMessageAsync(customer, (await _workContext.GetWorkingLanguageAsync()).Id, customer.Email, "MembershipDowngradeMessageTemplate");
                        }
                        var rewardPointsHistory = await _loyaltyrewardpointService.GetRewardPointsByCustomerIdAsync(customerId);
                        var previousPointsHistory = await _loyaltyPointsHistoryService.GetLoyaltyPointsHistoryByCustomerIdAsync(customerId);
                        if (rewardPointsHistory != null)
                        {
                            var currentPoint = rewardPointsHistory != null ? (int)rewardPointsHistory.PointsBalance : 0;
                            var previousPoint = previousPointsHistory != null ? previousPointsHistory.CurrentLoyaltyPoint : 0;
                            var membershipmodel = new LoyaltyMembership()
                            {
                                CurrentLoyaltyPoint = currentPoint,
                                PreviousLoyaltyPoint = previousPoint,
                                CustomerId = customerId,
                                OrderTotal = orderTotal,
                                PreviousMembership = previousPointsHistory != null ? String.Join(",", previousPointsHistory.CurrentMembership) : "Silver Membership",
                                CurrentMembership = String.Join(",", membership),
                                HistoryRecordedTime = DateTime.Now,
                                Active = true,
                                CreatedOnUtc = DateTime.Now,
                            };
                            await _loyaltyPointsHistoryService.InsertLoyaltyPointsHistoryAsync(membershipmodel);

                        }
                    }
                }
            }
        }
    }
}
