using Adapter.Scheduler.Configuration;
using Adapter.Scheduler.Jobs;
using Quartz;

namespace Adapter.Scheduler;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterSchedulerLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));
        services.AddBackgroundServices(configuration);

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        OutboxOptions outboxOptions = configuration
            .GetSection(nameof(OutboxOptions))
            .Get<OutboxOptions>() ?? new OutboxOptions();

        services.AddQuartz(q =>
        {
            JobKey jobKey = new(nameof(ProcessOutboxJob));

            q.AddJob<ProcessOutboxJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{nameof(ProcessOutboxJob)}-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(outboxOptions.IntervalInSeconds)
                    .RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
