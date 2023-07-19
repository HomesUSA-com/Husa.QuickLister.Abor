namespace Husa.Quicklister.Abor.Domain.Entities.Office
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class Office : Entity
    {
        public Office(OfficeValueObject officeValue)
        {
            if (officeValue is null)
            {
                throw new ArgumentNullException(nameof(officeValue));
            }

            this.OfficeValue = officeValue;
        }

        protected Office()
        {
        }

        public OfficeValueObject OfficeValue { get; set; }

        public void UpdateInformation(OfficeValueObject officeValue)
        {
            if (officeValue.MarketModified <= this.OfficeValue.MarketModified)
            {
                return;
            }

            if (this.OfficeValue == officeValue)
            {
                return;
            }

            this.OfficeValue = officeValue;
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.OfficeValue;
        }
    }
}
