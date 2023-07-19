namespace Husa.Quicklister.Abor.Application.Interfaces.Xml
{
    using System;
    using System.Threading.Tasks;

    public interface IProfileXmlService
    {
        Task ImportEntity(Guid companyId, string companyName, Guid entityId);
    }
}
