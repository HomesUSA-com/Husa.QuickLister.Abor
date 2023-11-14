namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using System.Collections.Generic;

    public class ListingSaleOpenHouseResponse
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string MlsNumber { get; set; }
        public ICollection<OpenHouseResponse> OpenHouses { get; set; }
    }
}
