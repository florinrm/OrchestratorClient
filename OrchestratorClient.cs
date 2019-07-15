using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrchestratorClient
{
    public class OrchestratorClient : FluentHttpClient, IOrchestratorClient
    {
        private readonly Uri _urlLogin = new Uri("/api/Account/Authenticate", UriKind.Relative);

        private class UserAuthentification
        {
            public string TenancyName { get; set; }
            public string UsernameOrEmailAddress { get; set; }
            public string Password { get; set; }
        }

        public override async Task<string> LoginAsync()
        {
            HttpContent httpContent = null;
            if (TenancyName != null && UsernameOrEmailAddress != null && Password != null)
            {
                httpContent = SerializeContent(new UserAuthentification()
                {
                    TenancyName = TenancyName,
                    UsernameOrEmailAddress = UsernameOrEmailAddress,
                    Password = Password

                });
            }
            return await RequestAsync(_urlLogin, HttpMethod.Post, httpContent, Headers, retryConnect: true);
        }

        public IOrchestratorClient WithOrganizationUnitId(int organizationUnitId)
        {
            this.Headers.Add("X-UIPATH-OrganizationUnitId", organizationUnitId.ToString());
            return this;
        }
    }
}
