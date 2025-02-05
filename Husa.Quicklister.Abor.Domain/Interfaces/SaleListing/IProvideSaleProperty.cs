namespace Husa.Quicklister.Abor.Domain.Interfaces.SaleListing
{
    using System;

    public interface IProvideSaleProperty : IProvideProperty
    {
        public DateTime? ConstructionCompletionDate { get; set; }
        public int? ConstructionStartYear { get; set; }
    }
}
