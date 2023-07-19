namespace Husa.Quicklister.Abor.Application.Interfaces.Community
{
    using System;
    using System.Threading.Tasks;

    public interface ICommunityMigrationService
    {
        Task MigrateByCompanyId(Guid companyId);
    }
}
