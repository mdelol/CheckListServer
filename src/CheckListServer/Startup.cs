
using System;
using System.Threading.Tasks;
using DAL.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CheckListServer
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

      if (env.IsEnvironment("Development"))
      {
        builder.AddApplicationInsightsSettings(developerMode: true);
      }

      builder.AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthorization();
      services.AddApplicationInsightsTelemetry(Configuration);
#if LINUX
      var connection = "Server=172.17.0.1;User Id=postgres;Port=5432;Database=Tasks;";
      services.AddDbContext<ProjectContext>(
        options => { options.UseNpgsql(connection, b => b.MigrationsAssembly("CheckListServer")); });
#else
          var connection = @"Server=(localdb)\mssqllocaldb;Database=Tasks;";
          services.AddDbContext<ProjectContext>(
              options => { options.UseSqlServer(connection, b => b.MigrationsAssembly("CheckListServer")); });
#endif


      services.AddMvc(config =>
      {
        var policy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .Build();
        config.Filters.Add(new AuthorizeFilter(policy));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      app.UseCookieAuthentication(new CookieAuthenticationOptions()
      {
        AuthenticationScheme = "Cookie",
        LoginPath = new PathString("/Account/Login/"),
        AccessDeniedPath = new PathString("/Account/Forbidden/"),
        AutomaticAuthenticate = true,
        AutomaticChallenge = true
      });

      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseApplicationInsightsRequestTelemetry();

      app.UseApplicationInsightsExceptionTelemetry();

      app.UseMvc();
    }
  }
}