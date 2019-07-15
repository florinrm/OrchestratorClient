using System.Threading.Tasks;

namespace OrchestratorClient
{
    public interface IOrchestratorClient : IFluentHttpClient
    {
        IOrchestratorClient WithOrganizationUnitId(int organizationUnitId);

        Task<string> LoginAsync();
    }
}
