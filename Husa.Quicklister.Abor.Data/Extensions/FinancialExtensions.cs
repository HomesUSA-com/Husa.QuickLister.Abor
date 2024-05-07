namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class FinancialExtensions
    {
        public static void ConfigureAgentBonusAmount<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideAgentBonusCommission
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder
            .Property(r => r.AgentBonusAmount)
            .HasColumnName(nameof(IProvideAgentBonusCommission.AgentBonusAmount))
                .HasPrecision(18, 2);

            builder.Property(r => r.AgentBonusAmountType).HasColumnName(nameof(IProvideAgentBonusCommission.AgentBonusAmountType))
                .HasEnumFieldValue<CommissionType>(maxLength: 1, isRequired: true);
        }

        public static void ConfigureAgentCommission<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideAgentCommission
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(r => r.BuyersAgentCommission).HasPrecision(18, 2).HasColumnName(nameof(IProvideAgentCommission.BuyersAgentCommission)).HasMaxLength(6);
            builder.Property(r => r.BuyersAgentCommissionType)
                .HasColumnName(nameof(IProvideAgentCommission.BuyersAgentCommissionType))
                .HasEnumFieldValue<CommissionType>(maxLength: 1, isRequired: true);
        }

        public static void ConfigureCommonFinancial<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideCommonFinancial
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ConfigureAgentCommission();
            builder.ConfigureAgentBonusAmount();

            builder.Property(r => r.HasAgentBonus).HasColumnName(nameof(IProvideCommonFinancial.HasAgentBonus));
            builder.Property(r => r.HasBonusWithAmount).HasColumnName(nameof(IProvideCommonFinancial.HasBonusWithAmount));
            builder.Property(r => r.BonusExpirationDate).HasColumnName(nameof(IProvideCommonFinancial.BonusExpirationDate));
            builder.Property(r => r.HasBuyerIncentive).HasColumnName(nameof(IProvideCommonFinancial.HasBuyerIncentive)).HasColumnType("bit")
                .IsRequired();

            builder
                .Property(r => r.TaxRate)
                .HasColumnName(nameof(IProvideCommonFinancial.TaxRate))
                .HasPrecision(18, 2);

            builder.Property(r => r.AcceptableFinancing)
                .HasColumnName(nameof(IProvideCommonFinancial.AcceptableFinancing))
                .HasEnumCollectionValue<AcceptableFinancing>(maxLength: 500);

            builder.Property(r => r.HoaIncludes)
                .HasColumnName(nameof(IProvideCommonFinancial.HoaIncludes))
                .HasEnumCollectionValue<HoaIncludes>(maxLength: 500);
            builder.Property(r => r.HOARequirement)
                .HasColumnName(nameof(IProvideCommonFinancial.HOARequirement))
                .HasEnumFieldValue<HoaRequirement>(maxLength: 10);
            builder.Property(r => r.HasHoa).HasColumnName(nameof(IProvideCommonFinancial.HasHoa))
                .HasColumnType("bit")
                .IsRequired();

            builder.Property(r => r.BillingFrequency)
                .HasColumnName(nameof(IProvideCommonFinancial.BillingFrequency))
                .HasConversion<EnumFieldValueConverter<BillingFrequency>>()
                .HasMaxLength(6);

            builder.Property(r => r.HoaName).HasColumnName(nameof(IProvideFinancial.HoaName)).HasMaxLength(70);
            builder
                .Property(p => p.HoaFee)
                .HasColumnName(nameof(IProvideFinancial.HoaFee))
                .HasPrecision(18, 2);
        }

        public static void ConfigureFinancial<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideFinancial
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(r => r.TaxExemptions)
                .HasColumnName(nameof(IProvideFinancial.TaxExemptions))
                .HasEnumCollectionValue<TaxExemptions>(maxLength: 500);
            builder.Property(r => r.TitleCompany).HasColumnName(nameof(IProvideFinancial.TitleCompany)).HasMaxLength(45);

            builder.ConfigureCommonFinancial();
        }

        public static void ConfigureLotFinancial<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideLotFinancial
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ConfigureCommonFinancial();
            builder.Property(r => r.EstimatedTax).HasColumnName(nameof(IProvideLotFinancial.EstimatedTax));
            builder.Property(r => r.TaxYear).HasColumnName(nameof(IProvideLotFinancial.TaxYear));
            builder.Property(r => r.TaxExemptions).HasColumnName(nameof(IProvideLotFinancial.TaxExemptions)).HasEnumCollectionValue<TaxExemptions>(40);
            builder.Property(r => r.TaxAssesedValue).HasColumnName(nameof(IProvideLotFinancial.TaxAssesedValue));
            builder.Property(r => r.PreferredTitleCompany).HasColumnName(nameof(IProvideLotFinancial.PreferredTitleCompany)).HasMaxLength(45);
            builder.Property(r => r.LandTitleEvidence).HasColumnName(nameof(IProvideLotFinancial.LandTitleEvidence)).HasEnumFieldValue<LandTitleEvidence>(16);
        }
    }
}
