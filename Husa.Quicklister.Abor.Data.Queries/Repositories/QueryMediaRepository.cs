namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public class QueryMediaRepository : IQueryMediaRepository
    {
        private readonly IMediaServiceClient mediaServiceClient;

        public QueryMediaRepository(IMediaServiceClient mediaServiceClient)
        {
            this.mediaServiceClient = mediaServiceClient ?? throw new ArgumentNullException(nameof(mediaServiceClient));
        }

        public async Task<IEnumerable<SummarySection>> GetMediaSummary(Guid currentRequestId, Guid? lastestCompletedRequestId)
        {
            var currentMedia = await this.mediaServiceClient.GetResources(currentRequestId, MediaType.ListingRequest);
            var lastestCompletedMedia = lastestCompletedRequestId != null ? await this.mediaServiceClient.GetResources((Guid)lastestCompletedRequestId, MediaType.ListingRequest) : new ResourceResponse();
            var mediaSection = this.GetMediaSummary(currentMedia, lastestCompletedMedia);

            return mediaSection;
        }

        private IEnumerable<SummarySection> GetMediaSummary(ResourceResponse currentResources, ResourceResponse oldResources)
        {
            return new[]
            {
                this.GetFilesSummary(currentResources.Media, oldResources.Media),
                this.GetVirtualTourSummary(currentResources.VirtualTour, oldResources.VirtualTour),
            };
        }

        private SummarySection GetFilesSummary(IEnumerable<MediaDetail> currentMedia, IEnumerable<MediaDetail> oldMedia)
        {
            var currentMediaIds = currentMedia != null ? currentMedia.Select(x => x.Id) : new List<Guid>();
            var oldMediaIds = oldMedia != null ? oldMedia.Select(x => x.Id) : new List<Guid>();

            var section = new SummarySection() { Name = "Media" };
            var summaryFields = new List<SummaryField>();

            if (currentMedia is not null)
            {
                var newMedia = currentMedia.Where(x => !oldMediaIds.Contains(x.Id));
                summaryFields.AddRange(this.FieldSummary(newMedia, true));
            }

            if (oldMedia is not null)
            {
                var removedMedia = oldMedia.Where(x => !currentMediaIds.Contains(x.Id));
                summaryFields.AddRange(this.FieldSummary(removedMedia, false));
            }

            if (currentMedia is not null && oldMedia is not null)
            {
                summaryFields.AddRange(this.UpdateFieldSummary(currentMedia, oldMedia));
            }

            section.Fields = summaryFields;

            return section;
        }

        private SummarySection GetVirtualTourSummary(IEnumerable<VirtualTourDetail> currentMedia, IEnumerable<VirtualTourDetail> oldMedia)
        {
            var currentMediaUris = currentMedia != null ? currentMedia.Select(x => x.Uri) : new List<System.Uri>();
            var oldMediaUris = oldMedia != null ? oldMedia.Select(x => x.Uri) : new List<System.Uri>();

            var section = new SummarySection() { Name = "MediaVirtualTour" };
            var summaryFields = new List<SummaryField>();

            if (currentMedia is not null)
            {
                var newMedia = currentMedia.Where(x => !oldMediaUris.Contains(x.Uri));
                summaryFields.AddRange(this.FieldSummary(newMedia, true));
            }

            if (oldMedia is not null)
            {
                var removedMedia = oldMedia.Where(x => !currentMediaUris.Contains(x.Uri));
                summaryFields.AddRange(this.FieldSummary(removedMedia, false));
            }

            if (currentMedia is not null && oldMedia is not null)
            {
                summaryFields.AddRange(this.UpdateFieldSummary(currentMedia, oldMedia));
            }

            section.Fields = summaryFields;

            return section;
        }

        private IEnumerable<SummaryField> FieldSummary(IEnumerable<MediaDetail> mediaValues, bool newValues)
        {
            foreach (var value in mediaValues)
            {
                var values = new { value.UriSmall, value.Uri };
                yield return new SummaryField()
                {
                    FieldName = value.Title,
                    NewValue = newValues ? values : null,
                    OldValue = newValues ? null : values,
                };
            }
        }

        private IEnumerable<SummaryField> FieldSummary(IEnumerable<VirtualTourDetail> mediaValues, bool newValues)
        {
            foreach (var value in mediaValues)
            {
                var values = new { value.Uri };
                yield return new SummaryField()
                {
                    FieldName = value.Title,
                    NewValue = newValues ? values : null,
                    OldValue = newValues ? null : values,
                };
            }
        }

        private IEnumerable<SummaryField> UpdateFieldSummary(IEnumerable<MediaDetail> currentMedia, IEnumerable<MediaDetail> oldMedia)
        {
            var mediaValues = currentMedia.Except(oldMedia, new MediaComparer())
                .Join(oldMedia, current => current.Id, old => old.Id, (current, old) => new { id = current.Id.ToString(), current, old });

            foreach (var value in mediaValues)
            {
                var summary = this.GetChanges(value.current, value.old);

                yield return new SummaryField()
                {
                    FieldName = value.current.Title,
                    NewValue = summary.NewValue,
                    OldValue = summary.OldValue,
                };
            }
        }

        private IEnumerable<SummaryField> UpdateFieldSummary(IEnumerable<VirtualTourDetail> currentMedia, IEnumerable<VirtualTourDetail> oldMedia)
        {
            var mediaValues = currentMedia.Except(oldMedia, new VirtualTourComparer())
                .Join(oldMedia, current => current.Id, old => old.Id, (current, old) => new { id = current.Id.ToString(), current, old });

            foreach (var value in mediaValues)
            {
                var summary = this.GetChanges(value.current, value.old);

                yield return new SummaryField()
                {
                    FieldName = value.current.Title,
                    NewValue = summary.NewValue,
                    OldValue = summary.OldValue,
                };
            }
        }

        private (Dictionary<string, object> NewValue, Dictionary<string, object> OldValue) GetChanges<T>(T newObject, T oldObject)
        {
            var propertiesInfo = typeof(T).GetProperties().ToList();

            var newValues = new Dictionary<string, object>();
            var oldValues = new Dictionary<string, object>();

            foreach (var propertyInfo in propertiesInfo)
            {
                var newValue = propertyInfo.GetValue(newObject);
                var oldValue = propertyInfo.GetValue(oldObject);

                if ((propertyInfo.PropertyType == typeof(bool) && !newValue.Equals(oldValue)) || !object.Equals(newValue, oldValue))
                {
                    newValues[propertyInfo.Name] = newValue;
                    oldValues[propertyInfo.Name] = oldValue;
                }
            }

            return (newValues, oldValues);
        }
    }
}
