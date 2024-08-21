namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Alert;
    using Husa.Quicklister.Extensions.Application.Interfaces.Alert;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class AlertsController : Husa.Quicklister.Extensions.Api.Controllers.AlertsController
    {
        private readonly IAlertQueriesRepository alertQueriesRepository;

        public AlertsController(
           IAlertQueriesRepository alertQueriesRepository,
           IViolationWarningAlertService violationWarningAlertService,
           ILogger<AlertsController> logger,
           IMapper mapper)
           : base(violationWarningAlertService, logger, mapper)
        {
            this.alertQueriesRepository = alertQueriesRepository ?? throw new ArgumentNullException(nameof(alertQueriesRepository));
        }

        [HttpGet("{alertType}")]
        public async Task<IActionResult> GetAsync([FromRoute] AlertType alertType, [FromQuery] BaseAlertFilterRequest filter)
        {
            this.logger.LogInformation("Starting to get alert {alertType} in ABOR", alertType);
            var queryFilter = this.mapper.Map<BaseAlertQueryFilter>(filter);
            var queryResponse = await this.alertQueriesRepository.GetAsync(alertType, filter: queryFilter);
            if (queryResponse == null || !queryResponse.Data.Any())
            {
                this.logger.LogInformation("No alerts were found of type {alertType} in ABOR", alertType);
                var total = queryResponse != null && queryFilter.IsOnlyCount ? queryResponse.Total : 0;
                return this.Ok(new DataSet<AlertDetailResponse>(
                    data: Array.Empty<AlertDetailResponse>(),
                    total));
            }

            var alertDetails = this.mapper.Map<IEnumerable<AlertDetailResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<AlertDetailResponse>(alertDetails, queryResponse.Total));
        }

        [HttpGet]
        public async Task<IActionResult> GetAlertTotal([FromQuery] IEnumerable<AlertType> alerts)
        {
            var total = 0;
            if (alerts is null || !alerts.Any())
            {
                this.logger.LogWarning("No alerts were provided to search, the value {totalAlerts} will be returned", total);
                return this.Ok(total);
            }

            total = await this.alertQueriesRepository.GetTotal(alerts);
            this.logger.LogInformation("The total alerts {totalAlerts} found for alerts {alertTypes} in ABOR", string.Join(',', alerts), total);
            return this.Ok(total);
        }
    }
}
