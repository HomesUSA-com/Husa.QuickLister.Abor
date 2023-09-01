namespace Husa.Quicklister.Abor.Application.Models
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsDto : ListingStatusFieldsDto
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public DateTime? ContractDate { get; set; }

        public string SellConcess { get; set; }
    }
}
