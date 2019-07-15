using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrchestratorClient
{
    public class Program
    {
        private const string DefaultTenantName = "default";
        private const string AdminUserName = "admin";
        private const string AdminPassword = "890iop";
        private static readonly Uri BaseUrl = new Uri("http://localhost:6234");
        private const string UsersUrl = "/odata/Users";

        private static readonly Dictionary<string, string> DefaultHeaders = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            };

        public static async Task Main(string[] args)
        {
            var organizationUnitId = 6;

            var client = new OrchestratorClient()
                .WithHeaders(DefaultHeaders)
                .WithBasicAuthentication(DefaultTenantName, AdminUserName, AdminPassword)
                .WithRelativeUrl(UsersUrl);

            var orchestratorClient = ((IOrchestratorClient)client)
                .WithOrganizationUnitId(organizationUnitId);

            var request = await orchestratorClient.LoginAsync();
            var response1 = await orchestratorClient.Get<string>(new Uri(UsersUrl, UriKind.Relative));
            Console.WriteLine(request);

            var loginUser = JsonConvert.DeserializeObject<ApiResponse<string>>(request);
            Console.WriteLine(loginUser);
        }
    }
}
