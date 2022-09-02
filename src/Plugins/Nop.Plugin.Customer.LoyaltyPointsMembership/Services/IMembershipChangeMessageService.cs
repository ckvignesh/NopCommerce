using Nop.Core.Domain.Messages;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    public partial interface IMembershipChangeMessageService
    {
        Task<IList<int>> SendMembershipChangeMessageAsync(Nop.Core.Domain.Customers.Customer customer, int languageId, string customerEmail, string templateName);
        Task<int> EnsureLanguageIsActiveAsync(int languageId, int storeId);
        Task<IList<MessageTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName, int storeId);
        Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate, int languageId);
    }
}