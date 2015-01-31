using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace NestLogger.BackgroundWorker.Jobs
{
    public class NestLoggerJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var logger = new NestLogger.Core.NestDataLogger();
            logger.Execute();
        }
    }
}
