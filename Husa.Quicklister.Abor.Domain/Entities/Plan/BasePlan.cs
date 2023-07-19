namespace Husa.Quicklister.Abor.Domain.Entities.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;
    using Enums = Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class BasePlan : ValueObject, IProvideSpacesDimensions
    {
        public BasePlan(string name, string ownerName)
            : this()
        {
            this.Name = name;
            this.OwnerName = ownerName;
        }

        protected BasePlan()
        {
            this.GarageDescription = new HashSet<GarageDescription>();
        }

        public string Name { get; set; }

        public string OwnerName { get; set; }

        public virtual Stories? Stories { get; set; }

        public int? BathsFull { get; set; }

        public int? BathsHalf { get; set; }

        public int? NumBedrooms { get; set; }

        public virtual ICollection<Enums.GarageDescription> GarageDescription { get; set; }

        public bool IsNewConstruction { get; set; }

        public static BasePlan ImportFromXml(PlanResponse xmlPlan, string companyName)
        {
            if (xmlPlan is null)
            {
                throw new ArgumentNullException(nameof(xmlPlan));
            }

            return new BasePlan(name: xmlPlan.Name, ownerName: companyName)
            {
                BathsFull = xmlPlan.Baths,
                BathsHalf = xmlPlan.HalfBaths,
                NumBedrooms = xmlPlan.Bedrooms,
                Stories = xmlPlan.Stories.ToStories(),
                GarageDescription = EnumExtensions.GetGarageDescription(xmlPlan.Garage),
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Name;
            yield return this.OwnerName;
            yield return this.Stories;
            yield return this.BathsFull;
            yield return this.NumBedrooms;
            yield return this.GarageDescription;
            yield return this.IsNewConstruction;
        }
    }
}
