namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideSpacesDimensions
    {
        Stories? Stories { get; set; }
        int? BathsFull { get; set; }
        int? BathsHalf { get; set; }
        int? NumBedrooms { get; set; }
        ICollection<GarageDescription> GarageDescription { get; set; }
    }
}
