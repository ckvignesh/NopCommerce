using System;
using System.ComponentModel.DataAnnotations;
using Nop.Core;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Domain
{
    public partial class LoyaltyMembership : BaseEntity
    {
        public int CustomerId { get; set; }
        public int PreviousLoyaltyPoint { get; set; }
        public int CurrentLoyaltyPoint { get; set; }
        public decimal OrderTotal { get; set; }
        public string? PreviousMembership { get; set; }
        public string? CurrentMembership { get; set; }
        public DateTime HistoryRecordedTime { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}