using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NestLogger.Core;
using Quartz;
using Quartz.Impl;

namespace NestLogger.BackgroundWorker
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new Config();
            
            // construct a scheduler
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<Jobs.NestLoggerJob>().Build();

            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInMinutes(config.JobMinutes).RepeatForever())
                            .Build();

            scheduler.ScheduleJob(job, trigger);

        }
    }
}
