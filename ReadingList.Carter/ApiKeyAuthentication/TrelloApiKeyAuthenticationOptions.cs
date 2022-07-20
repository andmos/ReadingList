using Microsoft.AspNetCore.Authentication;
namespace ReadingList.Carter.ApiKeyAuthentication
{
public class TrelloApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKeys";
        public string Scheme => DefaultScheme;
        public const string ApiKeyHeaderName = "TrelloAPIKey";
        public const string UserTokenHeaderName = "TrelloUserToken";
    }
}
