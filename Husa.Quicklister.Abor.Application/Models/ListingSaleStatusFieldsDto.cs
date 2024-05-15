namespace Husa.Quicklister.Abor.Application.Models
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsDto : ListingStatusFieldsDto
    {
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }
    }
}
