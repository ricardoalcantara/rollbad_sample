using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rollbar;

namespace nuget_error
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RollbarLocator
            .RollbarInstance
            // minimally required Rollbar configuration:
            .Configure(new RollbarConfig("123")
            {
                Environment = "Development",
            })
            // optional step if you would like to monitor Rollbar internal events within your application:
            // .InternalEvent += OnRollbarInternalEvent
            // .InternalEvent += (sender, e) => { Console.WriteLine(e.TraceAsString()); };
            ;

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRollbarMiddleware();

            app.UseMvc();
        }
    }
}
