namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideSaleStatusFields
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public DateTime? ContractDate { get; set; }

        public string SellConcess { get; set; }
    }
}
