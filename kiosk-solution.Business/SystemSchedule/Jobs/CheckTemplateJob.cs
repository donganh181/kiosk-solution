using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace kiosk_solution.Business.SystemSchedule.Jobs
{
    public class CheckTemplateJob : IJob
    {
        private readonly ILogger<CheckTemplateJob> _logger;

        public CheckTemplateJob(ILogger<CheckTemplateJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("check template job running...");
            return Task.CompletedTask;
        }
    }
}