namespace Husa.Quicklister.Abor.Application.Interfaces.Plan
{
    using System;
    using System.Threading.Tasks;

    public interface IPlanMigrationService
    {
        Task MigrateByCompanyId(Guid companyId);
    }
}
