using Quartz;
using Topshelf;

namespace SmartOutlet.Service
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var host = HostFactory.New(x =>
            {
//                x.UseNLog();
	    
                x.Service<NancyService>(s =>
                {
                    s.ConstructUsing(settings => new NancyService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                    
//                    s.WithNancyEndpoint(x, c =>
//                    {
//                        c.AddHost(port: 8001/*, path:"smart-things/"*/);
//                        c.ConfigureNancy(hc =>
//                        {
//                            hc.UrlReservations.CreateAutomatically = true;
//                        });
//                    });
                    
//                    s.ScheduleQuartzJob(q =>
//                        q.WithJob(() =>
//                                JobBuilder.Create<MyJob>().Build())
//                            .AddTrigger(() => TriggerBuilder.Create()
//                                .WithSimpleSchedule(b => b
//                                    .WithIntervalInSeconds(10)
//                                    .RepeatForever())
//                                .Build()));
                });
                x.StartAutomatically();
                x.SetServiceName("topshelf.nancy.sampleservice");
                x.SetDisplayName("SmartThings");
                x.SetDescription("SmartThings - plugue inteligente");
                x.RunAsNetworkService();
            });
	
            host.Run();
        }
    }

    internal class MyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            int a = 0;
        }
    }
}