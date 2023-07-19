namespace Husa.Quicklister.Abor.Application.Interfaces.Plan
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Interfaces.Xml;

    public interface IPlanXmlService : IProfileXmlService
    {
        Task ApprovePlan(Guid planId);
    }
}
