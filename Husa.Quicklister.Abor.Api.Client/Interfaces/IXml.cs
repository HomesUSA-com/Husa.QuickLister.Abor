namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Xml;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Xml;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;

    public interface IXml
    {
        Task<DataSet<XmlListingResponse>> GetListings(XmlListingFilterRequest filter, CancellationToken token = default);

        Task ProcessListingAsync(Guid xmlListingId, ListActionType type, CancellationToken token = default);

        Task ListLaterAsync(Guid xmlListingId, DateTime listOn, CancellationToken token = default);

        Task DeleteListingAsync(Guid xmlListingId, CancellationToken token = default);

        Task RestoreListingAsync(Guid xmlListingId, CancellationToken token = default);
    }
}
