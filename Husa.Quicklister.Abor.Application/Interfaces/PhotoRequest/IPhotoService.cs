namespace Husa.Quicklister.Abor.Application.Interfaces.PhotoRequest
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Request = Husa.PhotoService.Api.Contracts.Request;
    using Response = Husa.PhotoService.Api.Contracts.Response;

    public interface IPhotoService
    {
        public PropertyType PropertyType { get; }

        Task CreateAsync(Guid entityId, Request.PhotoRequest photoRequest);

        Task<Response.PhotoRequestDetail> GetByIdAsync(Guid entityId, Guid photoRequestId);

        Task<DataSet<Response.PhotoRequestResponse>> GetAsync(Guid entityId, Request.PhotoRequestFilter filters);

        Task DeleteByIdAsync(Guid entityId, Guid photorequestId);

        Task AssignLatestPhotoRequest(Guid entityId, Guid photorequestId, DateTime creationDate);

        Task<PhotoRequestBusMessage> GetPhotoRequestMessage(Guid entityId, Request.PhotoRequest photoRequest);
    }
}
