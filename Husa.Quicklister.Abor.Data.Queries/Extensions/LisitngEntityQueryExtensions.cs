namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models;

    public static class LisitngEntityQueryExtensions
    {
        public static ICollection<RoomQueryResult> ToProjectionRooms<T>(this ICollection<T> rooms)
            where T : Room
        {
            var roomsCollection = new List<RoomQueryResult>();
            foreach (var r in rooms)
            {
                var room = new RoomQueryResult
                {
                    Id = r.Id,
                    Level = r.Level,
                    RoomType = r.RoomType,
                    IsDeleted = r.IsDeleted,
                    Features = r.Features,
                };

                roomsCollection.Add(room);
            }

            return roomsCollection;
        }

        public static SchoolsInfoQueryResult ToProjectionSchoolsInfo<T>(this T schools)
            where T : SchoolsInfo
        {
            return new()
            {
                SchoolDistrict = schools.SchoolDistrict,
                ElementarySchool = schools.ElementarySchool,
                OtherElementarySchool = schools.OtherElementarySchool,
                MiddleSchool = schools.MiddleSchool,
                OtherMiddleSchool = schools.OtherMiddleSchool,
                HighSchool = schools.HighSchool,
                OtherHighSchool = schools.OtherHighSchool,
            };
        }

        public static PublishInfoQueryResult ToProjectionPublishInfo<T>(this T publishInfo)
            where T : class, IProvidePublishFields
        {
            if (publishInfo == null)
            {
                return new();
            }

            return new()
            {
                PublishType = publishInfo.PublishType,
                PublishDate = publishInfo.PublishDate,
                PublishStatus = publishInfo.PublishStatus,
                PublishUser = publishInfo.PublishUser,
            };
        }

        public static EmailLeadQueryResult ToProjectionEmailLead(this EmailLead emailLeads)
        {
            if (emailLeads == null)
            {
                return new();
            }

            return new()
            {
                EmailLeadPrincipal = emailLeads.EmailLeadPrincipal,
                EmailLeadSecondary = emailLeads.EmailLeadSecondary,
                EmailLeadOther = emailLeads.EmailLeadOther,
            };
        }

        public static TStatusResult ToProjectionStatusFieldsInfo<TStatusFields, TStatusResult>(this TStatusFields statusFieldsInfo)
            where TStatusFields : class, IProvideStatusFields
            where TStatusResult : IProvideStatusFields, new()
        {
            if (statusFieldsInfo == null)
            {
                return new();
            }

            return new()
            {
                PendingDate = statusFieldsInfo.PendingDate,
                ClosedDate = statusFieldsInfo.ClosedDate,
                EstimatedClosedDate = statusFieldsInfo.EstimatedClosedDate,
                CancelledReason = statusFieldsInfo.CancelledReason,
                ClosePrice = statusFieldsInfo.ClosePrice,
                AgentId = statusFieldsInfo.AgentId,
                HasBuyerAgent = statusFieldsInfo.HasBuyerAgent,
                HasSecondBuyerAgent = statusFieldsInfo.HasSecondBuyerAgent,
                AgentIdSecond = statusFieldsInfo.AgentIdSecond,
                BackOnMarketDate = statusFieldsInfo.BackOnMarketDate,
                OffMarketDate = statusFieldsInfo.OffMarketDate,
                HasContingencyInfo = statusFieldsInfo.HasContingencyInfo,
                SaleTerms = statusFieldsInfo.SaleTerms,
                SellConcess = statusFieldsInfo.SellConcess,
                ContingencyInfo = statusFieldsInfo.ContingencyInfo,
            };
        }
    }
}
