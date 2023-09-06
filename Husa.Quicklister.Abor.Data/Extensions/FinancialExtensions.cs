namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class FinancialExtensions
    {
        public static void ConfigureFinancial<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideFinancial
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .Property(r => r.TaxRate)
                .HasColumnName(nameof(IProvideFinancial.TaxRate))
                .HasPrecision(18, 2);

            builder.Property(r => r.AcceptableFinancing)
                .HasColumnName(nameof(IProvideFinancial.AcceptableFinancing))
                .HasEnumCollectionValue<AcceptableFinancing>(maxLength: 500);

            builder.Property(r => r.TaxExemptions)
                .HasColumnName(nameof(IProvideFinancial.TaxExemptions))
                .HasEnumCollectionValue<TaxExemptions>(maxLength: 500);

            builder.Property(r => r.HoaIncludes)
                .HasColumnName(nameof(IProvideFinancial.HoaIncludes))
                .HasEnumCollectionValue<HoaIncludes>(maxLength: 500);

            builder.Property(r => r.TitleCompany).HasColumnName(nameof(IProvideFinancial.TitleCompany)).HasMaxLength(45);
            builder.Property(r => r.HOARequirement)
                .HasColumnName(nameof(IProvideFinancial.HOARequirement))
                .HasEnumFieldValue<HoaRequirement>(maxLength: 10);
            builder.Property(r => r.HoaName).HasColumnName(nameof(IProvideFinancial.HoaName)).HasMaxLength(70);
            builder.Property(r => r.HasHoa).HasColumnName(nameof(IProvideFinancial.HasHoa))
                .HasColumnType("bit")
                .IsRequired();

            builder
                .Property(p => p.HoaFee)
                .HasColumnName(nameof(IProvideFinancial.HoaFee))
                .HasPrecision(18, 2);

            builder.Property(r => r.BillingFrequency)
                .HasColumnName(nameof(IProvideFinancial.BillingFrequency))
                .HasConversion<EnumFieldValueConverter<BillingFrequency>>()
                .HasMaxLength(6);

            builder.Property(r => r.BuyersAgentCommission).HasPrecision(18, 2).HasColumnName(nameof(IProvideFinancial.BuyersAgentCommission)).HasMaxLength(6);
            builder.Property(r => r.BuyersAgentCommissionType)
                .HasColumnName(nameof(IProvideFinancial.BuyersAgentCommissionType))
                .HasEnumFieldValue<CommissionType>(maxLength: 1, isRequired: true);

            builder.Property(r => r.HasAgentBonus).HasColumnName(nameof(IProvideFinancial.HasAgentBonus));
            builder.Property(r => r.HasBonusWithAmount).HasColumnName(nameof(IProvideFinancial.HasBonusWithAmount));
            builder
            .Property(r => r.AgentBonusAmount)
            .HasColumnName(nameof(IProvideFinancial.AgentBonusAmount))
                .HasPrecision(18, 2);

            builder.Property(r => r.AgentBonusAmountType).HasColumnName(nameof(IProvideFinancial.AgentBonusAmountType))
                .HasEnumFieldValue<CommissionType>(maxLength: 1, isRequired: true);
            builder.Property(r => r.BonusExpirationDate).HasColumnName(nameof(IProvideFinancial.BonusExpirationDate));
            builder.Property(r => r.HasBuyerIncentive).HasColumnName(nameof(IProvideFinancial.HasBuyerIncentive)).HasColumnType("bit")
                .IsRequired();
        }
    }
}
