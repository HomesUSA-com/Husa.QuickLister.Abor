namespace Husa.Quicklister.Abor.Application.Interfaces.Community
{
    using Husa.Quicklister.Abor.Application.Models.Community;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Community;

    public interface ISaleCommunityService : ExtensionsInterfaces.ISaleCommunityService<CommunitySaleCreateDto, CommunitySaleDto>
    {
    }
}
