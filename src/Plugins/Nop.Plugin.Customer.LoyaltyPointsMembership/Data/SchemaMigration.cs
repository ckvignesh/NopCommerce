using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Customer.LoyaltyPointsMembership.Domain;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Data
{
    [NopMigration("2022/08/24 09:30:17:6455422", "Customer.LoyaltyPointsMembership base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<LoyaltyMembership>();
        }
    }
}