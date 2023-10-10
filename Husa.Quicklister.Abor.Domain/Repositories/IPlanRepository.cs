namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using ExtensionRepository = Husa.Quicklister.Extensions.Domain.Repositories;

    public interface IPlanRepository : ExtensionRepository.IPlanRepository<Plan>
    {
    }
}
