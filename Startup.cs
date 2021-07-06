using System;
using System.Net.Http;
using HttpClientNetCore.Abstract;
using HttpClientNetCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HttpClientNetCore
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
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new SlugifyParameterTransformer()));
            });
            services.AddHttpClient(); //IHttpClientFactory implemented in post method

            #region TypeClientServices

            /*
                //###### Typed Clients is recommended over others! ######

                //for multiple api source
                services.AddHttpClient("WeatherAPI_v1", x =>
                {
                    //_httpClientFactory.CreateClient("WeatherAPI");
                    x.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
                });

                //github api source
                services.AddHttpClient("github", x =>
                {
                    //_httpClientFactory.CreateClient("WeatherAPI");
                    x.BaseAddress = new Uri("http://api.github.com/");
                    x.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                    x.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory");
                });

                */

            #endregion

            //IWeatherService & IGitHubService Registry this is another alternative.

            services.AddHttpClient<IWeatherService, WeatherModel>(x =>
            {
                //HttpClient is registered automatically ,in period of GitHubModel registry
                x.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5)); // default socket lifetime is 2 minutes.

            services.AddHttpClient<IGitHubService, GitHubModel>(g =>
            {
                //HttpClient is registered automatically ,in period of GitHubModel registry
                g.BaseAddress = new Uri("http://api.github.com/");
                g.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                g.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory");
            }); //.SetHandlerLifetime(TimeSpan.MaxValue);//to keep the socket alive for good

            // ## TimeSpan ##
            //Maximum TimeSpan       10675199.02:48:05.4775807
            //Minimum TimeSpan      -10675199.02:48:05.4775808
            //Zero TimeSpan                   00:00:00

            services.AddSwaggerDocument(configure =>
            {
                configure.Title = "HttpClientTest";
                configure.DocumentName = "API";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}