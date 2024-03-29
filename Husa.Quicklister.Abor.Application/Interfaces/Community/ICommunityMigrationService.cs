namespace Husa.Quicklister.Abor.Application.Interfaces.Community
{
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using ExtensionsInterface = Husa.Quicklister.Extensions.Application.Interfaces.Migration;

    public interface ICommunityMigrationService : ExtensionsInterface.ICommunityMigrationService<CommunitySale>
    {
    }
}
