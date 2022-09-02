using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Core.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;

namespace Nop.Plugin.Customer.LoyaltyPointsMembership.Services
{
    internal class MembershipChangeMessageService : IMembershipChangeMessageService
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ILocalizationService _localizationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkflowMessageService _workflowMessageService;
        public MembershipChangeMessageService(IEmailAccountService emailAccountService,
            IWorkContext workContext, IStoreContext storeContext,
            IEmailSender emailSender,
            ILanguageService languageService,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            EmailAccountSettings emailAccountSettings,
            ILocalizationService localizationService,
            IEventPublisher eventPublisher,
            IWorkflowMessageService workflowMessageService,
            ICustomerOrderService customerOrderService)
        {
            _emailSender = emailSender;
            _emailAccountService = emailAccountService;
            _workContext = workContext;
            _storeContext = storeContext;
            _languageService = languageService;
            _messageTemplateService = messageTemplateService;
            _messageTokenProvider = messageTokenProvider;
            _emailAccountSettings = emailAccountSettings;
            _localizationService = localizationService;
            _eventPublisher = eventPublisher;
            _workflowMessageService = workflowMessageService;
        }
        public virtual async Task<IList<int>> SendMembershipChangeMessageAsync(Nop.Core.Domain.Customers.Customer customer, int languageId, string customerEmail, string templateName)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync(templateName, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            await _messageTokenProvider.AddCustomerTokensAsync(commonTokens, customer);
            commonTokens.Add(new Token("MembershipUpgradeMessage.Email", customerEmail));

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);
                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, customerEmail, customer.Username);
            }).ToListAsync();
        }
        public virtual async Task<int> EnsureLanguageIsActiveAsync(int languageId, int storeId)
        {
            //load language by specified ID
            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = (await _languageService.GetAllLanguagesAsync(storeId: storeId)).FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }
        public virtual async Task<IList<MessageTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }
        public virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (await _emailAccountService.GetEmailAccountByIdAsync(emailAccountId) ?? await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId)) ??
                               (await _emailAccountService.GetAllEmailAccountsAsync()).FirstOrDefault();
            return emailAccount;
        }
    }
}
