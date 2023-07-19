namespace Husa.Quicklister.Abor.Api.Tests.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Quicklister.Abor.Api.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class QuicklisterAuthorizationFilterTest
    {
        private readonly Mock<IUserProvider> userProvider = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<ILogger<QuicklisterAuthorizationFilter>> logger = new();
        private readonly Mock<HttpContext> httpContextMock = new();

        private readonly Guid userId = new("7c189de0-2493-44fb-b9da-30d1a6657f1c");
        private readonly Guid companyId = new("7c189de0-44fb-b9da-2493-30d1a6657f1c");

        [Fact]
        public async Task AuthorizeEndpointWithAllowAnonymousAttribute()
        {
            // Arrange
            var fakeAuthFilterContext = this.MockAuthorizationContext(UserRole.User);
            var sut = new QuicklisterAuthorizationFilter(this.userProvider.Object, this.serviceSubscriptionClient.Object, this.logger.Object);

            // Act
            await sut.OnAuthorizationAsync(fakeAuthFilterContext);

            // Assert
            this.httpContextMock.VerifyGet(ctx => ctx.User, Times.Never);
            Assert.IsType<OkResult>(fakeAuthFilterContext.Result);
        }

        [Fact]
        public async Task AuthorizeEndpointWithPhotographerRetursForbidden()
        {
            // Arrange
            var fakeAuthFilterContext = this.MockAuthorizationContext(UserRole.Photographer, isAnonymous: false);
            var sut = new QuicklisterAuthorizationFilter(this.userProvider.Object, this.serviceSubscriptionClient.Object, this.logger.Object);

            // Act
            await sut.OnAuthorizationAsync(fakeAuthFilterContext);

            // Assert
            this.httpContextMock.VerifyGet(ctx => ctx.User, Times.Once);
            Assert.IsType<ForbidActionResult>(fakeAuthFilterContext.Result);
        }

        [Fact]
        public async Task AuthorizeEndpointValidateCompanyEmployeeUnauthorizedResult()
        {
            // Arrange
            var fakeAuthFilterContext = this.MockAuthorizationContext(UserRole.User, isAnonymous: false, setCompany: false);
            var employeeResource = new Mock<IEmployee>();
            this.serviceSubscriptionClient
                .SetupGet(sc => sc.Employee)
                .Returns(employeeResource.Object);

            var sut = new QuicklisterAuthorizationFilter(this.userProvider.Object, this.serviceSubscriptionClient.Object, this.logger.Object);

            // Act
            await sut.OnAuthorizationAsync(fakeAuthFilterContext);

            // Assert
            this.httpContextMock.VerifyGet(ctx => ctx.User, Times.Exactly(2));
            Assert.IsType<UnauthorizedObjectResult>(fakeAuthFilterContext.Result);
            employeeResource.Verify(e => e.GetEmployeeByUserAndCompany(It.Is<Guid>(u => u == this.userId), It.Is<Guid>(c => c == this.companyId), default), Times.Never);
            this.serviceSubscriptionClient.VerifyGet(sc => sc.Employee, Times.Never);
        }

        [Fact]
        public async Task AuthorizeEndpointValidateCompanyEmployeeSuccess()
        {
            // Arrange
            var fakeAuthFilterContext = this.MockAuthorizationContext(UserRole.User, isAnonymous: false);
            var employeeResource = new Mock<IEmployee>();
            employeeResource
                .Setup(e => e.GetEmployeeByUserAndCompany(It.Is<Guid>(u => u == this.userId), It.Is<Guid>(c => c == this.companyId), default))
                .ReturnsAsync(new Employee
                {
                    UserId = this.userId,
                    CompanyName = "fake-company",
                    Id = this.userId,
                    RoleName = RoleEmployee.SalesEmployee,
                })
                .Verifiable();
            this.serviceSubscriptionClient
                .SetupGet(sc => sc.Employee)
                .Returns(employeeResource.Object)
                .Verifiable();

            var sut = new QuicklisterAuthorizationFilter(this.userProvider.Object, this.serviceSubscriptionClient.Object, this.logger.Object);

            // Act
            await sut.OnAuthorizationAsync(fakeAuthFilterContext);

            // Assert
            this.httpContextMock.VerifyGet(ctx => ctx.User, Times.Exactly(2));
            Assert.IsType<OkResult>(fakeAuthFilterContext.Result);
            employeeResource.Verify();
            this.serviceSubscriptionClient.Verify();
        }

        [Fact]
        public async Task AuthorizeEndpointValidateCompanyEmployeeNotFound()
        {
            // Arrange
            var fakeAuthFilterContext = this.MockAuthorizationContext(UserRole.User, isAnonymous: false);
            var employeeResource = new Mock<IEmployee>();
            employeeResource
                .Setup(e => e.GetEmployeeByUserAndCompany(It.Is<Guid>(u => u == this.userId), It.Is<Guid>(c => c == this.companyId), default))
                .ReturnsAsync((Employee)null)
                .Verifiable();
            this.serviceSubscriptionClient
                .SetupGet(sc => sc.Employee)
                .Returns(employeeResource.Object)
                .Verifiable();

            var sut = new QuicklisterAuthorizationFilter(this.userProvider.Object, this.serviceSubscriptionClient.Object, this.logger.Object);

            // Act
            await sut.OnAuthorizationAsync(fakeAuthFilterContext);

            // Assert
            this.httpContextMock.VerifyGet(ctx => ctx.User, Times.Exactly(2));
            Assert.IsType<UnauthorizedObjectResult>(fakeAuthFilterContext.Result);
            employeeResource.Verify();
            this.serviceSubscriptionClient.Verify();
        }

        [Fact]
        public async Task AuthorizeEndpointValidateCompanyEmployeeForMLSAdministratorSuccess()
        {
            // Arrange
            var fakeAuthFilterContext = this.MockAuthorizationContext(UserRole.MLSAdministrator, isAnonymous: false);
            var employeeResource = new Mock<IEmployee>();
            this.serviceSubscriptionClient
                .SetupGet(sc => sc.Employee)
                .Returns(employeeResource.Object);

            var sut = new QuicklisterAuthorizationFilter(this.userProvider.Object, this.serviceSubscriptionClient.Object, this.logger.Object);

            // Act
            await sut.OnAuthorizationAsync(fakeAuthFilterContext);

            // Assert
            this.httpContextMock.VerifyGet(ctx => ctx.User, Times.Exactly(2));
            Assert.IsType<OkResult>(fakeAuthFilterContext.Result);
            employeeResource.Verify(e => e.GetEmployeeByUserAndCompany(It.Is<Guid>(u => u == this.userId), It.Is<Guid>(c => c == this.companyId), default), Times.Never);
            this.serviceSubscriptionClient.VerifyGet(sc => sc.Employee, Times.Never);
        }

        private AuthorizationFilterContext MockAuthorizationContext(UserRole userRole, bool isAnonymous = true, bool setCompany = true)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, userRole.ToString()),
                new(ClaimTypes.NameIdentifier, this.userId.ToString()),
                new("username", "test-user"),
            };

            var claimIdentity = new ClaimsIdentity(claims, "fake-auth-type");
            var claimPrincipal = new ClaimsPrincipal();
            claimPrincipal.AddIdentity(claimIdentity);

            this.httpContextMock
              .Setup(a => a.Request.Headers["Authorization"])
              .Returns("fake-api-key");

            if (setCompany)
            {
                this.httpContextMock
                  .Setup(a => a.Request.Headers["CurrentCompanySelected"])
                  .Returns(this.companyId.ToString());
            }

            this.httpContextMock
              .SetupGet(ctx => ctx.User)
              .Returns(claimPrincipal);

            var fakeActionContext = new ActionContext(
                this.httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor());

            var filterMetadata = new Mock<IFilterMetadata>();

            var authorizationFilter = new AuthorizationFilterContext(fakeActionContext, new[] { filterMetadata.Object })
            {
                Result = new OkResult(),
            };

            if (isAnonymous)
            {
                authorizationFilter.ActionDescriptor.EndpointMetadata = new[] { new AllowAnonymousAttribute() };
            }

            return authorizationFilter;
        }
    }
}
