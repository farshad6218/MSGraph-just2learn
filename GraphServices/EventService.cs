using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;

namespace Test1.GraphServices
{
    public class EventService : IEventService
    {
        private readonly IConfiguration _configuration;

        public EventService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateEventAsync(string subject, DateTime start, DateTime end)
        {
            try
            {

                var scopes = new[] { "https://graph.microsoft.com/.default" };
                // Values from app registration
                var clientId = _configuration["AzureAd:ClientId"];
                var tenantId = _configuration["AzureAd:TenantId"];
                var clientSecret = _configuration["AzureAd:ClientSecret"];

                // using Azure.Identity;
                var options = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                };
                var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

                //var tokenRequestContext = new TokenRequestContext(scopes);
                //var token = clientSecretCredential.GetTokenAsync(tokenRequestContext).Result.Token;

                //var graphServiceClient = new GraphServiceClient(token, scopes,null);
                var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);

                var newEvent = new Event
                {
                    Subject = subject,
                    Start = new DateTimeTimeZone { DateTime = start.ToString("yyyy-MM-ddTHH:mm:ss"), TimeZone = "Eastern Standard Time" },
                    End = new DateTimeTimeZone { DateTime = end.ToString("yyyy-MM-ddTHH:mm:ss"), TimeZone = "Eastern Standard Time" },
                };

                var createdEvent = await graphServiceClient.Users["farshad@riskmetis.com"].Calendar.Events.Request().AddAsync(newEvent);
                return createdEvent.Id;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
