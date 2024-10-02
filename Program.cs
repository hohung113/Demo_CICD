//using Microsoft.EntityFrameworkCore;
//using RazorPagesLab.DatabaseContext;
//using System.Configuration;

//namespace RazorPagesLab
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddRazorPages();

//            //var host = CreateHostBuilder(args).Build();

//            // add dbContext
//            builder.Services.AddDbContext<SchoolContext>(opt =>
//                      opt.UseSqlServer(
//                          builder.Configuration.GetConnectionString("SchoolContext")));
//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }

//            static void CreateDbIfNotExists(IHost host)
//            {
//                using (var scope = host.Services.CreateScope())
//                {
//                    var services = scope.ServiceProvider;
//                    try
//                    {
//                        var context = services.GetRequiredService<SchoolContext>();
//                        context.Database.EnsureCreated();
//                        DbInitializer.Initialize(context);
//                    }
//                    catch (Exception ex)
//                    {
//                        var logger = services.GetRequiredService<ILogger<Program>>();
//                        logger.LogError(ex, "An error occurred creating the DB.");
//                    }
//                }
//            }




//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthorization();

//            app.MapRazorPages();

//            app.Run();
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using RazorPagesLab.DatabaseContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace RazorPagesLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Add dbContext
            builder.Services.AddDbContext<SchoolContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Call to create the database if it does not exist
            CreateDbIfNotExists(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapGet("/", async context =>
            {
                context.Response.Redirect("/Students/Index");
            });
            app.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    context.Database.EnsureCreated();
                    DbInitializer.Initialize(context); // Ensure you have this method defined
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
