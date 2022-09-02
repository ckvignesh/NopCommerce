using Nop.Core.Domain.ScheduleTasks;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership
{
    public class LoyaltyPointsMembershipPlugin : BasePlugin, IWidgetPlugin
    {
        public bool HideInWidgetList => false;
        private readonly IScheduleTaskService _scheduleTaskService;
        public LoyaltyPointsMembershipPlugin(IScheduleTaskService scheduleTaskService)
        {
            _scheduleTaskService = scheduleTaskService;
        }
        public override async Task InstallAsync()
        {
            await _scheduleTaskService.InsertTaskAsync(new ScheduleTask
            {
                Name = "LoyaltyPointsMembership",
                Seconds = 60,
                LastEnabledUtc = DateTime.UtcNow,
                Type = "Nop.Plugin.Customer.LoyaltyPointsMembership.SheduleTask.LoyaltyPointsMembershipTask",
                Enabled = true,
                StopOnError = false
            });
            await base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            var task = await _scheduleTaskService.GetTaskByTypeAsync("Nop.Plugin.Customer.LoyaltyPointsMembership.SheduleTask.LoyaltyPointsMembershipTask");
            if (task != null)
                await _scheduleTaskService.DeleteTaskAsync(task);

            await base.UninstallAsync();
        }
        string IWidgetPlugin.GetWidgetViewComponentName(string widgetZone)
        {
            return "LoyaltyPointsMembership";
        }
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBeforeNews});
        }
    }
}
