namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]
    public class SaleListingBillingQueryExtensionsTests
    {
        [Fact]
        public void FilterByMlsNumber()
        {
            // Arrange
            var searchFilter = "123";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = searchFilter,
                    ZipCode = "968",
                    StreetNum = "4521",
                },
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "456",
                    ZipCode = "968",
                    StreetNum = "4521",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().MlsNumber);
        }

        [Fact]
        public void FilterByZipCode()
        {
            // Arrange
            var searchFilter = "123";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "abc",
                    ZipCode = searchFilter,
                    StreetNum = "4521",
                },
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "456",
                    ZipCode = "968",
                    StreetNum = "4521",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().ZipCode);
        }

        [Fact]
        public void FilterByStreetNum()
        {
            // Arrange
            var searchFilter = "123";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "abc",
                    ZipCode = "1456",
                    StreetNum = "4521",
                },
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "456",
                    ZipCode = "968",
                    StreetNum = searchFilter,
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().StreetNum);
        }

        [Fact]
        public void FilterByListFee()
        {
            // Arrange
            var searchFilter = "25";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "abc",
                    ZipCode = "1456",
                    StreetNum = "4521",
                    ListFee = (decimal?)30.00,
                },
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "456",
                    ZipCode = "968",
                    StreetNum = "4521",
                    ListFee = (decimal?)25.00,
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().ListFee.ToString());
        }

        [Theory]
        [InlineData("$30")]
        [InlineData("$3,000")]
        public void FilterByListFeeUsingPrice(string searchFilter)
        {
            // Arrange
            var cleanSearch = searchFilter.TrimStart('$');
            if (decimal.TryParse(cleanSearch, out _))
            {
                cleanSearch = string.Join(string.Empty, cleanSearch.Split(','));
            }

            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "abc",
                    ZipCode = "1456",
                    StreetNum = "4521",
                    ListFee = (decimal?)30.00,
                },
                new ListingSaleBillingQueryResult()
                {
                    MlsNumber = "456",
                    ZipCode = "968",
                    StreetNum = "4521",
                    ListFee = (decimal?)3000.00,
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(cleanSearch, result.Single().ListFee.ToString());
        }

        [Fact]
        public void FilterByStreetName()
        {
            // Arrange
            var searchFilter = "abc";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = searchFilter,
                    Subdivision = "subdivision",
                    OwnerName = "ownerName",
                    CreatedBy = "CreatedBy",
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownerName2",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().StreetName);
        }

        [Fact]
        public void FilterBySubdivison()
        {
            // Arrange
            var searchFilter = "abc";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "strtName",
                    Subdivision = searchFilter,
                    OwnerName = "ownerName",
                    CreatedBy = "CreatedBy",
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownerName2",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().Subdivision);
        }

        [Fact]
        public void FilterByOwnerName()
        {
            // Arrange
            var searchFilter = "abc";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "strtName",
                    Subdivision = "subdivision",
                    OwnerName = searchFilter,
                    CreatedBy = "CreatedBy",
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownerName2",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().OwnerName);
        }

        [Fact]
        public void FilterByCreatedBy()
        {
            // Arrange
            var searchFilter = "abc";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "strtName",
                    Subdivision = "subdivision",
                    OwnerName = "ownerName",
                    CreatedBy = searchFilter,
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownrName",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().CreatedBy);
        }

        [Fact]
        public void FilterByStreetNameAndStreetNumber()
        {
            // Arrange
            var searchFilter = "123 abc";
            var splitted = searchFilter.Split(" ", 2);
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = splitted[1],
                    StreetNum = splitted[0],
                    Subdivision = "subdivision",
                    OwnerName = "ownerName",
                    CreatedBy = "crtdBy",
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownrName",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, $"{result.Single().StreetNum} {result.Single().StreetName}");
        }

        [Fact]
        public void FilterBySubdivisionMultipleWords()
        {
            // Arrange
            var searchFilter = "sub division";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "strrtName",
                    StreetNum = "1235",
                    Subdivision = searchFilter,
                    OwnerName = "ownerName",
                    CreatedBy = "crtdBy",
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownrName",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().Subdivision);
        }

        [Fact]
        public void FilterByCreatedByMultipleWords()
        {
            // Arrange
            var searchFilter = "john doe";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "strrtName",
                    StreetNum = "1235",
                    Subdivision = "subdivision",
                    OwnerName = "ownerName",
                    CreatedBy = searchFilter,
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownrName",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().CreatedBy);
        }

        [Fact]
        public void FilterByOwnerNameByMultipleWords()
        {
            // Arrange
            var searchFilter = "john doe";
            var listings = new List<ListingSaleBillingQueryResult>()
            {
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "strrtName",
                    StreetNum = "1235",
                    Subdivision = "subdivision",
                    OwnerName = searchFilter,
                    CreatedBy = "CreatedBy",
                },
                new ListingSaleBillingQueryResult()
                {
                    StreetName = "streetName",
                    Subdivision = "subdivision2",
                    OwnerName = "ownrName",
                    CreatedBy = "CreatedBy2",
                },
            };

            // Act
            var result = listings.FilterBySearch(searchFilter);

            // Assert
            Assert.Single(result);
            Assert.Equal(searchFilter, result.Single().OwnerName);
        }
    }
}
