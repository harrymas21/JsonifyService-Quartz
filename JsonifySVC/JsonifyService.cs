using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Web.Script.Serialization;
using System;
using System.Configuration;
using System.Timers;
using Quartz;
using JsonifySVC.Job;
using System.Collections.Specialized;
using Quartz.Impl;

namespace JsonifySVC
{
    public partial class JsonifyService : ServiceBase
    {
        public JsonifyService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            try
            {
                IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<JsonJob>()
                    .WithIdentity("JsonJob", "Group1")
                    .Build();

                // Trigger the job to run now,  every 10 seconds starting at 10:15 AM.
                ITrigger trigger = TriggerBuilder.Create()
                  .WithIdentity("JsonJobTrigger", "Group1")
                 .WithDailyTimeIntervalSchedule
                    (s =>
                        s.WithIntervalInSeconds(10)
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(14, 18))
                    )
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.InnerException.ToString());
            }
        }

        protected override void OnStop()
        {
            Console.WriteLine("Jsonify Service  -  Has stopped!!!");
            base.OnStop();
        }
      
       
    }
}
