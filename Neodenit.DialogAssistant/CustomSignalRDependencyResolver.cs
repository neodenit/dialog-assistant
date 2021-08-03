using System;
using Microsoft.AspNet.SignalR;

namespace Neodenit.DialogAssistant
{
    public class CustomSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public CustomSignalRDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object GetService(Type serviceType)
        {
            var service = serviceProvider.GetService(serviceType);

            return service ?? base.GetService(serviceType);
        }
    }
}
