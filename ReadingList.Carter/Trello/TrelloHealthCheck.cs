using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ReadingList.Trello.Services;

namespace ReadingList.Carter.Trello
{
    public class TrelloHealthCheck : IHealthCheck 
    {
        private readonly ITrelloStatusService _statusService;

        public TrelloHealthCheck(ITrelloStatusService trelloStatusService)
        {
            _statusService = trelloStatusService;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var incidents = _statusService.GetStatusIncidents();

            incidents = incidents.Where(i => i.Updated.Date == DateTime.Today && !i.Resolved);

            if (incidents.Any())
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(data: incidents.ToDictionary(k => k.Id, v => (object)v)));
            }
            return Task.FromResult(HealthCheckResult.Healthy());
         }
    }
}
