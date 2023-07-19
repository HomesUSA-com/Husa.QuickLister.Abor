namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models.Xml;
    using XmlResponse = Husa.Xml.Api.Contracts.Response;

    public interface IQueryXmlRepository
    {
        Task<DataSet<XmlResponse.XmlListingResponse>> GetAsync(XmlListingQueryFilter filter);
    }
}
