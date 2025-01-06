namespace Husa.Quicklister.Abor.Data.Queries.Tests.Seeders
{
    using System;
    using Factory = ApplicationDbContextTestFactory;
    using Ids = ShowingTimeQueriesRepositoryDatabaseIds;

    public static class ShowingTimeQueriesRepositoryDatabase
    {
        public static void SeedShowingTimeQueriesRepositoryTestsDatabase(this ApplicationDbContext dbContext)
        {
            dbContext.Community.Add(
                Factory.CommunityFactory(Ids.CommunityOneId, Ids.CompanyOneId));
            dbContext.Community.Add(
                Factory.CommunityFactory(Ids.CommunityTwoId, Ids.CompanyOneId));
            dbContext.Community.Add(
                Factory.CommunityFactory(Ids.CommunityThreeId, Ids.CompanyTwoId));

            dbContext.ShowingTimeContacts.Add(
                Factory.ShowingTimeContactFactory(Ids.ContactOneId, Guid.Empty));
            dbContext.ShowingTimeContacts.Add(
                Factory.ShowingTimeContactFactory(Ids.ContactTwoId, Ids.CompanyOneId));
            dbContext.ShowingTimeContacts.Add(
                Factory.ShowingTimeContactFactory(Ids.ContactThreeId, Ids.CompanyOneId));

            dbContext.CommunityShowingTimeContacts.Add(
                Factory.CommunityShowingTimeContactFactory(Ids.ContactTwoId, Ids.CommunityOneId, 3));
            dbContext.CommunityShowingTimeContacts.Add(
                Factory.CommunityShowingTimeContactFactory(Ids.ContactTwoId, Ids.CommunityTwoId, 7));
        }
    }
}
