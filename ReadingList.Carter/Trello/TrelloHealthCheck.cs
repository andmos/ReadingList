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

            var trelloIncidents = incidents.ToList();
            return Task.FromResult(trelloIncidents.Any() 
                ? HealthCheckResult.Unhealthy(data: trelloIncidents.ToDictionary(k => k.Id, v => (object)v))
                : HealthCheckResult.Healthy());
        }
    }
}
