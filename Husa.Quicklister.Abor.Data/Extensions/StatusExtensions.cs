namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class StatusExtensions
    {
        public static void ConfigureStatusInfoMapping<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
           where TOwnerEntity : class
           where TDependentEntity : class, IProvideStatusFields
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(f => f.AgentId).HasColumnName(nameof(IProvideStatusFields.AgentId));
            builder.Property(f => f.HasBuyerAgent).HasColumnName(nameof(IProvideStatusFields.HasBuyerAgent)).HasDefaultValue(false);
            builder.Property(f => f.BackOnMarketDate).HasColumnType("datetime").HasColumnName(nameof(IProvideStatusFields.BackOnMarketDate));
            builder.Property(f => f.CancelledReason).HasMaxLength(300).HasColumnName(nameof(IProvideStatusFields.CancelledReason));
            builder.Property(f => f.ClosedDate).HasColumnType("datetime").HasColumnName(nameof(IProvideStatusFields.ClosedDate));
            builder.Property(f => f.ClosePrice).HasPrecision(18, 2).HasColumnName(nameof(IProvideStatusFields.ClosePrice));
            builder.Property(f => f.EstimatedClosedDate).HasColumnType("datetime").HasColumnName(nameof(IProvideStatusFields.EstimatedClosedDate));
            builder.Property(f => f.OffMarketDate).HasColumnType("datetime").HasColumnName(nameof(IProvideStatusFields.OffMarketDate));
            builder.Property(f => f.PendingDate).HasColumnType("datetime").HasColumnName(nameof(IProvideStatusFields.PendingDate));
            builder.Property(f => f.AgentIdSecond).HasColumnName(nameof(IProvideStatusFields.AgentIdSecond));
            builder.Property(f => f.HasSecondBuyerAgent).HasColumnName(nameof(IProvideStatusFields.HasSecondBuyerAgent));
            builder.Property(f => f.HasContingencyInfo).HasColumnName(nameof(IProvideStatusFields.HasContingencyInfo));
            builder.Property(f => f.SellConcess).HasMaxLength(50).HasColumnName(nameof(IProvideStatusFields.SellConcess));
            builder.Property(f => f.SaleTerms).HasColumnName(nameof(IProvideStatusFields.SaleTerms)).HasEnumCollectionValue<SaleTerms>(300);
        }
    }
}
