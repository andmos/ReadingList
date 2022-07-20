using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReadingList.Logging;
using ReadingList.Trello.Models;
using ReadingList.Trello.Services;

namespace ReadingList.Carter.ApiKeyAuthentication
{
    public class TrelloApiKeyAuthenticationHandler : AuthenticationHandler<TrelloApiKeyAuthenticationOptions>
    {
        private readonly ITrelloAuthorizationWrapper _trelloAuthWrapper;
        private readonly TrelloAuthSettings _configuredTrelloAuthSettings;
        private readonly ILog _logger;
        public TrelloApiKeyAuthenticationHandler(
            IOptionsMonitor<TrelloApiKeyAuthenticationOptions> options,
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock, 
            IOptions<TrelloAuthSettings> configuredTrelloAuthSettings,
            ITrelloAuthorizationWrapper trelloAuthWrapper,
            ILogFactory logFactory) 
            : base(options, logger, encoder, clock)
        {
            _configuredTrelloAuthSettings = configuredTrelloAuthSettings.Value;
            _trelloAuthWrapper = trelloAuthWrapper;
            _logger = logFactory.GetLogger(this.GetType());
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string missingApiOrUserToken = "TrelloAPIKey and TrelloUserToken is required in header to do this operation.";
            string invalidApiOrUserToken = "TrelloAPIKey and TrelloUserToken does not match configured APIKey or Token";
            
            if (!Request.Headers.TryGetValue(TrelloApiKeyAuthenticationOptions.ApiKeyHeaderName, out var extractedApiKey))
            {
                _logger.Info($"ApiKey was not provided. Attempt from {Request.Host.Host}");
                return Task.FromResult(AuthenticateResult.Fail(missingApiOrUserToken));
            }
            if (!Request.Headers.TryGetValue(TrelloApiKeyAuthenticationOptions.UserTokenHeaderName, out var extractedUserToken))
            {
                _logger.Info($"UserToken was not provided. Attempt from {Request.Host.Host}");
                return Task.FromResult(AuthenticateResult.Fail(missingApiOrUserToken));
            }
            var trelloKeys = new TrelloAuthSettings { TrelloAPIKey = extractedApiKey, TrelloUserToken = extractedUserToken };
            if (!_trelloAuthWrapper.IsValidKeys(trelloKeys))
            {
                _logger.Info($"Invalid ApiKey or UserToken was provided. Attempt from {Request.Host.Host}");
                return Task.FromResult(AuthenticateResult.Fail(invalidApiOrUserToken));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "ReadingList"),
                new Claim(ClaimTypes.Role, "AddOrUpdate")
            };

            var identity = new ClaimsIdentity(claims, Options.Scheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        
    }
}