using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.CustomerEmailReadOnly.Components
{
    [ViewComponent(Name = "MiscCustomerEmailReadOnly")]
    public class MiscCustomerEmailReadOnlyViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Misc.CustomerEmailReadOnly/Views/CustomerEmailReadOnly.cshtml");
        }
    }
}
