namespace Husa.Quicklister.Abor.Application.Models.Request
{
    using System;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;

    public class ListingSaleRequestDto : ListingRequestDto
    {
        public virtual Guid ListingSaleId { get; set; }

        public virtual ListingSaleStatusFieldsDto StatusFieldsInfo { get; set; }

        public virtual ListingSalePublishInfoDto PublishInfo { get; set; }

        public virtual SalePropertyDetailDto SaleProperty { get; set; }

        public virtual ShowingTimeDto ShowingTime { get; set; }
    }
}
