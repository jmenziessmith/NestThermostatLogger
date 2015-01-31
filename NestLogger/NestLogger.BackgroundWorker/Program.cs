using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NestLogger.Core;
using Quartz;
using Quartz.Impl;
using AppHarbor;
using NLog;

namespace NestLogger.BackgroundWorker
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            ConsoleMirror.Initialize();

            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                logger.Error(ConsoleMirror.Captured);
            }
        }
    }
}
