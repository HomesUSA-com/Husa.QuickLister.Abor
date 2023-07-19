namespace Husa.Quicklister.Abor.Application.Interfaces.Community
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Community;

    public interface ISaleCommunityService : ExtensionsInterfaces.ISaleCommunityService<CommunitySaleCreateDto, CommunitySaleDto>
    {
        Task CreateEmployeesAsync(CommunityEmployeesCreateDto communityEmployeesCreateDto);

        Task DeleteEmployeesAsync(CommunityEmployeesDeleteDto communityEmployeesDeleteDto);
    }
}
