namespace Husa.Quicklister.Abor.Application.Tests.Services
{
    using Husa.Extensions.Common;
    using Husa.Migration.Api.Contracts.Response.SaleListing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public static class MigrationProvider
    {
        public static SalePropertyResponse GetSaleProperty() => new()
        {
            OwnerName = "OwnerName",
            FinancialInfo = new(),
            SalesOfficeInfo = new()
            {
                SalesOfficeCity = "dallas",
            },
            ShowingInfo = new()
            {
                Directions = "Es un hecho establecido hace demasiado tiempo que un lector se distraerá con el contenido del texto de un sitio mientras que mira su diseño. El punto de usar Lorem Ipsum es que tiene una distribución más o menos normal de las letras, al contrario de usar textos como por ejemplo \"Contenido aquí, contenido aquí\". Estos textos hacen parecerlo un español que se puede leer. Muchos paquetes de autoedición y editores de páginas web usan el Lorem Ipsum como su texto por defecto, y al hacer una búsqueda de \"Lorem Ipsum\" va a dar por resultado muchos sitios web que usan este texto si se encuentran en estado de desarrollo. Muchas versiones han evolucionado a través de los años, algunas veces por accidente, otras veces a propósito (por ejemplo insertándole humor y cosas por el estilo).",
            },
            AddressInfo = new()
            {
                State = null,
                City = Cities.Airville.ToStringFromEnumMember(),
                County = Counties.Young.ToStringFromEnumMember(),
            },
            PropertyInfo = new(),
            SpacesDimensionsInfo = new()
            {
                Stories = "1.00",
            },
            FeaturesInfo = new()
            {
                WasherConnections = "ELCDR",
                SeniorCommunity = "none",
            },
            SchoolsInfo = new()
            {
                HighSchool = "079901010",
            },
        };

        public static StatusFieldsResponse GetStatusFieldsResponse() => new()
        {
            SellerConcessionDescription = "CONVT",
            AgentId = "AgentId",
        };
    }
}
