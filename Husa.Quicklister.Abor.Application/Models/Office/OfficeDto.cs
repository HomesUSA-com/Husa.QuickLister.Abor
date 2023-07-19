namespace Husa.Quicklister.Abor.Application.Models.Office
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class OfficeDto
    {
        public string MarketUniqueId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public Cities City { get; set; }

        public States? State { get; set; }

        public string Zip { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Status { get; set; }

        public string LicenseNumber { get; set; }

        public DateTime MarketModified { get; set; }
    }
}
