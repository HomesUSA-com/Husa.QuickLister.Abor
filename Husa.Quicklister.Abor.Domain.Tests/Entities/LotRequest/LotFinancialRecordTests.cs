namespace Husa.Quicklister.Abor.Domain.Tests.Entities.LotRequest
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Xunit;

    public class LotFinancialRecordTests
    {
        [Theory]
        [InlineData(45000, CommissionType.Amount)]
        [InlineData(10, CommissionType.Percent)]
        public void CommissionRangeAttribute_Fail(decimal amount, CommissionType commissionType)
        {
            // Arrange
            var model = new LotFinancialRecord()
            {
                BuyersAgentCommission = amount,
                BuyersAgentCommissionType = commissionType,
            };

            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, x => x.MemberNames.Contains(nameof(LotFinancialRecord.BuyersAgentCommission)));
        }

        [Theory]
        [InlineData(40000, CommissionType.Amount)]
        [InlineData(8, CommissionType.Percent)]
        public void CommissionRangeAttribute_Success(decimal amount, CommissionType commissionType)
        {
            // Arrange
            var model = new LotFinancialRecord()
            {
                BuyersAgentCommission = amount,
                BuyersAgentCommissionType = commissionType,
            };

            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Act
            Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.DoesNotContain(results, x => x.MemberNames.Contains(nameof(LotFinancialRecord.BuyersAgentCommission)));
        }
    }
}
