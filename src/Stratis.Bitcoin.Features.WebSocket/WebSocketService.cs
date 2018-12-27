using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stratis.Bitcoin.Builder.Feature;

namespace Stratis.Bitcoin.Features.WebSocket
{
    public class WebSocketService : IWebSocketService
    {
        private readonly object lockObject = new object();

        private readonly ILogger logger;

        private readonly bool started;

        private readonly IServiceProvider serviceProvider;

        public WebSocketService(WebSocketSettings settings, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.logger = loggerFactory.CreateLogger(GetType().FullName);

            this.serviceProvider = serviceProvider;
        }

        public static IWebHostBuilder CreateWebHostBuilder(FullNode fullNode, IEnumerable<ServiceDescriptor> services, IWebSocketService webSocketService, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .DiscoverMethods(fullNode) // Find all assemblies references that contains features.
                .ConfigureServices(collection =>
                {
                    // copies all the services defined for the full node to the Api.
                    // also copies over singleton instances already defined
                    foreach (ServiceDescriptor service in services)
                    {
                        object obj = fullNode.Services.ServiceProvider.GetService(service.ServiceType);
                        if (obj != null && service.Lifetime == ServiceLifetime.Singleton && service.ImplementationInstance == null)
                        {
                            collection.AddSingleton(service.ServiceType, obj);
                        }
                        else
                        {
                            collection.Add(service);
                        }
                    }

                    collection.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(IWebSocketService), webSocketService));
                })
                .UseStartup<Startup>().UseUrls("http://localhost:4336"); // TODO: Make this URL configureable.
        }

        /// <summary><see cref="ISignalRService.StartAsync" /></summary>
        public async Task<bool> StartAsync(FullNode fullNode, IEnumerable<ServiceDescriptor> services)
        {
            var started = await Task.Run(() =>
            {
                var args = new string[] { };

                CreateWebHostBuilder(fullNode, services, this, args).Build().Start(); //.Run();
                // this.logger.LogInformation("Web Socket Server listening on: " + Environment.NewLine + string.Join(Environment.NewLine, this.rpcSettings.GetUrls()));

                return true;

                //var address = this.Address.AbsoluteUri;
                //try
                //{
                //    // This allows injection of this service into the instance of the SignalR-generated hub.
                //    var signalRServiceDescriptor = new ServiceDescriptor(typeof(ISignalRService), this);
                //    this.webHost = new WebHostBuilder()
                //       .ConfigureServices(x => x.Add(signalRServiceDescriptor))
                //       .UseKestrel()
                //       .UseIISIntegration()
                //       .UseUrls(address)
                //       .UseStartup<Startup>()
                //       .Build();
                //    this.webHost.Start();
                //    this.logger.LogInformation("Hosted at {0}", address);
                //    return true;
                //}
                //catch (Exception e)
                //{
                //    this.logger.LogCritical("Failed to host at {0}: {1}", address, e.Message);
                //    return false;
                //}
            });

            return started;

            //return this.Started = started;
        }

        // this.logger.LogInformation("Web Socket Server is off based on configuration.");

        //public bool Started
        //{
        //    get { lock (this.lockObject) return this.started; }
        //    private set
        //    {
        //        lock (this.lockObject)
        //        {
        //            if (this.started == value) return;
        //            this.started = value;
        //        }

        //        if (value) this.startedStream.OnNext(this.HubRoute.AbsoluteUri);
        //    }
        //}

        public void Dispose()
        {
            //this.messageStream.OnCompleted();
            //this.startedStream.OnCompleted();
            //this.messageStream.Dispose();
            //this.startedStream.Dispose();
            //this.messageQueue?.Dispose();
            //this.messageQueue = null;
            //this.webHost?.Dispose();
            //this.webHost = null;
        }

        public async Task Broadcast(string message)
        {
            if (Startup.Provider == null)
            {
                return;
            }

            IHubContext<FullNodeHub> hubContext = Startup.Provider.GetService<IHubContext<FullNodeHub>>();

            if (hubContext == null)
            {
                return;
            }

            await hubContext.Clients.All.SendAsync("BroadcastMessage", "daemon", message);
        }
    }

    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class WebHostBuilderExtension
    {

        public static IWebHostBuilder DiscoverMethods(this IWebHostBuilder hostBuilder, FullNode fullNode)
        {
            hostBuilder.ConfigureServices(s =>
            {
                //IMvcCoreBuilder mvcBuilder = s.AddMvcCore(o =>
                //{
                //    o.ModelBinderProviders.Insert(0, new DestinationModelBinder());
                //    o.ModelBinderProviders.Insert(0, new MoneyModelBinder());
                //});

                var methods = new ControllerMethodList();

                // Include all feature assemblies for action discovery otherwise RPC actions will not execute
                // https://stackoverflow.com/questions/37725934/asp-net-core-mvc-controllers-in-separate-assembly
                foreach (Assembly assembly in fullNode.Services.Features.OfType<FullNodeFeature>().Select(x => x.GetType().GetTypeInfo().Assembly).Distinct())
                {
                    var asm = assembly;

                    // #1
                    //var controllers1 = asm.GetTypes()
                    //    .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                    //    .SelectMany(type => type.GetMethods())
                    //    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));

                    //// #2
                    //var controllers2 = asm.GetTypes()
                    //    .Where(type => typeof(Controller)
                    //        .IsAssignableFrom(type))
                    //    .SelectMany(type => type.GetMethods())
                    //    .Where(method => method.IsPublic
                    //        && !method.IsDefined(typeof(NonActionAttribute))
                    //        && (
                    //            method.ReturnType == typeof(ActionResult) ||
                    //            method.ReturnType == typeof(Task<ActionResult>) ||
                    //            method.ReturnType == typeof(String)
                    //            //method.ReturnType == typeof(IHttpResult) ||
                    //            )
                    //        )
                    //    .Select(m => m.Name);

                    //var controllers3 = asm
                    //    .GetTypes()
                    //    .Where(type => typeof(ApiController).IsAssignableFrom(type))
                    //    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    //    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    //    .GroupBy(x => x.DeclaringType.Name)
                    //    .Select(x => new { Controller = x.Key, Actions = x.Select(s => s.Name).ToList() })
                    //    .ToList();

                    var discovered = asm.GetTypes()
                            .Where(type => typeof(Controller).IsAssignableFrom(type))
                            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                            .Select(x => new ControllerMethod { Parameters = x.GetParameters(), Type = x.DeclaringType, Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                            .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

                    methods.List.AddRange(discovered);
                    //mvcBuilder.AddApplicationPart(assembly);
                }

                // Register a single instance of the ControllerMethodList, that contains a list of all available controller methods that 
                // we want to auto-expose through the Web Socket interface.
                s.AddSingleton(methods);
            });

            return hostBuilder;
        }
    }
}
