namespace Husa.Quicklister.Abor.Domain.Entities.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

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
        }

        public string Name { get; set; }
        public string OwnerName { get; set; }
        public bool IsNewConstruction { get; set; }
        public virtual Stories? StoriesTotal { get; set; }
        public virtual int? SqFtTotal { get; set; }
        public virtual int? DiningAreasTotal { get; set; }
        public virtual int? MainLevelBedroomTotal { get; set; }
        public virtual int? OtherLevelsBedroomTotal { get; set; }
        public virtual int? HalfBathsTotal { get; set; }
        public virtual int? FullBathsTotal { get; set; }
        public virtual int? LivingAreasTotal { get; set; }

        public static BasePlan ImportFromXml(PlanResponse xmlPlan, string companyName)
        {
            if (xmlPlan is null)
            {
                throw new ArgumentNullException(nameof(xmlPlan));
            }

            return new BasePlan(name: xmlPlan.Name, ownerName: companyName)
            {
                FullBathsTotal = xmlPlan.Baths,
                HalfBathsTotal = xmlPlan.HalfBaths,
                MainLevelBedroomTotal = null,
                StoriesTotal = xmlPlan.Stories.ToStories(),
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Name;
            yield return this.OwnerName;
            yield return this.IsNewConstruction;
            yield return this.StoriesTotal;
            yield return this.SqFtTotal;
            yield return this.DiningAreasTotal;
            yield return this.MainLevelBedroomTotal;
            yield return this.OtherLevelsBedroomTotal;
            yield return this.HalfBathsTotal;
            yield return this.FullBathsTotal;
            yield return this.LivingAreasTotal;
        }
    }
}
