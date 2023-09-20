namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;

    public interface IEntityOpenHouse<TOpenHouse>
        where TOpenHouse : OpenHouse
    {
        public ICollection<TOpenHouse> OpenHouses { get; set; }

        public void AddOpenHouses<T>(IEnumerable<T> openHouses)
             where T : OpenHouse;
    }
}
