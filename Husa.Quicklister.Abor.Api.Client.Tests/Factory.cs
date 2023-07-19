namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using System;

    public static class Factory
    {
        private static Guid userId;

        private static Guid companyId;

        private static Guid planId;

        private static Guid listingId;

        private static Guid communityId;

        private static Guid communityAwaitingApprovalId;

        private static Guid planAwaitingApprovalId;

        private static string communityName;

        private static string planName;

        private static string builderName;

        private static Guid notFoundXmlListingId;

        private static Guid xmlListingId;

        private static Guid listingIdForXml;

        private static Guid listingIdWithInadequateRemarks;

        public static Guid UserId
        {
            get
            {
                if (userId == Guid.Empty)
                {
                    userId = Guid.NewGuid();
                }

                return userId;
            }
        }

        public static Guid CompanyId
        {
            get
            {
                if (companyId == Guid.Empty)
                {
                    companyId = Guid.NewGuid();
                }

                return companyId;
            }
        }

        public static Guid NotFoundXmlListingId
        {
            get
            {
                if (notFoundXmlListingId == Guid.Empty)
                {
                    notFoundXmlListingId = Guid.NewGuid();
                }

                return notFoundXmlListingId;
            }
        }

        public static Guid XmlListingId
        {
            get
            {
                if (xmlListingId == Guid.Empty)
                {
                    xmlListingId = Guid.NewGuid();
                }

                return xmlListingId;
            }
        }

        public static Guid ListingIdForXml
        {
            get
            {
                if (listingIdForXml == Guid.Empty)
                {
                    listingIdForXml = Guid.NewGuid();
                }

                return listingIdForXml;
            }
        }

        public static Guid ListingIdWithInadequateRemarks
        {
            get
            {
                if (listingIdWithInadequateRemarks == Guid.Empty)
                {
                    listingIdWithInadequateRemarks = Guid.NewGuid();
                }

                return listingIdWithInadequateRemarks;
            }
        }

        public static Guid PlanId
        {
            get
            {
                if (planId == Guid.Empty)
                {
                    planId = Guid.NewGuid();
                }

                return planId;
            }
        }

        public static Guid PlanAwaitingApprovalId
        {
            get
            {
                if (planAwaitingApprovalId == Guid.Empty)
                {
                    planAwaitingApprovalId = Guid.NewGuid();
                }

                return planAwaitingApprovalId;
            }
        }

        public static Guid ListingId
        {
            get
            {
                if (listingId == Guid.Empty)
                {
                    listingId = Guid.NewGuid();
                }

                return listingId;
            }
        }

        public static Guid CommunityId
        {
            get
            {
                if (communityId == Guid.Empty)
                {
                    communityId = Guid.NewGuid();
                }

                return communityId;
            }
        }

        public static Guid CommunityAwaitingApprovalId
        {
            get
            {
                if (communityAwaitingApprovalId == Guid.Empty)
                {
                    communityAwaitingApprovalId = Guid.NewGuid();
                }

                return communityAwaitingApprovalId;
            }
        }

        public static string CommunityName
        {
            get
            {
                if (string.IsNullOrEmpty(communityName))
                {
                    communityName = "CommunityName";
                }

                return communityName;
            }
        }

        public static string PlanName
        {
            get
            {
                if (string.IsNullOrEmpty(planName))
                {
                    planName = "PlanName";
                }

                return planName;
            }
        }

        public static string BuilderName
        {
            get
            {
                if (string.IsNullOrEmpty(builderName))
                {
                    builderName = "BuilderName";
                }

                return builderName;
            }
        }
    }
}
