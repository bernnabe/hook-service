using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceHooks.Api.Maps;
using ServiceHooks.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceHooks.Repository;


namespace ServiceHooks.Api
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
            var mapConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddLogging();

            services.AddControllers();
            services.AddDbContext<ServiceHooks.Repository.ServiceHooksContext>(opt =>
                                                                                    opt.UseSqlServer(Configuration.GetConnectionString("ServiceHooksDatabase")),
                                                                                    ServiceLifetime.Transient);
            services.AddTransient<IEventService, EventService>();
            services.AddHttpClient<IWebHooksService, WebHooksService>();
            services.AddTransient<IEventResponseService, EventResponseService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Service Hooks API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ServiceHooksContext>();

         
                context.Database.EnsureCreated();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Service Hooks API V1");
            });

            app.UseAuthentication();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
