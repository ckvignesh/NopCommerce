using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Domain;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Data
{
    public partial class LoyaltyMembershipBuilder : NopEntityBuilder<LoyaltyMembership>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
               //.WithColumn(nameof(LoyaltyMembership.LoyaltyMembershipId)).AsInt32().PrimaryKey()
               .WithColumn(nameof(LoyaltyMembership.CustomerId)).AsInt32()
               .WithColumn(nameof(LoyaltyMembership.PreviousLoyaltyPoint)).AsInt32()
               .WithColumn(nameof(LoyaltyMembership.CurrentLoyaltyPoint)).AsInt32()
               .WithColumn(nameof(LoyaltyMembership.CreatedOnUtc)).AsDateTime()
               .WithColumn(nameof(LoyaltyMembership.HistoryRecordedTime)).AsDateTime()
               .WithColumn(nameof(LoyaltyMembership.OrderTotal)).AsDecimal()
               .WithColumn(nameof(LoyaltyMembership.PreviousMembership)).AsString()
               .WithColumn(nameof(LoyaltyMembership.CurrentMembership)).AsString();
            ;
            ;
        }
    }
}
