using Quartz;
using Quartz.Impl;

public class Scheduler
{
    private readonly IScheduler _scheduler;

    public Scheduler(AggregationJob aggregationJob, IConfiguration configuration)
    {
        var schedulerFactory = new StdSchedulerFactory();
        _scheduler = schedulerFactory.GetScheduler().Result;

        var jobDetail = JobBuilder.Create<AggregationJob>()
            .WithIdentity("aggregationJob", "group1")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInHours(configuration.GetValue<int>("AggregationJobSettings:SchedulePeriodInHours"))
                .RepeatForever())
            .Build();

        _scheduler.ScheduleJob(jobDetail, trigger);
    }

    public async Task StartAsync()
    {
        await _scheduler.Start();
    }

    public async Task StopAsync()
    {
        await _scheduler.Shutdown();
    }
}
