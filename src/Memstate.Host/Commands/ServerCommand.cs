﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Memstate.Logging;

namespace Memstate.Host.Commands
{
    public class ServerCommand : ICommand
    {
        public event EventHandler Done = (sender, args) => { };       

        private IWebHost _webHost;

        private Host _host;

        public Task Start(string[] arguments)
        {
            StartServer(arguments);
            return StartWebInterface();
        }

        public async Task Stop()
        {
            await _webHost.StopAsync();
            _host.Stop();
        }

        private void StartServer(string[] arguments)
        {
            _host = new Host(arguments);
            _host.Start();
        }

        private async Task StartWebInterface()
        {
            _webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Web.Startup>()
                .ConfigureServices(services => services.AddSingleton(_host.Settings))
                .Build();

            await _webHost.StartAsync();
        }
    }
}