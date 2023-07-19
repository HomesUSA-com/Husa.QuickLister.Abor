namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideSpecialtyRooms
    {
        ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }
    }
}
