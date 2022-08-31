using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Services.Cms;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.CustomerEmailReadOnly
{
    public class CustomerEmailReadOnlyProcessor : BasePlugin, IWidgetPlugin
    {
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            throw new System.NotImplementedException();
        }

        public override Task InstallAsync()
        {
            return base.InstallAsync();
        }
        public override Task UninstallAsync()
        {
            return base.UninstallAsync();
        }
    }
}