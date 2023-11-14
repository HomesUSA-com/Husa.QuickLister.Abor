namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using System.Collections.Generic;

    public class SaleListingOpenHouseQueryResult
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string MlsNumber { get; set; }
        public ICollection<OpenHousesQueryResult> OpenHouses { get; set; }
    }
}
