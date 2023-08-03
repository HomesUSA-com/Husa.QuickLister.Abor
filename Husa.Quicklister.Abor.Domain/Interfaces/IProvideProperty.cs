namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideProperty
    {
        string LotDimension { get; set; }

        ICollection<LotDescription> LotDescription { get; set; }

        PropertySubType? PropertyType { get; set; }

        MlsArea? MlsArea { get; set; }

        ConstructionStage? ConstructionStage { get; set; }

        string LotSize { get; set; }
    }
}
