# Windows Service Using Quartz.NET 3
Windows service that can send json files to a remote server using either ftp connection, email or to WCF/WEB API depending on how you configure it. The remote server has a public and static IP address.

### Why Quartz and not System.Timers.Timer?
.NET Framework has “built-in” timer capabilities, through the System.Timers.Timer class - why would someone use Quartz rather than these standard features?

There are many reasons! Here are a few:

    1. Timers have no persistence mechanism.
    2. Timers have inflexible scheduling (only able to set start-time & repeat interval, nothing based on dates, time of day, etc.
    3. Timers don’t utilize a thread-pool (one thread per timer)
    4. Timers have no real management schemes - you’d have to write your own mechanism for being able to remember, organize and retreive your tasks by name, etc.

…of course to some simple applications these features may not be important, in which case it may then be the right decision not to use Quartz.NET.

### Sample Job Class
```
 public class JsonJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //simulating querying and getting data
            List<SampleData> data = new List<SampleData>();
            data.Add(new SampleData() { Id = 1, SSN = 64654, Message = "A jgghfgh jhghg" });
            data.Add(new SampleData() { Id = 2, SSN = 255546, Message = "A j uuufu" });
            data.Add(new SampleData() { Id = 3, SSN = 26564654, Message = "A jgyg uyyugu" });
            data.Add(new SampleData() { Id = 4, SSN = 26549872, Message = "A jhgu yguyg y" });
            data.Add(new SampleData() { Id = 5, SSN = 54679132, Message = "A jhgyu uyguyg" });

            //fetched required data and convert to json
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);

            //write data to file
            string fileName = DateTime.Now.ToString("yyyyMMdd") + "SSN_messages";
            string filePath = @"D:\JSON_FILES\" + fileName + ".json";

            //For methods that are inherently synchronous, you need to wrap them in your own Task so you can await it.
            await Task.Run(()=>
            {
				//PLEASE CHECK IF PATH EXISTS TR CREATE PATH!!!!
                File.WriteAllText(filePath, json);
                //start FTP - SENDING
				//start EMAIL - SENDING
				//start WEB API/WCF - SENDING
			}
		}
	}
```

### Sample Job Scheduling
```
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

                // Trigger the job to run now,  every 10 seconds starting at 1418hrs
				//uses 24 hrs
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
```
	
#### The code is well commented and feel free to contact me in case of any problem.


### Happy Coding!!!!!!!!!!!!!!! Osu!