namespace Husa.Quicklister.Abor.Domain.Interfaces.Listing
{
    using System;

    public interface IPropertyInfo : IProvideProperty
    {
        public DateTime? ConstructionCompletionDate { get; set; }
    }
}
