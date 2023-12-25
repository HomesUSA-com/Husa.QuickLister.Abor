namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using DinkToPdf.Contracts;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Quickbooks.Interfaces;
    using Husa.Extensions.Quickbooks.Models.Invoice;
    using Husa.Extensions.Quickbooks.Models.Invoice.AttachResponse;
    using Husa.Extensions.Quickbooks.Models.Invoice.CustomerResponse;
    using Husa.Extensions.Quickbooks.Models.Invoice.InvoiceResponses;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.CompanyServicesManager.Api.Contracts.Request;
    using Response = Husa.CompanyServicesManager.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingBillServiceTests
    {
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient;
        private readonly Mock<IListingSaleRepository> listingSaleRepository;
        private readonly Mock<IQuickbooksApi> quickbooksApi;
        private readonly Mock<IConverter> converter;
        private readonly Mock<ILogger<SaleListingBillService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;
        public SaleListingBillServiceTests(ApplicationServicesFixture fixture)
        {
            this.logger = new Mock<ILogger<SaleListingBillService>>();
            this.quickbooksApi = new Mock<IQuickbooksApi>();
            this.converter = new Mock<IConverter>();
            this.serviceSubscriptionClient = new Mock<IServiceSubscriptionClient>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.listingSaleRepository = new Mock<IListingSaleRepository>();
            this.Sut = new SaleListingBillService(
                fixture.Options.Object,
                this.serviceSubscriptionClient.Object,
                this.listingSaleRepository.Object,
                this.quickbooksApi.Object,
                this.converter.Object,
                this.userContextProvider.Object,
                this.logger.Object);
        }

        private SaleListingBillService Sut { get; set; }
        [Fact]
        public async Task ProcessInvoiceCompanyNullError()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var listingIds = new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };
            var invoiceDto = new InvoiceDto()
            {
                CompanyId = companyId,
                ListingIds = listingIds,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
            };
            CompanyDetail companyDetail = null;
            this.serviceSubscriptionClient
                .Setup(x => x.Company.GetCompany(It.Is<Guid>(x => x == companyId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            // Act && Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ProcessInvoice(invoiceDto));
            this.serviceSubscriptionClient.Verify(
                sl => sl.Company.GetCompany(
                    It.Is<Guid>(id => id == companyId),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task ProcessInvoiceCustomerRefNull()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var listingIds = new List<Guid>()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };
            var invoiceDto = new InvoiceDto()
            {
                CompanyId = companyId,
                ListingIds = listingIds,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
            };
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            this.serviceSubscriptionClient
                .Setup(x => x.Company.GetCompany(It.Is<Guid>(x => x == companyId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            // Act && Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ProcessInvoice(invoiceDto));
            this.serviceSubscriptionClient.Verify(
                sl => sl.Company.GetCompany(
                    It.Is<Guid>(id => id == companyId),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task ProcessInvoiceCreateInvoice()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingId1 = Guid.NewGuid();
            var listingId2 = Guid.NewGuid();
            var listingId3 = Guid.NewGuid();
            var listingIds = new List<Guid>()
            {
                listingId1,
                listingId2,
                listingId3,
            };
            var invoiceDto = new InvoiceDto()
            {
                CompanyId = companyId,
                ListingIds = listingIds,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
            };
            var services = new List<Response.ServiceSubscription>()
            {
                new Response.ServiceSubscription()
                {
                    Price = 3232,
                    ServiceCode = ServiceCode.NewListing,
                },
            };
            var customerObjectResponse = new CustomerObjectResponse()
            {
                Status = 200,
                Data = new GetCustomer(),
            };
            var servicesDataSet = new DataSet<Response.ServiceSubscription>(services, services.Count);
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            var saleListing1 = TestModelProvider.GetListingSaleEntity(listingId1, true, companyId);
            saleListing1.PublishInfo.PublishType = ActionType.NewListing;
            saleListing1.ListDate = DateTime.UtcNow;
            saleListing1.InvoiceInfo = new();
            var saleListing3 = TestModelProvider.GetListingSaleEntity(listingId3, true, companyId);
            saleListing3.PublishInfo.PublishType = ActionType.Relist;
            saleListing3.ListDate = DateTime.UtcNow;
            saleListing3.InvoiceInfo = new();
            var invoiceResultDto = new InvoiceObjectResponse()
            {
                Data = "{\"invoice\":{\"txnDate\":\"2023-09-17\",\"domain\":\"QBO\",\"printStatus\":\"NeedToPrint\",\"salesTermRef\":null,\"totalAmt\":114467,\"line\":[{\"description\":\"New Listing from 1/1/0001 12:00:00 AM to 1/1/0001 12:00:00 AM.\",\"detailType\":\"SalesItemLineDetail\",\"salesItemLineDetail\":{\"taxCodeRef\":{\"value\":\"NON\"},\"qty\":2,\"unitPrice\":56785,\"itemRef\":{\"name\":\"MLS\",\"value\":\"3\"}},\"lineNum\":1,\"amount\":113570,\"id\":\"1\",\"subTotalLineDetail\":null},{\"description\":\"Comparable from 1/1/0001 12:00:00 AM to 1/1/0001 12:00:00 AM.\",\"detailType\":\"SalesItemLineDetail\",\"salesItemLineDetail\":{\"taxCodeRef\":{\"value\":\"NON\"},\"qty\":1,\"unitPrice\":897,\"itemRef\":{\"name\":\"MLS\",\"value\":\"3\"}},\"lineNum\":2,\"amount\":897,\"id\":\"2\",\"subTotalLineDetail\":null},{\"description\":null,\"detailType\":\"SubTotalLineDetail\",\"salesItemLineDetail\":null,\"lineNum\":0,\"amount\":114467,\"id\":null,\"subTotalLineDetail\":{}}],\"dueDate\":\"2023-09-17\",\"applyTaxAfterDiscount\":false,\"docNumber\":null,\"sparse\":false,\"customerMemo\":null,\"deposit\":0,\"balance\":114467,\"customerRef\":{\"name\":\"ABC Homes\",\"value\":\"761\"},\"txnTaxDetail\":null,\"syncToken\":\"0\",\"linkedTxn\":[],\"billEmail\":null,\"shipAddr\":{\"city\":\"Dallas\",\"line1\":\"123333 Dallas st\",\"postalCode\":\"75248\",\"lat\":null,\"long\":null,\"countrySubDivisionCode\":\"Tx\",\"id\":\"3551\"},\"emailStatus\":\"NotSet\",\"billAddr\":{\"line4\":null,\"line3\":null,\"line2\":null,\"line1\":\"123333 Dallas st\",\"long\":null,\"lat\":null,\"id\":\"3551\"},\"metaData\":{\"createTime\":\"2023-09-17T17:03:46-03:00\",\"lastUpdatedTime\":\"2023-09-17T17:03:46-03:00\"},\"customField\":[],\"id\":\"26722\"}}",
                Status = 200,
            };
            var attachResultDto = new AttachableObjectResponse()
            {
                Data = "{\"attachableResponse\":{\"attachable\":{\"id\":\"7000000000068734038\",\"syncToken\":\"0\",\"metaData\":{\"createTime\":\"2023-09-18T16:05:32-07:00\",\"lastUpdatedTime\":\"2023-09-18T16:05:32-07:00\"},\"attachableRef\":{\"entityRef\":\"26849\",\"includeOnSend\":\"false\"},\"fileName\":\"Kindred Homes_SanAntonio_09-18-2023.pdf\",\"fileAccessUri\":\"/v3/company/123146071669144/download/7000000000068734038\",\"tempDownloadUri\":\"https://intuit-qbo-prod-18.s3.amazonaws.com/123146071669144/attachments/73dc8bb4-6c9a-4040-ad2a-f24778bcde6dKindred%20Homes_SanAntonio_09-18-2023.pdf?X-Amz-Algorithm=AWS4-HMAC-SHA256\\u0026X-Amz-Date=20230918T230533Z\\u0026X-Amz-SignedHeaders=host\\u0026X-Amz-Expires=900\\u0026X-Amz-Credential=AKIA3V3MBG4KNG3VICVV%2F20230918%2Fus-east-1%2Fs3%2Faws4_request\\u0026X-Amz-Signature=95e13a40a1931f5a2387e73dfb0a726fc538bd91df0ab1e64a15903630a4309f\",\"size\":\"31766\",\"contentType\":\"application/pdf\"}},\"time\":\"2023-09-18T16:05:32.219-07:00\",\"xmlns\":null}",
                Status = 200,
            };
            companyDetail.CustomerRef = 34;
            this.serviceSubscriptionClient
                .Setup(x => x.Company.GetCompany(It.Is<Guid>(x => x == companyId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail).Verifiable();
            this.serviceSubscriptionClient
                .Setup(x => x.Company
                .GetCompanyServices(It.Is<Guid>(x => x == companyId), It.IsAny<Request.FilterServiceSubscriptionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(servicesDataSet).Verifiable();
            this.listingSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(x => x == listingId1), It.IsAny<bool>()))
                .ReturnsAsync(saleListing1).Verifiable();
            this.listingSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(x => x == listingId3), It.IsAny<bool>()))
                .ReturnsAsync(saleListing3).Verifiable();
            this.quickbooksApi
                .Setup(x => x.GetCustomerRef(It.Is<int>(x => x == companyDetail.CustomerRef)))
                .ReturnsAsync(customerObjectResponse).Verifiable();
            this.quickbooksApi
                .Setup(x => x.PostDataAsync(It.IsAny<CreateInvoiceDto>()))
                .ReturnsAsync(invoiceResultDto).Verifiable();
            this.quickbooksApi
                .Setup(x => x.AttachPdfToInvoice(It.IsAny<InvoiceAttachDto>()))
                .ReturnsAsync(attachResultDto).Verifiable();
            this.userContextProvider
                .Setup(x => x.GetCurrentUserId())
                .Returns(userId).Verifiable();

            // Act && Assert
            var result = await this.Sut.ProcessInvoice(invoiceDto);
            var objectResult = Assert.IsAssignableFrom<CommandSingleResult<string, string>>(result);
            Assert.IsType<string>(objectResult.Result);
            this.serviceSubscriptionClient.Verify(
                sl => sl.Company.GetCompany(
                    It.Is<Guid>(id => id == companyId),
                    It.IsAny<CancellationToken>()),
                Times.Once());
            this.listingSaleRepository.Verify(
                sl => sl.GetById(
                    It.Is<Guid>(id => id == listingId1),
                    It.IsAny<bool>()),
                Times.Once());
            this.listingSaleRepository.Verify(
                sl => sl.GetById(
                    It.Is<Guid>(id => id == listingId2),
                    It.IsAny<bool>()),
                Times.Once());
            this.listingSaleRepository.Verify(
                sl => sl.GetById(
                    It.Is<Guid>(id => id == listingId3),
                    It.IsAny<bool>()),
                Times.Once());
            this.quickbooksApi.Verify(
                sl => sl.PostDataAsync(
                    It.IsAny<CreateInvoiceDto>()),
                Times.Once());
            this.listingSaleRepository.Verify(
                sl => sl.SaveChangesAsync(),
                Times.Once());
        }
    }
}
