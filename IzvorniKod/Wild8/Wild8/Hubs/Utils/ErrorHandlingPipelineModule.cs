using Microsoft.AspNet.SignalR.Hubs;

namespace Wild8.Hubs.Utils
{
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        
        protected override void OnIncomingError(ExceptionContext ex, IHubIncomingInvokerContext context)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            base.OnIncomingError(ex, context);
        }
    }
}