using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Wild8.Hubs.Utils;
using Wild8.Utils;
using Wild8.StaticInfo;

[assembly: OwinStartup(typeof(Wild8.Startup))]

namespace Wild8
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            MailUtil.Initialize(StaticInfoMap.RestMailSmtpClient, StaticInfoMap.RestEmail, StaticInfoMap.RestEmail, StaticInfoMap.RestEmailPass);
            var config = new HubConfiguration();
            config.EnableDetailedErrors = true;
            app.MapSignalR(config);
        }
    }
}
