namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Linq;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Xunit;

    public class CommunityHistoryTests
    {
        [Fact]
        public void GetSummary_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);

            var oldRecord = community.GenerateRecord();
            oldRecord.Property.County = Counties.Rusk;
            oldRecord.UpdateBaseInformation(community);
            oldRecord.XmlStatus = XmlStatus.AwaitingApproval;

            var newRecord = community.GenerateRecord();
            newRecord.Property.County = Counties.Anderson;
            newRecord.UpdateBaseInformation(community);
            newRecord.XmlStatus = XmlStatus.Approved;

            // Act
            var summary = newRecord.GetSummary(oldRecord);

            // Assert
            Assert.Equal(2, summary.Count());
        }
    }
}
